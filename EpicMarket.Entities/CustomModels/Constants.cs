using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities.CustomModels
{




    public static class OrderType
    {
        public const string ONLINE = "Online";
        public const string OFFLINE = "Offline";
    }

    public static class Constants
    {

        public const string ADMIN_USERID = "admin@epicmarket.in";
        public const string ACCESSTYPE_READWRITE = "ReadWrite";
	}

	public static class StatusConstants
	{
		public const string UNVERIFIED = "Unverified";
        public const string SENDTOVERIFICATION = "SendToVerification";
		public const string PENDING = "Pending";
		public const string VERIFIED = "Verified";
		public const string REJECTED = "Rejected";
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
        public const string APIROUTE = "APIROUTE";
		public const string FILEURL = "FILEURL";
		public const string THUMBNAIL = "THUMBNAIL";
        public const string branches = "branches";
        public const string BranchesPhotos = "BranchesPhotos";
        public const string BranchThumbnail = "BranchThumbnail";
        public const string TASKPATH = "TaskPath";

    }
    public static class FilePathConstants
    {
        public const string Business = "BUSINESSPATH";
        public const string LOGOPATH = "LOGOPATH";
        public const string ProofPATH = "ProofPATH";
		public const string PRODUCTPATH = "ProductPath";
		public const string THUMBNAILPATH = "THUMBNAILPATH";
        public const string ProductInternal = "PRODUCTINTERNAL";//

        public const string branches = "branches";
        public const string BranchesPhotos = "BranchPhotos";
        public const string BranchThumbnail = "BranchThumbnail";

        public const string TASKPATH = "TaskPath";

    }

    public static class TaskTypeConstants
    {
        public const string Verification = "Verification";
        public const string Grievance = "Grievance";
        public const string Issues = "Issues";
        public const string Support = "Support";
    }

        public static class TaskDescriptions
    {
        public const string Business = "Verification For the Business";

    }
    public static class EntityConstants
    {
        public const string Catelog = "Catelog";
		public const string Branch = "Branch";
        public const string Employees = "Employees";
        public const string Order = "Order";
        public const string Business = "Business";
		public const string Tasks = "Tasks";
        public const string ProductInternal = "ProductInternal";//
        public const string Profile = "Profile";//
        public const string Proof = "Proof";
    }
    public static class AttachmentTypeConstants
    {
        public const string LOGO = "Logo";
        public const string PROOF = "Proof";
		public const string PRODUCTIMAGES = "Products";
		public const string THUMBNAIL = "Thumbnail";
        public const string BRANCH_PHOTOS = "BranchPhotos";
        public const string BRANCH_THUMBNAIL = "BranchThumbnail";
        public const string TASK = "Task";
        public const string ProductInternal = "ProductInternal";//

        public const string Profile = "Profile";//
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
        public const string EditBusiness = "EditBusiness";
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
    public static class MessageDataConstants
    {
        public const string AddBusiness = "Business added sucessfully";
        public const string EditBusiness = "Business updated sucessfully";
        public const string AddBranch = "Branch added sucessfully";
        public const string EditBranch = "Branch updated sucessfully";
        public const string AddCatelog = "Catelog added sucessfully ";
        public const string EditCatelog = "Catelog  updated sucessfully";
    }
    public static class OrderStatusConstants
    {
        public const string OrderPlaced = "Order Placed";
        public const string OrderConfirmed = "Order Confirmed";
        public const string OrderProcessing = "Order Processing";
        public const string Canceled = "Canceled";
        public const string AwaitingPickUp = "Awaiting Pickup";
        public const string Delivered = "Delivered";
    }


    public static class TaskStatusTypesConstants
    {
        public const string NEW = "New";
    }

    public static class FAQRoleType
    {
        public const string Customer = "Customer";
    }

    public static class SubscriptionStatusConstants
    {
        public const string Subscribed = "Subscribed";
        public const string Unsubscribed = "Unsubscribed";
    }
}

