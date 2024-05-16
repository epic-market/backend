using Azure;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
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

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService, ApplicationDbContext dbContext) : base(dbContext)
		{
            this.logger = logger;
            this.employeeService = employeeService;
    
        }


        [HttpPost("AddEmployee")]
		[Authorize(Roles = "businessOwner")]
		public async Task<ActionResult<OperationResult<AddEmployeeResult>>> Register(AddEmployeeParam addEmployeeParam)
        {
            var response = new OperationResult<AddEmployeeResult>();

            this.logger.LogInformation("Employee Controller -> Register()-> params {0}", JsonConvert.SerializeObject(new { Params = addEmployeeParam }));
            addEmployeeParam.UserID = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            var result = await employeeService.Register(addEmployeeParam,this.BusinessId);
            this.logger.LogInformation("Employee Controller -> Register()-> return {0}", JsonConvert.SerializeObject(new { Value = result }));

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
            this.logger.LogInformation("Employee Controller -> CheckEmployeeLink()-> return {0}", JsonConvert.SerializeObject(new { Value = result }));

            response.Data = result;  

            return Ok(response);
        }

        [HttpPost("CreateEmployeeAccount")]
        [AllowAnonymous] // we need to check the 
        public async Task<ActionResult<int>> CreateEmployeeAccount(EmployeeDto employeeDto)
        {

			var response = new OperationResult<int>();
			this.logger.LogInformation("Employee Controller -> CheckEmployeeLink()-> params {0}", JsonConvert.SerializeObject(new { Params = employeeDto }));
            var result = await employeeService.CreateEmployeeAccount(employeeDto);
            this.logger.LogInformation("Employee Controller -> CheckEmployeeLink()-> return {0}", JsonConvert.SerializeObject(new { Value = result }));

            response.Data = result;

            return Ok(response);
        }


        [HttpGet("GetAllEmployeesForMap")]
        public async Task<ActionResult<OperationResult<List<EmployeeMapOptionResult>>>> GetAllEmployeesForMap(int outletID)
        {
            var response = new OperationResult<List<EmployeeMapOptionResult>>();

            this.logger.LogInformation("Employee Controller -> GetAllProductForMap()-> params {0}", JsonConvert.SerializeObject(new { Params = outletID }));

            var results = await employeeService.GetAllEmployeesForMap(this.BusinessId, outletID);

            this.logger.LogInformation("Employee Controller -> GetAllProductForMap()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            response.Data = results;

            return Ok(response);
        }


        [HttpGet("GetAllEmployees")]
        public async Task<ActionResult<OperationResult<List<EmployeeResult>>>> GetAllEmployees([FromQuery]EmployeeParams employeeParams)
        {
            var response = new OperationResult<List<EmployeeResult>>();

            this.logger.LogInformation("Employee Controller -> GetAllEmployees()-> params {0}", JsonConvert.SerializeObject(new { Params = employeeParams }));

            var results = await employeeService.GetAllEmployees(employeeParams,this.BusinessId);

            this.logger.LogInformation("Employee Controller -> GetAllEmployees()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            response.Data = results;

            return Ok(response);
        }

        [HttpGet("GetEmployeeDetails")]
        public async Task<ActionResult<OperationResult<SingleEmployeeResult>>> GetEmployeeDetails(int employeeId)
        {
            var response = new OperationResult<SingleEmployeeResult>();

            this.logger.LogInformation("Employee Controller -> GetAllEmployees()-> params {0}", JsonConvert.SerializeObject(new { Params = employeeId }));

            var results = await employeeService.GetEmployeeDetails(employeeId);

            this.logger.LogInformation("Employee Controller -> GetAllEmployees()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            response.Data = results;

            return Ok(response);
        }


    }
}
