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


    public class EmployeesController : BaseApiController
    {
        private readonly ILogger<EmployeesController> logger;
        private readonly IEmployeeService employeeService;

        public EmployeesController(ILogger<EmployeesController> logger, IEmployeeService employeeService, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
		{
            this.logger = logger;
            this.employeeService = employeeService;
    
        }


        [HttpPost]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
		public async Task<ActionResult<OperationResult<AddEmployeeResult>>> Register(AddEmployeeParam addEmployeeParam)
        {
            var response = new OperationResult<AddEmployeeResult>();

            this.logger.LogInformation("Employee Controller -> Register()-> params {0}", JsonConvert.SerializeObject(new { Params = addEmployeeParam }));
            var UserID = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            var result = await employeeService.Register(addEmployeeParam,this.BusinessId, UserID);
            this.logger.LogInformation("Employee Controller -> Register()-> return {0}", JsonConvert.SerializeObject(new { Value = result }));

            response.Data = result;

            return Ok(response);
        }


        [HttpGet("Check")]
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

        [HttpPut("{id}")]
        [AllowAnonymous] // we need to check the 
        public async Task<ActionResult<int>> CreateEmployeeAccount(int id,[FromBody]EmployeeDto employeeDto)
        {

			var response = new OperationResult<int>();
			this.logger.LogInformation("Employee Controller -> CheckEmployeeLink()-> params {0}", JsonConvert.SerializeObject(new { Params = employeeDto }));
            var result = await employeeService.CreateEmployeeAccount(employeeDto,id);
            this.logger.LogInformation("Employee Controller -> CheckEmployeeLink()-> return {0}", JsonConvert.SerializeObject(new { Value = result }));
            response.Data = result;
            return Ok(response);
        }


        [HttpGet("Map/{outletID}")]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
		public async Task<ActionResult<OperationResult<List<EmployeeMapOptionResult>>>> GetAllEmployeesForMap(int outletID)
        {
            var response = new OperationResult<List<EmployeeMapOptionResult>>();

            this.logger.LogInformation("Employee Controller -> GetAllProductForMap()-> params {0}", JsonConvert.SerializeObject(new { Params = outletID }));

            var results = await employeeService.GetAllEmployeesForMap(this.BusinessId, outletID);

            this.logger.LogInformation("Employee Controller -> GetAllProductForMap()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            response.Data = results;

            return Ok(response);
        }


        [HttpGet]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
		public async Task<ActionResult<OperationResult<GetDataResult<List<EmployeeResult>>>>> GetAllEmployees([FromQuery]EmployeeParams employeeParams)
        {
            var response = new OperationResult<GetDataResult<List<EmployeeResult>>>();

            this.logger.LogInformation("Employee Controller -> GetAllEmployees()-> params {0}", JsonConvert.SerializeObject(new { Params = employeeParams }));

            var results = await employeeService.GetAllEmployees(employeeParams,this.BusinessId);

            this.logger.LogInformation("Employee Controller -> GetAllEmployees()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            response.Data = results;

            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = ROLES.BUSINESS_OWNER)]
        public async Task<ActionResult<OperationResult<SingleEmployeeResult>>> GetEmployeeDetails( int id)

		{
            var response = new OperationResult<SingleEmployeeResult>();

            this.logger.LogInformation("Employee Controller -> GetAllEmployees()-> params {0}", JsonConvert.SerializeObject(new { Params = id }));

            var results = await employeeService.GetEmployeeDetails(id);

            this.logger.LogInformation("Employee Controller -> GetAllEmployees()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            response.Data = results;

            return Ok(response);
        }



		[HttpDelete("{id}")]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
		public async Task<ActionResult> Delete(int id)
		{
			var response = new OperationResult<bool>();

			this.logger.LogInformation("Employee Controller -> GetAllEmployees()-> params {0}", JsonConvert.SerializeObject(new { Params = id }));

            await employeeService.DeleteEmployee(id);

            response.Data = true;

			return Ok(response);
		}


	}
}
