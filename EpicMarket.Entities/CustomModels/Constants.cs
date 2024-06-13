using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities.CustomModels
{

	public static class Constants
    {
       
    }

	public static class Business_Status
	{
		public const string BUSINESS_UNVERIFIED = "Unverified";
		public const string BUSINESS_PENDING = "Pending";
		public const string BUSINESS_VERIFIED = "Verified";
		public const string BUSINESS_REJECTED = "Rejected";
	}


	public static class ROLES
    {
        public const string BUSINESS_OWNER = "businessOwner";
        public const string BUSINESS_EMPLOYEE = "businessEmployee";
        public const string MEMBER = "member";
        public const string ADMIN = "admin";
        public const string MODERATOR = "moderator";

    }

	public static class ApplicationConfigurationConstants
	{
		public const string Products = "Products";
		public const string LOGO = "LOGO";
	}



}

