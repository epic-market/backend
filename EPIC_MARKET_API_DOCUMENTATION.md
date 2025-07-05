# Epic Market API Documentation

## Overview

Epic Market is a comprehensive e-commerce platform that provides APIs for business owners, employees, and customers. The platform supports multi-tenant architecture with businesses managing their outlets, products, orders, and employees.

## Base URL
- Development: `{{baseUrl}}/api`
- Production: Configure in environment variables

## Authentication

The API uses Bearer token authentication. Include the token in the Authorization header:
```
Authorization: Bearer {token}
```

## User Roles

- **BUSINESS_OWNER**: Can manage business, outlets, products, employees, and orders
- **BUSINESS_EMPLOYEE**: Can manage orders and view business data
- **MEMBER**: Regular customers who can browse and place orders

## API Endpoints

### 1. Account Management (`/api/user`)

#### Register User
```http
POST /api/user/register
Content-Type: application/json

{
  "email": "user@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "phone": "+1234567890",
  "password": "SecurePassword123!"
}
```

**Response:**
```json
{
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
  },
  "status": "SUCCESS",
  "message": null
}
```

#### Register Business Owner
```http
POST /api/user/business/register
Content-Type: application/json

{
  "email": "owner@business.com",
  "firstName": "Business",
  "lastName": "Owner",
  "phone": "+1234567890",
  "password": "SecurePassword123!"
}
```

#### Login
```http
POST /api/user/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePassword123!"
}
```

**Response:**
```json
{
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
  }
}
```

#### Login with Phone/OTP
```http
POST /api/user/login/phone
Content-Type: application/json

{
  "phone": "+1234567890",
  "otp": "123456",
  "referenceId": "ref-123-456"
}
```

#### Get User Details
```http
GET /api/user/details/business
Authorization: Bearer {token}
```

**Response:**
```json
{
  "data": {
    "userDetails": {
      "username": "user@example.com",
      "firstName": "John",
      "lastName": "Doe",
      "phone": "+1234567890"
    },
    "userBusiness": {
      "businessId": 123,
      "businessStatus": "Active"
    },
    "securables": [...]
  }
}
```

#### Update User Profile
```http
PUT /api/user/details/business
Authorization: Bearer {token}
Content-Type: application/json

{
  "email": "updated@example.com",
  "firstName": "Updated",
  "lastName": "Name",
  "phone": "+1234567890"
}
```

#### Change Password
```http
POST /api/user/change-password
Authorization: Bearer {token}
Content-Type: application/json

{
  "currentPassword": "CurrentPassword123!",
  "newPassword": "NewPassword123!"
}
```

#### Send OTP
```http
POST /api/user/send-otp
Content-Type: application/json

{
  "phone": "+1234567890",
  "purpose": "LOGIN"
}
```

**Response:**
```json
{
  "data": {
    "referenceId": "ref-123-456",
    "expiresAt": "2024-01-01T12:00:00Z"
  }
}
```

### 2. Business Management (`/api/business`)

#### Register Business Details
```http
POST /api/business
Authorization: Bearer {token}
Content-Type: multipart/form-data

{
  "businessCategoryID": 1,
  "businessName": "My Business",
  "contactNumber": "+1234567890",
  "contactEmail": "contact@business.com",
  "websiteURL": "https://mybusiness.com",
  "address": "123 Business St",
  "city": "Business City",
  "state": "Business State",
  "latitude": 40.7128,
  "longitude": -74.0060,
  "pinCode": "12345",
  "description": "Business description",
  "establishedOn": "2020-01-01",
  "proofType": "Tax ID",
  "proofNumber": "12345678",
  "logoFile": [file],
  "proofFile": [file1, file2]
}
```

**Response:**
```json
{
  "data": {
    "businessId": 123,
    "proofId": 456
  }
}
```

#### Get Business Details
```http
GET /api/business
Authorization: Bearer {token}
```

**Response:**
```json
{
  "data": {
    "businessId": 123,
    "businessName": "My Business",
    "contactNumber": "+1234567890",
    "contactEmail": "contact@business.com",
    "address": "123 Business St",
    "city": "Business City",
    "state": "Business State",
    "status": "Active",
    "establishedOn": "2020-01-01",
    "description": "Business description"
  }
}
```

