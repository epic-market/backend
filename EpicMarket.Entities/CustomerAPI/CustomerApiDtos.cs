using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EpicMarket.Entities.CustomerAPI
{
    #region Authentication DTOs

    /// <summary>
    /// Request DTO for user sign in
    /// </summary>
    public class SignInRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }

    /// <summary>
    /// Request DTO for user sign up
    /// </summary>
    public class SignUpRequest
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        
        public string Phone { get; set; }
    }

    /// <summary>
    /// Response DTO for authenticated user
    /// </summary>
    public class AuthUserResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool PhoneVerified { get; set; }
        public bool EmailVerified { get; set; }
        public string Token { get; set; }
        public string ProfileImage { get; set; }
        public List<UserAddressResponse> Addresses { get; set; } = new List<UserAddressResponse>();
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    /// <summary>
    /// Current user profile response
    /// </summary>
    public class CurrentUserResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool PhoneVerified { get; set; }
        public bool EmailVerified { get; set; }
        public string ProfileImage { get; set; }
        public List<UserAddressResponse> Addresses { get; set; } = new List<UserAddressResponse>();
        public bool IsActive { get; set; }
    }

    #endregion

    #region Category DTOs

    /// <summary>
    /// Response DTO for business category
    /// </summary>
    public class CategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Icon { get; set; }
        public int DisplayOrder { get; set; }
        public int BusinessCount { get; set; }
        public bool IsActive { get; set; }
    }

    #endregion

    #region Business/Outlet DTOs

    /// <summary>
    /// Response DTO for business/outlet listing
    /// </summary>
    public class BusinessResponse
    {
        public int Id { get; set; }
        public int OutletId { get; set; }
        public int BusinessId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Pincode { get; set; }
        public string Country { get; set; } = "India";
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ContactNumber { get; set; }
        public string ContactEmail { get; set; }
        public string Thumbnail { get; set; }
        public string Image { get; set; }
        public List<string> Photos { get; set; } = new List<string>();
        public bool IsOpen { get; set; }
        public List<TimingItem> TimingList { get; set; } = new List<TimingItem>();
        public string SocialMediaLinkFacebook { get; set; }
        public string SocialMediaLinkInstagram { get; set; }
        public string SpecialNoteOfTheDay { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public string WaitTime { get; set; }
        public string PriceRange { get; set; }
        public List<string> Features { get; set; } = new List<string>();
        public string Type { get; set; }
        public string Badge { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Timing item for business hours
    /// </summary>
    public class TimingItem
    {
        public string Day { get; set; }
        public string OpenTime { get; set; }
        public string CloseTime { get; set; }
    }

    /// <summary>
    /// Response DTO for detailed outlet information
    /// </summary>
    public class OutletDetailResponse
    {
        public int OutletId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Pincode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ContactEmail { get; set; }
        public string Thumbnail { get; set; }
        public List<string> Photos { get; set; } = new List<string>();
        public bool IsOpen { get; set; }
        public List<TimingItem> TimingList { get; set; } = new List<TimingItem>();
        public string SocialMediaLinkFacebook { get; set; }
        public string SocialMediaLinkInstagram { get; set; }
        public string SocialMediaLinkTwitter { get; set; }
        public string SocialMediaLinkYoutube { get; set; }
        public string SpecialNoteOfTheDay { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public string BusinessCategory { get; set; }
    }

    /// <summary>
    /// Business group for listings response
    /// </summary>
    public class BusinessGroupItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public List<BusinessResponse> Businesses { get; set; } = new List<BusinessResponse>();
    }

    /// <summary>
    /// Response DTO for business listings
    /// </summary>
    public class BusinessListingsResponse
    {
        public List<BusinessGroupItem> BusinessGroups { get; set; } = new List<BusinessGroupItem>();
    }

    #endregion

    #region Product/Catalog DTOs

    /// <summary>
    /// Response DTO for product category
    /// </summary>
    public class ProductCategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OutletId { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Product category info
    /// </summary>
    public class ProductCategoryInfo
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

    /// <summary>
    /// Variant option for products
    /// </summary>
    public class VariantOption
    {
        public string Name { get; set; }
        public List<string> Values { get; set; } = new List<string>();
    }

    /// <summary>
    /// Product highlight/attribute
    /// </summary>
    public class ProductHighlight
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    /// <summary>
    /// Variant attribute
    /// </summary>
    public class VariantAttribute
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    /// <summary>
    /// Product variant response
    /// </summary>
    public class ProductVariantResponse
    {
        public int VariantId { get; set; }
        public List<VariantAttribute> Attributes { get; set; } = new List<VariantAttribute>();
        public string Sku { get; set; }
        public decimal SalePrice { get; set; }
        public decimal? CompareAtPrice { get; set; }
        public int MaximumOrderQuantity { get; set; }
        public int MinimumOrderQuantity { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public string Thumbnail { get; set; }
        public bool IsDefaultVariant { get; set; }
        public int? StockQuantity { get; set; }
        public bool TrackInventory { get; set; }
    }

    /// <summary>
    /// Response DTO for product
    /// </summary>
    public class ProductResponse
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OutletId { get; set; }
        public int CategoryId { get; set; }
        public ProductCategoryInfo Category { get; set; }
        public bool RequiresRefrigeration { get; set; }
        public bool IsRecommended { get; set; }
        public bool IsFeatured { get; set; }
        public double Rating { get; set; }
        public int RatingCount { get; set; }
        public string Image { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public List<VariantOption> VariantOptions { get; set; } = new List<VariantOption>();
        public List<ProductHighlight> BaseHighlights { get; set; } = new List<ProductHighlight>();
        public List<ProductVariantResponse> Variants { get; set; } = new List<ProductVariantResponse>();
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    /// <summary>
    /// Paginated product response
    /// </summary>
    public class ProductListResponse
    {
        public List<ProductResponse> Items { get; set; } = new List<ProductResponse>();
        public int Count { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }

    #endregion

    #region Review DTOs

    /// <summary>
    /// Response DTO for review
    /// </summary>
    public class ReviewResponse
    {
        public int Id { get; set; }
        public int RatingId { get; set; }
        public int UserId { get; set; }
        public int RatingValue { get; set; }
        public string Review { get; set; }
        public string RatingType { get; set; }
        public int EntityId { get; set; }
        public string CustomerName { get; set; }
        public string Rating { get; set; }
        public string Date { get; set; }
        public int? OutletId { get; set; }
        public int? ProductId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    /// <summary>
    /// Request DTO for submitting a review
    /// </summary>
    public class SubmitReviewRequest
    {
        [Required]
        public string ReviewType { get; set; } // "product" or "outlet"
        
        [Required]
        public string RecordId { get; set; }
        
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        
        public string Comment { get; set; }
    }

    #endregion

    #region Search DTOs

    /// <summary>
    /// Search result branch item
    /// </summary>
    public class SearchBranchResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Distance { get; set; }
    }

    /// <summary>
    /// Search result product item
    /// </summary>
    public class SearchProductResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Thumbnail { get; set; }
        public string BranchName { get; set; }
        public int BranchId { get; set; }
        public double? Distance { get; set; }
    }

    /// <summary>
    /// Response DTO for search results
    /// </summary>
    public class SearchResponse
    {
        public List<SearchBranchResult> Branches { get; set; } = new List<SearchBranchResult>();
        public List<SearchProductResult> Products { get; set; } = new List<SearchProductResult>();
    }

    #endregion

    #region Order DTOs

    /// <summary>
    /// Order item for creating an order
    /// </summary>
    public class CreateOrderItem
    {
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        public int VariantId { get; set; }
        
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }

    /// <summary>
    /// Request DTO for creating an order
    /// </summary>
    public class CreateOrderRequest
    {
        [Required]
        public int OutletId { get; set; }
        
        [Required]
        public List<CreateOrderItem> Items { get; set; }
        
        [Required]
        public int DeliveryAddressId { get; set; }
        
        [Required]
        public string PaymentMethod { get; set; }
    }

    /// <summary>
    /// Order item response
    /// </summary>
    public class OrderItemResponse
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int VariantId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Image { get; set; }
        public List<VariantAttribute> Attributes { get; set; } = new List<VariantAttribute>();
    }

    /// <summary>
    /// Delivery address for order
    /// </summary>
    public class OrderDeliveryAddress
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
    }

    /// <summary>
    /// Response DTO for order
    /// </summary>
    public class OrderResponse
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public int UserId { get; set; }
        public int OutletId { get; set; }
        public string OutletName { get; set; }
        public string OrderType { get; set; }
        public List<OrderItemResponse> Items { get; set; } = new List<OrderItemResponse>();
        public decimal Subtotal { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal Taxes { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public OrderDeliveryAddress DeliveryAddress { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? EstimatedDelivery { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Response DTO for order cancellation
    /// </summary>
    public class OrderCancelResponse
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string Status { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    #endregion

    #region User Address DTOs

    /// <summary>
    /// Response DTO for user address
    /// </summary>
    public class UserAddressResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Label { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Landmark { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string Country { get; set; } = "India";
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public bool IsDefault { get; set; }
        public string AddressType { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    /// <summary>
    /// Request DTO for adding/updating user address
    /// </summary>
    public class AddUserAddressRequest
    {
        [Required]
        public string Label { get; set; }
        
        [Required]
        public string AddressLine1 { get; set; }
        
        public string AddressLine2 { get; set; }
        
        public string Landmark { get; set; }
        
        [Required]
        public string City { get; set; }
        
        [Required]
        public string State { get; set; }
        
        [Required]
        public string Pincode { get; set; }
        
        public bool IsDefault { get; set; }
        
        public string AddressType { get; set; } // "Home", "Office", "Other"
    }

    /// <summary>
    /// Request DTO for updating user address
    /// </summary>
    public class UpdateUserAddressRequest
    {
        public string Label { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Landmark { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public bool? IsDefault { get; set; }
        public string AddressType { get; set; }
    }

    #endregion
}
