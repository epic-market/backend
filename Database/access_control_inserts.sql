INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationTablesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationTablesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationTablesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationTablesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationTablesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationTablesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationTablesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationTablesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationTablesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationTablesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationTablesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationTablesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationConfigurationsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationConfigurationsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationConfigurationsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationConfigurationsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationConfigurationsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationConfigurationsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationConfigurationsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationConfigurationsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationConfigurationsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationConfigurationsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationConfigurationsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationConfigurationsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationSecurablesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationSecurablesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationSecurablesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationSecurablesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationSecurablesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationSecurablesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationSecurablesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationSecurablesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationSecurablesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationSecurablesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationSecurablesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ApplicationSecurablesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AccessControlListsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AccessControlListsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AccessControlListsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AccessControlListsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AccessControlListsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AccessControlListsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AccessControlListsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AccessControlListsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AccessControlListsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AccessControlListsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AccessControlListsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AccessControlListsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AccessTypesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AccessTypesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AccessTypesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AccessTypesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AccessTypesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AccessTypesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AccessTypesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AccessTypesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AccessTypesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AccessTypesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AccessTypesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AccessTypesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AppUsersView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AppUsersView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AppUsersView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AppUsersAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AppUsersAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AppUsersAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AppUsersEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AppUsersEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AppUsersEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AppUsersDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AppUsersDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AppUsersDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AppRolesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AppRolesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AppRolesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AppRolesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AppRolesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AppRolesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AppRolesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AppRolesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AppRolesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AppRolesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AppRolesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AppRolesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PersonTypesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PersonTypesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PersonTypesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PersonTypesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PersonTypesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PersonTypesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PersonTypesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PersonTypesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PersonTypesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PersonTypesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PersonTypesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PersonTypesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EntitiesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EntitiesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EntitiesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EntitiesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EntitiesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EntitiesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EntitiesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EntitiesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EntitiesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EntitiesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EntitiesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EntitiesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PagesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PagesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PagesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PagesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PagesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PagesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PagesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PagesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PagesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PagesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PagesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PagesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'HelpItemsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'HelpItemsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'HelpItemsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'HelpItemsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'HelpItemsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'HelpItemsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'HelpItemsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'HelpItemsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'HelpItemsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'HelpItemsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'HelpItemsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'HelpItemsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessCategoryInternalsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessCategoryInternalsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessCategoryInternalsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessCategoryInternalsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessCategoryInternalsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessCategoryInternalsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessCategoryInternalsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessCategoryInternalsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessCategoryInternalsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessCategoryInternalsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessCategoryInternalsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessCategoryInternalsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ProductInternalsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ProductInternalsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ProductInternalsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ProductInternalsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ProductInternalsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ProductInternalsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ProductInternalsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ProductInternalsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ProductInternalsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ProductInternalsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ProductInternalsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ProductInternalsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessEmployeeMapsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessEmployeeMapsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessEmployeeMapsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessEmployeeMapsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessEmployeeMapsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessEmployeeMapsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessEmployeeMapsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessEmployeeMapsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessEmployeeMapsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessEmployeeMapsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessEmployeeMapsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BusinessEmployeeMapsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OnboardingStepsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OnboardingStepsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OnboardingStepsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OnboardingStepsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OnboardingStepsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OnboardingStepsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OnboardingStepsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OnboardingStepsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OnboardingStepsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OnboardingStepsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OnboardingStepsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OnboardingStepsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'UserOnboardingProgressesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'UserOnboardingProgressesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'UserOnboardingProgressesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'UserOnboardingProgressesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'UserOnboardingProgressesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'UserOnboardingProgressesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'UserOnboardingProgressesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'UserOnboardingProgressesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'UserOnboardingProgressesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'UserOnboardingProgressesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'UserOnboardingProgressesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'UserOnboardingProgressesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'CatalogsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'CatalogsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'CatalogsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'CatalogsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'CatalogsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'CatalogsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'CatalogsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'CatalogsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'CatalogsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'CatalogsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'CatalogsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'CatalogsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletPersonsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletPersonsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletPersonsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletPersonsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletPersonsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletPersonsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletPersonsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletPersonsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletPersonsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletPersonsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletPersonsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletPersonsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletProductsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletProductsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletProductsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletProductsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletProductsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletProductsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletProductsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletProductsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletProductsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletProductsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletProductsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OutletProductsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrdersView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrdersView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrdersView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrdersAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrdersAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrdersAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrdersEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrdersEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrdersEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrdersDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrdersDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrdersDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderDetailsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderDetailsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderDetailsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderDetailsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderDetailsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderDetailsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderDetailsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderDetailsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderDetailsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderDetailsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderDetailsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderDetailsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderStatusOptionsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderStatusOptionsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderStatusOptionsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderStatusOptionsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderStatusOptionsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderStatusOptionsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderStatusOptionsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderStatusOptionsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderStatusOptionsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderStatusOptionsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderStatusOptionsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderStatusOptionsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderTypesOptionsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderTypesOptionsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderTypesOptionsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderTypesOptionsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderTypesOptionsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderTypesOptionsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderTypesOptionsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderTypesOptionsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderTypesOptionsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderTypesOptionsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderTypesOptionsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'OrderTypesOptionsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EventsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EventsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EventsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EventsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EventsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EventsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EventsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EventsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EventsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EventsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EventsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EventsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EventLogsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EventLogsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EventLogsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EventLogsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EventLogsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EventLogsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EventLogsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EventLogsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EventLogsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EventLogsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EventLogsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'EventLogsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'CommunicationQueuesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'CommunicationQueuesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'CommunicationQueuesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'CommunicationQueuesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'CommunicationQueuesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'CommunicationQueuesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'CommunicationQueuesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'CommunicationQueuesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'CommunicationQueuesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'CommunicationQueuesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'CommunicationQueuesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'CommunicationQueuesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ContactMethodsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ContactMethodsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ContactMethodsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ContactMethodsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ContactMethodsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ContactMethodsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ContactMethodsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ContactMethodsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ContactMethodsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ContactMethodsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ContactMethodsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'ContactMethodsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'NotificationsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'NotificationsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'NotificationsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'NotificationsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'NotificationsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'NotificationsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'NotificationsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'NotificationsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'NotificationsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'NotificationsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'NotificationsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'NotificationsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AttachmentTypesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AttachmentTypesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AttachmentTypesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AttachmentTypesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AttachmentTypesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AttachmentTypesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AttachmentTypesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AttachmentTypesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AttachmentTypesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AttachmentTypesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AttachmentTypesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AttachmentTypesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AttachmentsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AttachmentsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AttachmentsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AttachmentsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AttachmentsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AttachmentsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AttachmentsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AttachmentsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AttachmentsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AttachmentsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AttachmentsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'AttachmentsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BlogsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BlogsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BlogsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BlogsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BlogsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BlogsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BlogsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BlogsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BlogsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BlogsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BlogsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BlogsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BlogCategoriesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BlogCategoriesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BlogCategoriesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BlogCategoriesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BlogCategoriesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BlogCategoriesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BlogCategoriesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BlogCategoriesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BlogCategoriesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BlogCategoriesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BlogCategoriesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'BlogCategoriesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'FAQCategoriesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'FAQCategoriesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'FAQCategoriesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'FAQCategoriesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'FAQCategoriesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'FAQCategoriesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'FAQCategoriesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'FAQCategoriesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'FAQCategoriesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'FAQCategoriesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'FAQCategoriesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'FAQCategoriesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'FAQsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'FAQsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'FAQsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'FAQsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'FAQsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'FAQsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'FAQsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'FAQsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'FAQsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'FAQsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'FAQsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'FAQsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'SupportQuerysView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'SupportQuerysView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'SupportQuerysView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'SupportQuerysAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'SupportQuerysAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'SupportQuerysAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'SupportQuerysEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'SupportQuerysEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'SupportQuerysEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'SupportQuerysDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'SupportQuerysDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'SupportQuerysDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PromotionalLeadsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PromotionalLeadsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PromotionalLeadsView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PromotionalLeadsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PromotionalLeadsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PromotionalLeadsAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PromotionalLeadsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PromotionalLeadsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PromotionalLeadsEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PromotionalLeadsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PromotionalLeadsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'PromotionalLeadsDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TasksView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TasksView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TasksView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TasksAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TasksAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TasksAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TasksEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TasksEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TasksEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TasksDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TasksDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TasksDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TaskTypesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TaskTypesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TaskTypesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TaskTypesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TaskTypesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TaskTypesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TaskTypesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TaskTypesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TaskTypesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TaskTypesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TaskTypesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TaskTypesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TaskStatusTypesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TaskStatusTypesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TaskStatusTypesView'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TaskStatusTypesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TaskStatusTypesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TaskStatusTypesAdd'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TaskStatusTypesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TaskStatusTypesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TaskStatusTypesEdit'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'root'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TaskStatusTypesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'ReadWrite')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'admin'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TaskStatusTypesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);

INSERT INTO AccessControlLists (RoleID, SecurableID, AccessTypeID)
VALUES (
    (SELECT ID FROM AspNetRoles WHERE Name = 'support'),
    (SELECT ID FROM ApplicationSecurables WHERE Name = 'TaskStatusTypesDelete'),
    (SELECT ID FROM AccessTypes WHERE Name = 'Denied')
);