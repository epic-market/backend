using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EpicMarket.Business.API.Tests.TestHelpers
{
    public static class TestDataHelper
    {
        public static AppUser GetTestUser()
        {
            return new AppUser
            {
                Id = 1,
                UserName = "testuser@example.com",
                Email = "testuser@example.com",
                FirstName = "Test",
                LastName = "User",
                PhoneNumber = "+1234567890",
                IsActive = true
            };
        }

        public static AppUser GetTestBusinessOwner()
        {
            return new AppUser
            {
                Id = 2,
                UserName = "businessowner@example.com",
                Email = "businessowner@example.com",
                FirstName = "Business",
                LastName = "Owner",
                PhoneNumber = "+1234567891",
                IsActive = true
            };
        }

        public static RegisterDto GetValidRegisterDto()
        {
            return new RegisterDto
            {
                Email = "newuser@example.com",
                FirstName = "New",
                LastName = "User",
                Phone = "+1234567892",
                Password = "TestPassword123!"
            };
        }

        public static LoginDto GetValidLoginDto()
        {
            return new LoginDto
            {
                Email = "testuser@example.com",
                Password = "TestPassword123!"
            };
        }

        public static LoginPhoneDto GetValidLoginPhoneDto()
        {
            return new LoginPhoneDto
            {
                Phone = "+1234567890",
                OTP = "123456",
                ReferenceId = "ref-123-456"
            };
        }

        public static BusinessRegisterDto GetValidBusinessRegisterDto()
        {
            return new BusinessRegisterDto
            {
                BusinessCategoryID = 1,
                BusinessName = "Test Business",
                ContactNumber = "+1234567890",
                ContactEmail = "business@example.com",
                WebsiteURL = "https://testbusiness.com",
                Address = "123 Test Street",
                City = "Test City",
                State = "Test State",
                Latitude = 40.7128,
                Longitude = -74.0060,
                PinCode = "12345",
                Description = "Test business description",
                EstablishedOn = DateTime.Now.AddYears(-5),
                ProofType = "Tax ID",
                ProofNumber = "12345678"
            };
        }

        public static AddProductsParams GetValidAddProductsParams()
        {
            return new AddProductsParams
            {
                ProductName = "Test Product",
                Description = "Test product description",
                CategoryId = 1,
                BasePrice = 29.99m,
                IsActive = true,
                ProductVariants = new List<ProductVariantDto>
                {
                    new ProductVariantDto
                    {
                        Name = "Red",
                        Price = 29.99m,
                        Stock = 100,
                        SKU = "PROD-RED-001"
                    }
                }
            };
        }

        public static List<ClaimsIdentity> GetBusinessOwnerClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "2"),
                new Claim(ClaimTypes.Name, "businessowner@example.com"),
                new Claim(ClaimTypes.Role, "BUSINESS_OWNER")
            };

            return new List<ClaimsIdentity> { new ClaimsIdentity(claims, "Test") };
        }

        public static List<ClaimsIdentity> GetMemberClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "testuser@example.com"),
                new Claim(ClaimTypes.Role, "MEMBER")
            };

            return new List<ClaimsIdentity> { new ClaimsIdentity(claims, "Test") };
        }

        public static List<ClaimsIdentity> GetEmployeeClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "3"),
                new Claim(ClaimTypes.Name, "employee@example.com"),
                new Claim(ClaimTypes.Role, "BUSINESS_EMPLOYEE")
            };

            return new List<ClaimsIdentity> { new ClaimsIdentity(claims, "Test") };
        }

        public static OutletRegisterDto GetValidOutletRegisterDto()
        {
            return new OutletRegisterDto
            {
                Name = "Test Outlet",
                Description = "Test outlet description",
                Address = "456 Outlet Street",
                City = "Outlet City",
                State = "Outlet State",
                Pincode = "54321",
                ContactNumber = "+1234567893",
                ContactEmail = "outlet@example.com",
                Latitude = 40.7128,
                Longitude = -74.0060
            };
        }

        public static OrderDto GetValidOrderDto()
        {
            return new OrderDto
            {
                CustomerName = "Test Customer",
                CustomerPhone = "+1234567894",
                CustomerEmail = "customer@example.com",
                DeliveryAddress = "789 Customer Street",
                Items = new List<OrderItemDto>
                {
                    new OrderItemDto
                    {
                        ProductVariantId = 1,
                        Quantity = 2,
                        UnitPrice = 29.99m
                    }
                },
                PaymentMethod = "Credit Card",
                Notes = "Test order notes"
            };
        }

        public static EmployeeRegisterDto GetValidEmployeeRegisterDto()
        {
            return new EmployeeRegisterDto
            {
                FirstName = "Test",
                LastName = "Employee",
                Email = "employee@example.com",
                Phone = "+1234567895",
                Role = "Staff",
                Salary = 50000,
                OutletIds = new List<int> { 1, 2 }
            };
        }

        public static TokenDto GetValidTokenDto()
        {
            return new TokenDto
            {
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.test.token"
            };
        }

        public static OTPRequest GetValidOTPRequest()
        {
            return new OTPRequest
            {
                Phone = "+1234567890",
                Purpose = "LOGIN"
            };
        }

        public static OTPResponse GetValidOTPResponse()
        {
            return new OTPResponse
            {
                ReferenceId = "ref-123-456",
                ExpiresAt = DateTime.Now.AddMinutes(5)
            };
        }

        public static ChangePasswordParams GetValidChangePasswordParams()
        {
            return new ChangePasswordParams
            {
                CurrentPassword = "CurrentPassword123!",
                NewPassword = "NewPassword123!"
            };
        }

        public static List<BusinessCategoryDto> GetTestBusinessCategories()
        {
            return new List<BusinessCategoryDto>
            {
                new BusinessCategoryDto
                {
                    Id = 1,
                    Name = "Restaurant",
                    Description = "Food & Beverage",
                    ImageUrl = "https://example.com/restaurant.jpg"
                },
                new BusinessCategoryDto
                {
                    Id = 2,
                    Name = "Retail",
                    Description = "Retail Store",
                    ImageUrl = "https://example.com/retail.jpg"
                }
            };
        }

        public static GetDataResult<List<ProductResult>> GetTestProductResults()
        {
            return new GetDataResult<List<ProductResult>>
            {
                Data = new List<ProductResult>
                {
                    new ProductResult
                    {
                        ProductId = 1,
                        ProductName = "Test Product",
                        Description = "Test product description",
                        BasePrice = 29.99m,
                        CategoryName = "Electronics",
                        IsActive = true
                    }
                },
                TotalCount = 1,
                Page = 1,
                PageSize = 10
            };
        }

        public static GetDataResult<List<OutletResult>> GetTestOutletResults()
        {
            return new GetDataResult<List<OutletResult>>
            {
                Data = new List<OutletResult>
                {
                    new OutletResult
                    {
                        BranchId = 1,
                        Name = "Test Outlet",
                        Address = "123 Test Street",
                        City = "Test City",
                        State = "Test State",
                        ContactNumber = "+1234567890",
                        ContactEmail = "outlet@example.com",
                        IsOpen = true,
                        Rating = 4.5
                    }
                },
                TotalCount = 1,
                Page = 1,
                PageSize = 10
            };
        }

        public static GetDataResult<List<OrderResult>> GetTestOrderResults()
        {
            return new GetDataResult<List<OrderResult>>
            {
                Data = new List<OrderResult>
                {
                    new OrderResult
                    {
                        OrderId = 1,
                        CustomerName = "Test Customer",
                        CustomerPhone = "+1234567890",
                        OrderDate = DateTime.Now.AddDays(-1),
                        Status = "Pending",
                        TotalAmount = 99.99m
                    }
                },
                TotalCount = 1,
                Page = 1,
                PageSize = 10
            };
        }

        public static GetDataResult<List<EmployeeResult>> GetTestEmployeeResults()
        {
            return new GetDataResult<List<EmployeeResult>>
            {
                Data = new List<EmployeeResult>
                {
                    new EmployeeResult
                    {
                        EmployeeId = 1,
                        FirstName = "Test",
                        LastName = "Employee",
                        Email = "employee@example.com",
                        Phone = "+1234567890",
                        Role = "Staff",
                        IsActive = true,
                        JoinDate = DateTime.Now.AddMonths(-6)
                    }
                },
                TotalCount = 1,
                Page = 1,
                PageSize = 10
            };
        }
    }
}