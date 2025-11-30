using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities.CustomerAPI;
using EpicMarket.Entities.CustomModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EpicMarket.Business.API.Controllers.CustomerAPI
{
    /// <summary>
    /// Consumer-facing Orders API.
    /// Provides endpoints for creating, viewing, and cancelling orders.
    /// </summary>
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class ConsumerOrdersController : ControllerBase
    {
        private readonly ILogger<ConsumerOrdersController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IOrderService _orderService;

        public ConsumerOrdersController(
            ILogger<ConsumerOrdersController> logger,
            ApplicationDbContext dbContext,
            IOrderService orderService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _orderService = orderService;
        }

        /// <summary>
        /// Create a new order.
        /// </summary>
        /// <remarks>
        /// Route: POST api/orders
        /// Auth: Authorize
        /// </remarks>
        /// <param name="request">Order creation details</param>
        /// <returns>Created order details</returns>
        [HttpPost]
        public async Task<ActionResult<OperationResult<OrderResponse>>> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var response = new OperationResult<OrderResponse>();

            try
            {
                _logger.LogInformation("ConsumerOrdersController -> CreateOrder() -> outletId: {outletId}", request.OutletId);

                // Validate request
                if (request.Items == null || !request.Items.Any())
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Order items are required";
                    return BadRequest(response);
                }

                // Get current user
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (userId == 0)
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Unauthorized";
                    return Unauthorized(response);
                }

                var userName = User.FindFirst(ClaimTypes.Name)?.Value;

                // Verify outlet exists
                var outlet = await _dbContext.Outlets.FindAsync(request.OutletId);
                if (outlet == null || !outlet.IsActive)
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Outlet not found";
                    return NotFound(response);
                }

                // Verify delivery address belongs to user
                var deliveryAddress = await _dbContext.UserAddresses
                    .Include(ua => ua.Address)
                    .FirstOrDefaultAsync(ua => ua.Id == request.DeliveryAddressId && ua.UserId == userId);

                // Calculate order totals
                double totalPrice = 0;
                int totalItems = 0;
                var orderDetails = new List<OrderDetail>();

                foreach (var item in request.Items)
                {
                    var variant = await _dbContext.ProductVariants
                        .Include(v => v.Product)
                        .FirstOrDefaultAsync(v => v.ID == item.VariantId && v.ProductID == item.ProductId);

                    if (variant == null)
                    {
                        response.Status = OperationStatus.ERROR;
                        response.Message = $"Product variant not found: {item.VariantId}";
                        return BadRequest(response);
                    }

                    var lineTotal = variant.SalePrice * item.Quantity;
                    totalPrice += lineTotal;
                    totalItems += item.Quantity;

                    orderDetails.Add(new OrderDetail
                    {
                        VariantID = item.VariantId,
                        Quantity = item.Quantity,
                        Rate = variant.SalePrice,
                        TotalPrice = lineTotal,
                        IsActive = true,
                        CreateDate = DateTime.UtcNow,
                        CreateBy = userName,
                        ModifiedDate = DateTime.UtcNow,
                        ModifiedBy = userName
                    });
                }

                // Get default order status
                var pendingStatus = await _dbContext.OrderStatusOptions
                    .FirstOrDefaultAsync(s => s.OrderStatus == "Pending" || s.OrderStatus == "confirmed");
                
                var statusId = pendingStatus?.Id ?? 1;

                // Get default order type (online)
                var onlineOrderType = await _dbContext.OrderTypesOptions
                    .FirstOrDefaultAsync(t => t.Ordertype == "Online");
                var orderTypeId = onlineOrderType?.Id ?? 1;

                // Create order
                var order = new Order
                {
                    PersonID = userId,
                    OutletID = request.OutletId,
                    OrderTypeId = orderTypeId,
                    TotalPrice = totalPrice,
                    TotalItems = totalItems,
                    OrderAt = DateTime.UtcNow,
                    StatusId = statusId,
                    PaymentMode = request.PaymentMethod,
                    AddressID = deliveryAddress?.Address?.Id,
                    OrderDetails = orderDetails,
                    IsActive = true,
                    CreateDate = DateTime.UtcNow,
                    CreateBy = userName,
                    ModifiedDate = DateTime.UtcNow,
                    ModifiedBy = userName
                };

                await _dbContext.Orders.AddAsync(order);
                await _dbContext.SaveChangesAsync();

                // Map to response
                response.Message = "Order placed successfully";
                response.Data = await MapToOrderResponse(order.ID);

                _logger.LogInformation("ConsumerOrdersController -> CreateOrder() -> Order created with ID: {orderId}", order.ID);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                response.Status = OperationStatus.ERROR;
                response.Message = "An error occurred while creating order";
                response.ErrorDetail = ex.Message;
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Get all orders for the authenticated user.
        /// </summary>
        /// <remarks>
        /// Route: GET api/orders
        /// Auth: Authorize
        /// </remarks>
        /// <returns>List of orders</returns>
        [HttpGet]
        public async Task<ActionResult<OperationResult<List<OrderResponse>>>> GetOrders()
        {
            var response = new OperationResult<List<OrderResponse>>();

            try
            {
                _logger.LogInformation("ConsumerOrdersController -> GetOrders()");

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (userId == 0)
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Unauthorized";
                    return Unauthorized(response);
                }

                var orders = await _dbContext.Orders
                    .Include(o => o.Outlet)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.ProductVariants)
                            .ThenInclude(pv => pv.Product)
                    .Include(o => o.OrderStatusOptions)
                    .Include(o => o.Address)
                    .Where(o => o.PersonID == userId && o.IsActive)
                    .OrderByDescending(o => o.OrderAt)
                    .ToListAsync();

                var orderResponses = orders.Select(o => MapOrderToResponse(o)).ToList();

                response.Message = "Success";
                response.Data = orderResponses;

                _logger.LogInformation("ConsumerOrdersController -> GetOrders() -> Found {count} orders", orderResponses.Count);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders");
                response.Status = OperationStatus.ERROR;
                response.Message = "An error occurred";
                response.ErrorDetail = ex.Message;
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Get details of a specific order.
        /// </summary>
        /// <remarks>
        /// Route: GET api/orders/{orderId}
        /// Auth: Authorize
        /// </remarks>
        /// <param name="orderId">Order ID or Order Number</param>
        /// <returns>Order details</returns>
        [HttpGet("{orderId}")]
        public async Task<ActionResult<OperationResult<OrderResponse>>> GetOrderById(string orderId)
        {
            var response = new OperationResult<OrderResponse>();

            try
            {
                _logger.LogInformation("ConsumerOrdersController -> GetOrderById({orderId})", orderId);

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (userId == 0)
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Unauthorized";
                    return Unauthorized(response);
                }

                // Parse as integer ID
                Order order;
                if (int.TryParse(orderId, out int orderIdInt))
                {
                    order = await _dbContext.Orders
                        .Include(o => o.Outlet)
                        .Include(o => o.OrderDetails)
                            .ThenInclude(od => od.ProductVariants)
                                .ThenInclude(pv => pv.Product)
                        .Include(o => o.OrderStatusOptions)
                        .Include(o => o.Address)
                        .FirstOrDefaultAsync(o => o.ID == orderIdInt && o.PersonID == userId && o.IsActive);
                }
                else
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Invalid order ID";
                    return BadRequest(response);
                }

                if (order == null)
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Order not found";
                    response.ErrorDetail = $"No order found with ID: {orderId}";
                    return NotFound(response);
                }

                response.Message = "Success";
                response.Data = MapOrderToResponse(order);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order by ID: {orderId}", orderId);
                response.Status = OperationStatus.ERROR;
                response.Message = "An error occurred";
                response.ErrorDetail = ex.Message;
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Cancel an existing order.
        /// </summary>
        /// <remarks>
        /// Route: POST api/orders/{orderId}/cancel
        /// Auth: Authorize
        /// </remarks>
        /// <param name="orderId">Order ID or Order Number</param>
        /// <returns>Cancellation confirmation</returns>
        [HttpPost("{orderId}/cancel")]
        public async Task<ActionResult<OperationResult<OrderCancelResponse>>> CancelOrder(string orderId)
        {
            var response = new OperationResult<OrderCancelResponse>();

            try
            {
                _logger.LogInformation("ConsumerOrdersController -> CancelOrder({orderId})", orderId);

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (userId == 0)
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Unauthorized";
                    return Unauthorized(response);
                }

                // Parse as integer ID
                Order order;
                if (int.TryParse(orderId, out int orderIdInt))
                {
                    order = await _dbContext.Orders
                        .Include(o => o.OrderStatusOptions)
                        .FirstOrDefaultAsync(o => o.ID == orderIdInt && o.PersonID == userId && o.IsActive);
                }
                else
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Invalid order ID";
                    return BadRequest(response);
                }

                if (order == null)
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Order not found";
                    response.ErrorDetail = $"No order found with ID: {orderId}";
                    return NotFound(response);
                }

                // Check if order can be cancelled
                var nonCancellableStatuses = new[] { "delivered", "cancelled", "refunded" };
                var currentStatus = order.OrderStatusOptions?.OrderStatus?.ToLower() ?? "";
                
                if (nonCancellableStatuses.Contains(currentStatus))
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Cannot cancel order";
                    response.ErrorDetail = "Order is already delivered or cancelled";
                    return BadRequest(response);
                }

                // Get cancelled status
                var cancelledStatus = await _dbContext.OrderStatusOptions
                    .FirstOrDefaultAsync(s => s.OrderStatus.ToLower() == "cancelled");

                if (cancelledStatus != null)
                {
                    order.StatusId = cancelledStatus.Id;
                }

                order.ModifiedDate = DateTime.UtcNow;
                order.ModifiedBy = User.FindFirst(ClaimTypes.Name)?.Value;

                _dbContext.Orders.Update(order);
                await _dbContext.SaveChangesAsync();

                response.Message = "Order cancelled successfully";
                response.Data = new OrderCancelResponse
                {
                    OrderId = order.ID,
                    OrderNumber = $"ORD{order.ID:D6}",
                    Status = "cancelled",
                    ModifiedDate = DateTime.UtcNow
                };

                _logger.LogInformation("ConsumerOrdersController -> CancelOrder() -> Order cancelled: {orderId}", orderId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling order: {orderId}", orderId);
                response.Status = OperationStatus.ERROR;
                response.Message = "An error occurred";
                response.ErrorDetail = ex.Message;
                return StatusCode(500, response);
            }
        }

        #region Private Helper Methods

        private async Task<OrderResponse> MapToOrderResponse(int orderId)
        {
            var order = await _dbContext.Orders
                .Include(o => o.Outlet)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductVariants)
                        .ThenInclude(pv => pv.Product)
                .Include(o => o.OrderStatusOptions)
                .Include(o => o.Address)
                .FirstOrDefaultAsync(o => o.ID == orderId);

            return MapOrderToResponse(order);
        }

        private OrderResponse MapOrderToResponse(Order order)
        {
            if (order == null) return null;

            var items = order.OrderDetails?.Select(od => new OrderItemResponse
            {
                OrderDetailId = od.ID,
                OrderId = od.OrderID,
                ProductId = od.ProductVariants?.ProductID ?? 0,
                VariantId = od.VariantID,
                Name = $"{od.ProductVariants?.Product?.Name ?? "Product"}{(od.ProductVariants?.Attributes != null ? $" - {od.ProductVariants.Attributes}" : "")}",
                Quantity = od.Quantity,
                UnitPrice = (decimal)od.Rate,
                TotalPrice = (decimal)od.TotalPrice,
                Image = null, // Product thumbnail would need to be fetched from attachments
                Attributes = !string.IsNullOrEmpty(od.ProductVariants?.Attributes) ? new List<VariantAttribute>
                {
                    new VariantAttribute { Name = "Option", Value = od.ProductVariants.Attributes }
                } : new List<VariantAttribute>()
            }).ToList() ?? new List<OrderItemResponse>();

            OrderDeliveryAddress deliveryAddress = null;
            if (order.Address != null)
            {
                deliveryAddress = new OrderDeliveryAddress
                {
                    Id = order.Address.Id,
                    Label = order.Address.Address1,
                    AddressLine1 = order.Address.Address1,
                    City = order.Address.City,
                    State = order.Address.State,
                    Pincode = order.Address.Pincode.ToString()
                };
            }

            return new OrderResponse
            {
                Id = order.ID,
                OrderId = order.ID,
                OrderNumber = $"ORD{order.ID:D6}",
                UserId = order.PersonID,
                OutletId = order.OutletID,
                OutletName = order.Outlet?.Name ?? "",
                OrderType = "delivery",
                Items = items,
                Subtotal = (decimal)order.TotalPrice,
                DeliveryFee = 0,
                Taxes = 0,
                Discount = 0,
                Total = (decimal)order.TotalPrice,
                Status = order.OrderStatusOptions?.OrderStatus?.ToLower() ?? "pending",
                PaymentMethod = order.PaymentMode,
                PaymentStatus = "completed",
                DeliveryAddress = deliveryAddress,
                OrderDate = order.OrderAt,
                EstimatedDelivery = order.OrderAt.AddMinutes(45),
                DeliveredAt = null,
                IsActive = order.IsActive
            };
        }

        #endregion
    }
}
