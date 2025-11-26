# EpicMarket API Documentation

> **Complete RESTful API Reference** for the EpicMarket Business API

---

## Table of Contents

- [Overview](#overview)
- [API Architecture](#api-architecture)
- [Authentication](#authentication)
- [Controllers Reference](#controllers-reference)
- [Services & Repositories](#services--repositories)
- [Entity Details](#entity-details)
- [Endpoints Documentation](#endpoints-documentation)
  - [Account Controller](#account-controller)
  - [Business Controller](#business-controller)
  - [Products Controller](#products-controller)
  - [Orders Controller](#orders-controller)
  - [Branch Controller](#branch-controller)
  - [Employees Controller](#employees-controller)
  - [Home Controller](#home-controller)
  - [Static Controller](#static-controller)
  - [Files Controller](#files-controller)
  - [Support Controller](#support-controller)
- [Middleware Details](#middleware-details)
- [Common Response Format](#common-response-format)
- [Error Handling](#error-handling)
- [Pagination & Filtering](#pagination--filtering)

---

## Overview

The EpicMarket Business API is a RESTful service built with ASP.NET Core 8.0 that provides endpoints for:

- **User Authentication** - Registration, login, password management
- **Business Management** - Business registration and configuration
- **Product Catalog** - Product CRUD with image management
- **Order Processing** - Order creation and status management
- **Branch Management** - Multi-location outlet management
- **Employee Management** - Employee invitations and account setup
- **Static Data** - Dropdown options and lookup data
- **Support** - Task and support ticket management

**Base URL:** `https://api.epicmarket.com/api`

**API Version:** v1

---

## API Architecture

```mermaid
graph TB
    subgraph "API Layer"
        Controllers[Controllers]
        Middleware[Middleware Pipeline]
        Extensions[Service Extensions]
    end

    subgraph "Authentication"
        JWT[JWT Bearer Token]
        Identity[ASP.NET Identity]
        CustomAuth[Custom Authorize Attribute]
    end

    subgraph "Business Layer"
        Services[Service Classes]
        AutoMapper[AutoMapper Profiles]
    end

    subgraph "Data Layer"
        DbContext[ApplicationDbContext]
        Entities[Entity Models]
    end

    subgraph "External"
        S3[AWS S3]
        SQLServer[(SQL Server)]
    end

    Controllers --> Middleware
    Middleware --> JWT
    JWT --> Identity
    Controllers --> CustomAuth
    Controllers --> Services
    Services --> AutoMapper
    Services --> DbContext
    DbContext --> SQLServer
    Controllers --> S3
```

### Project Structure

```
EpicMarket.Business.API/
├── Controllers/
│   ├── AccountController.cs      # Authentication endpoints
│   ├── BaseApiController.cs      # Base controller with shared logic
│   ├── BranchController.cs       # Branch/outlet management
│   ├── BusinessController.cs     # Business registration
│   ├── EmployeesController.cs    # Employee management
│   ├── FilesController.cs        # File upload/download
│   ├── HomeController.cs         # Public content (FAQs, Blogs)
│   ├── OrdersController.cs       # Order processing
│   ├── ProductsController.cs     # Product catalog
│   ├── StaticController.cs       # Lookup data
│   └── SupportController.cs      # Support tickets
├── Extension/
│   ├── ApplicationServiceExtensions.cs  # DI registration
│   ├── IdentityServiceExtensions.cs     # Identity & JWT setup
│   ├── ClaimsPrincipleExtensions.cs     # User claims helpers
│   └── CustomAuthorizeAttribute.cs      # Custom authorization
├── Middleware/
│   └── ExceptionMiddleware.cs    # Global exception handling
├── Helpers/
│   ├── AutoMapperProfiles.cs     # DTO mapping profiles
│   └── LogUserActivity.cs        # Activity logging filter
├── Errors/
│   └── ApiException.cs           # Exception response model
└── Program.cs                    # Application entry point
```

---

## Authentication

### JWT Bearer Token

The API uses JWT (JSON Web Token) for authentication. Tokens are obtained via the login endpoint and must be included in subsequent requests.

**Token Configuration:**
- **Algorithm:** HS256 (HMAC SHA-256)
- **Expiration:** Configured in TokenService
- **Issuer Validation:** Disabled
- **Audience Validation:** Disabled

### Obtaining a Token

```http
POST /api/Account/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "YourPassword123"
}
```

### Using the Token

Include the token in the `Authorization` header:

```http
GET /api/Products
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Swagger Authentication

In Swagger UI, click "Authorize" and enter:
```
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Role-Based Authorization

| Role | Description |
|------|-------------|
| `Admin` | Full system access |
| `Business_Owner` | Manage own business, employees, products |
| `Business_Employee` | Limited business operations |
| `Member` | Basic user access |

### Custom Authorization

Some endpoints use custom securables-based authorization:

```csharp
[CustomAuthorize(Securable = Securables.STATUS_OPTIONS)]
public async Task<ActionResult<OperationResult<List<DropDownOptions>>>> GetStatusOptions()
```

---

## Controllers Reference

| Controller | Route Prefix | Description | Auth Required |
|------------|--------------|-------------|---------------|
| `AccountController` | `/api/Account` | Authentication & user management | Mixed |
| `BusinessController` | `/api/Business` | Business registration | Yes |
| `ProductsController` | `/api/Products` | Product catalog CRUD | Yes |
| `OrdersController` | `/api/Orders` | Order management | Yes |
| `BranchController` | `/api/Branch` | Branch/outlet management | Yes |
| `EmployeesController` | `/api/Employees` | Employee management | Mixed |
| `HomeController` | `/api/Home` | FAQs, Blogs (public) | No |
| `StaticController` | `/api/Static` | Lookup data | Mixed |
| `FilesController` | `/api/Files` | File operations | No |
| `SupportController` | `/api/Support` | Support tasks | Yes |

---

## Services & Repositories

### Service Layer

| Service Interface | Implementation | Description |
|-------------------|----------------|-------------|
| `ITokenService` | `TokenService` | JWT token generation, password reset |
| `IBusinessService` | `BusinessService` | Business registration & management |
| `IProductService` | `ProductService` | Product CRUD operations |
| `IOrderService` | `OrderService` | Order processing |
| `IBranchService` | `BranchService` | Branch management & mapping |
| `IEmployeeService` | `EmployeeService` | Employee invitation & registration |
| `IHomeService` | `HomeService` | FAQ & Blog operations |
| `IStaticService` | `StaticService` | Dropdown/lookup data |
| `IFileService` | `FileService` | AWS S3 file operations |
| `IAttachmentService` | `AttachmentService` | Attachment linking |
| `ITasksService` | `TasksService` | Support task management |
| `IProfileService` | `ProfileService` | User permissions |
| `ICommunicationService` | `CommunicationService` | Email/SMS communication |
| `IEventLogService` | `EventLogService` | Audit logging |
| `IApplicationConfigurationService` | `ApplicationConfigurationService` | System configuration |

### Repository Pattern

| Repository Interface | Implementation | Description |
|---------------------|----------------|-------------|
| `IUserRepository` | `UserRepository` | User operations & permissions |
| `IUnitOfWork` | `UnitOfWork` | Transaction management |

---

## Entity Details

### Core Entities

#### AppUser
```csharp
public class AppUser : IdentityUser<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UniqueGuid { get; set; }
    public int OTP { get; set; }
    public bool IsActive { get; set; }
    public DateTime LastActive { get; set; }
}
```

#### Business
```csharp
public class Business : BaseModel
{
    public int ID { get; set; }
    public int PersonID { get; set; }           // FK to AppUser
    public int? StatusId { get; set; }          // FK to StatusOptionSet
    public int BusinessCategoryID { get; set; } // FK to BusinessCategoryInternal
    public string Name { get; set; }
    public string Description { get; set; }
    public string? Banner { get; set; }
    public string? Logo { get; set; }
    public long ContactNumber { get; set; }
    public string ContactEmail { get; set; }
    public int AddressID { get; set; }          // FK to Address
    public int? Rating { get; set; }
    public int? ReviewCount { get; set; }
    public bool IsOpen { get; set; }
    public double? Weight { get; set; }
}
```

#### Catalog (Product)
```csharp
public class Catalog : BaseModel
{
    public int ID { get; set; }
    public int BusinessID { get; set; }
    public long? Barcode { get; set; }
    public string Name { get; set; }            // Max 50 chars
    public string Description { get; set; }
    public string? Category { get; set; }
    public double Rate { get; set; }
    public bool InStock { get; set; }
    public bool IsRecommended { get; set; }
    public int? MaximumOrderPurchase { get; set; }
    public double? Rating { get; set; }
    public int? ReviewCount { get; set; }
    public int? OrderCount { get; set; }
    public int StatusId { get; set; }
}
```

#### Order
```csharp
public class Order : BaseModel
{
    public int ID { get; set; }
    public int PersonID { get; set; }
    public int OutletID { get; set; }
    public int OrderTypeId { get; set; }    // Online/Offline
    public double TotalPrice { get; set; }
    public int TotalItems { get; set; }
    public DateTime OrderAt { get; set; }
    public int StatusId { get; set; }       // Delivered, Packing, etc.
    public string PaymentMode { get; set; } // Cash, Online
    public int? AddressID { get; set; }
}
```

#### Outlet (Branch)
```csharp
public class Outlet : BaseModel
{
    public int ID { get; set; }
    public int BussinessID { get; set; }
    public int AddressID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public long ContactNumber { get; set; }
    public string ContactEmail { get; set; }
    public int? Rating { get; set; }
    public int? ReviewCount { get; set; }
    public bool IsOpen { get; set; }
    public double? Weight { get; set; }
    public int StatusId { get; set; }
}
```

### Supporting Entities

| Entity | Description |
|--------|-------------|
| `Address` | Physical address with coordinates |
| `Attachment` | File metadata |
| `AttachmentLink` | Links attachments to entities |
| `BusinessEmployeeMap` | Employee-business relationship |
| `OrderDetail` | Line items in an order |
| `OutletProduct` | Products available at outlet |
| `OutletPerson` | Employees assigned to outlet |
| `Tasks` | Support/internal tasks |
| `Comment` | Task comments |
| `EventLog` | Audit trail |

---

## Endpoints Documentation

---

### Account Controller

**Base Route:** `/api/Account`

---

#### Register User

Creates a new user account.

| | |
|---|---|
| **URL** | `POST /api/Account/register` |
| **Auth** | None (Anonymous) |
| **Roles** | - |

**Request Body:**

```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "phone": "1234567890",
  "password": "SecurePassword123!"
}
```

**Response:**

```json
{
  "status": "SUCCESS",
  "message": "",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
  },
  "errorDetail": ""
}
```

| Status Code | Description |
|-------------|-------------|
| `200 OK` | Registration successful |
| `400 Bad Request` | Username taken or validation error |

---

#### Login

Authenticates user and returns JWT token.

| | |
|---|---|
| **URL** | `POST /api/Account/login` |
| **Auth** | None (Anonymous) |

**Request Body:**

```json
{
  "email": "john.doe@example.com",
  "password": "SecurePassword123!"
}
```

**Response:**

```json
{
  "status": "SUCCESS",
  "message": "",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwidW5pcXVlX25hbWUiOiJqb2huLmRvZUBleGFtcGxlLmNvbSIsInJvbGUiOlsiQnVzaW5lc3NfT3duZXIiXSwibmJmIjoxNzAwMDAwMDAwLCJleHAiOjE3MDAwODY0MDAsImlhdCI6MTcwMDAwMDAwMH0.xxx"
  },
  "errorDetail": ""
}
```

| Status Code | Description |
|-------------|-------------|
| `200 OK` | Login successful |
| `401 Unauthorized` | Invalid credentials |

---

#### Get User Info

Returns current user information with permissions.

| | |
|---|---|
| **URL** | `GET /api/Account/info` |
| **Auth** | Required |

**Response:**

```json
{
  "status": "SUCCESS",
  "data": {
    "userDetails": {
      "username": "john.doe@example.com",
      "firstName": "John",
      "lastName": "Doe",
      "phone": "1234567890"
    },
    "userBusiness": {
      "businessId": 123,
      "businessStatus": "Approved"
    },
    "securables": [
      {
        "securable": "STATUS_OPTIONS",
        "hasAccess": true
      }
    ]
  }
}
```

---

#### Change Password

Changes the password for authenticated user.

| | |
|---|---|
| **URL** | `POST /api/Account/changepassword` |
| **Auth** | Required |

**Request Body:**

```json
{
  "currentPassword": "OldPassword123!",
  "newPassword": "NewSecurePassword456!"
}
```

**Response:**

```json
{
  "status": "SUCCESS",
  "data": "Password changed Successfully"
}
```

| Status Code | Description |
|-------------|-------------|
| `200 OK` | Password changed |
| `401 Unauthorized` | Current password incorrect |

---

#### Reset Password Request

Initiates password reset process.

| | |
|---|---|
| **URL** | `POST /api/Account/ResetPassword` |
| **Auth** | None (Anonymous) |

**Request Body:**

```json
{
  "email": "john.doe@example.com"
}
```

---

#### Check Reset Password Link

Validates password reset token.

| | |
|---|---|
| **URL** | `GET /api/Account/CheckResetPasswordLink` |
| **Auth** | None (Anonymous) |

**Query Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `queryParam` | string | Yes | Reset token |

---

#### Set New Password

Completes password reset with new password.

| | |
|---|---|
| **URL** | `POST /api/Account/setNewPassword` |
| **Auth** | None (Anonymous) |

**Request Body:**

```json
{
  "token": "reset-token-here",
  "email": "john.doe@example.com",
  "newPassword": "NewSecurePassword789!"
}
```

---

### Business Controller

**Base Route:** `/api/Business`

---

#### Register Business

Registers a new business with logo and proof documents.

| | |
|---|---|
| **URL** | `POST /api/Business/RegisterDetails` |
| **Auth** | Required |
| **Content-Type** | `multipart/form-data` |

**Request Body (Form Data):**

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `name` | string | Yes | Business name |
| `description` | string | Yes | Business description |
| `businessCategoryId` | int | Yes | Category ID |
| `contactNumber` | long | Yes | Contact phone |
| `contactEmail` | string | Yes | Contact email |
| `proofType` | int | Yes | PAN=1, Aadhaar=2 |
| `proofNumber` | string | Yes | Proof document number |
| `address.addressLine1` | string | Yes | Street address |
| `address.city` | string | Yes | City |
| `address.state` | string | Yes | State |
| `address.pincode` | string | Yes | Postal code |
| `logoFile` | file | Yes | Business logo image |
| `proofFile` | file | Yes | Proof document file |

**Response:**

```json
{
  "status": "SUCCESS",
  "data": {
    "businessId": 123
  }
}
```

| Status Code | Description |
|-------------|-------------|
| `200 OK` | Business registered |
| `400 Bad Request` | Validation error |
| `401 Unauthorized` | Not authenticated |

---

### Products Controller

**Base Route:** `/api/Products`

---

#### Get All Products

Returns paginated list of products for the business.

| | |
|---|---|
| **URL** | `GET /api/Products` |
| **Auth** | Required |
| **Roles** | `Business_Owner`, `Business_Employee` |

**Query Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `pageNumber` | int | No | Page number (default: 1) |
| `pageSize` | int | No | Items per page (default: 10) |
| `searchTerm` | string | No | Search by name |
| `category` | string | No | Filter by category |
| `sortBy` | string | No | Sort field |
| `sortOrder` | string | No | `asc` or `desc` |

**Response:**

```json
{
  "status": "SUCCESS",
  "data": {
    "totalRecords": 150,
    "pageNumber": 1,
    "pageSize": 10,
    "data": [
      {
        "id": 1,
        "name": "Product Name",
        "description": "Product description",
        "rate": 29.99,
        "category": "Electronics",
        "inStock": true,
        "isRecommended": false,
        "status": "Approved",
        "thumbnailUrl": "https://s3..."
      }
    ]
  }
}
```

---

#### Get Product Details

Returns single product with full details.

| | |
|---|---|
| **URL** | `GET /api/Products/{id}` |
| **Auth** | Required |
| **Roles** | `Business_Owner`, `Business_Employee` |

**Path Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `id` | int | Product ID |

**Response:**

```json
{
  "status": "SUCCESS",
  "data": {
    "id": 1,
    "businessId": 123,
    "barcode": 1234567890123,
    "name": "Product Name",
    "description": "Full product description",
    "category": "Electronics",
    "rate": 29.99,
    "inStock": true,
    "isRecommended": true,
    "maximumOrderPurchase": 10,
    "rating": 4.5,
    "reviewCount": 25,
    "orderCount": 100,
    "status": "Approved",
    "images": [
      "https://s3.../image1.jpg",
      "https://s3.../image2.jpg"
    ]
  }
}
```

---

#### Add Product

Creates a new product with images.

| | |
|---|---|
| **URL** | `POST /api/Products` |
| **Auth** | Required |
| **Roles** | `Business_Owner` |
| **Content-Type** | `multipart/form-data` |

**Request Body (Form Data):**

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `name` | string | Yes | Product name (max 50 chars) |
| `description` | string | Yes | Product description |
| `barcode` | long | No | Product barcode |
| `category` | string | No | Category name |
| `rate` | double | Yes | Price |
| `inStock` | bool | No | Stock availability |
| `isRecommended` | bool | No | Featured product |
| `maximumOrderPurchase` | int | No | Max quantity per order |
| `products[]` | file[] | Yes | Product images |
| `thumbnail` | file | Yes | Thumbnail image |

**Response:**

```json
{
  "status": "SUCCESS",
  "data": 456
}
```

---

#### Update Product

Updates an existing product.

| | |
|---|---|
| **URL** | `PUT /api/Products/{id}` |
| **Auth** | Required |
| **Roles** | `Business_Owner` |
| **Content-Type** | `multipart/form-data` |

**Path Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `id` | int | Product ID |

**Request Body:** Same as Add Product (fields are optional)

---

#### Delete Product

Deletes a product.

| | |
|---|---|
| **URL** | `DELETE /api/Products/{id}` |
| **Auth** | Required |
| **Roles** | `Business_Owner` |

**Response:**

```json
{
  "status": "SUCCESS",
  "data": true
}
```

---

#### Verify Catalog

Verifies/approves a product catalog.

| | |
|---|---|
| **URL** | `POST /api/Products/verify` |
| **Auth** | Required |
| **Roles** | `Business_Owner` |

**Request Body:**

```json
{
  "id": 123,
  "verify": true
}
```

---

#### Get Products for Mapping

Returns products for outlet mapping.

| | |
|---|---|
| **URL** | `GET /api/Products/Map/{outletID}` |
| **Auth** | Required |
| **Roles** | `Business_Owner` |

---

#### Delete Product Images

Deletes specific product images.

| | |
|---|---|
| **URL** | `DELETE /api/Products/images` |
| **Auth** | Required |
| **Roles** | `Business_Owner` |

**Request Body:**

```json
{
  "keys": ["image-key-1", "image-key-2"]
}
```

---

### Orders Controller

**Base Route:** `/api/Orders`

---

#### Get All Orders

Returns paginated list of orders.

| | |
|---|---|
| **URL** | `GET /api/Orders/GetAllOrders` |
| **Auth** | Required |
| **Roles** | `Business_Owner`, `Business_Employee` |

**Request Body:**

```json
{
  "pageNumber": 1,
  "pageSize": 10,
  "status": "Pending",
  "fromDate": "2024-01-01",
  "toDate": "2024-12-31"
}
```

**Response:**

```json
{
  "status": "SUCCESS",
  "data": {
    "totalRecords": 50,
    "data": [
      {
        "id": 1,
        "orderAt": "2024-01-15T10:30:00",
        "totalPrice": 150.00,
        "totalItems": 3,
        "status": "Pending",
        "paymentMode": "Cash",
        "customerName": "Jane Doe",
        "outletName": "Main Store"
      }
    ]
  }
}
```

---

#### Get Single Order

Returns order details with line items.

| | |
|---|---|
| **URL** | `GET /api/Orders/GetSingleOrder` |
| **Auth** | Required |
| **Roles** | `Business_Owner`, `Business_Employee` |

**Query Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `OrderId` | int | Order ID |

**Response:**

```json
{
  "status": "SUCCESS",
  "data": {
    "id": 1,
    "personId": 456,
    "outletId": 789,
    "orderTypeId": 1,
    "totalPrice": 150.00,
    "totalItems": 3,
    "orderAt": "2024-01-15T10:30:00",
    "statusId": 1,
    "paymentMode": "Cash",
    "orderDetails": [
      {
        "catalogId": 1,
        "productName": "Product A",
        "quantity": 2,
        "price": 50.00
      },
      {
        "catalogId": 2,
        "productName": "Product B",
        "quantity": 1,
        "price": 50.00
      }
    ]
  }
}
```

---

#### Add Order

Creates a new order.

| | |
|---|---|
| **URL** | `POST /api/Orders/AddOrder` |
| **Auth** | Required |
| **Roles** | `Business_Owner`, `Business_Employee` |

**Request Body:**

```json
{
  "outletId": 789,
  "orderTypeId": 1,
  "paymentMode": "Cash",
  "addressId": 123,
  "orderDetails": [
    {
      "catalogId": 1,
      "quantity": 2,
      "price": 50.00
    },
    {
      "catalogId": 2,
      "quantity": 1,
      "price": 50.00
    }
  ]
}
```

**Response:**

```json
{
  "status": "SUCCESS",
  "data": 1001
}
```

---

#### Update Order Status

Updates the status of an order.

| | |
|---|---|
| **URL** | `POST /api/Orders/UpdateStatus` |
| **Auth** | Required |
| **Roles** | `Business_Owner`, `Business_Employee` |

**Query Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `OrderId` | int | Order ID |
| `OrderStatus` | string | New status |

**Response:**

```json
{
  "status": "SUCCESS",
  "data": 1001
}
```

---

#### Get Order Status Options

Returns available order statuses.

| | |
|---|---|
| **URL** | `GET /api/Orders/GetOrderStatusOptions` |
| **Auth** | Required |
| **Roles** | `Business_Owner`, `Business_Employee` |

**Response:**

```json
{
  "status": "SUCCESS",
  "data": [
    { "value": 1, "text": "Pending" },
    { "value": 2, "text": "Processing" },
    { "value": 3, "text": "Shipped" },
    { "value": 4, "text": "Delivered" },
    { "value": 5, "text": "Cancelled" }
  ]
}
```

---

### Branch Controller

**Base Route:** `/api/Branch`

---

#### Get All Branches

Returns all branches for the business.

| | |
|---|---|
| **URL** | `GET /api/Branch/GetAllBranches` |
| **Auth** | Required |
| **Roles** | `Business_Owner` |

**Query Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `pageNumber` | int | Page number |
| `pageSize` | int | Items per page |
| `searchTerm` | string | Search filter |

**Response:**

```json
{
  "status": "SUCCESS",
  "data": {
    "totalRecords": 5,
    "data": [
      {
        "id": 1,
        "name": "Main Store",
        "description": "Primary outlet",
        "contactNumber": 1234567890,
        "contactEmail": "main@store.com",
        "city": "Mumbai",
        "status": "Active",
        "isOpen": true
      }
    ]
  }
}
```

---

#### Get Branch By ID

Returns single branch details.

| | |
|---|---|
| **URL** | `GET /api/Branch/GetBranchByID` |
| **Auth** | Required |
| **Roles** | `Business_Owner` |

**Query Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `branchId` | int | Branch ID |

---

#### Add or Update Branch

Creates or updates a branch.

| | |
|---|---|
| **URL** | `POST /api/Branch/AddOrUpdateBranch` |
| **Auth** | Required |
| **Roles** | `Business_Owner` |

**Request Body:**

```json
{
  "id": 0,
  "name": "New Branch",
  "description": "Branch description",
  "contactNumber": 1234567890,
  "contactEmail": "branch@store.com",
  "address": {
    "addressLine1": "123 Main St",
    "city": "Mumbai",
    "state": "Maharashtra",
    "pincode": "400001"
  }
}
```

---

#### Map Branch to People

Assigns employees to a branch.

| | |
|---|---|
| **URL** | `POST /api/Branch/MapBranchToPeople` |
| **Auth** | Required |
| **Roles** | `Business_Owner` |

**Request Body:**

```json
{
  "branchId": 1,
  "employeeIds": [101, 102, 103]
}
```

---

#### Map Branch to Products

Assigns products to a branch.

| | |
|---|---|
| **URL** | `POST /api/Branch/MapBranchToProduct` |
| **Auth** | Required |
| **Roles** | `Business_Owner` |

**Request Body:**

```json
{
  "branchId": 1,
  "productIds": [201, 202, 203]
}
```

---

#### Verify Branch

Approves a branch.

| | |
|---|---|
| **URL** | `POST /api/Branch/verifyBranchs` |
| **Auth** | Required |
| **Roles** | `Business_Owner` |

**Request Body:**

```json
{
  "id": 1,
  "verify": true
}
```

---

### Employees Controller

**Base Route:** `/api/Employees`

---

#### Get All Employees

Returns all employees for the business.

| | |
|---|---|
| **URL** | `GET /api/Employees` |
| **Auth** | Required |
| **Roles** | `Business_Owner` |

**Query Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `pageNumber` | int | Page number |
| `pageSize` | int | Items per page |

**Response:**

```json
{
  "status": "SUCCESS",
  "data": {
    "totalRecords": 10,
    "data": [
      {
        "id": 1,
        "firstName": "John",
        "lastName": "Employee",
        "email": "john@company.com",
        "phone": "1234567890",
        "status": "Active"
      }
    ]
  }
}
```

---

#### Get Employee Details

Returns single employee details.

| | |
|---|---|
| **URL** | `GET /api/Employees/{id}` |
| **Auth** | Required |
| **Roles** | `Business_Owner` |

---

#### Register Employee (Invite)

Sends invitation to new employee.

| | |
|---|---|
| **URL** | `POST /api/Employees` |
| **Auth** | Required |
| **Roles** | `Business_Owner` |

**Request Body:**

```json
{
  "firstName": "New",
  "lastName": "Employee",
  "email": "new.employee@company.com",
  "phone": "1234567890"
}
```

**Response:**

```json
{
  "status": "SUCCESS",
  "data": {
    "employeeId": 101,
    "inviteLink": "https://app.epicmarket.com/employee/register?token=xxx"
  }
}
```

---

#### Check Employee Link

Validates employee invitation link.

| | |
|---|---|
| **URL** | `GET /api/Employees/Check` |
| **Auth** | None (Anonymous) |

**Query Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `queryParam` | string | Invitation token |

---

#### Create Employee Account

Completes employee registration.

| | |
|---|---|
| **URL** | `PUT /api/Employees/{id}` |
| **Auth** | None (Anonymous) |

**Request Body:**

```json
{
  "password": "SecurePassword123!",
  "confirmPassword": "SecurePassword123!"
}
```

---

#### Get Employees for Mapping

Returns employees for outlet assignment.

| | |
|---|---|
| **URL** | `GET /api/Employees/Map/{outletID}` |
| **Auth** | Required |
| **Roles** | `Business_Owner` |

---

#### Delete Employee

Removes an employee.

| | |
|---|---|
| **URL** | `DELETE /api/Employees/{id}` |
| **Auth** | Required |
| **Roles** | `Business_Owner` |

---

### Home Controller

**Base Route:** `/api/Home`

All endpoints are public (no authentication required).

---

#### Get FAQ Categories

Returns FAQ categories.

| | |
|---|---|
| **URL** | `GET /api/Home/GetAllFaqCategory/{typeOfCategory}` |

---

#### Get FAQs by Category

Returns FAQs for a category.

| | |
|---|---|
| **URL** | `GET /api/Home/GetAllFAQ/{CategoryID}` |

---

#### Get All Blogs

Returns blog posts.

| | |
|---|---|
| **URL** | `GET /api/Home/GetAllBlogs` |

**Query Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `pageNumber` | int | Page number |
| `pageSize` | int | Items per page |

---

#### Get Blogs by Category

Returns blogs filtered by category.

| | |
|---|---|
| **URL** | `GET /api/Home/GetAllBlogsByCategory` |

---

#### Get Blog Details

Returns single blog post.

| | |
|---|---|
| **URL** | `GET /api/Home/GetBlogDetails/{blogId}` |

---

### Static Controller

**Base Route:** `/api/Static`

---

#### Get Business Categories

| | |
|---|---|
| **URL** | `GET /api/Static/GetBusinessCategoriesOptions` |
| **Auth** | None |

**Response:**

```json
{
  "status": "SUCCESS",
  "data": [
    { "value": 1, "text": "Restaurant" },
    { "value": 2, "text": "Retail" },
    { "value": 3, "text": "Services" }
  ]
}
```

---

#### Get Status Options

| | |
|---|---|
| **URL** | `GET /api/Static/GetStatusOptions` |
| **Auth** | Required (Custom Securable) |

---

#### Get Order Status Options

| | |
|---|---|
| **URL** | `GET /api/Static/GetOderStatusOptions` |
| **Auth** | None |

---

#### Get Order Type Options

| | |
|---|---|
| **URL** | `GET /api/Static/GetOderTypeOptions` |
| **Auth** | None |

---

#### Get Blog Categories

| | |
|---|---|
| **URL** | `GET /api/Static/GetAllblogCategories` |
| **Auth** | None |

---

#### Get Support Categories

| | |
|---|---|
| **URL** | `GET /api/Static/GetAllSupportCategorys` |
| **Auth** | None |

---

#### Subscribe for Offers

| | |
|---|---|
| **URL** | `POST /api/Static/subscribeforOffers` |
| **Auth** | None |

**Query Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `gmail` | string | Email address |

---

#### Get Help Items

| | |
|---|---|
| **URL** | `GET /api/Static/GetHelpItemsforBypage` |
| **Auth** | None |

**Query Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `pagename` | string | Page name |

---

#### Get Support Queries

| | |
|---|---|
| **URL** | `GET /api/Static/GetAllSupportQuery` |
| **Auth** | None |

---

#### Get Person Types

| | |
|---|---|
| **URL** | `GET /api/Static/GetAllPersonTypes` |
| **Auth** | None |

---

#### Get Proof Types

| | |
|---|---|
| **URL** | `GET /api/Static/proofType` |
| **Auth** | None |

**Response:**

```json
{
  "status": "SUCCESS",
  "data": [
    { "value": 1, "text": "PAN" },
    { "value": 2, "text": "Aadhaar" }
  ]
}
```

---

### Files Controller

**Base Route:** `/api/Files`

---

#### Upload File

Uploads a file to S3.

| | |
|---|---|
| **URL** | `POST /api/Files` |
| **Content-Type** | `multipart/form-data` |

**Request:**

| Field | Type | Description |
|-------|------|-------------|
| `file` | file | File to upload |
| `prefix` | string | Optional path prefix |

---

#### Get All Files

Lists files in S3 bucket.

| | |
|---|---|
| **URL** | `GET /api/Files` |

**Query Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `prefix` | string | Path prefix filter |

---

#### Get File Preview

Returns file for preview.

| | |
|---|---|
| **URL** | `GET /api/Files/preview` |
| **Cache** | 1 hour |

**Query Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `key` | string | S3 file key |

---

#### Delete File

Deletes a file from S3.

| | |
|---|---|
| **URL** | `DELETE /api/Files` |

**Query Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `key` | string | S3 file key |

---

### Support Controller

**Base Route:** `/api/Support`

---

#### Add/Edit Task

Creates or updates a support task.

| | |
|---|---|
| **URL** | `POST /api/Support/AddEditTask` |

**Request Body:**

```json
{
  "id": 0,
  "name": "Support Request",
  "description": "Customer issue description",
  "taskTypeId": 1,
  "taskStatusId": 1,
  "priority": 2,
  "assignedToPersonId": 100
}
```

---

#### Add Task Comment

Adds a comment to a task.

| | |
|---|---|
| **URL** | `POST /api/Support/AddTaskComment` |

**Request Body:**

```json
{
  "taskId": 1,
  "commentText": "Working on this issue"
}
```

---

#### Get All Comments

Returns comments for a task.

| | |
|---|---|
| **URL** | `GET /api/Support/GetAllComments` |

**Query Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `taskId` | int | Task ID |

---

#### Get Task Details

Returns full task details.

| | |
|---|---|
| **URL** | `GET /api/Support/GettaskDetails` |

**Query Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `taskId` | int | Task ID |

---

#### Get Support by Person

Returns support tickets for a person.

| | |
|---|---|
| **URL** | `GET /api/Support/GetSupportByPersonId` |

**Query Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `personId` | int | Person ID |

---

#### Add Support Task

Creates a support ticket.

| | |
|---|---|
| **URL** | `POST /api/Support/AddSupportTask` |

**Request Body:**

```json
{
  "name": "Support Ticket",
  "description": "Issue description",
  "personTypeId": 1,
  "supportQueryId": 1,
  "email": "customer@example.com",
  "phone": "1234567890"
}
```

---

## Middleware Details

### Exception Middleware

Global exception handling middleware catches all unhandled exceptions and returns a consistent error response.

**Configuration:**
- Development: Returns full stack trace
- Production: Returns generic error message

**Response Format:**

```json
{
  "statusCode": 500,
  "message": "Internal Server Error",
  "details": "Stack trace..." // Development only
}
```

### Authentication Middleware

JWT Bearer token authentication is configured in `IdentityServiceExtensions`:

```csharp
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
```

### CORS Middleware

Cross-Origin Resource Sharing is configured to allow:
- Any header
- Any method
- Credentials
- Any origin (configurable)

```csharp
app.UseCors(x => x
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .SetIsOriginAllowed(origin => true));
```

### Logging Middleware

Serilog request logging captures:
- HTTP method
- Request path
- Status code
- Response time

```csharp
app.UseSerilogRequestLogging();
```

---

## Common Response Format

All API responses use the `OperationResult<T>` wrapper:

```csharp
public class OperationResult<T>
{
    public string Status { get; set; }      // "SUCCESS" or "ERROR"
    public string Message { get; set; }     // Optional message
    public string RedirectUrl { get; set; } // Optional redirect
    public T Data { get; set; }             // Response payload
    public string ErrorDetail { get; set; } // Error details
}
```

### Success Response

```json
{
  "status": "SUCCESS",
  "message": "",
  "redirectUrl": "",
  "data": { /* payload */ },
  "errorDetail": ""
}
```

### Error Response

```json
{
  "status": "ERROR",
  "message": "Validation failed",
  "redirectUrl": "",
  "data": null,
  "errorDetail": "Email is already registered"
}
```

---

## Error Handling

### HTTP Status Codes

| Code | Description |
|------|-------------|
| `200 OK` | Successful request |
| `201 Created` | Resource created |
| `204 No Content` | Successful deletion |
| `400 Bad Request` | Validation error |
| `401 Unauthorized` | Authentication required |
| `403 Forbidden` | Insufficient permissions |
| `404 Not Found` | Resource not found |
| `500 Internal Server Error` | Server error |

### Validation Errors

Model validation errors return:

```json
{
  "errors": {
    "Name": ["The Name field is required."],
    "Email": ["The Email field is not a valid e-mail address."]
  },
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "traceId": "00-xxx"
}
```

---

## Pagination & Filtering

### Standard Pagination

Most list endpoints support pagination:

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `pageNumber` | int | 1 | Current page |
| `pageSize` | int | 10 | Items per page |

### Paginated Response

```json
{
  "status": "SUCCESS",
  "data": {
    "totalRecords": 150,
    "pageNumber": 1,
    "pageSize": 10,
    "data": [...]
  }
}
```

### Filtering

Common filter parameters:

| Parameter | Description |
|-----------|-------------|
| `searchTerm` | Text search across relevant fields |
| `status` | Filter by status |
| `fromDate` | Start date filter |
| `toDate` | End date filter |
| `category` | Category filter |

### Sorting

| Parameter | Values | Description |
|-----------|--------|-------------|
| `sortBy` | Field name | Sort column |
| `sortOrder` | `asc`, `desc` | Sort direction |

---

## API Versioning

Currently, the API uses a single version (v1). The version is specified in the Swagger configuration:

```csharp
opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
```

Future versions will be supported via URL path versioning:
- `/api/v1/Products`
- `/api/v2/Products`

---

## Rate Limiting

*Currently not implemented. Future versions may include rate limiting via ASP.NET Core middleware.*

---

## Related Documentation

- [Main README](./README.md) - Solution overview
- [MVC Screens Documentation](./MVC_SCREENS.md) - Admin panel guide
