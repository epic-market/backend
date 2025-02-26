using EpicMarket.Entities.CustomModels;
using Microsoft.AspNetCore.Http;
using System;
using EpicMarket.Admin.MVC.Services;
using Microsoft.Extensions.Logging;

namespace EpicMarket.Admin.MVC.Services
{
    public interface ISecurableService
    {
        bool HasAccess(string securable);
    }

    public class SecurableService : ISecurableService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<SecurableService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SecurableService(
            IUserRepository userRepository,
            ILogger<SecurableService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public bool HasAccess(string securable)
        {
            if (string.IsNullOrEmpty(securable))
            {
                _logger.LogWarning("Empty securable name provided");
                return false;
            }

            try
            {
                var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                
                if (string.IsNullOrEmpty(username))
                {
                    _logger.LogWarning("No username found when checking securable '{Securable}'", securable);
                    return false;
                }

                return _userRepository.HasPermission(username, securable);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking permission for securable '{Securable}'", securable);
                return false;
            }
        }
    }
} 