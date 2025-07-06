using EpicMarket.Business.API.Controllers;
using EpicMarket.Business.API.Tests.TestHelpers;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EpicMarket.Business.API.Tests.ControllerTests
{
    public class OrderControllerTests : BaseControllerTest<OrderController>
    {
        [Fact]
        public void Setup_OrderController()
        {
            // Arrange & Act
            Setup();
            Controller = new OrderController(
                MockLogger.Object,
                MockOrderService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            // Assert
            Controller.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAllOrders_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var orderParams = new OrderListParams
            {
                Page = 1,
                PageSize = 10,
                Status = "pending",
                DateFrom = DateTime.Now.AddDays(-30),
                DateTo = DateTime.Now
            };

            var orderResults = TestDataHelper.GetTestOrderResults();

            MockOrderService.Setup(x => x.GetAllOrders(It.IsAny<OrderListParams>(), It.IsAny<int>()))
                .ReturnsAsync(orderResults);

            Controller = new OrderController(
                MockLogger.Object,
                MockOrderService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.GetAllOrders(orderParams);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<GetDataResult<List<OrderResult>>>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.Data.Should().HaveCount(1);
            response.Data.Data[0].CustomerName.Should().Be("Test Customer");
        }

        [Fact]
        public async Task GetOrderDetails_WithValidId_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var orderId = 1;
            var orderDetails = new OrderDetailsResult
            {
                OrderId = orderId,
                CustomerName = "Test Customer",
                CustomerPhone = "+1234567890",
                CustomerEmail = "customer@example.com",
                OrderDate = DateTime.Now.AddDays(-1),
                Status = "Pending",
                TotalAmount = 99.99m,
                DeliveryAddress = "123 Customer St",
                Items = new List<OrderItemResult>
                {
                    new OrderItemResult
                    {
                        ProductId = 1,
                        ProductName = "Test Product",
                        VariantName = "Red",
                        Quantity = 2,
                        UnitPrice = 29.99m,
                        TotalPrice = 59.98m
                    }
                },
                PaymentMethod = "Credit Card",
                Notes = "Special delivery instructions"
            };

            MockOrderService.Setup(x => x.GetOrderDetails(orderId))
                .ReturnsAsync(orderDetails);

            Controller = new OrderController(
                MockLogger.Object,
                MockOrderService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.GetOrderDetails(orderId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<OrderDetailsResult>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.OrderId.Should().Be(orderId);
            response.Data.CustomerName.Should().Be("Test Customer");
            response.Data.Items.Should().HaveCount(1);
        }

        [Fact]
        public async Task CreateOrder_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var orderDto = TestDataHelper.GetValidOrderDto();

            MockOrderService.Setup(x => x.CreateOrder(
                It.IsAny<OrderDto>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<string>()))
                .ReturnsAsync(1);

            Controller = new OrderController(
                MockLogger.Object,
                MockOrderService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.CreateOrder(orderDto);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<int>;
            response.Should().NotBeNull();
            response.Data.Should().Be(1);
        }

        [Fact]
        public async Task UpdateOrderStatus_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var orderId = 1;
            var orderStatusId = 2;

            MockOrderService.Setup(x => x.UpdateOrderStatus(orderId, orderStatusId, It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            Controller = new OrderController(
                MockLogger.Object,
                MockOrderService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.UpdateOrderStatus(orderId, orderStatusId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<bool>;
            response.Should().NotBeNull();
            response.Data.Should().BeTrue();
        }

        [Fact]
        public async Task CheckNewOrders_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var orderedAfter = DateTime.Now.AddHours(-1);
            var outletId = 123;

            var newOrders = new List<OrderResult>
            {
                new OrderResult
                {
                    OrderId = 1,
                    CustomerName = "New Customer",
                    CustomerPhone = "+1234567890",
                    OrderDate = DateTime.Now.AddMinutes(-30),
                    Status = "New",
                    TotalAmount = 49.99m
                }
            };

            MockOrderService.Setup(x => x.CheckNewOrders(orderedAfter, outletId))
                .ReturnsAsync(newOrders);

            Controller = new OrderController(
                MockLogger.Object,
                MockOrderService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.CheckNewOrders(orderedAfter, outletId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<List<OrderResult>>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.Should().HaveCount(1);
            response.Data[0].CustomerName.Should().Be("New Customer");
        }

        [Fact]
        public async Task GetOrdersForMobile_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var mobileParams = new OrderMobileParams
            {
                Page = 1,
                PageSize = 10,
                Status = "pending"
            };

            var mobileOrders = new GetDataResult<List<OrderMobileResult>>
            {
                Data = new List<OrderMobileResult>
                {
                    new OrderMobileResult
                    {
                        OrderId = 1,
                        CustomerName = "Mobile Customer",
                        OrderDate = DateTime.Now.AddHours(-2),
                        Status = "Pending",
                        TotalAmount = 39.99m,
                        ItemCount = 2
                    }
                },
                TotalCount = 1,
                Page = 1,
                PageSize = 10
            };

            MockOrderService.Setup(x => x.GetOrdersForMobile(mobileParams, It.IsAny<int>()))
                .ReturnsAsync(mobileOrders);

            Controller = new OrderController(
                MockLogger.Object,
                MockOrderService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.GetOrdersForMobile(mobileParams);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<GetDataResult<List<OrderMobileResult>>>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.Data.Should().HaveCount(1);
            response.Data.Data[0].CustomerName.Should().Be("Mobile Customer");
        }

        [Fact]
        public async Task GetMobileOrderDetails_WithValidId_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var orderId = 1;
            var mobileOrderDetails = new OrderMobileDetailsResult
            {
                OrderId = orderId,
                CustomerName = "Mobile Customer",
                CustomerPhone = "+1234567890",
                OrderDate = DateTime.Now.AddHours(-2),
                Status = "Pending",
                TotalAmount = 39.99m,
                Items = new List<OrderItemResult>
                {
                    new OrderItemResult
                    {
                        ProductName = "Mobile Product",
                        Quantity = 1,
                        UnitPrice = 39.99m,
                        TotalPrice = 39.99m
                    }
                }
            };

            MockOrderService.Setup(x => x.GetMobileOrderDetails(orderId))
                .ReturnsAsync(mobileOrderDetails);

            Controller = new OrderController(
                MockLogger.Object,
                MockOrderService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.GetMobileOrderDetails(orderId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<OrderMobileDetailsResult>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.OrderId.Should().Be(orderId);
            response.Data.CustomerName.Should().Be("Mobile Customer");
        }

        [Fact]
        public async Task CreateCustomerOrder_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var customerOrderDto = new CustomerOrderDto
            {
                OutletId = 123,
                Items = new List<CustomerOrderItemDto>
                {
                    new CustomerOrderItemDto
                    {
                        ProductVariantId = 1,
                        Quantity = 2
                    }
                },
                DeliveryAddress = "123 Customer St",
                Notes = "Customer special instructions"
            };

            MockOrderService.Setup(x => x.CreateCustomerOrder(
                It.IsAny<CustomerOrderDto>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
                .ReturnsAsync(1);

            Controller = new OrderController(
                MockLogger.Object,
                MockOrderService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            SetupMemberContext(Controller);

            // Act
            var result = await Controller.CreateCustomerOrder(customerOrderDto);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<int>;
            response.Should().NotBeNull();
            response.Data.Should().Be(1);
        }

        [Fact]
        public async Task GetCustomerOrderHistory_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var historyParams = new CustomerOrderHistoryParams
            {
                Page = 1,
                PageSize = 10,
                SortBy = "date",
                SortOrder = "desc"
            };

            var orderHistory = new GetDataResult<List<CustomerOrderHistoryResult>>
            {
                Data = new List<CustomerOrderHistoryResult>
                {
                    new CustomerOrderHistoryResult
                    {
                        OrderId = 1,
                        BusinessName = "Test Restaurant",
                        OutletName = "Main Branch",
                        OrderDate = DateTime.Now.AddDays(-2),
                        Status = "Delivered",
                        TotalAmount = 59.99m,
                        ItemCount = 3
                    }
                },
                TotalCount = 1,
                Page = 1,
                PageSize = 10
            };

            MockOrderService.Setup(x => x.GetCustomerOrderHistory(historyParams, It.IsAny<string>()))
                .ReturnsAsync(orderHistory);

            Controller = new OrderController(
                MockLogger.Object,
                MockOrderService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            SetupMemberContext(Controller);

            // Act
            var result = await Controller.GetCustomerOrderHistory(historyParams);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<GetDataResult<List<CustomerOrderHistoryResult>>>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.Data.Should().HaveCount(1);
            response.Data.Data[0].BusinessName.Should().Be("Test Restaurant");
        }

        [Fact]
        public async Task CreateOrder_WithNullData_ReturnsBadRequest()
        {
            // Arrange
            Setup();
            Controller = new OrderController(
                MockLogger.Object,
                MockOrderService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.CreateOrder(null);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task UpdateOrderStatus_WithInvalidOrderId_ReturnsNotFound()
        {
            // Arrange
            Setup();
            var orderId = 999; // Non-existent order
            var orderStatusId = 2;

            MockOrderService.Setup(x => x.UpdateOrderStatus(orderId, orderStatusId, It.IsAny<string>()))
                .ThrowsAsync(new ArgumentException("Order not found"));

            Controller = new OrderController(
                MockLogger.Object,
                MockOrderService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            SetupBusinessOwnerContext(Controller);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                Controller.UpdateOrderStatus(orderId, orderStatusId));
        }

        [Fact]
        public async Task GetOrderDetails_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            Setup();
            var orderId = 999; // Non-existent order

            MockOrderService.Setup(x => x.GetOrderDetails(orderId))
                .ReturnsAsync((OrderDetailsResult)null);

            Controller = new OrderController(
                MockLogger.Object,
                MockOrderService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.GetOrderDetails(orderId);

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task CheckNewOrders_WithNoNewOrders_ReturnsEmptyList()
        {
            // Arrange
            Setup();
            var orderedAfter = DateTime.Now.AddHours(-1);
            var outletId = 123;

            MockOrderService.Setup(x => x.CheckNewOrders(orderedAfter, outletId))
                .ReturnsAsync(new List<OrderResult>());

            Controller = new OrderController(
                MockLogger.Object,
                MockOrderService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.CheckNewOrders(orderedAfter, outletId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<List<OrderResult>>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.Should().BeEmpty();
        }

        protected override void Cleanup()
        {
            base.Cleanup();
        }
    }
}