#### Update Business
```http
PUT /api/business
Authorization: Bearer {token}
Content-Type: application/json

{
  "businessName": "Updated Business Name",
  "contactNumber": "+1234567890",
  "contactEmail": "updated@business.com",
  "address": "456 Updated St",
  "city": "Updated City",
  "state": "Updated State",
  "description": "Updated description"
}
```

#### Get Business Categories
```http
GET /api/business/categories
```

**Response:**
```json
{
  "data": [
    {
      "id": 1,
      "name": "Restaurant",
      "description": "Food & Beverage",
      "imageUrl": "https://example.com/restaurant.jpg"
    },
    {
      "id": 2,
      "name": "Retail",
      "description": "Retail Store",
      "imageUrl": "https://example.com/retail.jpg"
    }
  ]
}
```

#### Get Business Listings
```http
GET /api/business/listings?category=restaurant
```

**Response:**
```json
{
  "data": {
    "businessGroups": [
      {
        "title": "Trending",
        "businesses": [
          {
            "businessId": 123,
            "businessName": "Popular Restaurant",
            "rating": 4.5,
            "imageUrl": "https://example.com/business.jpg",
            "category": "Restaurant"
          }
        ]
      }
    ]
  }
}
```

### 3. Outlet/Branch Management (`/api/outlet`)

#### Get All Outlets
```http
GET /api/outlet?page=1&pageSize=10&searchTerm=branch
Authorization: Bearer {token}
```

**Response:**
```json
{
  "data": {
    "data": [
      {
        "branchId": 1,
        "name": "Main Branch",
        "address": "123 Main St",
        "city": "Main City",
        "state": "Main State",
        "contactNumber": "+1234567890",
        "contactEmail": "main@business.com",
        "isOpen": true,
        "rating": 4.5
      }
    ],
    "totalCount": 1,
    "page": 1,
    "pageSize": 10
  }
}
```

#### Get Outlet by ID
```http
GET /api/outlet/{outletId}
Authorization: Bearer {token}
```

#### Create Outlet
```http
POST /api/outlet
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "New Branch",
  "description": "Branch description",
  "address": "456 Branch St",
  "city": "Branch City",
  "state": "Branch State",
  "pincode": "54321",
  "contactNumber": "+1234567890",
  "contactEmail": "branch@business.com",
  "latitude": 40.7128,
  "longitude": -74.0060,
  "photos": ["photo1-key", "photo2-key"],
  "thumbnail": "thumbnail-key"
}
```

**Response:**
```json
{
  "data": 123
}
```

#### Update Outlet
```http
PUT /api/outlet/{outletId}
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Updated Branch",
  "description": "Updated description",
  "address": "789 Updated St",
  "city": "Updated City",
  "state": "Updated State",
  "contactNumber": "+1234567890",
  "contactEmail": "updated@business.com"
}
```

#### Delete Outlet
```http
DELETE /api/outlet/{outletId}
Authorization: Bearer {token}
```

#### Map Employees to Outlet
```http
POST /api/outlet/map/employees
Authorization: Bearer {token}
Content-Type: application/json

{
  "branchId": 123,
  "employeeIds": [1, 2, 3]
}
```

#### Map Products to Outlet
```http
POST /api/outlet/map/product-variants
Authorization: Bearer {token}
Content-Type: application/json

{
  "branchId": 123,
  "productVariantIds": [1, 2, 3]
}
```

#### Update Outlet Status
```http
PATCH /api/outlet
Authorization: Bearer {token}
Content-Type: application/json

{
  "branchId": 123,
  "is_Open": true
}
```

#### Get Nearby Outlets (Customer)
```http
GET /api/outlet/NearBy/Outlets?latitude=40.7128&longitude=-74.0060&radiusKm=10&category=restaurant&minRating=4.0&sortBy=rating&sortDirection=desc&page=1&pageSize=10
```

