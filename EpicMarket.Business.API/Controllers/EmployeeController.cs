using EpicMarket.Contracts;
using EpicMarket.Entities;
using EpicMarket.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Security.Claims;

namespace EpicMarket.Business.API.Controllers
{

    public class EmployeeController : BaseApiController
    {
        private readonly ILogger<EmployeeController> logger;
        private readonly IEmployeeService employeeService;


        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            this.logger = logger;
            this.employeeService = employeeService;
    
        }


        [HttpPost("AddEmployee")]
        [AllowAnonymous]
        public async Task<ActionResult<int>> Register(AddEmployeeParam addEmployeeParam)
        {
            this.logger.LogInformation("Employee Controller -> Register()-> params {0}", JsonConvert.SerializeObject(new { Params = addEmployeeParam }));
            addEmployeeParam.UserID = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            var id = await employeeService.Register(addEmployeeParam);
            this.logger.LogInformation("Business Controller -> Register()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));

            return Ok(id);
        }


        [HttpGet("CheckEmployeeLink")]
        [AllowAnonymous]
        public async Task<ActionResult<int>> CheckEmployeeLink(string queryParam)
        {
            this.logger.LogInformation("Employee Controller -> CheckEmployeeLink()-> params {0}", JsonConvert.SerializeObject(new { Params = queryParam }));
            var id =  employeeService.CheckEmployeeLink(queryParam);
            this.logger.LogInformation("Business Controller -> CheckEmployeeLink()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));

            return Ok(id);
        }

        [HttpPost("CreateEmployeeAccount")]
        [AllowAnonymous]
        public async Task<ActionResult<int>> CreateEmployeeAccount(EmployeeDto employeeDto)
        {
            this.logger.LogInformation("Employee Controller -> CheckEmployeeLink()-> params {0}", JsonConvert.SerializeObject(new { Params = employeeDto }));
            var id = await employeeService.CreateEmployeeAccount(employeeDto);
            this.logger.LogInformation("Business Controller -> CheckEmployeeLink()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));
            return Ok(id);
        }


    }
}
