using EpicMarket.Admin.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EpicMarket.Admin.MVC.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SecurablesController : ControllerBase
    {
        private readonly ISecurableService _securableService;

        public SecurablesController(ISecurableService securableService)
        {
            _securableService = securableService;
        }

        /// <summary>
        /// Gets the list of securables that the current user has access to
        /// </summary>
        /// <returns>List of securable names</returns>
        [HttpGet("GetUserPermissions")]
        public async Task<ActionResult<IEnumerable<string>>> GetUserPermissions()
        {
            var permissions = await _securableService.GetUserPermissions();
            return Ok(permissions);
        }
    }
} 