**Response:**
```json
{
  "data": [
    {
      "outletId": 123,
      "businessName": "Local Restaurant",
      "outletName": "Main Branch",
      "address": "123 Main St",
      "city": "Main City",
      "distance": 2.5,
      "rating": 4.5,
      "isOpen": true,
      "imageUrl": "https://example.com/outlet.jpg"
    }
  ],
  "totalCount": 1,
  "page": 1,
  "pageSize": 10
}
```

#### Get Customer Outlet Details
```http
GET /api/outlet/customer/{outletId}
```

**Response:**
```json
{
  "data": {
    "outletId": 123,
    "businessName": "Restaurant Name",
    "outletName": "Main Branch",
    "address": "123 Main St",
    "city": "Main City",
    "phone": "+1234567890",
    "email": "contact@restaurant.com",
    "rating": 4.5,
    "isOpen": true,
    "openingHours": "9:00 AM - 10:00 PM",
    "images": ["image1.jpg", "image2.jpg"],
    "isSubscribed": false
  }
}
```

#### Subscribe to Outlet
```http
POST /api/outlet/subscribe/{outletId}
Authorization: Bearer {token}
```

#### Get Subscribed Outlets
```http
GET /api/outlet/subscribed-outlets?page=1&pageSize=10
Authorization: Bearer {token}
```

### 4. Catalog/Product Management (`/api/catalog`)

#### Get All Products
```http
GET /api/catalog?page=1&pageSize=10&searchTerm=product&categoryId=1
Authorization: Bearer {token}
```

**Response:**
```json
{
  "data": {
    "data": [
      {
        "productId": 1,
        "productName": "Sample Product",
        "description": "Product description",
        "price": 29.99,
        "category": "Electronics",
        "imageUrl": "https://example.com/product.jpg",
        "isActive": true,
        "stock": 100
      }
    ],
    "totalCount": 1,
    "page": 1,
    "pageSize": 10
  }
}
```

#### Get Product Details
```http
GET /api/catalog/{productId}
Authorization: Bearer {token}
```

**Response:**
```json
{
  "data": {
    "productId": 1,
    "productName": "Sample Product",
    "description": "Detailed product description",
    "price": 29.99,
    "category": "Electronics",
    "variants": [
      {
        "variantId": 1,
        "name": "Red",
        "price": 29.99,
        "stock": 50
      }
    ],
    "images": ["image1.jpg", "image2.jpg"],
    "specifications": {
      "weight": "1kg",
      "dimensions": "10x10x5cm"
    }
  }
}
```

#### Create Product
```http
POST /api/catalog
Authorization: Bearer {token}
Content-Type: application/json

{
  "productName": "New Product",
  "description": "Product description",
  "categoryId": 1,
  "basePrice": 29.99,
  "variants": [
    {
      "name": "Red",
      "price": 29.99,
      "stock": 100,
      "sku": "PROD-RED-001"
    }
  ]
}
```

#### Update Product
```http
PUT /api/catalog/{productId}
Authorization: Bearer {token}
Content-Type: application/json

{
  "productName": "Updated Product",
  "description": "Updated description",
  "basePrice": 39.99
}
```

#### Delete Product
```http
DELETE /api/catalog/{productId}
Authorization: Bearer {token}
```

#### Quick Actions (Bulk Operations)
```http
POST /api/catalog/QuickActions
Authorization: Bearer {token}
Content-Type: application/json

{
  "action": "ACTIVATE",
  "productIds": [1, 2, 3]
}
```

#### Get Product Variants
```http
GET /api/catalog/{productId}/variants
Authorization: Bearer {token}
```

#### Get Products for POS
```http
GET /api/catalog/POS/{outletId}?page=1&pageSize=20&searchTerm=product
Authorization: Bearer {token}
```

#### Get Customer Products
```http
GET /api/catalog/customer/products?outletId=123&categoryId=1&searchTerm=product&page=1&pageSize=10
```

**Response:**
```json
{
  "data": {
    "data": [
      {
        "productId": 1,
        "productName": "Customer Product",
        "description": "Product for customers",
        "price": 29.99,
        "discountedPrice": 24.99,
        "imageUrl": "https://example.com/product.jpg",
        "rating": 4.5,
        "category": "Electronics",
        "isAvailable": true
      }
    ],
    "totalCount": 1,
    "page": 1,
    "pageSize": 10
  }
}
```

