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
        public const string ROOT = "root";
        public const string SUPPORT = "support";

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
        public const string CatelogVariant = "CatelogVariant";
		public const string Branch = "Branch";
        public const string Employees = "Employees";
        public const string Order = "Order";
        public const string Business = "Business";
		public const string Tasks = "Tasks";
        public const string ProductInternal = "ProductInternal";//
        public const string Profile = "Profile";//
        public const string Proof = "Proof";
        public const string AccessControlList = "AccessControlList";
        public const string ApplicationTable = "ApplicationTable";
        public const string ApplicationSecurable = "ApplicationSecurable";
        public const string AccessType = "AccessType";
        public const string ApplicationConfiguration = "ApplicationConfiguration";
        public const string AppRole = "AppRole";
        public const string Attachment = "Attachment";
        public const string AttachmentType = "AttachmentType";
        public const string BusinessCategory = "BusinessCategory";
        public const string AppUser = "AppUser";
        public const string Blog = "Blog";
        public const string BlogCategory = "BlogCategory";
        public const string BusinessEmployeeMap = "BusinessEmployeeMap";
        public const string Catalog = "Catalog";
        public const string CommunicationQueue = "CommunicationQueue";
        public const string ContactMethod = "ContactMethod";
        public const string Entity = "Entity";
        public const string EventCategory = "EventCategory";
        public const string Event = "Event";
        public const string EventLog = "EventLog";
        public const string UserOnboardingProgress = "UserOnboardingProgress";
        public const string TaskType = "TaskType";
        public const string TaskStatusType = "TaskStatusType";
        public const string SupportTicket = "SupportTicket";
        public const string SupportQuery = "SupportQuery";
        public const string StatusOptionSet = "StatusOptionSet";
        public const string PromotionalLead = "PromotionalLead";
        public const string HelpItem = "HelpItem";
        public const string FAQ = "FAQ";
        public const string FAQCategory = "FAQCategory";
        public const string OrderDetail = "OrderDetail";
        public const string OnboardingStep = "OnboardingStep";
        public const string Notification = "Notification";
        public const string Inventory = "Inventory";
        public const string OrderTypesOption = "OrderTypesOption";
        public const string PersonType = "PersonType";
        public const string Page = "Page";
        public const string Outlet = "Outlet";
        public const string OutletPerson = "OutletPerson";
    }
    public static class AttachmentTypeConstants
    {
        public const string BUSINESS_CATEGORY = "BusinessCategory";
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
        public const string AddAccessControlList = "AddAccessControlList";
        public const string EditAccessControlList = "EditAccessControlList";
        public const string DeleteAccessControlList = "DeleteAccessControlList";
        public const string AddApplicationTable = "AddApplicationTable";
        public const string EditApplicationTable = "EditApplicationTable";
        public const string DeleteApplicationTable = "DeleteApplicationTable";
        public const string AddApplicationSecurable = "AddApplicationSecurable";
        public const string EditApplicationSecurable = "EditApplicationSecurable";
        public const string DeleteApplicationSecurable = "DeleteApplicationSecurable";
        public const string AddAccessType = "AddAccessType";
        public const string EditAccessType = "EditAccessType";
        public const string DeleteAccessType = "DeleteAccessType";
        public const string AddApplicationConfiguration = "AddApplicationConfiguration";
        public const string EditApplicationConfiguration = "EditApplicationConfiguration";
        public const string DeleteApplicationConfiguration = "DeleteApplicationConfiguration";
        public const string AddAppRole = "AddAppRole";
        public const string EditAppRole = "EditAppRole";
        public const string DeleteAppRole = "DeleteAppRole";
        public const string AddAttachment = "AddAttachment";
        public const string EditAttachment = "EditAttachment";
        public const string DeleteAttachment = "DeleteAttachment";
        public const string AddAttachmentType = "AddAttachmentType";
        public const string EditAttachmentType = "EditAttachmentType";
        public const string DeleteAttachmentType = "DeleteAttachmentType";
        public const string AddBusinessCategory = "AddBusinessCategory";
        public const string EditBusinessCategory = "EditBusinessCategory";
        public const string DeleteBusinessCategory = "DeleteBusinessCategory";
        public const string AddAppUser = "AddAppUser";
        public const string EditAppUser = "EditAppUser";
        public const string DeleteAppUser = "DeleteAppUser";
        public const string AddBlog = "AddBlog";
        public const string EditBlog = "EditBlog";
        public const string DeleteBlog = "DeleteBlog";
        public const string AddBlogCategory = "AddBlogCategory";
        public const string EditBlogCategory = "EditBlogCategory";
        public const string DeleteBlogCategory = "DeleteBlogCategory";
        public const string AddBusinessEmployeeMap = "AddBusinessEmployeeMap";
        public const string EditBusinessEmployeeMap = "EditBusinessEmployeeMap";
        public const string DeleteBusinessEmployeeMap = "DeleteBusinessEmployeeMap";
        public const string AddCatalog = "AddCatalog";
        public const string EditCatalog = "EditCatalog";
        public const string DeleteCatalog = "DeleteCatalog";
        public const string AddCommunicationQueue = "AddCommunicationQueue";
        public const string EditCommunicationQueue = "EditCommunicationQueue";
        public const string DeleteCommunicationQueue = "DeleteCommunicationQueue";
        public const string AddContactMethod = "AddContactMethod";
        public const string EditContactMethod = "EditContactMethod";
        public const string DeleteContactMethod = "DeleteContactMethod";
        public const string AddEntity = "AddEntity";
        public const string EditEntity = "EditEntity";
        public const string DeleteEntity = "DeleteEntity";
        public const string AddEventCategory = "AddEventCategory";
        public const string EditEventCategory = "EditEventCategory";
        public const string DeleteEventCategory = "DeleteEventCategory";
        public const string AddEvent = "AddEvent";
        public const string EditEvent = "EditEvent";
        public const string DeleteEvent = "DeleteEvent";
        public const string DeleteBusiness = "DeleteBusiness";
        public const string AddUserOnboardingProgress = "AddUserOnboardingProgress";
        public const string EditUserOnboardingProgress = "EditUserOnboardingProgress";
        public const string DeleteUserOnboardingProgress = "DeleteUserOnboardingProgress";
        public const string AddTaskType = "AddTaskType";
        public const string EditTaskType = "EditTaskType";
        public const string DeleteTaskType = "DeleteTaskType";
        public const string AddTaskStatusType = "AddTaskStatusType";
        public const string EditTaskStatusType = "EditTaskStatusType";
        public const string DeleteTaskStatusType = "DeleteTaskStatusType";
        public const string AddTask = "AddTask";
        public const string EditTask = "EditTask";
        public const string DeleteTask = "DeleteTask";
        public const string AddSupportTicket = "AddSupportTicket";
        public const string EditSupportTicket = "EditSupportTicket";
        public const string DeleteSupportTicket = "DeleteSupportTicket";
        public const string AddSupportQuery = "AddSupportQuery";
        public const string EditSupportQuery = "EditSupportQuery";
        public const string DeleteSupportQuery = "DeleteSupportQuery";
        public const string AddStatusOptionSet = "AddStatusOptionSet";
        public const string EditStatusOptionSet = "EditStatusOptionSet";
        public const string DeleteStatusOptionSet = "DeleteStatusOptionSet";
        public const string AddPromotionalLead = "AddPromotionalLead";
        public const string EditPromotionalLead = "EditPromotionalLead";
        public const string DeletePromotionalLead = "DeletePromotionalLead";
        public const string CREATE = "Create";
        public const string UPDATE = "Update";
        public const string DELETE = "Delete";
        public const string CreateHelpItem = "CreateHelpItem";
        public const string EditHelpItem = "EditHelpItem";
        public const string DeleteHelpItem = "DeleteHelpItem";
        public const string CreateFAQ = "CreateFAQ";
        public const string EditFAQ = "EditFAQ";
        public const string DeleteFAQ = "DeleteFAQ";
        public const string CreateFAQCategory = "CreateFAQCategory";
        public const string EditFAQCategory = "EditFAQCategory";
        public const string DeleteFAQCategory = "DeleteFAQCategory";
        public const string CreateOrderDetail = "CreateOrderDetail";
        public const string EditOrderDetail = "EditOrderDetail";
        public const string DeleteOrderDetail = "DeleteOrderDetail";
        public const string CreateOnboardingStep = "CreateOnboardingStep";
        public const string EditOnboardingStep = "EditOnboardingStep";
        public const string DeleteOnboardingStep = "DeleteOnboardingStep";
        public const string GetNotifications = "GetNotifications";
        public const string ReadNotification = "ReadNotification";
        public const string CreateInventory = "CreateInventory";
        public const string EditInventory = "EditInventory";
        public const string DeleteInventory = "DeleteInventory";
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

