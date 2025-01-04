using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Data.Models;
using EpicMarket.Contracts;

public class OTPService : IOTPService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICommunicationService _communicationService;
    private readonly Random _random;
    private const int OTP_LENGTH = 6;
    private const int OTP_EXPIRY_MINUTES = 5;

    public OTPService(
        ApplicationDbContext dbContext,
        ICommunicationService communicationService)
    {
        _dbContext = dbContext;
        _communicationService = communicationService;
        _random = new Random();
    }

    public async Task<(string ReferenceId, DateTime Timestamp)> SendOTPAsync(string username, string type)
    {
        // Generate OTP
        string otp = GenerateOTP();
        string referenceId = Guid.NewGuid().ToString();
        DateTime expirationTime = DateTime.UtcNow.AddMinutes(OTP_EXPIRY_MINUTES);

        // Save OTP details
        var otpVerification = new OTPVerification
        {
            Username = username,
            OTP = otp,
            ReferenceID = referenceId,
            ExpirationTime = expirationTime,
            IsVerified = false,
            Type = type,
            IsActive = true,
            CreateBy = "System",
            CreateDate = DateTime.UtcNow
        };

        _dbContext.OTPVerifications.Add(otpVerification);
        await _dbContext.SaveChangesAsync();

        // Send OTP based on type
        if (type.ToUpper() == "EMAIL")
        {
            _communicationService.SendEmailAsync(
                username,
                "OTP Verification",
                $"Your OTP is: {otp}. It will expire in {OTP_EXPIRY_MINUTES} minutes."
            );
        }
        else if (type.ToUpper() == "PHONE")
        {
            // Implement SMS sending logic here using SendGrid
            // For now, we'll just log it
            Console.WriteLine($"SMS OTP {otp} would be sent to {username}");
        }

        return (referenceId, expirationTime);
    }

    public async Task<bool> VerifyOTPAsync(string referenceId, string otp)
    {
        var verification = await _dbContext.OTPVerifications
            .Where(v => v.ReferenceID == referenceId && v.IsActive)
            .OrderByDescending(v => v.CreateDate)
            .FirstOrDefaultAsync();

        if (verification == null)
            return false;

        if (verification.ExpirationTime < DateTime.UtcNow)
            return false;

        if (verification.OTP != otp)
            return false;

        verification.IsVerified = true;
        verification.IsActive = false;
        await _dbContext.SaveChangesAsync();

        return true;
    }

    private string GenerateOTP()
    {
        return string.Join("", Enumerable.Range(0, OTP_LENGTH)
            .Select(_ => _random.Next(0, 10)));
    }
} 