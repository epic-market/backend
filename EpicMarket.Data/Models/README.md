# EpicMarket Data Models Documentation

This document provides a comprehensive overview of all Entity Framework models used in the EpicMarket application.

---

## Table of Contents

1. [Base Model](#base-model)
2. [User & Authentication](#user--authentication)
3. [Business & Outlets](#business--outlets)
4. [Products & Catalog](#products--catalog)
5. [Orders](#orders)
6. [Inventory](#inventory)
7. [Communication](#communication)
8. [Tasks & Support](#tasks--support)
9. [Attachments & Comments](#attachments--comments)
10. [Events & Logging](#events--logging)
11. [Content Management](#content-management)
12. [Finance & Payments](#finance--payments)
13. [Miscellaneous](#miscellaneous)

---

## Base Model

### `BaseModel`
Common base class inherited by most models providing audit fields.

| Property | Type | Description |
|----------|------|-------------|
| `CreateDate` | `DateTime` | Record creation timestamp (default: `DateTime.Now`) |
| `CreateBy` | `string` | User who created the record (default: "System") |
| `ModifiedDate` | `DateTime?` | Last modification timestamp |
| `ModifiedBy` | `string?` | User who last modified the record |
| `IsActive` | `bool` | Soft delete flag (default: `true`) |

---

## User & Authentication

### `AppUser`
Extends `IdentityUser<int>` for user management.

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `int` | Primary key (inherited) |
| `FirstName` | `string` | User's first name |
| `LastName` | `string` | User's last name |
| `UniqueGuid` | `string` | Unique identifier GUID |
| `OTP` | `int` | One-time password |
| `IsActive` | `bool` | Active status |
| `LastActive` | `DateTime` | Last activity timestamp |

**Navigation Properties:**
- `Businesses` → `ICollection<Business>`
- `Orders` → `ICollection<Order>`
- `UserRoles` → `ICollection<AppUserRole>`
- `OutletPeople` → `ICollection<OutletPerson>`
- `SupportTickets` → `ICollection<SupportTicket>`
- `UserAddresses` → `ICollection<UserAddress>`
- `BusinessEmployeeMaps` → `ICollection<BusinessEmployeeMap>`
- `Tasks` → `ICollection<Tasks>`
- `Notifications` → `ICollection<Notification>`
- `UserOnboardingProgresses` → `ICollection<UserOnboardingProgress>`
- `Ratings` → `ICollection<Rating>`
- `Subscriptions` → `ICollection<Subscription>`

---

### `AppRole`
Extends `IdentityRole<int>` for role management.

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `int` | Primary key (inherited) |

**Navigation Properties:**
- `UserRoles` → `ICollection<AppUserRole>`
- `AccessControlLists` → `ICollection<AccessControlList>`

---

### `AppUserRole`
Extends `IdentityUserRole<int>` - junction table for User-Role relationship.

| Property | Type | Description |
|----------|------|-------------|
| `UserId` | `int` | Foreign key to AppUser (inherited) |
| `RoleId` | `int` | Foreign key to AppRole (inherited) |

**Navigation Properties:**
- `User` → `AppUser`
- `Roles` → `AppRole`

---

### `UserAddress`
Junction table linking users to their addresses.

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `int` | Primary key |
| `UserId` | `int` | Foreign key to AppUser |
| `AddressId` | `int` | Foreign key to Address |

**Navigation Properties:**
- `User` → `AppUser`
- `Address` → `Address`

---

### `UserOnboardingProgress`
Tracks user onboarding completion status.

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `int` | Primary key |
| `UserID` | `int` | Foreign key to AppUser |
| `StepID` | `int` | Foreign key to OnboardingStep |
| `CompletedAt` | `DateTime?` | Completion timestamp |
| `IsCompleted` | `bool` | Completion status (default: `false`) |

**Navigation Properties:**
- `User` → `AppUser`
- `Step` → `OnboardingStep`

---

### `OTPVerification`
Stores OTP verification records.

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `Username` | `string` | Username for verification |
| `OTP` | `string` | One-time password |
| `ReferenceID` | `string` | Reference identifier |
| `ExpirationTime` | `DateTime` | OTP expiration timestamp |
| `IsVerified` | `bool` | Verification status |
| `Type` | `string` | Type: EMAIL/PHONE |
| `IsActive` | `bool` | Active status |
| `CreateBy` | `string` | Creator |
| `CreateDate` | `DateTime` | Creation timestamp |

---

## Business & Outlets

### `Business`
Represents a business entity. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `PersonID` | `int` | Foreign key to owner (AppUser) |
| `StatusId` | `int?` | Foreign key to StatusOptionSet |
| `BusinessCategoryID` | `int` | Foreign key to BusinessCategoryInternal |
| `Name` | `string` | Business name |
| `Description` | `string` | Business description |
| `Banner` | `string?` | Banner image URL |
| `Logo` | `string?` | Logo image URL |
| `ContactNumber` | `long` | Contact phone number |
| `ContactEmail` | `string` | Contact email |
| `AddressID` | `int?` | Foreign key to Address |
| `Rating` | `int?` | Average rating |
| `ReviewCount` | `int?` | Total review count |
| `IsOpen` | `bool` | Open/closed status |
| `Weight` | `double?` | Search weight/priority |

**Navigation Properties:**
- `Person` → `AppUser`
- `BusinessCategory` → `BusinessCategoryInternal`
- `Address` → `Address`
- `Status` → `StatusOptionSet`
- `BusinessEmployees` → `ICollection<BusinessEmployeeMap>`
- `Categories` → `ICollection<CatalogCategory>`

---

### `BusinessCategoryInternal`
Business category/type classification. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `Name` | `string` | Category name |
| `Description` | `string` | Category description |
| `Type` | `string` | Category type |

**Navigation Properties:**
- `Businesses` → `ICollection<Business>`

---

### `BusinessEmployeeMap`
Junction table linking businesses to employees. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `BussinessID` | `int` | Foreign key to Business |
| `EmployeeID` | `int` | Foreign key to AppUser |

**Navigation Properties:**
- `Employee` → `AppUser`
- `Bussiness` → `Business`

---

### `Outlet`
Represents a business outlet/branch. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `BussinessID` | `int` | Foreign key to Business |
| `AddressID` | `int` | Foreign key to Address |
| `Name` | `string` | Outlet name (max 255 chars) |
| `Description` | `string` | Outlet description |
| `TimingList` | `string` | JSON array of operating hours |
| `SocialMediaLinkFacebook` | `string` | Facebook URL |
| `SocialMediaLinkInstagram` | `string` | Instagram URL |
| `SocialMediaLinkTwitter` | `string` | Twitter URL |
| `SocialMediaLinkYoutube` | `string` | YouTube URL |
| `SpecialNoteOfTheDay` | `string` | Daily special note |
| `ContactNumber` | `long` | Contact phone |
| `ContactEmail` | `string` | Contact email |
| `Rating` | `double?` | Average rating |
| `ReviewCount` | `int?` | Review count |
| `IsOpen` | `bool` | Open status |
| `Weight` | `double?` | Search weight |
| `StatusId` | `int` | Foreign key to StatusOptionSet |

**Navigation Properties:**
- `Bussiness` → `Business`
- `Address` → `Address`
- `StatusOptionSets` → `StatusOptionSet`
- `OutletPeople` → `ICollection<OutletPerson>`
- `Orders` → `ICollection<Order>`
- `Inventory` → `ICollection<Inventory>`
- `Ratings` → `ICollection<Rating>`
- `Subscriptions` → `ICollection<Subscription>`
- `MerchantFinances` → `MerchantFinance`

---

### `OutletPerson`
Junction table linking outlets to staff members.

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `PersonId` | `int` | Foreign key to AppUser |
| `OutletId` | `int` | Foreign key to Outlet |

**Navigation Properties:**
- `Person` → `AppUser`
- `Outlet` → `Outlet`

---

### `Address`
Physical address information. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `int` | Primary key |
| `Address1` | `string` | Primary address line |
| `Address2` | `string?` | Secondary address line |
| `State` | `string` | State (max 50 chars) |
| `City` | `string` | City (max 50 chars) |
| `Pincode` | `int` | Postal code |
| `Latitude` | `double` | GPS latitude |
| `Longitude` | `double` | GPS longitude |
| `MetaData` | `string?` | Additional metadata |

**Navigation Properties:**
- `Persons` → `ICollection<AppUser>`
- `Businesses` → `ICollection<Business>`
- `Orders` → `ICollection<Order>`
- `Outlets` → `ICollection<Outlet>`
- `UserAddresses` → `ICollection<UserAddress>`

---

## Products & Catalog

### `Catalog` (Table: `Product`)
Represents a product. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `BusinessID` | `int` | Foreign key to Business |
| `Name` | `string` | Product name (max 50 chars) |
| `Description` | `string` | Product description |
| `CategoryID` | `int?` | Foreign key to CatalogCategory |
| `IsRecommended` | `bool` | Recommended flag |
| `Rating` | `double?` | Average rating |
| `ReviewCount` | `int?` | Review count |
| `OrderCount` | `int?` | Total order count |
| `RequiresRefrigeration` | `bool` | Cold storage requirement |
| `BaseHightlights` | `string` | Base product highlights |
| `VariantOptions` | `string` | Variant configuration JSON |
| `StatusId` | `int` | Foreign key to StatusOptionSet |

**Navigation Properties:**
- `Business` → `Business`
- `StatusOptionSets` → `StatusOptionSet`
- `Category` → `CatalogCategory`
- `Ratings` → `ICollection<Rating>`
- `ProductVariants` → `ICollection<CatalogVariants>`

---

### `CatalogVariants` (Table: `ProductVariants`)
Product variant details. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `ProductID` | `int` | Foreign key to Catalog |
| `SKU` | `string` | Stock keeping unit (max 50 chars) |
| `Barcode` | `string` | Product barcode |
| `Attributes` | `string` | JSON attributes (e.g., `{"Size": "XL", "Color": "Red"}`) |
| `CostPrice` | `double` | Purchase cost price |
| `SalePrice` | `double` | Selling price |
| `CompareAtPrice` | `double?` | Compare/original price |
| `AdditionalHightlights` | `string` | Variant-specific highlights |
| `MaximumOrderQuantity` | `int?` | Max order quantity |
| `MinimumOrderQuantity` | `int?` | Min order quantity |
| `PackedHeight` | `double?` | Package height |
| `PackedWidth` | `double?` | Package width |
| `PackedDepth` | `double?` | Package depth |
| `WeightUnit` | `string` | Weight unit (kg, g, lbs, oz) |
| `Weight` | `double?` | Weight value |
| `IsDefaultVariant` | `bool` | Default variant flag |

**Navigation Properties:**
- `Product` → `Catalog`
- `Inventory` → `ICollection<Inventory>`
- `OrderDetails` → `ICollection<OrderDetail>`

---

### `CatalogCategory` (Table: `ProductCategory`)
Product categories. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `BusinessID` | `int` | Foreign key to Business |
| `Name` | `string` | Category name |
| `Description` | `string` | Category description |
| `ParentID` | `int?` | Parent category ID (for hierarchy) |

**Navigation Properties:**
- `Product` → `ICollection<Catalog>`
- `Business` → `Business`

---

### `ProductInternal`
Internal product reference. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `BarCode` | `string` | Product barcode |
| `Name` | `string` | Product name |
| `Description` | `string` | Product description |
| `PackedHeight` | `double` | Package height |
| `PackedWidhth` | `double` | Package width |
| `PackedDepth` | `double` | Package depth |
| `Weight` | `double` | Product weight |
| `CostPrice` | `double` | Cost price |
| `Rate` | `double` | Rate/price |

---

## Orders

### `Order`
Customer order. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `PersonID` | `int` | Foreign key to AppUser (customer) |
| `OutletID` | `int` | Foreign key to Outlet |
| `OrderTypeId` | `int` | Foreign key to OrderTypesOptions |
| `TotalPrice` | `double` | Order total value |
| `TotalItems` | `int` | Total item quantity |
| `OrderAt` | `DateTime` | Order timestamp |
| `StatusId` | `int` | Foreign key to OrderStatusOptions |
| `PaymentMode` | `string` | Payment method (cash, online) |
| `AddressID` | `int?` | Foreign key to delivery Address |

**Navigation Properties:**
- `Person` → `AppUser`
- `Outlet` → `Outlet`
- `Address` → `Address`
- `OrderStatusOptions` → `OrderStatusOptions`
- `OrderTypesOptions` → `OrderTypesOptions`
- `OrderDetails` → `ICollection<OrderDetail>`

---

### `OrderDetail`
Order line items. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `OrderID` | `int` | Foreign key to Order |
| `VariantID` | `int` | Foreign key to CatalogVariants |
| `Quantity` | `int` | Item quantity |
| `Rate` | `double` | Unit price |
| `TotalPrice` | `double` | Line total |

**Navigation Properties:**
- `Order` → `Order`
- `ProductVariants` → `CatalogVariants`

---

### `OrderStatusOptions`
Order status lookup table.

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `int` | Primary key |
| `OrderStatus` | `string` | Status name (e.g., delivered, packing) |

**Navigation Properties:**
- `Orders` → `ICollection<Order>`

---

### `OrderTypesOptions`
Order type lookup table.

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `int` | Primary key |
| `Ordertype` | `string` | Type name (online, offline) |

**Navigation Properties:**
- `Orders` → `ICollection<Order>`

---

## Inventory

### `Inventory`
Stock management for product variants at outlets.

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `OutletID` | `int` | Foreign key to Outlet |
| `ProductVariantID` | `int` | Foreign key to CatalogVariants |
| `TrackInventory` | `bool` | Enable inventory tracking |
| `IsInStock` | `bool` | Manual stock status override |
| `QuantityAvailable` | `int?` | Current stock level |
| `MinimumStockLevel` | `int?` | Minimum stock threshold |
| `MaximumStockLevel` | `int?` | Maximum stock level |
| `ReorderPoint` | `int?` | Reorder trigger level |
| `BackOrders` | `bool` | Allow backorders when out of stock |

**Navigation Properties:**
- `ProductVariants` → `CatalogVariants`
- `Outlet` → `Outlet`

---

## Communication

### `CommunicationQueue`
Queue for outgoing communications. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `ContactMethodID` | `int?` | Foreign key to ContactMethod |
| `MessageData` | `string` | Message body (HTML for email) |
| `Subject` | `string` | Message subject (max 255 chars) |
| `MessageText` | `string` | Plain text message (for SMS) |
| `RetryCount` | `int` | Retry attempts (default: 0) |
| `ScheduledDate` | `DateTime?` | Scheduled send time |
| `NotificationRecipient` | `string` | Recipient address |
| `CommunicationStatusId` | `int?` | Foreign key to CommunicationStatus |
| `TemplateName` | `string` | Template name used |
| `SentDate` | `DateTime?` | Successful send timestamp |
| `ErrorMessage` | `string` | Error details |
| `SysStartTime` | `DateTime?` | System start time (computed) |
| `SysEndTime` | `DateTime?` | System end time (computed) |

**Navigation Properties:**
- `ContactMethod` → `ContactMethod`
- `CommunicationStatus` → `CommunicationStatus`

---

### `CommunicationStatus`
Communication status lookup. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `Name` | `string` | Status name (max 50 chars) |
| `Description` | `string` | Status description (max 255 chars) |

---

### `ContactMethod`
Contact method types (email, phone, etc.). *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `Name` | `string` | Method name (max 50 chars) |
| `Description` | `string` | Method description (max 255 chars) |

**Navigation Properties:**
- `CommunicationQueues` → `ICollection<CommunicationQueue>`

---

### `Notification`
User notifications.

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `int` | Primary key |
| `Message` | `string` | Notification message |
| `DateCreated` | `DateTime` | Creation timestamp |
| `IsRead` | `bool` | Read status |
| `UserId` | `int` | Foreign key to AppUser |
| `ContactMethodId` | `int` | Foreign key to ContactMethod |
| `PageId` | `int` | Foreign key to Page |

**Navigation Properties:**
- `User` → `AppUser`
- `ContactMethod` → `ContactMethod`
- `Page` → `Page`

---

## Tasks & Support

### `Tasks`
Task management. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `Name` | `string` | Task name (max 200 chars) |
| `Description` | `string` | Task description/comment |
| `TaskTypeID` | `int?` | Foreign key to TaskType |
| `ParentID` | `int?` | Parent task ID |
| `TaskStatusID` | `int?` | Foreign key to TaskStatusType |
| `TaskPriorityID` | `int?` | Priority level |
| `TaskEntityID` | `int?` | Foreign key to Entity |
| `PrimaryAssignedToPersonID` | `int?` | Foreign key to AppUser (assignee) |
| `DateAssigned` | `DateTime?` | Assignment date |
| `DateDue` | `DateTime?` | Due date |
| `DateStarted` | `DateTime?` | Start date |
| `DateCompleted` | `DateTime?` | Completion date |
| `SubmittedByPersonID` | `int?` | Submitter's AppUser ID |
| `TaskData` | `string` | Additional task data |
| `ReceivedDate` | `DateTime?` | Received date |

**Navigation Properties:**
- `TaskTypes` → `TaskType`
- `TaskStatusType` → `TaskStatusType`
- `Entity` → `Entity`
- `AppUser` → `AppUser`
- `SupportTickets` → `ICollection<SupportTicket>`

---

### `TaskType`
Task type classification. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `Name` | `string` | Type name (max 50 chars) |
| `Description` | `string` | Type description (max 255 chars) |
| `TaskCategoryID` | `int?` | Foreign key to ApplicationsTable |
| `DefaultDueDateHours` | `int?` | Default due date hours |
| `ShortDescription` | `string` | Short description (max 20 chars) |

**Navigation Properties:**
- `EventCategorys` → `ApplicationsTable`
- `Tasks` → `ICollection<Tasks>`
- `SupportQuerys` → `ICollection<SupportQueries>`

---

### `TaskStatusType`
Task status lookup. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `Status` | `string` | Status name (max 255 chars) |
| `StatusDescription` | `string` | Status description (max 500 chars) |

**Navigation Properties:**
- `Tasks` → `ICollection<Tasks>`

---

### `SupportTicket`
Customer support tickets. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `Email` | `string` | Contact email |
| `Phonenumber` | `string` | Contact phone |
| `Fullname` | `string` | Full name |
| `TypeofPersonid` | `int` | Foreign key to PersonType |
| `Taskid` | `int?` | Foreign key to Tasks |

**Navigation Properties:**
- `Tasks` → `Tasks`
- `PersonType` → `PersonType`

---

### `SupportQueries`
Predefined support query templates. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `Query` | `string` | Query text (max 500 chars) |
| `TaskTypeID` | `int?` | Foreign key to TaskType |
| `TypeofPersonid` | `int?` | Foreign key to PersonType |

**Navigation Properties:**
- `PersonType` → `PersonType`
- `TaskTypes` → `TaskType`

---

### `PersonType`
Person type classification.

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `Type` | `string` | Type name |
| `Description` | `string` | Type description |

**Navigation Properties:**
- `SupportTickets` → `ICollection<SupportTicket>`
- `SupportQuerys` → `ICollection<SupportQueries>`

---

## Attachments & Comments

### `Attachment`
File attachments. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `Name` | `string` | Attachment name (max 255 chars) |
| `Comment` | `string` | Attachment comment |
| `DocumentType` | `string` | Document type (max 100 chars) |
| `DocumentFileType` | `string` | File type (max 100 chars) |
| `DocumentFolderPath` | `string` | Storage path (max 500 chars) |
| `DocumentFile` | `string` | File content/reference |
| `EntityId` | `int?` | Foreign key to Entity |
| `RecordId` | `int?` | Related record ID |

**Navigation Properties:**
- `AttachmentLinks` → `ICollection<AttachmentLink>`
- `Entity` → `Entity`

---

### `AttachmentLink`
Links attachments to entities. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `AttachmentID` | `int` | Foreign key to Attachment |
| `EntityID` | `int` | Foreign key to Entity |
| `AttachmentTypeID` | `int` | Foreign key to AttachmentType |
| `RecordID` | `int` | Related record ID |

**Navigation Properties:**
- `Attachments` → `Attachment`
- `Entity` → `Entity`
- `AttachmentType` → `AttachmentType`

---

### `AttachmentType`
Attachment type classification.

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `Name` | `string` | Type name (max 255 chars) |
| `Description` | `string` | Type description |

**Navigation Properties:**
- `AttachmentLinks` → `ICollection<AttachmentLink>`

---

### `Comment`
Comments on entities. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `CommentText` | `string` | Comment content |
| `Status` | `string` | Comment status (max 50 chars) |
| `EntityID` | `int` | Foreign key to Entity |
| `RecordID` | `int?` | Related record ID |

**Navigation Properties:**
- `Entity` → `Entity`

---

## Events & Logging

### `Event`
Event type definitions. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `EventCategoryID` | `int` | Foreign key to ApplicationsTable |
| `Name` | `string` | Event name (max 50 chars) |
| `Description` | `string` | Event description (max 255 chars) |
| `PriorityID` | `int?` | Priority level |

**Navigation Properties:**
- `EventCategorys` → `ApplicationsTable`
- `EventLogs` → `ICollection<EventLog>`

---

### `EventLog`
Event log entries. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `long` | Primary key |
| `EventID` | `int` | Foreign key to Event |
| `EntityID` | `int` | Foreign key to Entity |
| `RecordID` | `int?` | Related record ID |
| `Source` | `string` | API source (max 255 chars) |
| `Description` | `string` | Event description (max 2000 chars) |
| `Data` | `string` | Event data payload |

**Navigation Properties:**
- `Event` → `Event`
- `Entity` → `Entity`

---

### `Entity`
Entity type definitions for generic references. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `Name` | `string` | Entity name (max 50 chars) - e.g., order, products, outlet, staff |
| `Description` | `string` | Entity description (max 255 chars) |

**Navigation Properties:**
- `EventLogs` → `ICollection<EventLog>`
- `Tasks` → `ICollection<Tasks>`
- `AttachmentLinks` → `ICollection<AttachmentLink>`
- `Comments` → `ICollection<Comment>`

---

## Content Management

### `Blog`
Blog posts. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `int` | Primary key |
| `BlogCategoryID` | `int` | Foreign key to BlogCategory |
| `Title` | `string` | Blog title |
| `Description` | `string` | Blog description |
| `ImageUrl` | `string` | Featured image URL |
| `InnerHtml` | `string` | HTML content |
| `Authour` | `string` | Author name |

**Navigation Properties:**
- `BlogCategory` → `BlogCategory`

---

### `BlogCategory`
Blog categories. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `int` | Primary key |
| `Name` | `string` | Category name |
| `Description` | `string` | Category description |

**Navigation Properties:**
- `Blogs` → `ICollection<Blog>`

---

### `FAQ`
Frequently asked questions. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `int` | Primary key |
| `Title` | `string` | Question title |
| `Description` | `string` | Answer content |
| `CategoryId` | `int` | Foreign key to FAQCategory |

**Navigation Properties:**
- `Category` → `FAQCategory`

---

### `FAQCategory`
FAQ categories. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `int` | Primary key |
| `CategoryTitle` | `string` | Category title |
| `TypeOfFAQ` | `string` | FAQ type (customer, business) |

**Navigation Properties:**
- `FAQs` → `ICollection<FAQ>`

---

### `HelpItem`
Help items for pages. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `Name` | `string` | Item name (max 100 chars) |
| `Title` | `string` | Item title (max 200 chars) |
| `Description` | `string` | Help content |
| `PageID` | `int?` | Foreign key to Page |
| `IsShownOnPage` | `bool` | Display on page flag |

**Navigation Properties:**
- `Pages` → `Page`

---

### `Page`
Application pages. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `Name` | `string` | Page name (max 100 chars) |
| `Description` | `string` | Page description (max 500 chars) |
| `Url` | `string` | Page URL |
| `ApplicationId` | `int` | Foreign key to ApplicationsTable |

**Navigation Properties:**
- `HelpItems` → `ICollection<HelpItem>`
- `ApplicationsTable` → `ApplicationsTable`

---

### `ApplicationsTable`
Application type definitions. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `Name` | `string` | Application name (max 255 chars) |
| `Description` | `string` | Application description |
| `Sequence` | `int` | Display sequence |

**Navigation Properties:**
- `Events` → `ICollection<Event>`
- `Pages` → `ICollection<Page>`
- `TaskTypes` → `ICollection<TaskType>`

---

### `OnboardingStep`
User onboarding steps.

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `int` | Primary key |
| `StepName` | `string` | Step name |
| `StepDescription` | `string` | Step description |
| `PageId` | `int` | Foreign key to Page |
| `StepOrder` | `int` | Step sequence order |
| `CreatedAt` | `DateTime` | Creation timestamp |

**Navigation Properties:**
- `Page` → `Page`
- `OnboardingProgress` → `ICollection<UserOnboardingProgress>`

---

## Finance & Payments

### `MerchantFinance`
Merchant financial settings. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `int` | Primary key |
| `OutletID` | `int` | Foreign key to Outlet |

**Navigation Properties:**
- `UpiAccounts` → `ICollection<MerchantUpiAccount>`
- `BankAccounts` → `ICollection<MerchantBankAccount>`
- `Outlet` → `Outlet`

---

### `MerchantBankAccount`
Merchant bank account details. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `MerchantFinanceId` | `int` | Foreign key to MerchantFinance |
| `AccountNumber` | `string` | Bank account number |
| `IfscCode` | `string` | IFSC code |
| `BankName` | `string` | Bank name |
| `BranchName` | `string` | Branch name |
| `AccountHolderName` | `string` | Account holder name |
| `IsPrimaryAccount` | `bool` | Primary account flag |

**Navigation Properties:**
- `MerchantFinance` → `MerchantFinance`

---

### `MerchantUpiAccount`
Merchant UPI account details. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `MerchantFinanceId` | `int` | Foreign key to MerchantFinance |
| `UpiIdentifier` | `string` | UPI ID |
| `MerchantName` | `string` | Merchant display name |
| `QrCodeUrl` | `string` | QR code image URL |
| `IsPrimaryAccount` | `bool` | Primary account flag |

**Navigation Properties:**
- `MerchantFinance` → `MerchantFinance`

---

### `Subscription`
Customer subscriptions to outlets.

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `int` | Primary key |
| `CustomerId` | `int` | Foreign key to AppUser |
| `OutletId` | `int` | Foreign key to Outlet |
| `SubscribedDate` | `DateTime` | Subscription start date |
| `UnsubscribedDate` | `DateTime?` | Subscription end date |
| `StatusID` | `int` | Foreign key to SubscriptionStatus |

**Navigation Properties:**
- `Customer` → `AppUser`
- `Outlet` → `Outlet`
- `Status` → `SubscriptionStatus`

---

### `SubscriptionStatus`
Subscription status lookup.

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `int` | Primary key |
| `Name` | `string` | Status name (max 50 chars) |

**Navigation Properties:**
- `Subscriptions` → `ICollection<Subscription>`

---

## Miscellaneous

### `Rating`
Product and outlet ratings.

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `int` | Primary key |
| `CustomerId` | `int` | Foreign key to AppUser |
| `ProductId` | `int?` | Foreign key to Catalog |
| `OutletId` | `int?` | Foreign key to Outlet |
| `Stars` | `int` | Star rating (1-5) |
| `Review` | `string` | Review text |
| `CreatedDate` | `DateTime` | Creation timestamp |
| `ModifiedDate` | `DateTime?` | Modification timestamp |
| `IsVerified` | `bool` | Verified review flag |

**Navigation Properties:**
- `Customer` → `AppUser`
- `Outlet` → `Outlet`
- `Product` → `Catalog`

---

### `StatusOptionSet`
Generic status options. *Inherits from BaseModel.*

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `int` | Primary key |
| `Status` | `string` | Status name (max 255 chars) |
| `StatusDescription` | `string` | Status description |

**Navigation Properties:**
- `Businesses` → `ICollection<Business>`
- `Outlets` → `ICollection<Outlet>`
- `Products` → `ICollection<Catalog>`

---

### `Proof`
Identity/business proofs.

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `int` | Primary key |
| `EntityType` | `string` | Entity type ("Business" or "Person") |
| `EntityId` | `int` | Entity ID |
| `ProofNumber` | `string` | Proof document number |
| `ProofTypeId` | `int` | Foreign key to ProofType |

**Navigation Properties:**
- `ProofType` → `ProofType`

---

### `ProofType`
Proof type definitions.

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `int` | Primary key |
| `Name` | `string` | Proof type name (max 100 chars) |

---

### `PromotionalLeads`
Marketing leads collection.

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `int` | Primary key |
| `Gmail` | `string` | Email address |
| `CreateDate` | `DateTime` | Creation date |
| `Time` | `TimeSpan` | Creation time |
| `WhichApplication` | `string` | Source application |

---

### `DatabaseVersion`
Database migration version tracking.

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `int` | Primary key |
| `VersionClass` | `string` | Version class name |
| `Status` | `bool` | Execution status |
| `Description` | `string` | Version description |
| `CreateDate` | `DateTime` | Creation timestamp |
| `CreateBy` | `string` | Creator |

---

## Entity Relationship Diagram Overview

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                              USER MANAGEMENT                                 │
├─────────────────────────────────────────────────────────────────────────────┤
│  AppUser ──┬──> AppUserRole <──── AppRole                                   │
│            ├──> UserAddress ───── Address                                   │
│            ├──> BusinessEmployeeMap ───── Business                          │
│            └──> UserOnboardingProgress ── OnboardingStep                    │
└─────────────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────────────┐
│                           BUSINESS & PRODUCTS                                │
├─────────────────────────────────────────────────────────────────────────────┤
│  Business ──┬──> BusinessCategoryInternal                                   │
│             ├──> Outlet ──┬──> Inventory ──── CatalogVariants              │
│             │             ├──> Order ──────── OrderDetail                   │
│             │             ├──> MerchantFinance                              │
│             │             └──> Rating                                       │
│             └──> CatalogCategory ──── Catalog ──── CatalogVariants         │
└─────────────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────────────┐
│                           TASKS & SUPPORT                                    │
├─────────────────────────────────────────────────────────────────────────────┤
│  Tasks ──┬──> TaskType ──── ApplicationsTable                               │
│          ├──> TaskStatusType                                                │
│          ├──> Entity                                                        │
│          └──> SupportTicket ──── PersonType                                │
└─────────────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────────────┐
│                              COMMUNICATION                                   │
├─────────────────────────────────────────────────────────────────────────────┤
│  CommunicationQueue ──┬──> ContactMethod                                    │
│                       └──> CommunicationStatus                              │
│  Notification ────────────> AppUser, Page                                   │
└─────────────────────────────────────────────────────────────────────────────┘
```

---

## Notes

1. **Soft Delete**: Most models inherit from `BaseModel` which includes an `IsActive` flag for soft delete functionality.

2. **Audit Trail**: `CreateDate`, `CreateBy`, `ModifiedDate`, and `ModifiedBy` fields track record changes.

3. **Table Naming**: Some models have custom table names:
   - `Catalog` → `Product`
   - `CatalogVariants` → `ProductVariants`
   - `CatalogCategory` → `ProductCategory`

4. **JSON Storage**: Several fields store JSON data:
   - `Outlet.TimingList` - Operating hours
   - `CatalogVariants.Attributes` - Variant attributes
   - `Catalog.VariantOptions` - Variant configuration

5. **Generic Entity System**: The `Entity` model provides a flexible way to reference different record types (orders, products, outlets, etc.) in `EventLog`, `Tasks`, `Comment`, and `AttachmentLink`.

