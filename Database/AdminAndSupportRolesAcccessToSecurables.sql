
-- Fetch Role IDs for 'Admin' and 'Support'
DECLARE @AdminRoleID INT, @SupportRoleID INT;

SELECT @AdminRoleID = ID FROM AspNetRoles WHERE NAME = 'admin';
SELECT @SupportRoleID = ID FROM AspNetRoles WHERE NAME = 'support';

-- Create a temporary table to store permissions
CREATE TABLE #TempPermissions (
    SecurableName NVARCHAR(255),
    AdminAccess NVARCHAR(50),
    SupportAccess NVARCHAR(50)
);

INSERT INTO #TempPermissions (SecurableName, AdminAccess, SupportAccess) VALUES
('ApplicationTablesView', 'Denied', 'Denied'),
('ApplicationTablesAdd', 'Denied', 'Denied'),
('ApplicationTablesEdit', 'Denied', 'Denied'),
('ApplicationTablesDelete', 'Denied', 'Denied'),
('ApplicationConfigurationsView', 'Denied', 'Denied'),
('ApplicationConfigurationsAdd', 'Denied', 'Denied'),
('ApplicationConfigurationsEdit', 'Denied', 'Denied'),
('ApplicationConfigurationsDelete', 'Denied', 'Denied'),
('ApplicationSecurablesView', 'Denied', 'Denied'),
('ApplicationSecurablesAdd', 'Denied', 'Denied'),
('ApplicationSecurablesEdit', 'Denied', 'Denied'),
('ApplicationSecurablesDelete', 'Denied', 'Denied'),
('AccessControlListsView', 'Denied', 'Denied'),
('AccessControlListsAdd', 'Denied', 'Denied'),
('AccessControlListsEdit', 'Denied', 'Denied'),
('AccessControlListsDelete', 'Denied', 'Denied'),
('AccessTypesView', 'Denied', 'Denied'),
('AccessTypesAdd', 'Denied', 'Denied'),
('AccessTypesEdit', 'Denied', 'Denied'),
('AccessTypesDelete', 'Denied', 'Denied'),
('AppUsersView', 'ReadWrite', 'Denied'),
('AppUsersAdd', 'ReadWrite', 'Denied'),
('AppUsersEdit', 'ReadWrite', 'Denied'),
('AppUsersDelete', 'Denied', 'Denied'),
('AppRolesView', 'ReadWrite', 'Denied'),
('AppRolesAdd', 'ReadWrite', 'Denied'),
('AppRolesEdit', 'ReadWrite', 'Denied'),
('AppRolesDelete', 'Denied', 'Denied'),
('PersonTypesView', 'Denied', 'Denied'),
('PersonTypesAdd', 'Denied', 'Denied'),
('PersonTypesEdit', 'Denied', 'Denied'),
('PersonTypesDelete', 'Denied', 'Denied'),
('EntitiesView', 'Denied', 'Denied'),
('EntitiesAdd', 'Denied', 'Denied'),
('EntitiesEdit', 'Denied', 'Denied'),
('EntitiesDelete', 'Denied', 'Denied'),
('PagesView', 'ReadWrite', 'Denied'),
('PagesAdd', 'ReadWrite', 'Denied'),
('PagesEdit', 'ReadWrite', 'Denied'),
('PagesDelete', 'Denied', 'Denied'),
('HelpItemsView', 'ReadWrite', 'Denied'),
('HelpItemsAdd', 'ReadWrite', 'Denied'),
('HelpItemsEdit', 'ReadWrite', 'Denied'),
('HelpItemsDelete', 'Denied', 'Denied'),
('BusinessCategoryInternalsView', 'Denied', 'Denied'),
('BusinessCategoryInternalsAdd', 'Denied', 'Denied'),
('BusinessCategoryInternalsEdit', 'Denied', 'Denied'),
('BusinessCategoryInternalsDelete', 'Denied', 'Denied'),
('ProductInternalsView', 'Denied', 'Denied'),
('ProductInternalsAdd', 'Denied', 'Denied'),
('ProductInternalsEdit', 'Denied', 'Denied'),
('ProductInternalsDelete', 'Denied', 'Denied'),
('BusinessesView', 'ReadWrite', 'Denied'),
('BusinessesAdd', 'ReadWrite', 'Denied'),
('BusinessesEdit', 'ReadWrite', 'Denied'),
('BusinessesDelete', 'Denied', 'Denied'),
('BusinessEmployeeMapsView', 'ReadWrite', 'Denied'),
('BusinessEmployeeMapsAdd', 'ReadWrite', 'Denied'),
('BusinessEmployeeMapsEdit', 'ReadWrite', 'Denied'),
('BusinessEmployeeMapsDelete', 'Denied', 'Denied'),
('OnboardingStepsView', 'ReadWrite', 'Denied'),
('OnboardingStepsAdd', 'ReadWrite', 'Denied'),
('OnboardingStepsEdit', 'ReadWrite', 'Denied'),
('OnboardingStepsDelete', 'Denied', 'Denied'),
('UserOnboardingProgressesView', 'ReadWrite', 'Denied'),
('UserOnboardingProgressesAdd', 'ReadWrite', 'Denied'),
('UserOnboardingProgressesEdit', 'ReadWrite', 'Denied'),
('UserOnboardingProgressesDelete', 'Denied', 'Denied'),
('CatalogsView', 'ReadWrite', 'Denied'),
('CatalogsAdd', 'ReadWrite', 'Denied'),
('CatalogsEdit', 'ReadWrite', 'Denied'),
('CatalogsDelete', 'Denied', 'Denied'),
('OutletsView', 'ReadWrite', 'Denied'),
('OutletsAdd', 'ReadWrite', 'Denied'),
('OutletsEdit', 'ReadWrite', 'Denied'),
('OutletsDelete', 'Denied', 'Denied'),
('OutletPersonsView', 'ReadWrite', 'Denied'),
('OutletPersonsAdd', 'ReadWrite', 'Denied'),
('OutletPersonsEdit', 'ReadWrite', 'Denied'),
('OutletPersonsDelete', 'Denied', 'Denied'),
('OutletProductsView', 'ReadWrite', 'Denied'),
('OutletProductsAdd', 'ReadWrite', 'Denied'),
('OutletProductsEdit', 'ReadWrite', 'Denied'),
('OutletProductsDelete', 'Denied', 'Denied'),
('OrdersView', 'ReadWrite', 'Denied'),
('OrdersAdd', 'ReadWrite', 'Denied'),
('OrdersEdit', 'ReadWrite', 'Denied'),
('OrdersDelete', 'Denied', 'Denied'),
('OrderDetailsView', 'ReadWrite', 'Denied'),
('OrderDetailsAdd', 'ReadWrite', 'Denied'),
('OrderDetailsEdit', 'ReadWrite', 'Denied'),
('OrderDetailsDelete', 'Denied', 'Denied'),
('OrderStatusOptionsView', 'Denied', 'Denied'),
('OrderStatusOptionsAdd', 'Denied', 'Denied'),
('OrderStatusOptionsEdit', 'Denied', 'Denied'),
('OrderStatusOptionsDelete', 'Denied', 'Denied'),
('OrderTypesOptionsView', 'Denied', 'Denied'),
('OrderTypesOptionsAdd', 'Denied', 'Denied'),
('OrderTypesOptionsEdit', 'Denied', 'Denied'),
('OrderTypesOptionsDelete', 'Denied', 'Denied'),
('EventsView', 'Denied', 'Denied'),
('EventsAdd', 'Denied', 'Denied'),
('EventsEdit', 'Denied', 'Denied'),
('EventsDelete', 'Denied', 'Denied'),
('EventLogsView', 'ReadWrite', 'Denied'),
('EventLogsAdd', 'ReadWrite', 'Denied'),
('EventLogsEdit', 'ReadWrite', 'Denied'),
('EventLogsDelete', 'Denied', 'Denied'),
('CommunicationQueuesView', 'Denied', 'Denied'),
('CommunicationQueuesAdd', 'Denied', 'Denied'),
('CommunicationQueuesEdit', 'Denied', 'Denied'),
('CommunicationQueuesDelete', 'Denied', 'Denied'),
('ContactMethodsView', 'Denied', 'Denied'),
('ContactMethodsAdd', 'Denied', 'Denied'),
('ContactMethodsEdit', 'Denied', 'Denied'),
('ContactMethodsDelete', 'Denied', 'Denied'),
('NotificationsView', 'ReadWrite', 'Denied'),
('NotificationsAdd', 'ReadWrite', 'Denied'),
('NotificationsEdit', 'ReadWrite', 'Denied'),
('NotificationsDelete', 'Denied', 'Denied'),
('AttachmentTypesView', 'Denied', 'Denied'),
('AttachmentTypesAdd', 'Denied', 'Denied'),
('AttachmentTypesEdit', 'Denied', 'Denied'),
('AttachmentTypesDelete', 'Denied', 'Denied'),
('AttachmentsView', 'ReadWrite', 'Denied'),
('AttachmentsAdd', 'ReadWrite', 'Denied'),
('AttachmentsEdit', 'ReadWrite', 'Denied'),
('AttachmentsDelete', 'Denied', 'Denied'),
('BlogsView', 'ReadWrite', 'Denied'),
('BlogsAdd', 'ReadWrite', 'Denied'),
('BlogsEdit', 'ReadWrite', 'Denied'),
('BlogsDelete', 'Denied', 'Denied'),
('BlogCategoriesView', 'ReadWrite', 'Denied'),
('BlogCategoriesAdd', 'ReadWrite', 'Denied'),
('BlogCategoriesEdit', 'ReadWrite', 'Denied'),
('BlogCategoriesDelete', 'Denied', 'Denied'),
('FAQCategoriesView', 'ReadWrite', 'Denied'),
('FAQCategoriesAdd', 'ReadWrite', 'Denied'),
('FAQCategoriesEdit', 'ReadWrite', 'Denied'),
('FAQCategoriesDelete', 'Denied', 'Denied'),
('FAQsView', 'ReadWrite', 'Denied'),
('FAQsAdd', 'ReadWrite', 'Denied'),
('FAQsEdit', 'ReadWrite', 'Denied'),
('FAQsDelete', 'Denied', 'Denied'),
('SupportQuerysView', 'ReadWrite', 'ReadWrite'),
('SupportQuerysAdd', 'ReadWrite', 'ReadWrite'),
('SupportQuerysEdit', 'ReadWrite', 'ReadWrite'),
('SupportQuerysDelete', 'Denied', 'Denied'),
('PromotionalLeadsView', 'ReadWrite', 'ReadWrite'),
('PromotionalLeadsAdd', 'ReadWrite', 'ReadWrite'),
('PromotionalLeadsEdit', 'ReadWrite', 'ReadWrite'),
('PromotionalLeadsDelete', 'Denied', 'Denied'),
('TasksView', 'ReadWrite', 'ReadWrite'),
('TasksAdd', 'ReadWrite', 'ReadWrite'),
('TasksEdit', 'ReadWrite', 'ReadWrite'),
('TasksDelete', 'Denied', 'Denied'),
('TaskTypesView', 'Denied', 'Denied'),
('TaskTypesAdd', 'Denied', 'Denied'),
('TaskTypesEdit', 'Denied', 'Denied'),
('TaskTypesDelete', 'Denied', 'Denied'),
('TaskStatusTypesView', 'Denied', 'Denied'),
('TaskStatusTypesAdd', 'Denied', 'Denied'),
('TaskStatusTypesEdit', 'Denied', 'Denied'),
('TaskStatusTypesDelete', 'Denied', 'Denied');

