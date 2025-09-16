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
    /// <summary>
    /// Manages employee operations including registration, account creation, and employee-outlet mappings
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class EmployeesController : BaseApiController
    {
        private readonly ILogger<EmployeesController> logger;
        private readonly IEmployeeService employeeService;

        public EmployeesController(ILogger<EmployeesController> logger, IEmployeeService employeeService, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
		{
            this.logger = logger;
            this.employeeService = employeeService;
    
        }


        /// <summary>
        /// Registers a new employee and sends invitation link
        /// </summary>
        /// <param name="addEmployeeParam">Employee information including email and initial details</param>
        /// <returns>Employee registration result with invitation link details</returns>
        /// <response code="200">Employee successfully registered and invitation sent</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not a business owner</response>
        /// <response code="400">Invalid employee data or email already exists</response>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/employees
        ///     {
        ///        "email": "employee@example.com",
        ///        "firstName": "John",
        ///        "lastName": "Doe",
        ///        "role": "Manager",
        ///        "department": "Sales"
        ///     }
        /// </remarks>
        [HttpPost]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
        [ProducesResponseType(typeof(OperationResult<AddEmployeeResult>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
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


        /// <summary>
        /// Validates an employee invitation link
        /// </summary>
        /// <param name="queryParam">Encrypted token from invitation email</param>
        /// <returns>Link validation status and employee information</returns>
        /// <response code="200">Returns link validation result</response>
        /// <remarks>
        /// This endpoint is called when an employee clicks on their invitation link.
        /// It validates the token and returns employee details for account creation.
        /// </remarks>
        [HttpGet("Check")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(OperationResult<CheckLinkResult>), 200)]
        public async Task<ActionResult<OperationResult<CheckLinkResult>>> CheckEmployeeLink(string queryParam)
        {
            var response = new OperationResult<CheckLinkResult>();
			this.logger.LogInformation("Employee Controller -> CheckEmployeeLink()-> params {0}", JsonConvert.SerializeObject(new { Params = queryParam }));
            var result =  employeeService.CheckEmployeeLink(queryParam);
            this.logger.LogInformation("Employee Controller -> CheckEmployeeLink()-> return {0}", JsonConvert.SerializeObject(new { Value = result }));
            response.Data = result;  
            return Ok(response);
        }

        /// <summary>
        /// Creates an employee account after invitation acceptance
        /// </summary>
        /// <param name="id">Employee ID from the invitation link</param>
        /// <param name="employeeDto">Employee account details including password</param>
        /// <returns>Created employee account ID</returns>
        /// <response code="200">Employee account successfully created</response>
        /// <response code="400">Invalid employee data or ID</response>
        /// <response code="404">Employee invitation not found or expired</response>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /api/employees/123
        ///     {
        ///        "password": "SecurePassword123!",
        ///        "confirmPassword": "SecurePassword123!",
        ///        "phone": "+1234567890",
        ///        "address": "456 Employee St"
        ///     }
        /// </remarks>
        [HttpPut("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(OperationResult<int>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<int>> CreateEmployeeAccount(int id,[FromBody]EmployeeDto employeeDto)
        {

			var response = new OperationResult<int>();
			this.logger.LogInformation("Employee Controller -> CheckEmployeeLink()-> params {0}", JsonConvert.SerializeObject(new { Params = employeeDto }));
            var result = await employeeService.CreateEmployeeAccount(employeeDto,id);
            this.logger.LogInformation("Employee Controller -> CheckEmployeeLink()-> return {0}", JsonConvert.SerializeObject(new { Value = result }));
            response.Data = result;
            return Ok(response);
        }


        /// <summary>
        /// Retrieves employees available for outlet mapping
        /// </summary>
        /// <param name="outletID">The outlet ID to check existing mappings</param>
        /// <returns>List of employees with their mapping status</returns>
        /// <response code="200">Returns list of employees with mapping options</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not a business owner</response>
        /// <remarks>
        /// Returns all business employees with indicators showing:
        /// - Already mapped to the specified outlet
        /// - Available for mapping
        /// - Mapped to other outlets
        /// </remarks>
        [HttpGet("Map/{outletID}")]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
        [ProducesResponseType(typeof(OperationResult<List<EmployeeMapOptionResult>>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
		public async Task<ActionResult<OperationResult<List<EmployeeMapOptionResult>>>> GetAllEmployeesForMap(int outletID)
        {
            var response = new OperationResult<List<EmployeeMapOptionResult>>();

            this.logger.LogInformation("Employee Controller -> GetAllProductForMap()-> params {0}", JsonConvert.SerializeObject(new { Params = outletID }));

            var results = await employeeService.GetAllEmployeesForMap(this.BusinessId, outletID);

            this.logger.LogInformation("Employee Controller -> GetAllProductForMap()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            response.Data = results;

            return Ok(response);
        }


        /// <summary>
        /// Retrieves all employees for the current business with pagination
        /// </summary>
        /// <param name="employeeParams">Filter and pagination parameters</param>
        /// <returns>Paginated list of employees</returns>
        /// <response code="200">Returns paginated employee list</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not a business owner</response>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/employees?pageNumber=1&amp;pageSize=10&amp;searchTerm=john&amp;status=active
        /// </remarks>
        [HttpGet]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
        [ProducesResponseType(typeof(OperationResult<GetDataResult<List<EmployeeResult>>>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
		public async Task<ActionResult<OperationResult<GetDataResult<List<EmployeeResult>>>>> GetAllEmployees([FromQuery]EmployeeParams employeeParams)
        {
            var response = new OperationResult<GetDataResult<List<EmployeeResult>>>();

            this.logger.LogInformation("Employee Controller -> GetAllEmployees()-> params {0}", JsonConvert.SerializeObject(new { Params = employeeParams }));

            var results = await employeeService.GetAllEmployees(employeeParams,this.BusinessId);

            this.logger.LogInformation("Employee Controller -> GetAllEmployees()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            response.Data = results;

            return Ok(response);
        }

        /// <summary>
        /// Retrieves detailed information for a specific employee
        /// </summary>
        /// <param name="id">The employee ID</param>
        /// <returns>Complete employee details including roles and assignments</returns>
        /// <response code="200">Returns employee details</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not a business owner</response>
        /// <response code="404">Employee not found</response>
        [HttpGet("{id}")]
        [Authorize(Roles = ROLES.BUSINESS_OWNER)]
        [ProducesResponseType(typeof(OperationResult<SingleEmployeeResult>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OperationResult<SingleEmployeeResult>>> GetEmployeeDetails( int id)

		{
            var response = new OperationResult<SingleEmployeeResult>();

            this.logger.LogInformation("Employee Controller -> GetAllEmployees()-> params {0}", JsonConvert.SerializeObject(new { Params = id }));

            var results = await employeeService.GetEmployeeDetails(id);

            this.logger.LogInformation("Employee Controller -> GetAllEmployees()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            response.Data = results;

            return Ok(response);
        }



		/// <summary>
        /// Deletes an employee from the business
        /// </summary>
        /// <param name="id">The employee ID to delete</param>
        /// <returns>Success status</returns>
        /// <response code="200">Employee successfully deleted</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not a business owner</response>
        /// <response code="404">Employee not found</response>
        /// <remarks>
        /// This action will:
        /// - Remove the employee from all outlet mappings
        /// - Deactivate the employee account
        /// - Preserve historical data for audit purposes
        /// </remarks>
        [HttpDelete("{id}")]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
        [ProducesResponseType(typeof(OperationResult<bool>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
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