#### Get Customer Product Details
```http
GET /api/catalog/customer/products/{productId}
```

#### Get Categories by Outlet
```http
GET /api/catalog/customer/categories/{outletId}
```

#### Add Product Rating
```http
POST /api/catalog/Rating
Authorization: Bearer {token}
Content-Type: application/json

{
  "productId": 1,
  "rating": 5,
  "comment": "Excellent product!"
}
```

#### Get/Update Product Inventory
```http
GET /api/catalog/Inventory?productVariantId=1&branchId=123
Authorization: Bearer {token}
```

```http
POST /api/catalog/Inventory
Authorization: Bearer {token}
Content-Type: application/json

{
  "productVariantId": 1,
  "branchId": 123,
  "quantity": 100,
  "minStock": 10,
  "maxStock": 200
}
```

### 5. Order Management (`/api/order`)

#### Get All Orders
```http
GET /api/order?page=1&pageSize=10&status=pending&dateFrom=2024-01-01&dateTo=2024-01-31
Authorization: Bearer {token}
```

**Response:**
```json
{
  "data": {
    "data": [
      {
        "orderId": 1,
        "customerName": "John Doe",
        "customerPhone": "+1234567890",
        "orderDate": "2024-01-15T10:30:00Z",
        "status": "Pending",
        "totalAmount": 99.99,
        "items": [
          {
            "productName": "Product 1",
            "quantity": 2,
            "price": 29.99
          }
        ]
      }
    ],
    "totalCount": 1,
    "page": 1,
    "pageSize": 10
  }
}
```

#### Get Order Details
```http
GET /api/order/{orderId}
Authorization: Bearer {token}
```

**Response:**
```json
{
  "data": {
    "orderId": 1,
    "customerName": "John Doe",
    "customerPhone": "+1234567890",
    "customerEmail": "john@example.com",
    "orderDate": "2024-01-15T10:30:00Z",
    "status": "Pending",
    "totalAmount": 99.99,
    "deliveryAddress": "123 Customer St",
    "items": [
      {
        "productId": 1,
        "productName": "Product 1",
        "variantName": "Red",
        "quantity": 2,
        "unitPrice": 29.99,
        "totalPrice": 59.98
      }
    ],
    "paymentMethod": "Credit Card",
    "notes": "Special delivery instructions"
  }
}
```

#### Create Order
```http
POST /api/order
Authorization: Bearer {token}
Content-Type: application/json

{
  "customerName": "John Doe",
  "customerPhone": "+1234567890",
  "customerEmail": "john@example.com",
  "deliveryAddress": "123 Customer St",
  "items": [
    {
      "productVariantId": 1,
      "quantity": 2,
      "unitPrice": 29.99
    }
  ],
  "paymentMethod": "Credit Card",
  "notes": "Special instructions"
}
```

#### Update Order Status
```http
PUT /api/order/{orderId}?OrderStatusId=2
Authorization: Bearer {token}
```

#### Check New Orders
```http
GET /api/order/new?ordered_after=2024-01-15T10:00:00Z&outlet_id=123
Authorization: Bearer {token}
```

#### Get Orders for Mobile
```http
GET /api/order/Mobile?page=1&pageSize=10&status=pending
Authorization: Bearer {token}
```

#### Get Mobile Order Details
```http
GET /api/order/Mobile/{orderId}
Authorization: Bearer {token}
```

#### Create Customer Order
```http
POST /api/order/customer
Authorization: Bearer {token}
Content-Type: application/json

{
  "outletId": 123,
  "items": [
    {
      "productVariantId": 1,
      "quantity": 2
    }
  ],
  "deliveryAddress": "123 Customer St",
  "notes": "Special instructions"
}
```

#### Get Customer Order History
```http
GET /api/order/Customer/OrderHistory?page=1&pageSize=10&sortBy=date&sortOrder=desc
Authorization: Bearer {token}
```

### 6. Employee Management (`/api/employees`)

