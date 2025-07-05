# Epic Market Backend - Comprehensive API Documentation

## Table of Contents

1. [Project Overview](#project-overview)
2. [Architecture](#architecture)
3. [Authentication & Authorization](#authentication--authorization)
4. [API Endpoints](#api-endpoints)
5. [Data Transfer Objects (DTOs)](#data-transfer-objects-dtos)
6. [Services & Business Logic](#services--business-logic)
7. [MVC Admin Controllers](#mvc-admin-controllers)
8. [Database Models](#database-models)
9. [Usage Examples](#usage-examples)
10. [Error Handling](#error-handling)
11. [File Management](#file-management)
12. [Configuration](#configuration)

---

## Project Overview

**Epic Market Backend** is a comprehensive e-commerce marketplace system built with .NET 8, featuring:

- **REST API** for frontend applications
- **MVC Admin Panel** for administrative tasks
- **Multi-tenant architecture** supporting multiple businesses
- **Role-based access control** (Business Owner, Business Employee, Admin)
- **File upload and management** with AWS S3 integration
- **Order management system**
- **Product catalog management**
- **User authentication and authorization**

### Key Technologies
- .NET 8
- Entity Framework Core
- ASP.NET Core Identity
- AutoMapper
- JWT Authentication
- Docker & Docker Compose
- AWS S3 (File Storage)

---

## Architecture

The project follows a clean architecture pattern with the following layers:

```
EpicMarket.Business.API/     # Web API Controllers
EpicMarket.Admin.MVC/        # MVC Admin Panel
EpicMarket.Services/         # Business Logic Services
EpicMarket.Contracts/        # Service Interfaces
EpicMarket.Entities/         # DTOs and Models
EpicMarket.Data/             # Data Layer & EF Context
EpicMarket.Data.Webapp/      # Additional Web Application
```

---

## Authentication & Authorization

### Roles
- **MEMBER**: Basic user role
- **BUSINESS_OWNER**: Business owner with full business management rights
- **BUSINESS_EMPLOYEE**: Employee with limited business access
- **ADMIN**: System administrator

### Authentication Flow
1. User registers or logs in
2. JWT token is generated and returned
3. Token must be included in Authorization header for protected endpoints
4. Token contains user claims including roles and business information

---

## API Endpoints

### Base API Controller

All API controllers inherit from `BaseApiController` which provides:

```csharp
// Base URL: /api/[controller]
// Common properties:
- BusinessId: Current user's business ID
- LoggedInUserName: Current user's username
- PageSource: Current request URL
- AdminPersonID: System admin user ID
```

### Account Controller (`/api/account`)

Handles user authentication and account management.

#### Endpoints

| Method | Endpoint | Description | Roles | Request Body |
|--------|----------|-------------|-------|--------------|
| POST | `/register` | Register new user | Anonymous | `RegisterDto` |
| POST | `/login` | User login | Anonymous | `LoginDto` |
| GET | `/info` | Get current user info | Authenticated | - |
| POST | `/changepassword` | Change user password | Authenticated | `ChangePasswordParams` |
| POST | `/ResetPassword` | Reset user password | Anonymous | `ResetPasswordParams` |
| GET | `/CheckResetPasswordLink` | Validate reset password link | Anonymous | Query: `queryParam` |
| POST | `/setNewPassword` | Set new password after reset | Anonymous | `SetNewPasswordParams` |

#### Request/Response Examples

**Register User:**
```json
POST /api/account/register
{
    "firstName": "John",
    "lastName": "Doe",
    "email": "john@example.com",
    "phone": "+1234567890",
    "password": "Password123!"
}

Response:
{
    "status": "SUCCESS",
    "data": {
        "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
    }
}
```

**Login:**
```json
POST /api/account/login
{
    "email": "john@example.com",
    "password": "Password123!"
}

Response:
{
    "status": "SUCCESS",
    "data": {
        "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
    }
}
```

### Products Controller (`/api/products`)

Manages product catalog functionality.

#### Endpoints

| Method | Endpoint | Description | Roles | Request Body |
|--------|----------|-------------|-------|--------------|
| GET | `/Map/{outletID}` | Get products for outlet mapping | Business Owner | - |
| POST | `/` | Add new product | Business Owner | `AddProductsDto` (with files) |
| PUT | `/{id}` | Update product | Business Owner | `AddProductsDto` (with files) |
| GET | `/` | Get all products | Business Owner, Employee | Query: `ProductParams` |
| GET | `/{id}` | Get product details | Business Owner, Employee | - |
| POST | `/verify` | Verify product catalog | Business Owner | `VerifyDto` |
| DELETE | `/images` | Delete product images | Business Owner | `ListOfImages` |
| DELETE | `/{id}` | Delete product | Business Owner | - |

#### Request/Response Examples

**Add Product:**
```json
POST /api/products
Content-Type: multipart/form-data

{
    "barcode": 1234567890,
    "name": "Premium Coffee",
    "description": "High-quality coffee beans",
    "category": "Beverages",
    "rate": 29.99,
    "inStock": true,
    "isRecommended": true,
    "maximumOrderPurchase": 10,
    "products": [file1, file2], // Image files
    "thumbnail": file3 // Thumbnail file
}

Response:
{
    "status": "SUCCESS",
    "data": 123 // Product ID
}
```

**Get All Products:**
```json
GET /api/products?pageIndex=1&pageSize=10&sortColumn=name&ascending=true&searchTerm=coffee

Response:
{
    "status": "SUCCESS",
    "data": {
        "items": [
            {
                "productId": 123,
                "name": "Premium Coffee",
                "description": "High-quality coffee beans",
                "rate": 29.99,
                "isActive": true,
                "inStock": true,
                "count": 50,
                "thumbnail": "https://...",
                "status": "Active"
            }
        ],
        "totalCount": 25,
        "pageIndex": 1,
        "pageSize": 10
    }
}
```

### Orders Controller (`/api/orders`)

Handles order management functionality.

#### Endpoints

| Method | Endpoint | Description | Roles | Request Body |
|--------|----------|-------------|-------|--------------|
| POST | `/AddOrder` | Create new order | Business Owner, Employee | `OrdersDto` |
| GET | `/GetSingleOrder` | Get order details | Business Owner, Employee | Query: `OrderId` |
| POST | `/UpdateStatus` | Update order status | Business Owner, Employee | Query: `OrderId`, `OrderStatus` |
| GET | `/GetOrderStatusOptions` | Get status options | Business Owner, Employee | - |
| GET | `/GetAllOrders` | Get all orders | Business Owner, Employee | `OrderParams` |

#### Request/Response Examples

**Create Order:**
```json
POST /api/orders/AddOrder
{
    "outletId": 1,
    "orderDate": "2024-01-15T10:30:00Z",
    "paymentMode": "Credit Card",
    "orderedModeId": 1,
    "statusId": 1,
    "totalPrice": 89.97,
    "totalItems": 3,
    "customerName": "Jane Smith",
    "customerEmail": "jane@example.com",
    "customerPhone": "+1234567890",
    "orderDetails": "Special instructions",
    "orderDetailsDtos": [
        {
            "productId": 123,
            "quantity": 2,
            "unitPrice": 29.99
        }
    ]
}

Response:
{
    "status": "SUCCESS",
    "data": 456 // Order ID
}
```

### Business Controller (`/api/business`)

Manages business registration and information.

#### Endpoints

| Method | Endpoint | Description | Roles | Request Body |
|--------|----------|-------------|-------|--------------|
| POST | `/register` | Register new business | Authenticated | `BusinessRegisterDto` |
| GET | `/info` | Get business information | Business Owner, Employee | - |
| PUT | `/update` | Update business info | Business Owner | `BusinessDTO` |

### Branch Controller (`/api/branch`)

Manages business branches/outlets.

#### Endpoints

| Method | Endpoint | Description | Roles | Request Body |
|--------|----------|-------------|-------|--------------|
| POST | `/` | Add new branch | Business Owner | `BranchDto` |
| GET | `/` | Get all branches | Business Owner, Employee | `BranchParams` |
| GET | `/{id}` | Get branch details | Business Owner, Employee | - |
| PUT | `/{id}` | Update branch | Business Owner | `BranchDto` |
| DELETE | `/{id}` | Delete branch | Business Owner | - |

### Employees Controller (`/api/employees`)

Manages business employees.

#### Endpoints

| Method | Endpoint | Description | Roles | Request Body |
|--------|----------|-------------|-------|--------------|
| POST | `/` | Add new employee | Business Owner | `EmployeeDto` |
| GET | `/` | Get all employees | Business Owner | `EmployeeParams` |
| GET | `/{id}` | Get employee details | Business Owner | - |
| PUT | `/{id}` | Update employee | Business Owner | `EmployeeDto` |
| DELETE | `/{id}` | Delete employee | Business Owner | - |

### Static Controller (`/api/static`)

Provides static data and configuration.

#### Endpoints

| Method | Endpoint | Description | Roles | Request Body |
|--------|----------|-------------|-------|--------------|
| GET | `/faqs` | Get FAQ list | Anonymous | - |
| GET | `/pages` | Get static pages | Anonymous | - |
| GET | `/configurations` | Get app configurations | Authenticated | - |

### Files Controller (`/api/files`)

Handles file operations.

#### Endpoints

| Method | Endpoint | Description | Roles | Request Body |
|--------|----------|-------------|-------|--------------|
| POST | `/upload` | Upload file | Authenticated | `IFormFile` |
| GET | `/{fileKey}` | Get file | Authenticated | - |
| DELETE | `/{fileKey}` | Delete file | Authenticated | - |

### Home Controller (`/api/home`)

Provides dashboard and summary data.

#### Endpoints

| Method | Endpoint | Description | Roles | Request Body |
|--------|----------|-------------|-------|--------------|
| GET | `/dashboard` | Get dashboard data | Business Owner, Employee | - |
| GET | `/summary` | Get business summary | Business Owner, Employee | - |

### Support Controller (`/api/support`)

Handles support tickets and queries.

#### Endpoints

| Method | Endpoint | Description | Roles | Request Body |
|--------|----------|-------------|-------|--------------|
| POST | `/ticket` | Create support ticket | Authenticated | `SupportDTO` |
| GET | `/tickets` | Get user tickets | Authenticated | - |
| GET | `/ticket/{id}` | Get ticket details | Authenticated | - |

---

## Data Transfer Objects (DTOs)

### Core DTOs

#### RegisterDto
```csharp
public class RegisterDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
}
```

#### LoginDto
```csharp
public class LoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}
```

#### ProductsDto
```csharp
public class ProductsDto
{
    public int? Id { get; set; }
    public long? Barcode { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public double Rate { get; set; }
    public bool IsActive { get; set; }
    public bool InStock { get; set; }
    public List<string> Images { get; set; }
    public bool IsRecommended { get; set; }
    public int? MaximumOrderPurchase { get; set; }
    public string Status { get; set; }
    public string Thumbnail { get; set; }
}
```

#### AddProductsDto
```csharp
public class AddProductsDto
{
    public long? Barcode { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    public string Category { get; set; }
    [Required]
    public double Rate { get; set; }
    public bool InStock { get; set; } = false;
    public bool IsRecommended { get; set; } = false;
    public int? MaximumOrderPurchase { get; set; }
    public IFormFile[] Products { get; set; }
    public IFormFile Thumbnail { get; set; }
}
```

#### OrdersDto
```csharp
public class OrdersDto
{
    public int OutletId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public string PaymentMode { get; set; }
    public int OrderedModeId { get; set; }
    public int StatusId { get; set; }
    public double TotalPrice { get; set; }
    public int TotalItems { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerPhone { get; set; }
    public string OrderDetails { get; set; }
    public List<OrderDetailsDto> orderDetailsDtos { get; set; }
}
```

#### OrderDetailsDto
```csharp
public class OrderDetailsDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public double UnitPrice { get; set; }
    public double TotalPrice { get; set; }
    public string ProductName { get; set; }
}
```

#### BranchDto
```csharp
public class BranchDto
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public string ContactPersonName { get; set; }
    public string ContactPersonPhone { get; set; }
}
```

#### EmployeeDto
```csharp
public class EmployeeDto
{
    public int? Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Position { get; set; }
    public bool IsActive { get; set; }
    public int BranchId { get; set; }
}
```

### Response DTOs

#### OperationResult<T>
```csharp
public class OperationResult<T>
{
    public string Status { get; set; } = "SUCCESS";
    public T Data { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }
}
```

#### GetDataResult<T>
```csharp
public class GetDataResult<T>
{
    public T Items { get; set; }
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}
```

---

## Services & Business Logic

### Service Interfaces

#### IProductService
```csharp
public interface IProductService
{
    Task<List<ProductsMapOptionResult>> GetAllProductForMap(int businessID, int outletID);
    Task<int> AddProduct(AddProductsDto productsDto, string UserName, int businessID, string PageSource);
    Task<int> UpdateProducts(AddProductsDto productsDto, int id, string UserName, int businessID, string PageSource);
    Task<GetDataResult<List<ProductResult>>> GetAllProducts(ProductParams productResult, int businessID);
    Task<ProductsDto> GetProductDetails(int productId);
    Task<int> VerifyCatalog(VerifyDto verifyBranchDto, string UserName, int AdminPersonID, string PageSource);
    Task<bool> deleteImage(ListOfImages keys, string UserName);
    Task deleteCatelog(int id, string UserName);
}
```

#### IOrderService
```csharp
public interface IOrderService
{
    Task<int> CreateOrder(OrdersDto order, string UserName, string PageSource);
    Task<OrdersDto> GetSingleOrder(int OrderId);
    Task<int> UpdateStatus(int OrderId, string OrderStatus);
    Task<List<DropDownOptions>> GetOrderStatusOptions();
    Task<GetDataResult<List<OrderResult>>> GetAllOrders(OrderParams orderParams, int businessID);
}
```

#### ITokenService
```csharp
public interface ITokenService
{
    Task<string> CreateToken(AppUser user);
    Task<string> ResetPassword(ResetPasswordParams resetPassword);
    CheckResetLinkResult CheckResetPasswordLink(string queryParam);
    Task<string> setNewPassword(SetNewPasswordParams setNewPasswordParams);
}
```

#### IFileService
```csharp
public interface IFileService
{
    Task<string> UploadFileAsync(IFormFile file, string folderPath, string fileName);
    Task<FileDto> GetFileByKeyAsync(string fileKey);
    Task<bool> DeleteFileAsync(string fileKey);
    Task<List<string>> GetFileListAsync(string folderPath);
}
```

### Key Services

1. **ProductService**: Handles all product-related operations
2. **OrderService**: Manages order lifecycle
3. **BranchService**: Manages business branches
4. **EmployeeService**: Handles employee management
5. **TokenService**: JWT token operations
6. **FileService**: File upload/download operations
7. **CommunicationService**: Email/SMS notifications
8. **ProfileService**: User profile management

---

## MVC Admin Controllers

The admin panel provides comprehensive management interfaces with the following controllers:

### Core Admin Controllers

1. **BusinessesController**: Business management
2. **ProductsController**: Product catalog management
3. **OrdersController**: Order management
4. **EmployeesController**: Employee management
5. **TasksController**: Task management
6. **SupportTicketsController**: Support ticket management
7. **ApplicationConfigurationsController**: System configuration
8. **FAQsController**: FAQ management
9. **BlogsController**: Blog management
10. **EventLogsController**: System event logs

### Admin Features

- **Full CRUD operations** for all entities
- **Data grids** with sorting, filtering, and pagination
- **File upload** interfaces
- **Role-based access control**
- **Audit trail** for all changes
- **Dashboard** with analytics
- **Report generation**

---

## Database Models

### Core Entities

1. **AppUser**: User accounts
2. **Business**: Business information
3. **Branch (Outlet)**: Business locations
4. **Catalog**: Product catalog
5. **Orders**: Order records
6. **OrderDetails**: Order line items
7. **Employees**: Business employees
8. **Attachments**: File attachments
9. **Tasks**: Task management
10. **SupportTickets**: Support tickets

### Relationships

- **Business** → **Branches** (1:Many)
- **Business** → **Employees** (1:Many)
- **Business** → **Products** (1:Many)
- **Branch** → **Orders** (1:Many)
- **Orders** → **OrderDetails** (1:Many)
- **Products** → **Attachments** (1:Many)

---

## Usage Examples

### Complete Authentication Flow

```csharp
// 1. Register new user
var registerDto = new RegisterDto
{
    FirstName = "John",
    LastName = "Doe",
    Email = "john@example.com",
    Phone = "+1234567890",
    Password = "Password123!"
};

var registerResponse = await httpClient.PostAsJsonAsync("/api/account/register", registerDto);
var registerResult = await registerResponse.Content.ReadFromJsonAsync<OperationResult<TokenDto>>();

// 2. Use token for authenticated requests
httpClient.DefaultRequestHeaders.Authorization = 
    new AuthenticationHeaderValue("Bearer", registerResult.Data.Token);

// 3. Get user information
var userInfoResponse = await httpClient.GetFromJsonAsync<OperationResult<LoginResult>>("/api/account/info");
```

### Product Management Flow

```csharp
// 1. Create product with images
var formData = new MultipartFormDataContent();
formData.Add(new StringContent("Premium Coffee"), "name");
formData.Add(new StringContent("High-quality coffee beans"), "description");
formData.Add(new StringContent("29.99"), "rate");
formData.Add(new StringContent("true"), "inStock");

// Add image files
var imageContent = new ByteArrayContent(imageBytes);
imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
formData.Add(imageContent, "products", "coffee.jpg");

var productResponse = await httpClient.PostAsync("/api/products", formData);
var productResult = await productResponse.Content.ReadFromJsonAsync<OperationResult<int>>();

// 2. Get all products
var products = await httpClient.GetFromJsonAsync<OperationResult<GetDataResult<List<ProductResult>>>>(
    "/api/products?pageIndex=1&pageSize=10");
```

### Order Management Flow

```csharp
// 1. Create order
var orderDto = new OrdersDto
{
    OutletId = 1,
    PaymentMode = "Credit Card",
    OrderedModeId = 1,
    StatusId = 1,
    TotalPrice = 89.97,
    TotalItems = 3,
    CustomerName = "Jane Smith",
    CustomerEmail = "jane@example.com",
    CustomerPhone = "+1234567890",
    OrderDetails = "Special instructions",
    orderDetailsDtos = new List<OrderDetailsDto>
    {
        new OrderDetailsDto
        {
            ProductId = 123,
            Quantity = 2,
            UnitPrice = 29.99
        }
    }
};

var orderResponse = await httpClient.PostAsJsonAsync("/api/orders/AddOrder", orderDto);
var orderResult = await orderResponse.Content.ReadFromJsonAsync<OperationResult<int>>();

// 2. Update order status
var updateResponse = await httpClient.PostAsync(
    $"/api/orders/UpdateStatus?OrderId={orderResult.Data}&OrderStatus=Processing", null);
```

### File Upload Example

```csharp
// Upload file with metadata
var formData = new MultipartFormDataContent();
var fileContent = new ByteArrayContent(fileBytes);
fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/pdf");
formData.Add(fileContent, "file", "document.pdf");
formData.Add(new StringContent("BUSINESS_LOGO"), "entityType");

var uploadResponse = await httpClient.PostAsync("/api/files/upload", formData);
var uploadResult = await uploadResponse.Content.ReadFromJsonAsync<OperationResult<string>>();
```

---

## Error Handling

### Standard Error Response Format

```json
{
    "status": "ERROR",
    "message": "Error description",
    "errors": [
        "Validation error 1",
        "Validation error 2"
    ],
    "data": null
}
```

### Common HTTP Status Codes

- **200 OK**: Successful request
- **201 Created**: Resource created successfully
- **400 Bad Request**: Invalid request data
- **401 Unauthorized**: Authentication required
- **403 Forbidden**: Access denied
- **404 Not Found**: Resource not found
- **500 Internal Server Error**: Server error

### Error Handling Best Practices

1. **Always check response status** before processing data
2. **Handle authentication errors** by redirecting to login
3. **Show user-friendly messages** for validation errors
4. **Log errors** for debugging purposes
5. **Implement retry logic** for transient errors

---

## File Management

### Supported File Types

- **Images**: JPG, PNG, GIF, WebP
- **Documents**: PDF, DOC, DOCX, XLS, XLSX
- **Archives**: ZIP, RAR

### File Storage Structure

```
/BusinessID/
  ├── products/
  │   ├── images/
  │   └── thumbnails/
  ├── business/
  │   ├── logos/
  │   └── documents/
  └── attachments/
      ├── support/
      └── tasks/
```

### File Upload Constraints

- **Maximum file size**: 10MB
- **Supported formats**: As defined in configuration
- **Virus scanning**: Enabled for uploaded files
- **Auto-resizing**: For images based on requirements

---

## Configuration

### Application Settings

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EpicMarket;Trusted_Connection=true;"
  },
  "JwtSettings": {
    "TokenKey": "your-secret-key",
    "Issuer": "EpicMarket",
    "Audience": "EpicMarket-Users",
    "ExpiryInMinutes": 1440
  },
  "AWS": {
    "S3": {
      "BucketName": "epicmarket-files",
      "Region": "us-east-1"
    }
  },
  "Email": {
    "SmtpServer": "smtp.gmail.com",
    "Port": 587,
    "Username": "your-email@gmail.com",
    "Password": "your-app-password"
  }
}
```

### Environment Variables

- `ASPNETCORE_ENVIRONMENT`: Development/Production
- `CONNECTION_STRING`: Database connection
- `JWT_SECRET`: JWT signing key
- `AWS_ACCESS_KEY_ID`: AWS access key
- `AWS_SECRET_ACCESS_KEY`: AWS secret key

### Docker Configuration

```yaml
# docker-compose.yml
version: '3.8'
services:
  api:
    build: .
    ports:
      - "8080:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Server=db;Database=EpicMarket;User=sa;Password=Password123!
    depends_on:
      - db
  
  db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - SA_PASSWORD=Password123!
      - ACCEPT_EULA=Y
    ports:
      - "1433:1433"
```

---

## Security Considerations

### Authentication & Authorization

1. **JWT tokens** with expiration
2. **Role-based access control** (RBAC)
3. **Password hashing** with Identity
4. **HTTPS enforcement** in production
5. **CORS configuration** for allowed origins

### Data Protection

1. **Input validation** on all endpoints
2. **SQL injection prevention** with EF Core
3. **XSS protection** with input sanitization
4. **File upload validation** with type checking
5. **Rate limiting** for API endpoints

### Best Practices

1. **Always validate user input**
2. **Use parameterized queries**
3. **Implement proper error handling**
4. **Log security events**
5. **Regular security audits**
6. **Keep dependencies updated**

---

This documentation provides a comprehensive overview of the Epic Market Backend system. For specific implementation details, refer to the source code and inline comments within each component.