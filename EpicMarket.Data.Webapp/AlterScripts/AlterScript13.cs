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
    public class AlterScript13 : BaseAlterScript,IAlterScript
    {
        private readonly ApplicationDbContext dbContext;

        public AlterScript13(ApplicationDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }



        public void Execute()
        {

            Console.WriteLine("Started Executing " + this.GetType().Name);

          var securables = new List<ApplicationSecurables>
            {
                // ApplicationTables
                new ApplicationSecurables { Name = "ApplicationTablesView", Description = "Viewing data Application" },
                new ApplicationSecurables { Name = "ApplicationTablesAdd", Description = "Adding data Application" },
                new ApplicationSecurables { Name = "ApplicationTablesEdit", Description = "Editing data Application" },
                new ApplicationSecurables { Name = "ApplicationTablesDelete", Description = "Deleting data Application" },
                
                // ApplicationConfigurations
                new ApplicationSecurables { Name = "ApplicationConfigurationsView", Description = "Viewing Application Configurations" },
                new ApplicationSecurables { Name = "ApplicationConfigurationsAdd", Description = "Adding Application Configurations" },
                new ApplicationSecurables { Name = "ApplicationConfigurationsEdit", Description = "Editing Application Configurations" },
                new ApplicationSecurables { Name = "ApplicationConfigurationsDelete", Description = "Deleting Application Configurations" },
                
                // ApplicationSecurables
                new ApplicationSecurables { Name = "ApplicationSecurablesView", Description = "Viewing Application Securables" },
                new ApplicationSecurables { Name = "ApplicationSecurablesAdd", Description = "Adding Application Securables" },
                new ApplicationSecurables { Name = "ApplicationSecurablesEdit", Description = "Editing Application Securables" },
                new ApplicationSecurables { Name = "ApplicationSecurablesDelete", Description = "Deleting Application Securables" },
                
                // AccessControlLists
                new ApplicationSecurables { Name = "AccessControlListsView", Description = "Viewing Access Control Lists" },
                new ApplicationSecurables { Name = "AccessControlListsAdd", Description = "Adding Access Control Lists" },
                new ApplicationSecurables { Name = "AccessControlListsEdit", Description = "Editing Access Control Lists" },
                new ApplicationSecurables { Name = "AccessControlListsDelete", Description = "Deleting Access Control Lists" },
                
                // AccessTypes
                new ApplicationSecurables { Name = "AccessTypesView", Description = "Viewing Access Types" },
                new ApplicationSecurables { Name = "AccessTypesAdd", Description = "Adding Access Types" },
                new ApplicationSecurables { Name = "AccessTypesEdit", Description = "Editing Access Types" },
                new ApplicationSecurables { Name = "AccessTypesDelete", Description = "Deleting Access Types" },
                
                // AppUsers
                new ApplicationSecurables { Name = "AppUsersView", Description = "Viewing App Users" },
                new ApplicationSecurables { Name = "AppUsersAdd", Description = "Adding App Users" },
                new ApplicationSecurables { Name = "AppUsersEdit", Description = "Editing App Users" },
                new ApplicationSecurables { Name = "AppUsersDelete", Description = "Deleting App Users" },
                
                // AppRoles
                new ApplicationSecurables { Name = "AppRolesView", Description = "Viewing App Roles" },
                new ApplicationSecurables { Name = "AppRolesAdd", Description = "Adding App Roles" },
                new ApplicationSecurables { Name = "AppRolesEdit", Description = "Editing App Roles" },
                new ApplicationSecurables { Name = "AppRolesDelete", Description = "Deleting App Roles" },
                
                // PersonTypes
                new ApplicationSecurables { Name = "PersonTypesView", Description = "Viewing Person Types" },
                new ApplicationSecurables { Name = "PersonTypesAdd", Description = "Adding Person Types" },
                new ApplicationSecurables { Name = "PersonTypesEdit", Description = "Editing Person Types" },
                new ApplicationSecurables { Name = "PersonTypesDelete", Description = "Deleting Person Types" },
                
                // Entities
                new ApplicationSecurables { Name = "EntitiesView", Description = "Viewing Entities" },
                new ApplicationSecurables { Name = "EntitiesAdd", Description = "Adding Entities" },
                new ApplicationSecurables { Name = "EntitiesEdit", Description = "Editing Entities" },
                new ApplicationSecurables { Name = "EntitiesDelete", Description = "Deleting Entities" },
                
                // Pages
                new ApplicationSecurables { Name = "PagesView", Description = "Viewing Pages" },
                new ApplicationSecurables { Name = "PagesAdd", Description = "Adding Pages" },
                new ApplicationSecurables { Name = "PagesEdit", Description = "Editing Pages" },
                new ApplicationSecurables { Name = "PagesDelete", Description = "Deleting Pages" },
                
                // HelpItems
                new ApplicationSecurables { Name = "HelpItemsView", Description = "Viewing Help Items" },
                new ApplicationSecurables { Name = "HelpItemsAdd", Description = "Adding Help Items" },
                new ApplicationSecurables { Name = "HelpItemsEdit", Description = "Editing Help Items" },
                new ApplicationSecurables { Name = "HelpItemsDelete", Description = "Deleting Help Items" },
                
                // BusinessCategoryInternals
                new ApplicationSecurables { Name = "BusinessCategoryInternalsView", Description = "Viewing Business Category Internals" },
                new ApplicationSecurables { Name = "BusinessCategoryInternalsAdd", Description = "Adding Business Category Internals" },
                new ApplicationSecurables { Name = "BusinessCategoryInternalsEdit", Description = "Editing Business Category Internals" },
                new ApplicationSecurables { Name = "BusinessCategoryInternalsDelete", Description = "Deleting Business Category Internals" },
                
                // ProductInternals
                new ApplicationSecurables { Name = "ProductInternalsView", Description = "Viewing Product Internals" },
                new ApplicationSecurables { Name = "ProductInternalsAdd", Description = "Adding Product Internals" },
                new ApplicationSecurables { Name = "ProductInternalsEdit", Description = "Editing Product Internals" },
                new ApplicationSecurables { Name = "ProductInternalsDelete", Description = "Deleting Product Internals" },
                
                // Businesses
                new ApplicationSecurables { Name = "BusinessesView", Description = "Viewing Businesses" },
                new ApplicationSecurables { Name = "BusinessesAdd", Description = "Adding Businesses" },
                new ApplicationSecurables { Name = "BusinessesEdit", Description = "Editing Businesses" },
                new ApplicationSecurables { Name = "BusinessesDelete", Description = "Deleting Businesses" },
                
                // BusinessEmployeeMaps
                new ApplicationSecurables { Name = "BusinessEmployeeMapsView", Description = "Viewing Business Employee Maps" },
                new ApplicationSecurables { Name = "BusinessEmployeeMapsAdd", Description = "Adding Business Employee Maps" },
                new ApplicationSecurables { Name = "BusinessEmployeeMapsEdit", Description = "Editing Business Employee Maps" },
                new ApplicationSecurables { Name = "BusinessEmployeeMapsDelete", Description = "Deleting Business Employee Maps" },
                
                // OnboardingSteps
                new ApplicationSecurables { Name = "OnboardingStepsView", Description = "Viewing Onboarding Steps" },
                new ApplicationSecurables { Name = "OnboardingStepsAdd", Description = "Adding Onboarding Steps" },
                new ApplicationSecurables { Name = "OnboardingStepsEdit", Description = "Editing Onboarding Steps" },
                new ApplicationSecurables { Name = "OnboardingStepsDelete", Description = "Deleting Onboarding Steps" },
                
                // UserOnboardingProgresses
                new ApplicationSecurables { Name = "UserOnboardingProgressesView", Description = "Viewing User Onboarding Progresses" },
                new ApplicationSecurables { Name = "UserOnboardingProgressesAdd", Description = "Adding User Onboarding Progresses" },
                new ApplicationSecurables { Name = "UserOnboardingProgressesEdit", Description = "Editing User Onboarding Progresses" },
                new ApplicationSecurables { Name = "UserOnboardingProgressesDelete", Description = "Deleting User Onboarding Progresses" },
                
                // Products
                new ApplicationSecurables { Name = "ProductsView", Description = "Viewing Products" },
                new ApplicationSecurables { Name = "ProductsAdd", Description = "Adding Products" },
                new ApplicationSecurables { Name = "ProductsEdit", Description = "Editing Products" },
                new ApplicationSecurables { Name = "ProductsDelete", Description = "Deleting Products" },
                
                // Outlets
                new ApplicationSecurables { Name = "OutletsView", Description = "Viewing Outlets" },
                new ApplicationSecurables { Name = "OutletsAdd", Description = "Adding Outlets" },
                new ApplicationSecurables { Name = "OutletsEdit", Description = "Editing Outlets" },
                new ApplicationSecurables { Name = "OutletsDelete", Description = "Deleting Outlets" },
                
                // OutletPersons
                new ApplicationSecurables { Name = "OutletPersonsView", Description = "Viewing Outlet Persons" },
                new ApplicationSecurables { Name = "OutletPersonsAdd", Description = "Adding Outlet Persons" },
                new ApplicationSecurables { Name = "OutletPersonsEdit", Description = "Editing Outlet Persons" },
                new ApplicationSecurables { Name = "OutletPersonsDelete", Description = "Deleting Outlet Persons" },
                
                // OutletProducts
                new ApplicationSecurables { Name = "OutletProductsView", Description = "Viewing Outlet Products" },
                new ApplicationSecurables { Name = "OutletProductsAdd", Description = "Adding Outlet Products" },
                new ApplicationSecurables { Name = "OutletProductsEdit", Description = "Editing Outlet Products" },
                new ApplicationSecurables { Name = "OutletProductsDelete", Description = "Deleting Outlet Products" },
                
                // Orders
                new ApplicationSecurables { Name = "OrdersView", Description = "Viewing Orders" },
                new ApplicationSecurables { Name = "OrdersAdd", Description = "Adding Orders" },
                new ApplicationSecurables { Name = "OrdersEdit", Description = "Editing Orders" },
                new ApplicationSecurables { Name = "OrdersDelete", Description = "Deleting Orders" },
                
                // OrderDetails
                new ApplicationSecurables { Name = "OrderDetailsView", Description = "Viewing Order Details" },
                new ApplicationSecurables { Name = "OrderDetailsAdd", Description = "Adding Order Details" },
                new ApplicationSecurables { Name = "OrderDetailsEdit", Description = "Editing Order Details" },
                new ApplicationSecurables { Name = "OrderDetailsDelete", Description = "Deleting Order Details" },
                
                // OrderStatusOptions
                new ApplicationSecurables { Name = "OrderStatusOptionsView", Description = "Viewing Order Status Options" },
                new ApplicationSecurables { Name = "OrderStatusOptionsAdd", Description = "Adding Order Status Options" },
                new ApplicationSecurables { Name = "OrderStatusOptionsEdit", Description = "Editing Order Status Options" },
                new ApplicationSecurables { Name = "OrderStatusOptionsDelete", Description = "Deleting Order Status Options" },
                
                // OrderTypesOptions
                new ApplicationSecurables { Name = "OrderTypesOptionsView", Description = "Viewing Order Types Options" },
                new ApplicationSecurables { Name = "OrderTypesOptionsAdd", Description = "Adding Order Types Options" },
                new ApplicationSecurables { Name = "OrderTypesOptionsEdit", Description = "Editing Order Types Options" },
                new ApplicationSecurables { Name = "OrderTypesOptionsDelete", Description = "Deleting Order Types Options" },
                
                // Events
                new ApplicationSecurables { Name = "EventsView", Description = "Viewing Events" },
                new ApplicationSecurables { Name = "EventsAdd", Description = "Adding Events" },
                new ApplicationSecurables { Name = "EventsEdit", Description = "Editing Events" },
                new ApplicationSecurables { Name = "EventsDelete", Description = "Deleting Events" },
                
                // EventLogs
                new ApplicationSecurables { Name = "EventLogsView", Description = "Viewing Event Logs" },
                new ApplicationSecurables { Name = "EventLogsAdd", Description = "Adding Event Logs" },
                new ApplicationSecurables { Name = "EventLogsEdit", Description = "Editing Event Logs" },
                new ApplicationSecurables { Name = "EventLogsDelete", Description = "Deleting Event Logs" },
                
                // CommunicationQueues
                new ApplicationSecurables { Name = "CommunicationQueuesView", Description = "Viewing Communication Queues" },
                new ApplicationSecurables { Name = "CommunicationQueuesAdd", Description = "Adding Communication Queues" },
                new ApplicationSecurables { Name = "CommunicationQueuesEdit", Description = "Editing Communication Queues" },
                new ApplicationSecurables { Name = "CommunicationQueuesDelete", Description = "Deleting Communication Queues" },
                
                // ContactMethods
                new ApplicationSecurables { Name = "ContactMethodsView", Description = "Viewing Contact Methods" },
                new ApplicationSecurables { Name = "ContactMethodsAdd", Description = "Adding Contact Methods" },
                new ApplicationSecurables { Name = "ContactMethodsEdit", Description = "Editing Contact Methods" },
                new ApplicationSecurables { Name = "ContactMethodsDelete", Description = "Deleting Contact Methods" },
                
                // Notifications
                new ApplicationSecurables { Name = "NotificationsView", Description = "Viewing Notifications" },
                new ApplicationSecurables { Name = "NotificationsAdd", Description = "Adding Notifications" },
                new ApplicationSecurables { Name = "NotificationsEdit", Description = "Editing Notifications" },
                new ApplicationSecurables { Name = "NotificationsDelete", Description = "Deleting Notifications" },
                
                // AttachmentTypes
                new ApplicationSecurables { Name = "AttachmentTypesView", Description = "Viewing Attachment Types" },
                new ApplicationSecurables { Name = "AttachmentTypesAdd", Description = "Adding Attachment Types" },
                new ApplicationSecurables { Name = "AttachmentTypesEdit", Description = "Editing Attachment Types" },
                new ApplicationSecurables { Name = "AttachmentTypesDelete", Description = "Deleting Attachment Types" },
                
                // Attachments
                new ApplicationSecurables { Name = "AttachmentsView", Description = "Viewing Attachments" },
                new ApplicationSecurables { Name = "AttachmentsAdd", Description = "Adding Attachments" },
                new ApplicationSecurables { Name = "AttachmentsEdit", Description = "Editing Attachments" },
                new ApplicationSecurables { Name = "AttachmentsDelete", Description = "Deleting Attachments" },
                
                // Blogs
                new ApplicationSecurables { Name = "BlogsView", Description = "Viewing Blogs" },
                new ApplicationSecurables { Name = "BlogsAdd", Description = "Adding Blogs" },
                new ApplicationSecurables { Name = "BlogsEdit", Description = "Editing Blogs" },
                new ApplicationSecurables { Name = "BlogsDelete", Description = "Deleting Blogs" },
                
                // BlogCategories
                new ApplicationSecurables { Name = "BlogCategoriesView", Description = "Viewing Blog Categories" },
                new ApplicationSecurables { Name = "BlogCategoriesAdd", Description = "Adding Blog Categories" },
                new ApplicationSecurables { Name = "BlogCategoriesEdit", Description = "Editing Blog Categories" },
                new ApplicationSecurables { Name = "BlogCategoriesDelete", Description = "Deleting Blog Categories" },
                
                // FAQCategories
                new ApplicationSecurables { Name = "FAQCategoriesView", Description = "Viewing FAQ Categories" },
                new ApplicationSecurables { Name = "FAQCategoriesAdd", Description = "Adding FAQ Categories" },
                new ApplicationSecurables { Name = "FAQCategoriesEdit", Description = "Editing FAQ Categories" },
                new ApplicationSecurables { Name = "FAQCategoriesDelete", Description = "Deleting FAQ Categories" },
                
                // FAQs
                new ApplicationSecurables { Name = "FAQsView", Description = "Viewing FAQs" },
                new ApplicationSecurables { Name = "FAQsAdd", Description = "Adding FAQs" },
                new ApplicationSecurables { Name = "FAQsEdit", Description = "Editing FAQs" },
                new ApplicationSecurables { Name = "FAQsDelete", Description = "Deleting FAQs" },
                
                // SupportQuerys
                new ApplicationSecurables { Name = "SupportQuerysView", Description = "Viewing Support Queries" },
                new ApplicationSecurables { Name = "SupportQuerysAdd", Description = "Adding Support Queries" },
                new ApplicationSecurables { Name = "SupportQuerysEdit", Description = "Editing Support Queries" },
                new ApplicationSecurables { Name = "SupportQuerysDelete", Description = "Deleting Support Queries" },
                
                // PromotionalLeads
                new ApplicationSecurables { Name = "PromotionalLeadsView", Description = "Viewing Promotional Leads" },
                new ApplicationSecurables { Name = "PromotionalLeadsAdd", Description = "Adding Promotional Leads" },
                new ApplicationSecurables { Name = "PromotionalLeadsEdit", Description = "Editing Promotional Leads" },
                new ApplicationSecurables { Name = "PromotionalLeadsDelete", Description = "Deleting Promotional Leads" },
                
                // Tasks
                new ApplicationSecurables { Name = "TasksView", Description = "Viewing Tasks" },
                new ApplicationSecurables { Name = "TasksAdd", Description = "Adding Tasks" },
                new ApplicationSecurables { Name = "TasksEdit", Description = "Editing Tasks" },
                new ApplicationSecurables { Name = "TasksDelete", Description = "Deleting Tasks" },
                
                // TaskTypes
                new ApplicationSecurables { Name = "TaskTypesView", Description = "Viewing Task Types" },
                new ApplicationSecurables { Name = "TaskTypesAdd", Description = "Adding Task Types" },
                new ApplicationSecurables { Name = "TaskTypesEdit", Description = "Editing Task Types" },
                new ApplicationSecurables { Name = "TaskTypesDelete", Description = "Deleting Task Types" },
                
                // TaskStatusTypes
                new ApplicationSecurables { Name = "TaskStatusTypesView", Description = "Viewing Task Status Types" },
                new ApplicationSecurables { Name = "TaskStatusTypesAdd", Description = "Adding Task Status Types" },
                new ApplicationSecurables { Name = "TaskStatusTypesEdit", Description = "Editing Task Status Types" },
                new ApplicationSecurables { Name = "TaskStatusTypesDelete", Description = "Deleting Task Status Types" },
            };

            foreach (var securable in securables)
            {
                if (!dbContext.ApplicationSecurables.Any(cm => cm.Name == securable.Name))
                {
                    dbContext.ApplicationSecurables.Add(securable);
                }
            }

            dbContext.SaveChanges();

            this.updateDatabaseVersion(this.GetType().Name);

            Console.WriteLine("Completed Executing " + this.GetType().Name);
        }

        //update the same row that it is complete the running 

    }
}
