public interface IOTPService
{
    Task<(string ReferenceId, DateTime Timestamp)> SendOTPAsync(string username, string type);
    Task<bool> VerifyOTPAsync(string referenceId, string otp);
} 