#### Get All Employees
```http
GET /api/employees?page=1&pageSize=10&searchTerm=employee
Authorization: Bearer {token}
```

**Response:**
```json
{
  "data": {
    "data": [
      {
        "employeeId": 1,
        "firstName": "John",
        "lastName": "Employee",
        "email": "employee@business.com",
        "phone": "+1234567890",
        "role": "Manager",
        "isActive": true,
        "joinDate": "2024-01-01"
      }
    ],
    "totalCount": 1,
    "page": 1,
    "pageSize": 10
  }
}
```

#### Get Employee Details
```http
GET /api/employees/{employeeId}
Authorization: Bearer {token}
```

#### Register Employee
```http
POST /api/employees
Authorization: Bearer {token}
Content-Type: application/json

{
  "firstName": "New",
  "lastName": "Employee",
  "email": "newemployee@business.com",
  "phone": "+1234567890",
  "role": "Staff",
  "salary": 50000,
  "outletIds": [1, 2]
}
```

#### Delete Employee
```http
DELETE /api/employees/{employeeId}
Authorization: Bearer {token}
```

#### Get Employees for Mapping
```http
GET /api/employees/Map/{outletId}
Authorization: Bearer {token}
```

#### Check Employee Link
```http
GET /api/employees/Check?queryParam=encrypted-employee-data
```

#### Create Employee Account
```http
PUT /api/employees/{employeeId}
Content-Type: application/json

{
  "firstName": "Employee",
  "lastName": "Name",
  "email": "employee@business.com",
  "phone": "+1234567890",
  "password": "SecurePassword123!"
}
```

### 7. Dashboard & Analytics (`/api/dashboard`)

#### Get Onboarding Steps
```http
GET /api/dashboard/OnboardingSteps
Authorization: Bearer {token}
```

**Response:**
```json
{
  "data": [
    {
      "stepId": 1,
      "title": "Complete Business Profile",
      "description": "Add your business information",
      "isCompleted": true,
      "order": 1
    },
    {
      "stepId": 2,
      "title": "Add First Product",
      "description": "Add your first product to the catalog",
      "isCompleted": false,
      "order": 2
    }
  ]
}
```

#### Complete Onboarding Step
```http
POST /api/dashboard/completed?stepId=2
Authorization: Bearer {token}
```

#### Get Active Users
```http
GET /api/dashboard/active-users/{outletId}
Authorization: Bearer {token}
```

**Response:**
```json
{
  "data": {
    "monthly": [
      {
        "date": "2024-01",
        "count": 150
      }
    ],
    "weekly": [
      {
        "date": "2024-01-01",
        "count": 35
      }
    ]
  }
}
```

#### Get Gross Merchandise Value
```http
GET /api/dashboard/gmv/{outletId}
Authorization: Bearer {token}
```

#### Get Customer Retention Rate
```http
GET /api/dashboard/retention-rate/{outletId}
Authorization: Bearer {token}
```

#### Get Top Selling Products
```http
GET /api/dashboard/top-products/{outletId}
Authorization: Bearer {token}
```

#### Get Order Status Distribution
```http
GET /api/dashboard/order-status/{outletId}?date=2024-01-15&period=day
Authorization: Bearer {token}
```

### 8. Static Data (`/api/static`)

#### Get Business Categories Options
```http
GET /api/static/business-categories-options
```

**Response:**
```json
{
  "data": [
    {
      "value": 1,
      "text": "Restaurant"
    },
    {
      "value": 2,
      "text": "Retail Store"
    }
  ]
}
```

#### Get Order Status Options
```http
GET /api/static/GetOrderStatusOptions
Authorization: Bearer {token}
```

#### Get Order Type Options
```http
GET /api/static/GetOderTypeOptions
```

#### Get Blog Categories
```http
GET /api/static/GetAllblogCategories
```

#### Get Support Categories
```http
GET /api/static/GetAllSupportCategorys
```

#### Get Person Types
```http
GET /api/static/GetAllPersonTypes
```

#### Get Proof Types
```http
GET /api/static/proof-types
```

