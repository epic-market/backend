using EpicMarket.Entities.CustomModels;

namespace EpicMarket.Business.API.Errors
{
    public class ApiException : OperationResult<string>
    {
        public ApiException(int statusCode, string message = null, string details = null)
        {
            StatusCode = statusCode;
            Message = message;
            ErrorDetail = details;
			Status = OperationStatus.ERROR;
            Data = "";
		}

        public int StatusCode { get; set; }
    }
}
