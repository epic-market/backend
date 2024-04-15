using Azure;
using EpicMarket.Contracts;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
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
        public async Task<ActionResult<OperationResult<AddEmployeeResult>>> Register(AddEmployeeParam addEmployeeParam)
        {
            var response = new OperationResult<AddEmployeeResult>();

            this.logger.LogInformation("Employee Controller -> Register()-> params {0}", JsonConvert.SerializeObject(new { Params = addEmployeeParam }));
            addEmployeeParam.UserID = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            var result = await employeeService.Register(addEmployeeParam);
            this.logger.LogInformation("Business Controller -> Register()-> return {0}", JsonConvert.SerializeObject(new { Value = result }));

            response.Data = result;

            return Ok(response);
        }


        [HttpGet("CheckEmployeeLink")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<CheckLinkResult>>> CheckEmployeeLink(string queryParam)
        {
            var response = new OperationResult<CheckLinkResult>();


			this.logger.LogInformation("Employee Controller -> CheckEmployeeLink()-> params {0}", JsonConvert.SerializeObject(new { Params = queryParam }));
            var result =  employeeService.CheckEmployeeLink(queryParam);
            this.logger.LogInformation("Business Controller -> CheckEmployeeLink()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));

            response.Data = result;  

            return Ok(response);
        }

        [HttpPost("CreateEmployeeAccount")]
        [AllowAnonymous]
        public async Task<ActionResult<int>> CreateEmployeeAccount(EmployeeDto employeeDto)
        {

			var response = new OperationResult<int>();


			this.logger.LogInformation("Employee Controller -> CheckEmployeeLink()-> params {0}", JsonConvert.SerializeObject(new { Params = employeeDto }));
            var result = await employeeService.CreateEmployeeAccount(employeeDto);
            this.logger.LogInformation("Business Controller -> CheckEmployeeLink()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));

            response.Data = result;

            return Ok(response);
        }


    }
}
