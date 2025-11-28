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
    /// User Address management API.
    /// Provides CRUD operations for user addresses.
    /// </summary>
    [Route("api/user/addresses")]
    [ApiController]
    [Authorize]
    public class UserAddressController : ControllerBase
    {
        private readonly ILogger<UserAddressController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public UserAddressController(
            ILogger<UserAddressController> logger,
            ApplicationDbContext dbContext,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get all addresses for the authenticated user.
        /// </summary>
        /// <remarks>
        /// Route: GET api/user/addresses
        /// Auth: Authorize
        /// </remarks>
        /// <returns>List of user addresses</returns>
        [HttpGet]
        public async Task<ActionResult<OperationResult<List<UserAddressResponse>>>> GetUserAddresses()
        {
            var response = new OperationResult<List<UserAddressResponse>>();

            try
            {
                _logger.LogInformation("UserAddressController -> GetUserAddresses()");

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (userId == 0)
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Unauthorized";
                    return Unauthorized(response);
                }

                var userAddresses = await _dbContext.UserAddresses
                    .Include(ua => ua.Address)
                    .Where(ua => ua.UserId == userId)
                    .ToListAsync();

                var addressResponses = userAddresses.Select(ua => MapToAddressResponse(ua, userId)).ToList();

                // Ensure at least one is marked as default
                if (addressResponses.Any() && !addressResponses.Any(a => a.IsDefault))
                {
                    addressResponses[0].IsDefault = true;
                }

                response.Message = "Success";
                response.Data = addressResponses;

                _logger.LogInformation("UserAddressController -> GetUserAddresses() -> Found {count} addresses", addressResponses.Count);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user addresses");
                response.Status = OperationStatus.ERROR;
                response.Message = "An error occurred";
                response.ErrorDetail = ex.Message;
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Add a new address for the authenticated user.
        /// </summary>
        /// <remarks>
        /// Route: POST api/user/addresses
        /// Auth: Authorize
        /// </remarks>
        /// <param name="request">Address details</param>
        /// <returns>Created address</returns>
        [HttpPost]
        public async Task<ActionResult<OperationResult<UserAddressResponse>>> AddUserAddress([FromBody] AddUserAddressRequest request)
        {
            var response = new OperationResult<UserAddressResponse>();

            try
            {
                _logger.LogInformation("UserAddressController -> AddUserAddress()");

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userName = User.FindFirst(ClaimTypes.Name)?.Value;

                if (userId == 0)
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Unauthorized";
                    return Unauthorized(response);
                }

                // Parse pincode
                int.TryParse(request.Pincode, out int pincode);

                // Create address
                var address = new Address
                {
                    Address1 = request.AddressLine1,
                    Address2 = request.AddressLine2,
                    City = request.City,
                    State = request.State,
                    Pincode = pincode,
                    Latitude = 0,
                    Longitude = 0,
                    CreateDate = DateTime.UtcNow,
                    CreateBy = userName,
                    ModifiedDate = DateTime.UtcNow,
                    ModifiedBy = userName,
                    IsActive = true
                };

                await _dbContext.Addresses.AddAsync(address);
                await _dbContext.SaveChangesAsync();

                // If this is the default address, unset other defaults
                if (request.IsDefault)
                {
                    var existingDefaults = await _dbContext.UserAddresses
                        .Where(ua => ua.UserId == userId)
                        .ToListAsync();
                    // Note: We would need to add IsDefault to UserAddress model
                    // For now, we track this in the response
                }

                // Create user address link
                var userAddress = new UserAddress
                {
                    UserId = userId,
                    AddressId = address.Id
                };

                await _dbContext.UserAddresses.AddAsync(userAddress);
                await _dbContext.SaveChangesAsync();

                response.Message = "Address added successfully";
                response.Data = new UserAddressResponse
                {
                    Id = userAddress.Id,
                    UserId = userId,
                    Label = request.Label,
                    AddressLine1 = request.AddressLine1,
                    AddressLine2 = request.AddressLine2,
                    Landmark = request.Landmark,
                    City = request.City,
                    State = request.State,
                    Pincode = request.Pincode,
                    Country = "India",
                    Latitude = null,
                    Longitude = null,
                    IsDefault = request.IsDefault,
                    AddressType = request.AddressType ?? "Home",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                };

                _logger.LogInformation("UserAddressController -> AddUserAddress() -> Address added: {id}", userAddress.Id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user address");
                response.Status = OperationStatus.ERROR;
                response.Message = "An error occurred";
                response.ErrorDetail = ex.Message;
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Update an existing address.
        /// </summary>
        /// <remarks>
        /// Route: PUT api/user/addresses/{addressId}
        /// Auth: Authorize
        /// </remarks>
        /// <param name="addressId">Address ID</param>
        /// <param name="request">Updated address details</param>
        /// <returns>Updated address</returns>
        [HttpPut("{addressId}")]
        public async Task<ActionResult<OperationResult<UserAddressResponse>>> UpdateUserAddress(int addressId, [FromBody] UpdateUserAddressRequest request)
        {
            var response = new OperationResult<UserAddressResponse>();

            try
            {
                _logger.LogInformation("UserAddressController -> UpdateUserAddress({addressId})", addressId);

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userName = User.FindFirst(ClaimTypes.Name)?.Value;

                if (userId == 0)
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Unauthorized";
                    return Unauthorized(response);
                }

                // Find user address
                var userAddress = await _dbContext.UserAddresses
                    .Include(ua => ua.Address)
                    .FirstOrDefaultAsync(ua => ua.Id == addressId && ua.UserId == userId);

                if (userAddress == null)
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Address not found";
                    response.ErrorDetail = $"No address found with ID: {addressId}";
                    return NotFound(response);
                }

                // Update address fields
                var address = userAddress.Address;
                if (!string.IsNullOrWhiteSpace(request.AddressLine1))
                    address.Address1 = request.AddressLine1;
                if (!string.IsNullOrWhiteSpace(request.AddressLine2))
                    address.Address2 = request.AddressLine2;
                if (!string.IsNullOrWhiteSpace(request.City))
                    address.City = request.City;
                if (!string.IsNullOrWhiteSpace(request.State))
                    address.State = request.State;
                if (!string.IsNullOrWhiteSpace(request.Pincode) && int.TryParse(request.Pincode, out int pincode))
                    address.Pincode = pincode;

                address.ModifiedDate = DateTime.UtcNow;
                address.ModifiedBy = userName;

                _dbContext.Addresses.Update(address);
                await _dbContext.SaveChangesAsync();

                response.Message = "Address updated successfully";
                response.Data = new UserAddressResponse
                {
                    Id = userAddress.Id,
                    UserId = userId,
                    Label = request.Label ?? address.Address1,
                    AddressLine1 = address.Address1,
                    AddressLine2 = address.Address2,
                    Landmark = request.Landmark,
                    City = address.City,
                    State = address.State,
                    Pincode = address.Pincode.ToString(),
                    Country = "India",
                    Latitude = address.Latitude,
                    Longitude = address.Longitude,
                    IsDefault = request.IsDefault ?? false,
                    AddressType = request.AddressType ?? "Home",
                    IsActive = true,
                    ModifiedDate = DateTime.UtcNow
                };

                _logger.LogInformation("UserAddressController -> UpdateUserAddress() -> Address updated: {id}", addressId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user address: {addressId}", addressId);
                response.Status = OperationStatus.ERROR;
                response.Message = "An error occurred";
                response.ErrorDetail = ex.Message;
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Delete an existing address.
        /// </summary>
        /// <remarks>
        /// Route: DELETE api/user/addresses/{addressId}
        /// Auth: Authorize
        /// </remarks>
        /// <param name="addressId">Address ID</param>
        /// <returns>Deletion confirmation</returns>
        [HttpDelete("{addressId}")]
        public async Task<ActionResult<OperationResult<bool>>> DeleteUserAddress(int addressId)
        {
            var response = new OperationResult<bool>();

            try
            {
                _logger.LogInformation("UserAddressController -> DeleteUserAddress({addressId})", addressId);

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                if (userId == 0)
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Unauthorized";
                    return Unauthorized(response);
                }

                // Find user address
                var userAddress = await _dbContext.UserAddresses
                    .Include(ua => ua.Address)
                    .FirstOrDefaultAsync(ua => ua.Id == addressId && ua.UserId == userId);

                if (userAddress == null)
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Address not found";
                    response.ErrorDetail = $"No address found with ID: {addressId}";
                    return NotFound(response);
                }

                // Soft delete - mark as inactive
                if (userAddress.Address != null)
                {
                    userAddress.Address.IsActive = false;
                    _dbContext.Addresses.Update(userAddress.Address);
                }

                // Remove the user address link
                _dbContext.UserAddresses.Remove(userAddress);
                await _dbContext.SaveChangesAsync();

                response.Message = "Address deleted successfully";
                response.Data = true;

                _logger.LogInformation("UserAddressController -> DeleteUserAddress() -> Address deleted: {id}", addressId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user address: {addressId}", addressId);
                response.Status = OperationStatus.ERROR;
                response.Message = "An error occurred";
                response.ErrorDetail = ex.Message;
                return StatusCode(500, response);
            }
        }

        #region Private Helper Methods

        private UserAddressResponse MapToAddressResponse(UserAddress userAddress, int userId)
        {
            var address = userAddress.Address;
            return new UserAddressResponse
            {
                Id = userAddress.Id,
                UserId = userId,
                Label = address?.Address1 ?? "",
                AddressLine1 = address?.Address1 ?? "",
                AddressLine2 = address?.Address2 ?? "",
                Landmark = null,
                City = address?.City ?? "",
                State = address?.State ?? "",
                Pincode = address?.Pincode.ToString() ?? "",
                Country = "India",
                Latitude = address?.Latitude,
                Longitude = address?.Longitude,
                IsDefault = false, // Would need to track this in database
                AddressType = "Home",
                IsActive = address?.IsActive ?? true,
                CreatedDate = address?.CreateDate ?? DateTime.UtcNow,
                ModifiedDate = address?.ModifiedDate ?? DateTime.UtcNow
            };
        }

        #endregion
    }
}
