-- Fetch the AccessTypeID for 'ReadWrite'
DECLARE @ReadWriteAccessTypeID INT;
SELECT @ReadWriteAccessTypeID = ID FROM AccessTypes WHERE NAME = 'ReadWrite';

-- Define the root user RoleID
DECLARE @RootRoleID INT;
SELECT @RootRoleID = ID FROM AspNetRoles WHERE NAME = 'root';

-- Update existing records to 'ReadWrite' access
UPDATE AccessControlLists
SET AccessTypeID = @ReadWriteAccessTypeID
WHERE RoleID = @RootRoleID
AND SecurableID IN (SELECT Id FROM ApplicationSecurables WHERE IsActive = 1);

-- Insert missing records
INSERT INTO AccessControlLists (RoleID, AccessTypeID, SecurableID)
SELECT @RootRoleID, @ReadWriteAccessTypeID, A.Id 
FROM ApplicationSecurables A
WHERE A.IsActive = 1
AND NOT EXISTS (
    SELECT 1 FROM AccessControlLists ACL 
    WHERE ACL.RoleID = @RootRoleID 
    AND ACL.SecurableID = A.Id
);



SELECT COUNT(*) From AccessControlLists