#### Subscribe for Offers
```http
POST /api/static/subscribe?gmail=user@example.com
```

#### Get Help Items
```http
GET /api/static/GetHelpItemsforBypage?pagelink=help-page
```

### 9. Search (`/api/search`)

#### Search Products/Businesses
```http
GET /api/search?query=restaurant&type=business&latitude=40.7128&longitude=-74.0060&radius=10
```

**Response:**
```json
{
  "data": {
    "businesses": [
      {
        "businessId": 123,
        "businessName": "Local Restaurant",
        "rating": 4.5,
        "distance": 2.5,
        "imageUrl": "https://example.com/business.jpg"
      }
    ],
    "products": [
      {
        "productId": 1,
        "productName": "Pizza",
        "businessName": "Local Restaurant",
        "price": 15.99,
        "imageUrl": "https://example.com/pizza.jpg"
      }
    ]
  }
}
```

### 10. Notifications (`/api/notification`)

#### Get Notifications
```http
GET /api/notification?page=1&pageSize=10
Authorization: Bearer {token}
```

#### Mark Notification as Read
```http
POST /api/notification/{notificationId}/read
Authorization: Bearer {token}
```

### 11. Support (`/api/support`)

#### Submit Support Query
```http
POST /api/support
Content-Type: application/json

{
  "name": "John Doe",
  "email": "john@example.com",
  "phone": "+1234567890",
  "category": "Technical Issue",
  "subject": "Unable to login",
  "message": "I'm having trouble logging into my account"
}
```

#### Get Support Categories
```http
GET /api/support/categories
```

### 12. Reviews (`/api/review`)

#### Submit Review
```http
POST /api/review
Authorization: Bearer {token}
Content-Type: application/json

{
  "businessId": 123,
  "outletId": 456,
  "rating": 5,
  "comment": "Excellent service and food quality!",
  "orderId": 789
}
```

#### Get Reviews
```http
GET /api/review?businessId=123&page=1&pageSize=10
```

### 13. Files (`/api/files`)

#### Upload File
```http
POST /api/files/upload
Authorization: Bearer {token}
Content-Type: multipart/form-data

{
  "file": [binary file data],
  "category": "product-image"
}
```

**Response:**
```json
{
  "data": {
    "fileKey": "uploads/products/12345-image.jpg",
    "url": "https://cdn.example.com/uploads/products/12345-image.jpg"
  }
}
```

### 14. Activity (`/api/activity`)

#### Get Activity Log
```http
GET /api/activity?page=1&pageSize=10&dateFrom=2024-01-01&dateTo=2024-01-31
Authorization: Bearer {token}
```

## Error Responses

All endpoints return errors in the following format:

```json
{
  "status": "ERROR",
  "message": "Error description",
  "data": null
}
```

### Common HTTP Status Codes

- `200 OK`: Successful request
- `201 Created`: Resource created successfully
- `400 Bad Request`: Invalid request parameters
- `401 Unauthorized`: Authentication required or invalid token
- `403 Forbidden`: Insufficient permissions
- `404 Not Found`: Resource not found
- `500 Internal Server Error`: Server error

## Data Models

### User Model
```json
{
  "id": 1,
  "userName": "user@example.com",
  "email": "user@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "+1234567890",
  "isActive": true,
  "roles": ["BUSINESS_OWNER"]
}
```

### Business Model
```json
{
  "businessId": 123,
  "businessName": "My Business",
  "category": "Restaurant",
  "contactNumber": "+1234567890",
  "contactEmail": "contact@business.com",
  "address": "123 Business St",
  "city": "Business City",
  "state": "Business State",
  "pinCode": "12345",
  "latitude": 40.7128,
  "longitude": -74.0060,
  "status": "Active",
  "establishedOn": "2020-01-01",
  "description": "Business description",
  "logoUrl": "https://example.com/logo.jpg"
}
```

### Product Model
```json
{
  "productId": 1,
  "productName": "Sample Product",
  "description": "Product description",
  "categoryId": 1,
  "category": "Electronics",
  "basePrice": 29.99,
  "isActive": true,
  "images": ["image1.jpg", "image2.jpg"],
  "variants": [
    {
      "variantId": 1,
      "name": "Red",
      "price": 29.99,
      "stock": 100,
      "sku": "PROD-RED-001"
    }
  ]
}
```

