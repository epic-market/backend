using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities.CustomModels
{

	public static class Constants
    {

        public const string ADMIN_USERID = "admin@epicmarket.in";
        public const string ACCESSTYPE_READWRITE = "ReadWrite";
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
        public const string BasePath = "BASEPATH";
        public const string Business = "BUSINESS";
    }
    public static class FilePathConstants
    {
        public const string Business = "BUSINESSPATH";
    }
    public static class EntityConstants
    {
        public const string Catelog = "Catelog";
        public const string Branch = "Branch";
        public const string Employees = "Employees";
        public const string Order = "Order";
        public const string Business = "Business";
    }
    public static class AttachmentTypeConstants
    {
        public const string LOGO = "Logo";
    }
    public static class DocumentTypeConstants
    {
        public const string FILE = "File";
    }
    public static class EventConstants
    {
        public const string AddCatelog = "AddCatelog";
        public const string EditCatelog = "EditCatelog";
        public const string AddBranch = "AddBranch";
        public const string EditBranch = "EditBranch";
        public const string AddEmployees = "AddEmployees";
        public const string EditEmployees = "EditEmployees";
        public const string AddOrder = "AddOrder";
        public const string EditOrder = "EditOrder"; 
        public const string AddBusiness = "AddBusiness";
    }
    public static class ContactMethodConstants
    {
        public const string EMAIL = "Email";
    }

    public static class VerificationConstants
    {
        public const string BranchName = "Branches";
        public const string BranchDescription = "List of branchs to be verified";
        public const string CatelogName = "Catelogs";
        public const string CatelogDescription = "List of Catelog to be verified";

    }

}

