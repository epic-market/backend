using EpicMarket.Data.ApplicationModels;
using EpicMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Webapp.AlterScripts
{
    public class AlterScript10 : BaseAlterScript,IAlterScript
    {
        private readonly ApplicationDbContext dbContext;

        public AlterScript10(ApplicationDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }



        public void Execute()
        {

            Console.WriteLine("Started Executing " + this.GetType().Name);

                    
            if (!dbContext.Entity.Any(cm => cm.Name == "AccessControlList"))
                {
                    var entity = new Entity
                    {
                        Name = "AccessControlList",
                        Description = "Access Control List Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add ApplicationTable
                if (!dbContext.Entity.Any(cm => cm.Name == "ApplicationTable"))
                {
                    var entity = new Entity
                    {
                        Name = "ApplicationTable",
                        Description = "Application Table Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add ApplicationSecurable
                if (!dbContext.Entity.Any(cm => cm.Name == "ApplicationSecurable"))
                {
                    var entity = new Entity
                    {
                        Name = "ApplicationSecurable",
                        Description = "Application Securable Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add AccessType
                if (!dbContext.Entity.Any(cm => cm.Name == "AccessType"))
                {
                    var entity = new Entity
                    {
                        Name = "AccessType",
                        Description = "Access Type Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add ApplicationConfiguration
                if (!dbContext.Entity.Any(cm => cm.Name == "ApplicationConfiguration"))
                {
                    var entity = new Entity
                    {
                        Name = "ApplicationConfiguration",
                        Description = "Application Configuration Entity"
                    };
                    dbContext.Entity.Add(entity);
                }
                // Add AppRole
                if (!dbContext.Entity.Any(cm => cm.Name == "AppRole"))
                {
                    var entity = new Entity
                    {
                        Name = "AppRole",
                        Description = "App Role Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add Attachment
                if (!dbContext.Entity.Any(cm => cm.Name == "Attachment"))
                {
                    var entity = new Entity
                    {
                        Name = "Attachment",
                        Description = "Attachment Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add AttachmentType
                if (!dbContext.Entity.Any(cm => cm.Name == "AttachmentType"))
                {
                    var entity = new Entity
                    {
                        Name = "AttachmentType",
                        Description = "Attachment Type Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add BusinessCategory
                if (!dbContext.Entity.Any(cm => cm.Name == "BusinessCategory"))
                {
                    var entity = new Entity
                    {
                        Name = "BusinessCategory",
                        Description = "Business Category Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add AppUser
                if (!dbContext.Entity.Any(cm => cm.Name == "AppUser"))
                {
                    var entity = new Entity
                    {
                        Name = "AppUser",
                        Description = "App User Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add Blog
                if (!dbContext.Entity.Any(cm => cm.Name == "Blog"))
                {
                    var entity = new Entity
                    {
                        Name = "Blog",
                        Description = "Blog Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add BlogCategory
                if (!dbContext.Entity.Any(cm => cm.Name == "BlogCategory"))
                {
                    var entity = new Entity
                    {
                        Name = "BlogCategory",
                        Description = "Blog Category Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add BusinessEmployeeMap
                if (!dbContext.Entity.Any(cm => cm.Name == "BusinessEmployeeMap"))
                {
                    var entity = new Entity
                    {
                        Name = "BusinessEmployeeMap",
                        Description = "Business Employee Map Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add Product
                if (!dbContext.Entity.Any(cm => cm.Name == "Product"))
                {
                    var entity = new Entity
                    {
                        Name = "Product",
                        Description = "Product Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add CommunicationQueue
                if (!dbContext.Entity.Any(cm => cm.Name == "CommunicationQueue"))
                {
                    var entity = new Entity
                    {
                        Name = "CommunicationQueue",
                        Description = "Communication Queue Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add ContactMethod
                if (!dbContext.Entity.Any(cm => cm.Name == "ContactMethod"))
                {
                    var entity = new Entity
                    {
                        Name = "ContactMethod",
                        Description = "Contact Method Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add Entity
                if (!dbContext.Entity.Any(cm => cm.Name == "Entity"))
                {
                    var entity = new Entity
                    {
                        Name = "Entity",
                        Description = "Entity Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add EventCategory
                if (!dbContext.Entity.Any(cm => cm.Name == "EventCategory"))
                {
                    var entity = new Entity
                    {
                        Name = "EventCategory",
                        Description = "Event Category Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add Event
                if (!dbContext.Entity.Any(cm => cm.Name == "Event"))
                {
                    var entity = new Entity
                    {
                        Name = "Event",
                        Description = "Event Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add EventLog
                if (!dbContext.Entity.Any(cm => cm.Name == "EventLog"))
                {
                    var entity = new Entity
                    {
                        Name = "EventLog",
                        Description = "Event Log Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add UserOnboardingProgress
                if (!dbContext.Entity.Any(cm => cm.Name == "UserOnboardingProgress"))
                {
                    var entity = new Entity
                    {
                        Name = "UserOnboardingProgress",
                        Description = "User Onboarding Progress Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add TaskType
                if (!dbContext.Entity.Any(cm => cm.Name == "TaskType"))
                {
                    var entity = new Entity
                    {
                        Name = "TaskType",
                        Description = "Task Type Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add TaskStatusType
                if (!dbContext.Entity.Any(cm => cm.Name == "TaskStatusType"))
                {
                    var entity = new Entity
                    {
                        Name = "TaskStatusType",
                        Description = "Task Status Type Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add SupportTicket
                if (!dbContext.Entity.Any(cm => cm.Name == "SupportTicket"))
                {
                    var entity = new Entity
                    {
                        Name = "SupportTicket",
                        Description = "Support Ticket Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add SupportQuery
                if (!dbContext.Entity.Any(cm => cm.Name == "SupportQuery"))
                {
                    var entity = new Entity
                    {
                        Name = "SupportQuery",
                        Description = "Support Query Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add StatusOptionSet
                if (!dbContext.Entity.Any(cm => cm.Name == "StatusOptionSet"))
                {
                    var entity = new Entity
                    {
                        Name = "StatusOptionSet",
                        Description = "Status Option Set Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add PromotionalLead
                if (!dbContext.Entity.Any(cm => cm.Name == "PromotionalLead"))
                {
                    var entity = new Entity
                    {
                        Name = "PromotionalLead",
                        Description = "Promotional Lead Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add HelpItem
                if (!dbContext.Entity.Any(cm => cm.Name == "HelpItem"))
                {
                    var entity = new Entity
                    {
                        Name = "HelpItem",
                        Description = "Help Item Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add FAQ
                if (!dbContext.Entity.Any(cm => cm.Name == "FAQ"))
                {
                    var entity = new Entity
                    {
                        Name = "FAQ",
                        Description = "FAQ Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add FAQCategory
                if (!dbContext.Entity.Any(cm => cm.Name == "FAQCategory"))
                {
                    var entity = new Entity
                    {
                        Name = "FAQCategory",
                        Description = "FAQ Category Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add OrderDetail
                if (!dbContext.Entity.Any(cm => cm.Name == "OrderDetail"))
                {
                    var entity = new Entity
                    {
                        Name = "OrderDetail",
                        Description = "Order Detail Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add OnboardingStep
                if (!dbContext.Entity.Any(cm => cm.Name == "OnboardingStep"))
                {
                    var entity = new Entity
                    {
                        Name = "OnboardingStep",
                        Description = "Onboarding Step Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add Notification
                if (!dbContext.Entity.Any(cm => cm.Name == "Notification"))
                {
                    var entity = new Entity
                    {
                        Name = "Notification",
                        Description = "Notification Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add Inventory
                if (!dbContext.Entity.Any(cm => cm.Name == "Inventory"))
                {
                    var entity = new Entity
                    {
                        Name = "Inventory",
                        Description = "Inventory Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add OrderTypesOption
                if (!dbContext.Entity.Any(cm => cm.Name == "OrderTypesOption"))
                {
                    var entity = new Entity
                    {
                        Name = "OrderTypesOption",
                        Description = "Order Types Option Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add PersonType
                if (!dbContext.Entity.Any(cm => cm.Name == "PersonType"))
                {
                    var entity = new Entity
                    {
                        Name = "PersonType",
                        Description = "Person Type Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add Page
                if (!dbContext.Entity.Any(cm => cm.Name == "Page"))
                {
                    var entity = new Entity
                    {
                        Name = "Page",
                        Description = "Page Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add Outlet
                if (!dbContext.Entity.Any(cm => cm.Name == "Outlet"))
                {
                    var entity = new Entity
                    {
                        Name = "Outlet",
                        Description = "Outlet Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                // Add OutletPerson
                if (!dbContext.Entity.Any(cm => cm.Name == "OutletPerson"))
                {
                    var entity = new Entity
                    {
                        Name = "OutletPerson",
                        Description = "Outlet Person Entity"
                    };
                    dbContext.Entity.Add(entity);
                }

                 // Access Control List Events
                if (!dbContext.Event.Any(cm => cm.Name == "AddAccessControlList"))
                {
                    var event1 = new Event
                    {
                        Name = "AddAccessControlList",
                        Description = "Add Access Control List",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event1);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditAccessControlList"))
                {
                    var event2 = new Event
                    {
                        Name = "EditAccessControlList",
                        Description = "Edit Access Control List",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event2);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteAccessControlList"))
                {
                    var event3 = new Event
                    {
                        Name = "DeleteAccessControlList",
                        Description = "Delete Access Control List",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event3);
                }

                // Application Table Events
                if (!dbContext.Event.Any(cm => cm.Name == "AddApplicationTable"))
                {
                    var event4 = new Event
                    {
                        Name = "AddApplicationTable",
                        Description = "Add Application Table",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event4);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditApplicationTable"))
                {
                    var event5 = new Event
                    {
                        Name = "EditApplicationTable",
                        Description = "Edit Application Table",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event5);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteApplicationTable"))
                {
                    var event6 = new Event
                    {
                        Name = "DeleteApplicationTable",
                        Description = "Delete Application Table",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event6);
                }

                // Application Securable Events
                if (!dbContext.Event.Any(cm => cm.Name == "AddApplicationSecurable"))
                {
                    var event7 = new Event
                    {
                        Name = "AddApplicationSecurable",
                        Description = "Add Application Securable",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event7);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditApplicationSecurable"))
                {
                    var event8 = new Event
                    {
                        Name = "EditApplicationSecurable",
                        Description = "Edit Application Securable",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event8);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteApplicationSecurable"))
                {
                    var event9 = new Event
                    {
                        Name = "DeleteApplicationSecurable",
                        Description = "Delete Application Securable",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event9);
                }

                // Access Type Events
                if (!dbContext.Event.Any(cm => cm.Name == "AddAccessType"))
                {
                    var event10 = new Event
                    {
                        Name = "AddAccessType",
                        Description = "Add Access Type",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event10);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditAccessType"))
                {
                    var event11 = new Event
                    {
                        Name = "EditAccessType",
                        Description = "Edit Access Type",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event11);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteAccessType"))
                {
                    var event12 = new Event
                    {
                        Name = "DeleteAccessType",
                        Description = "Delete Access Type",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event12);
                }

                // Application Configuration Events
                if (!dbContext.Event.Any(cm => cm.Name == "AddApplicationConfiguration"))
                {
                    var event13 = new Event
                    {
                        Name = "AddApplicationConfiguration",
                        Description = "Add Application Configuration",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event13);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditApplicationConfiguration"))
                {
                    var event14 = new Event
                    {
                        Name = "EditApplicationConfiguration",
                        Description = "Edit Application Configuration",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event14);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteApplicationConfiguration"))
                {
                    var event15 = new Event
                    {
                        Name = "DeleteApplicationConfiguration",
                        Description = "Delete Application Configuration",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event15);
                }



                // App Role Events
                if (!dbContext.Event.Any(cm => cm.Name == "AddAppRole"))
                {
                    var event16 = new Event
                    {
                        Name = "AddAppRole",
                        Description = "Add App Role",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event16);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditAppRole"))
                {
                    var event17 = new Event
                    {
                        Name = "EditAppRole", 
                        Description = "Edit App Role",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event17);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteAppRole"))
                {
                    var event18 = new Event
                    {
                        Name = "DeleteAppRole",
                        Description = "Delete App Role",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event18);
                }

                // Attachment Events
                if (!dbContext.Event.Any(cm => cm.Name == "AddAttachment"))
                {
                    var event19 = new Event
                    {
                        Name = "AddAttachment",
                        Description = "Add Attachment",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event19);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditAttachment"))
                {
                    var event20 = new Event
                    {
                        Name = "EditAttachment",
                        Description = "Edit Attachment",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event20);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteAttachment"))
                {
                    var event21 = new Event
                    {
                        Name = "DeleteAttachment",
                        Description = "Delete Attachment",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event21);
                }

                // Attachment Type Events
                if (!dbContext.Event.Any(cm => cm.Name == "AddAttachmentType"))
                {
                    var event22 = new Event
                    {
                        Name = "AddAttachmentType",
                        Description = "Add Attachment Type",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event22);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditAttachmentType"))
                {
                    var event23 = new Event
                    {
                        Name = "EditAttachmentType",
                        Description = "Edit Attachment Type",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event23);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteAttachmentType"))
                {
                    var event24 = new Event
                    {
                        Name = "DeleteAttachmentType",
                        Description = "Delete Attachment Type",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event24);
                }

                // Business Category Events
                if (!dbContext.Event.Any(cm => cm.Name == "AddBusinessCategory"))
                {
                    var event25 = new Event
                    {
                        Name = "AddBusinessCategory",
                        Description = "Add Business Category",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event25);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditBusinessCategory"))
                {
                    var event26 = new Event
                    {
                        Name = "EditBusinessCategory",
                        Description = "Edit Business Category",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event26);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteBusinessCategory"))
                {
                    var event27 = new Event
                    {
                        Name = "DeleteBusinessCategory",
                        Description = "Delete Business Category",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event27);
                }

                // App User Events
                if (!dbContext.Event.Any(cm => cm.Name == "AddAppUser"))
                {
                    var event28 = new Event
                    {
                        Name = "AddAppUser",
                        Description = "Add App User",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event28);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditAppUser"))
                {
                    var event29 = new Event
                    {
                        Name = "EditAppUser",
                        Description = "Edit App User",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event29);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteAppUser"))
                {
                    var event30 = new Event
                    {
                        Name = "DeleteAppUser",
                        Description = "Delete App User",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event30);
                }

                // Blog Events
                if (!dbContext.Event.Any(cm => cm.Name == "AddBlog"))
                {
                    var event31 = new Event
                    {
                        Name = "AddBlog",
                        Description = "Add Blog",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event31);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditBlog"))
                {
                    var event32 = new Event
                    {
                        Name = "EditBlog",
                        Description = "Edit Blog",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event32);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteBlog"))
                {
                    var event33 = new Event
                    {
                        Name = "DeleteBlog",
                        Description = "Delete Blog",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event33);
                }

                // Blog Category Events
                if (!dbContext.Event.Any(cm => cm.Name == "AddBlogCategory"))
                {
                    var event34 = new Event
                    {
                        Name = "AddBlogCategory",
                        Description = "Add Blog Category",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event34);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditBlogCategory"))
                {
                    var event35 = new Event
                    {
                        Name = "EditBlogCategory",
                        Description = "Edit Blog Category",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event35);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteBlogCategory"))
                {
                    var event36 = new Event
                    {
                        Name = "DeleteBlogCategory",
                        Description = "Delete Blog Category",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event36);
                }

                // Business Employee Map Events
                if (!dbContext.Event.Any(cm => cm.Name == "AddBusinessEmployeeMap"))
                {
                    var event37 = new Event
                    {
                        Name = "AddBusinessEmployeeMap",
                        Description = "Add Business Employee Map",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event37);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditBusinessEmployeeMap"))
                {
                    var event38 = new Event
                    {
                        Name = "EditBusinessEmployeeMap",
                        Description = "Edit Business Employee Map",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event38);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteBusinessEmployeeMap"))
                {
                    var event39 = new Event
                    {
                        Name = "DeleteBusinessEmployeeMap",
                        Description = "Delete Business Employee Map",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event39);
                }

                // Product Events
                if (!dbContext.Event.Any(cm => cm.Name == "AddProduct"))
                {
                    var event40 = new Event
                    {
                        Name = "AddProduct",
                        Description = "Add Product",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event40);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditProduct"))
                {
                    var event41 = new Event
                    {
                        Name = "EditProduct",
                        Description = "Edit Product",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event41);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteProduct"))
                {
                    var event42 = new Event
                    {
                        Name = "DeleteProduct",
                        Description = "Delete Product",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event42);
                }

                // Communication Queue Events
                if (!dbContext.Event.Any(cm => cm.Name == "AddCommunicationQueue"))
                {
                    var event43 = new Event
                    {
                        Name = "AddCommunicationQueue",
                        Description = "Add Communication Queue",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event43);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditCommunicationQueue"))
                {
                    var event44 = new Event
                    {
                        Name = "EditCommunicationQueue",
                        Description = "Edit Communication Queue",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event44);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteCommunicationQueue"))
                {
                    var event45 = new Event
                    {
                        Name = "DeleteCommunicationQueue",
                        Description = "Delete Communication Queue",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event45);
                }

                // Contact Method Events
                if (!dbContext.Event.Any(cm => cm.Name == "AddContactMethod"))
                {
                    var event46 = new Event
                    {
                        Name = "AddContactMethod",
                        Description = "Add Contact Method",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event46);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditContactMethod"))
                {
                    var event47 = new Event
                    {
                        Name = "EditContactMethod",
                        Description = "Edit Contact Method",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event47);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteContactMethod"))
                {
                    var event48 = new Event
                    {
                        Name = "DeleteContactMethod",
                        Description = "Delete Contact Method",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event48);
                }

                // Entity Events
                if (!dbContext.Event.Any(cm => cm.Name == "AddEntity"))
                {
                    var event49 = new Event
                    {
                        Name = "AddEntity",
                        Description = "Add Entity",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event49);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditEntity"))
                {
                    var event50 = new Event
                    {
                        Name = "EditEntity",
                        Description = "Edit Entity",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event50);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteEntity"))
                {
                    var event51 = new Event
                    {
                        Name = "DeleteEntity",
                        Description = "Delete Entity",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event51);
                }

                // Event Category Events
                if (!dbContext.Event.Any(cm => cm.Name == "AddEventCategory"))
                {
                    var event52 = new Event
                    {
                        Name = "AddEventCategory",
                        Description = "Add Event Category",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event52);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditEventCategory"))
                {
                    var event53 = new Event
                    {
                        Name = "EditEventCategory",
                        Description = "Edit Event Category",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event53);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteEventCategory"))
                {
                    var event54 = new Event
                    {
                        Name = "DeleteEventCategory",
                        Description = "Delete Event Category",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event54);
                }

                // Event Events
                if (!dbContext.Event.Any(cm => cm.Name == "AddEvent"))
                {
                    var event55 = new Event
                    {
                        Name = "AddEvent",
                        Description = "Add Event",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event55);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditEvent"))
                {
                    var event56 = new Event
                    {
                        Name = "EditEvent",
                        Description = "Edit Event",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event56);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteEvent"))
                {
                    var event57 = new Event
                    {
                        Name = "DeleteEvent",
                        Description = "Delete Event",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event57);
                }

                // Business Events
                if (!dbContext.Event.Any(cm => cm.Name == "DeleteBusiness"))
                {
                    var event58 = new Event
                    {
                        Name = "DeleteBusiness",
                        Description = "Delete Business",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event58);
                }

                // User Onboarding Progress Events
                if (!dbContext.Event.Any(cm => cm.Name == "AddUserOnboardingProgress"))
                {
                    var event59 = new Event
                    {
                        Name = "AddUserOnboardingProgress",
                        Description = "Add User Onboarding Progress",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event59);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditUserOnboardingProgress"))
                {
                    var event60 = new Event
                    {
                        Name = "EditUserOnboardingProgress",
                        Description = "Edit User Onboarding Progress",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event60);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteUserOnboardingProgress"))
                {
                    var event61 = new Event
                    {
                        Name = "DeleteUserOnboardingProgress",
                        Description = "Delete User Onboarding Progress",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event61);
                }

                // Task Type Events
                if (!dbContext.Event.Any(cm => cm.Name == "AddTaskType"))
                {
                    var event62 = new Event
                    {
                        Name = "AddTaskType",
                        Description = "Add Task Type",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event62);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditTaskType"))
                {
                    var event63 = new Event
                    {
                        Name = "EditTaskType",
                        Description = "Edit Task Type",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event63);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteTaskType"))
                {
                    var event64 = new Event
                    {
                        Name = "DeleteTaskType",
                        Description = "Delete Task Type",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event64);
                }

                // Task Status Type Events
                if (!dbContext.Event.Any(cm => cm.Name == "AddTaskStatusType"))
                {
                    var event65 = new Event
                    {
                        Name = "AddTaskStatusType",
                        Description = "Add Task Status Type",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event65);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditTaskStatusType"))
                {
                    var event66 = new Event
                    {
                        Name = "EditTaskStatusType",
                        Description = "Edit Task Status Type",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event66);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteTaskStatusType"))
                {
                    var event67 = new Event
                    {
                        Name = "DeleteTaskStatusType",
                        Description = "Delete Task Status Type",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event67);
                }

                // Task Events
                if (!dbContext.Event.Any(cm => cm.Name == "AddTask"))
                {
                    var event68 = new Event
                    {
                        Name = "AddTask",
                        Description = "Add Task",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event68);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditTask"))
                {
                    var event69 = new Event
                    {
                        Name = "EditTask",
                        Description = "Edit Task",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event69);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteTask"))
                {
                    var event70 = new Event
                    {
                        Name = "DeleteTask",
                        Description = "Delete Task",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event70);
                }

                // Support Ticket Events
                if (!dbContext.Event.Any(cm => cm.Name == "AddSupportTicket"))
                {
                    var event71 = new Event
                    {
                        Name = "AddSupportTicket",
                        Description = "Add Support Ticket",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event71);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditSupportTicket"))
                {
                    var event72 = new Event
                    {
                        Name = "EditSupportTicket",
                        Description = "Edit Support Ticket",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event72);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteSupportTicket"))
                {
                    var event73 = new Event
                    {
                        Name = "DeleteSupportTicket",
                        Description = "Delete Support Ticket",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event73);
                }

                // Support Query Events
                if (!dbContext.Event.Any(cm => cm.Name == "AddSupportQuery"))
                {
                    var event74 = new Event
                    {
                        Name = "AddSupportQuery",
                        Description = "Add Support Query",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event74);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditSupportQuery"))
                {
                    var event75 = new Event
                    {
                        Name = "EditSupportQuery",
                        Description = "Edit Support Query",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event75);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteSupportQuery"))
                {
                    var event76 = new Event
                    {
                        Name = "DeleteSupportQuery",
                        Description = "Delete Support Query",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event76);
                }

                // Status Option Set Events
                if (!dbContext.Event.Any(cm => cm.Name == "AddStatusOptionSet"))
                {
                    var event77 = new Event
                    {
                        Name = "AddStatusOptionSet",
                        Description = "Add Status Option Set",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event77);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditStatusOptionSet"))
                {
                    var event78 = new Event
                    {
                        Name = "EditStatusOptionSet",
                        Description = "Edit Status Option Set",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event78);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteStatusOptionSet"))
                {
                    var event79 = new Event
                    {
                        Name = "DeleteStatusOptionSet",
                        Description = "Delete Status Option Set",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event79);
                }

                // Promotional Lead Events
                if (!dbContext.Event.Any(cm => cm.Name == "AddPromotionalLead"))
                {
                    var event80 = new Event
                    {
                        Name = "AddPromotionalLead",
                        Description = "Add Promotional Lead",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event80);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditPromotionalLead"))
                {
                    var event81 = new Event
                    {
                        Name = "EditPromotionalLead",
                        Description = "Edit Promotional Lead",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event81);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeletePromotionalLead"))
                {
                    var event82 = new Event
                    {
                        Name = "DeletePromotionalLead",
                        Description = "Delete Promotional Lead",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event82);
                }

                // CRUD Events
                if (!dbContext.Event.Any(cm => cm.Name == "Create"))
                {
                    var event83 = new Event
                    {
                        Name = "Create",
                        Description = "Create",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event83);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "Update"))
                {
                    var event84 = new Event
                    {
                        Name = "Update",
                        Description = "Update",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event84);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "Delete"))
                {
                    var event85 = new Event
                    {
                        Name = "Delete",
                        Description = "Delete",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event85);
                }

                // Help Item Events
                if (!dbContext.Event.Any(cm => cm.Name == "CreateHelpItem"))
                {
                    var event86 = new Event
                    {
                        Name = "CreateHelpItem",
                        Description = "Create Help Item",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event86);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditHelpItem"))
                {
                    var event87 = new Event
                    {
                        Name = "EditHelpItem",
                        Description = "Edit Help Item",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event87);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteHelpItem"))
                {
                    var event88 = new Event
                    {
                        Name = "DeleteHelpItem",
                        Description = "Delete Help Item",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event88);
                }

                // FAQ Events
                if (!dbContext.Event.Any(cm => cm.Name == "CreateFAQ"))
                {
                    var event89 = new Event
                    {
                        Name = "CreateFAQ",
                        Description = "Create FAQ",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event89);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditFAQ"))
                {
                    var event90 = new Event
                    {
                        Name = "EditFAQ",
                        Description = "Edit FAQ",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event90);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteFAQ"))
                {
                    var event91 = new Event
                    {
                        Name = "DeleteFAQ",
                        Description = "Delete FAQ",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event91);
                }

                // FAQ Category Events
                if (!dbContext.Event.Any(cm => cm.Name == "CreateFAQCategory"))
                {
                    var event92 = new Event
                    {
                        Name = "CreateFAQCategory",
                        Description = "Create FAQ Category",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event92);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditFAQCategory"))
                {
                    var event93 = new Event
                    {
                        Name = "EditFAQCategory",
                        Description = "Edit FAQ Category",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event93);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteFAQCategory"))
                {
                    var event94 = new Event
                    {
                        Name = "DeleteFAQCategory",
                        Description = "Delete FAQ Category",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event94);
                }

                // Order Detail Events
                if (!dbContext.Event.Any(cm => cm.Name == "CreateOrderDetail"))
                {
                    var event95 = new Event
                    {
                        Name = "CreateOrderDetail",
                        Description = "Create Order Detail",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event95);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditOrderDetail"))
                {
                    var event96 = new Event
                    {
                        Name = "EditOrderDetail",
                        Description = "Edit Order Detail",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event96);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteOrderDetail"))
                {
                    var event97 = new Event
                    {
                        Name = "DeleteOrderDetail",
                        Description = "Delete Order Detail",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event97);
                }

                // Onboarding Step Events
                if (!dbContext.Event.Any(cm => cm.Name == "CreateOnboardingStep"))
                {
                    var event98 = new Event
                    {
                        Name = "CreateOnboardingStep",
                        Description = "Create Onboarding Step",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event98);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditOnboardingStep"))
                {
                    var event99 = new Event
                    {
                        Name = "EditOnboardingStep",
                        Description = "Edit Onboarding Step",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event99);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteOnboardingStep"))
                {
                    var event100 = new Event
                    {
                        Name = "DeleteOnboardingStep",
                        Description = "Delete Onboarding Step",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event100);
                }

                // Notification Events
                if (!dbContext.Event.Any(cm => cm.Name == "GetNotifications"))
                {
                    var event101 = new Event
                    {
                        Name = "GetNotifications",
                        Description = "Get Notifications",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event101);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "ReadNotification"))
                {
                    var event102 = new Event
                    {
                        Name = "ReadNotification", 
                        Description = "Read Notification",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event102);
                }

                // Inventory Events
                if (!dbContext.Event.Any(cm => cm.Name == "CreateInventory"))
                {
                    var event103 = new Event
                    {
                        Name = "CreateInventory",
                        Description = "Create Inventory",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event103);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "EditInventory"))
                {
                    var event104 = new Event
                    {
                        Name = "EditInventory",
                        Description = "Edit Inventory", 
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event104);
                }

                if (!dbContext.Event.Any(cm => cm.Name == "DeleteInventory"))
                {
                    var event105 = new Event
                    {
                        Name = "DeleteInventory",
                        Description = "Delete Inventory",
                        EventCategoryID = 1
                    };
                    dbContext.Event.Add(event105);
                }



          dbContext.SaveChanges();

            this.updateDatabaseVersion(this.GetType().Name);

            Console.WriteLine("Completed Executing " + this.GetType().Name);
        }

        //update the same row that it is complete the running 

    }
}