-- Update existing records for Admin role
UPDATE ACL
SET ACL.AccessTypeID = AT.ID
FROM AccessControlLists ACL
JOIN ApplicationSecurables S ON ACL.SecurableID = S.Id
JOIN #TempPermissions TP ON S.Name = TP.SecurableName
JOIN AccessTypes AT ON TP.AdminAccess = AT.Name
WHERE ACL.RoleID = @AdminRoleID AND TP.AdminAccess <> 'Denied';

-- Update existing records for Support role
UPDATE ACL
SET ACL.AccessTypeID = AT.ID
FROM AccessControlLists ACL
JOIN ApplicationSecurables S ON ACL.SecurableID = S.Id
JOIN #TempPermissions TP ON S.Name = TP.SecurableName
JOIN AccessTypes AT ON TP.SupportAccess = AT.Name
WHERE ACL.RoleID = @SupportRoleID AND TP.SupportAccess <> 'Denied';

-- Insert missing records for Admin role
INSERT INTO AccessControlLists (RoleID, AccessTypeID, SecurableID)
SELECT @AdminRoleID, AT.ID, S.Id
FROM ApplicationSecurables S
JOIN #TempPermissions TP ON S.Name = TP.SecurableName
JOIN AccessTypes AT ON TP.AdminAccess = AT.Name
WHERE TP.AdminAccess <> 'Denied'
AND NOT EXISTS (
    SELECT 1 FROM AccessControlLists ACL 
    WHERE ACL.RoleID = @AdminRoleID 
    AND ACL.SecurableID = S.Id
);

-- Insert missing records for Support role
INSERT INTO AccessControlLists (RoleID, AccessTypeID, SecurableID)
SELECT @SupportRoleID, AT.ID, S.Id
FROM ApplicationSecurables S
JOIN #TempPermissions TP ON S.Name = TP.SecurableName
JOIN AccessTypes AT ON TP.SupportAccess = AT.Name
WHERE TP.SupportAccess <> 'Denied'
AND NOT EXISTS (
    SELECT 1 FROM AccessControlLists ACL 
    WHERE ACL.RoleID = @SupportRoleID 
    AND ACL.SecurableID = S.Id
);

-- Drop the temporary table
DROP TABLE #TempPermissions;
