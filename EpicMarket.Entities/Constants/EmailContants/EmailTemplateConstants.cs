using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EpicMarket.Entities.Constants
{
  
    public static class EmailTemplateConstants
    {
        public const string UnderReviewForBusiness = "UnderReviewForBusiness";
        public const string ResetPasswordLink = "ResetPasswordLink";
        public const string BusinessRegistrationComplete = "BusinessRegisterSuccussfullyAskingToCompleteDetetials";
    }


    public static class EmailSubjectConstants
    {
        public const string UnderReviewForBusiness = "Under Review For Business";
        public const string ResetPasswordLink = "Reset Password Link";
        public const string BusinessRegistrationComplete = "Business Registration Successful - Complete Your Details";
    }

    public static class EmailModel
    {
        public static object GetUnderReviewForBusinessModel(string businessName)
        {
            return new
            {
                Business_Name = businessName,
                Processing_Days = "5",
                Support_Email = Constants.SUPPORT_EMAIL,
                Support_Phone = Constants.SUPPORT_PHONE,
                Current_Year = DateTime.Now.Year.ToString(),
                Company_Address = Constants.COMPANY_ADDRESS
            };
        }
        
        public static object GetResetPasswordModel(string resetUrl)
        {
            return new
            {
                Reset_Password_URL = resetUrl,
                Support_Email = Constants.SUPPORT_EMAIL,
                Support_Phone = Constants.SUPPORT_PHONE,
                Current_Year = DateTime.Now.Year.ToString(),
                Company_Address = Constants.COMPANY_ADDRESS
            };
        }
        
        public static object GetBusinessRegistrationCompleteModel()
        {
            return new
            {
                Document_Upload_URL = "owner.epicmarket.in",
                Support_Email = Constants.SUPPORT_EMAIL,
                Support_Phone = Constants.SUPPORT_PHONE,
                Current_Year = DateTime.Now.Year.ToString(),
                Company_Address = Constants.COMPANY_ADDRESS
            };
        }
    }
}