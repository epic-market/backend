using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EpicMarket.Data.Models;

namespace EpicMarket.Admin.MVC.Models
{
    public class ProductImportDto
    {
         // Catalog properties
        public string CatalogName { get; set; }
        public string CatalogDescription { get; set; }
        public string CategoryName { get; set; }
        public bool IsRecommended { get; set; }
        public bool RequiresRefrigeration { get; set; }
        
        // Variant properties
        public string SKU { get; set; }
        public string Barcode { get; set; }
        
        // Attributes
        public string AttributeName1 { get; set; }
        public string AttributeValue1 { get; set; }
        public string AttributeName2 { get; set; }
        public string AttributeValue2 { get; set; }
        public string AttributeName3 { get; set; }
        public string AttributeValue3 { get; set; }
        
        // Pricing and inventory
        public double CostPrice { get; set; }
        public double SalePrice { get; set; }
        public double? CompareAtPrice { get; set; }
        public int? MaximumOrderQuantity { get; set; }
        public int? MinimumOrderQuantity { get; set; }
        
        // Dimensions
        public double? PackedHeight { get; set; }
        public double? PackedWidth { get; set; }  // Note: Fixed typo in property name from "Widhth"
        public double? PackedDepth { get; set; }
        public double? Weight { get; set; }
        
        // Helper method to convert attributes to JSON
        public string GetAttributesJson()
        {
            var attributes = new Dictionary<string, string>();
            
            if (!string.IsNullOrEmpty(AttributeName1) && !string.IsNullOrEmpty(AttributeValue1))
                attributes.Add(AttributeName1, AttributeValue1);
                
            if (!string.IsNullOrEmpty(AttributeName2) && !string.IsNullOrEmpty(AttributeValue2))
                attributes.Add(AttributeName2, AttributeValue2);
                
            if (!string.IsNullOrEmpty(AttributeName3) && !string.IsNullOrEmpty(AttributeValue3))
                attributes.Add(AttributeName3, AttributeValue3);
                
            return JsonSerializer.Serialize(attributes);
        }
        
        // Helper methods to convert to domain model objects
        public Catalog ToCatalog(int businessId, int statusId)
        {
            return new Catalog
            {
                BusinessID = businessId,
                Name = CatalogName,
                Description = CatalogDescription,
                // CategoryID will need to be looked up separately
                IsRecommended = IsRecommended,
                RequiresRefrigeration = RequiresRefrigeration,
                StatusId = statusId,
            };
        }

        
        public CatalogVariants ToCatalogVariant(int catalogId, string createdBy)
        {
            return new CatalogVariants
            {
                CatalogID = catalogId,
                SKU = SKU,
                Barcode = Barcode,
                Attributes = GetAttributesJson(),
                CostPrice = CostPrice,
                SalePrice = SalePrice,
                CompareAtPrice = CompareAtPrice,
                MaximumOrderQuantity = MaximumOrderQuantity,
                MinimumOrderQuantity = MinimumOrderQuantity,
                PackedHeight = PackedHeight,
                PackedWidth = PackedWidth,  // Note: The model has a typo, so we match it
                PackedDepth = PackedDepth,
                Weight = Weight,
                WeightUnit = "g",  // Default unit
                IsActive = true,
                CreateDate = DateTime.UtcNow,
                CreateBy = createdBy
            };
        }
    }
}