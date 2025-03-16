using EpicMarket.Entities.CustomModels;
using Microsoft.AspNetCore.Http;
using System;
using EpicMarket.Admin.MVC.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace EpicMarket.Admin.MVC.Services
{
    public class SecurableService : ISecurableService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<SecurableService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        // Simple in-memory cache for permissions
        private static readonly ConcurrentDictionary<string, bool> _permissionCache = new ConcurrentDictionary<string, bool>();
        private static readonly ConcurrentDictionary<string, List<string>> _userPermissionsCache = new ConcurrentDictionary<string, List<string>>();

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

                // Create a cache key combining username and securable
                var cacheKey = $"{username}:{securable}";
                
                // Check if the permission is cached
                if (_permissionCache.TryGetValue(cacheKey, out bool hasAccess))
                {
                    return hasAccess;
                }
                
                // If not cached, check from the repository
                hasAccess = _userRepository.HasPermission(username, securable);
                
                // Cache the result
                _permissionCache[cacheKey] = hasAccess;
                
                return hasAccess;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking permission for securable '{Securable}'", securable);
                return false;
            }
        }

        public async Task<IEnumerable<string>> GetUserPermissions()
        {
            try
            {
                var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                
                if (string.IsNullOrEmpty(username))
                {
                    _logger.LogWarning("No username found when getting user permissions");
                    return new List<string>();
                }
                
                // Check if permissions are cached for this user
                if (_userPermissionsCache.TryGetValue(username, out List<string> permissions))
                {
                    return permissions;
                }
                
                // If not cached, get from repository
                var userPermissions = await _userRepository.GetUserPermissions(username);
                var permissionsList = new List<string>(userPermissions);
                
                // Cache the permissions
                _userPermissionsCache[username] = permissionsList;
                
                return permissionsList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user permissions");
                return new List<string>();
            }
        }
        
        // Method to clear the cache for a specific user
        public static void ClearUserCache(string username)
        {
            if (string.IsNullOrEmpty(username))
                return;
                
            // Remove user permissions from cache
            _userPermissionsCache.TryRemove(username, out _);
            
            // Remove individual permission entries for this user
            foreach (var key in _permissionCache.Keys)
            {
                if (key.StartsWith($"{username}:"))
                {
                    _permissionCache.TryRemove(key, out _);
                }
            }
        }
        
        // Method to clear the entire cache
        public static void ClearAllCache()
        {
            _permissionCache.Clear();
            _userPermissionsCache.Clear();
        }
    }
} 