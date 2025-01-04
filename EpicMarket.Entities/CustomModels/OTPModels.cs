public class OTPRequest
{
    public string Username { get; set; }
    public string Type { get; set; } // EMAIL/PHONE
}

public class OTPResponse
{
    public string ReferenceId { get; set; }
    public DateTime Timestamp { get; set; }
}

public class VerifyOTPRequest
{
    public string ReferenceId { get; set; }
    public string OTP { get; set; }
} 