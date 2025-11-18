public class OTPVerification
{
    public int ID { get; set; }
    public string Username { get; set; }
    public string OTP { get; set; }
    public string ReferenceID { get; set; }
    public DateTime ExpirationTime { get; set; }
    public bool IsVerified { get; set; }
    public string Type { get; set; } // EMAIL/PHONE
    public bool IsActive { get; set; }
    public string CreateBy { get; set; }
    public DateTime CreateDate { get; set; }
} 