### Order Model
```json
{
  "orderId": 1,
  "customerName": "John Doe",
  "customerPhone": "+1234567890",
  "customerEmail": "john@example.com",
  "orderDate": "2024-01-15T10:30:00Z",
  "status": "Pending",
  "totalAmount": 99.99,
  "deliveryAddress": "123 Customer St",
  "items": [
    {
      "productId": 1,
      "productName": "Product 1",
      "variantName": "Red",
      "quantity": 2,
      "unitPrice": 29.99,
      "totalPrice": 59.98
    }
  ],
  "paymentMethod": "Credit Card",
  "notes": "Special delivery instructions"
}
```

### Outlet Model
```json
{
  "outletId": 123,
  "name": "Main Branch",
  "description": "Main branch description",
  "address": "123 Main St",
  "city": "Main City",
  "state": "Main State",
  "pinCode": "12345",
  "contactNumber": "+1234567890",
  "contactEmail": "main@business.com",
  "latitude": 40.7128,
  "longitude": -74.0060,
  "isOpen": true,
  "rating": 4.5,
  "images": ["image1.jpg", "image2.jpg"]
}
```

## Pagination

Most list endpoints support pagination with these parameters:

- `page`: Page number (starts from 1)
- `pageSize`: Number of items per page (default: 10, max: 100)

Response format:
```json
{
  "data": {
    "data": [...],
    "totalCount": 100,
    "page": 1,
    "pageSize": 10,
    "totalPages": 10
  }
}
```

## Filtering and Sorting

Many endpoints support filtering and sorting:

### Common Filter Parameters
- `searchTerm`: Text search across relevant fields
- `dateFrom`, `dateTo`: Date range filtering
- `status`: Filter by status
- `categoryId`: Filter by category
- `isActive`: Filter by active/inactive status

### Common Sort Parameters
- `sortBy`: Field to sort by
- `sortDirection`: `asc` or `desc`

## Rate Limiting

API requests are rate-limited to prevent abuse:
- Authenticated users: 1000 requests per hour
- Anonymous users: 100 requests per hour

## File Upload Guidelines

### Supported File Types
- **Images**: JPG, PNG, GIF, WebP (max 5MB each)
- **Documents**: PDF, DOC, DOCX (max 10MB each)

### File Storage
- Files are stored in AWS S3
- CDN URLs are returned for public access
- Files are automatically optimized for web delivery

## Security

### Authentication
- JWT tokens with 24-hour expiration
- Refresh tokens for extended sessions
- Role-based access control (RBAC)

### Data Protection
- All sensitive data is encrypted
- HTTPS required for all API calls
- Input validation on all endpoints
- SQL injection prevention

## Environment Variables

```bash
# Database
DATABASE_URL=postgresql://username:password@localhost:5432/epicmarket

# JWT
JWT_SECRET=your-jwt-secret-key
JWT_EXPIRES_IN=24h

# File Storage
AWS_ACCESS_KEY_ID=your-aws-access-key
AWS_SECRET_ACCESS_KEY=your-aws-secret-key
AWS_S3_BUCKET=your-s3-bucket-name

# Email Service
SMTP_HOST=smtp.gmail.com
SMTP_PORT=587
SMTP_USER=your-email@gmail.com
SMTP_PASSWORD=your-app-password

# SMS Service
TWILIO_ACCOUNT_SID=your-twilio-account-sid
TWILIO_AUTH_TOKEN=your-twilio-auth-token
```

## Postman Collection

A comprehensive Postman collection is available at:
`/Postman Json/Business Owner Web.postman_collection.json`

Import this collection to test all API endpoints with pre-configured examples and environment variables.

## Support

For API support and documentation updates, please contact:
- Email: api-support@epicmarket.com
- Documentation: [API Documentation Portal]
- GitHub Issues: [Repository Issues]

---

**Last Updated**: January 2024
**API Version**: 1.0
**Documentation Version**: 1.0