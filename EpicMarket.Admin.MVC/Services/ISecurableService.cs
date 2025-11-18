using System.Collections.Generic;
using System.Threading.Tasks;

namespace EpicMarket.Admin.MVC.Services
{
    /// <summary>
    /// Service for checking user permissions against securables
    /// </summary>
    public interface ISecurableService
    {
        /// <summary>
        /// Checks if the current user has access to the specified securable
        /// </summary>
        /// <param name="securable">The securable to check</param>
        /// <returns>True if the user has access, false otherwise</returns>
        bool HasAccess(string securable);

        /// <summary>
        /// Gets the list of securables that the current user has access to
        /// </summary>
        /// <returns>List of securable names</returns>
        Task<IEnumerable<string>> GetUserPermissions();
    }
} 