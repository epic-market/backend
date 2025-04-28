using EpicMarket.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EpicMarket.Admin.MVC.Models
{
    /// <summary>
    /// Comprehensive view model for editing Product entities including variants and attachments
    /// </summary>
    public class ProductEditViewModel
    {
        // Main Product entity
        public EpicMarket.Data.Models.Catalog Product { get; set; }

        // Variants information
        public List<CatalogVariants> Variants { get; set; } = new List<CatalogVariants>();
        // Product-level attachments
        public List<AttachmentModel> ProductThumbnails { get; set; } = new List<AttachmentModel>();
        public List<AttachmentModel> ProductProductImages { get; set; } = new List<AttachmentModel>();
        
        // Variant-level attachments organized by variant ID
        public Dictionary<int, VariantAttachmentsModel> VariantAttachments { get; set; } = new Dictionary<int, VariantAttachmentsModel>();
        
        // Dropdown data
        public List<SelectListItem> BusinessOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> StatusOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> CategoryOptions { get; set; } = new List<SelectListItem>();
        
        // For tracking variant removals
        public string RemovedVariantIds { get; set; }
        
        // Flag to indicate if catalog uses variants
        public bool HasVariants { get; set; }
        
        // JSON representation of variants for client-side handling
        public string VariantDataJson { get; set; }
        
        // Variant options definition (e.g., "Color,Size")
        public string VariantOptions { get; set; }
    }

    /// <summary>
    /// Model for individual variant editing
    /// </summary>
    public class ProductVariantEditModel
    {
        public int ID { get; set; }
        public string SKU { get; set; }
        public string Barcode { get; set; }
        public double CostPrice { get; set; }
        public double SalePrice { get; set; }
        public double? CompareAtPrice { get; set; }
        public int? MinimumOrderQuantity { get; set; }
        public int? MaximumOrderQuantity { get; set; }
        public double? PackedHeight { get; set; }
        public double? PackedWidth { get; set; }
        public double? PackedDepth { get; set; }
        public string WeightUnit { get; set; }
        public double? Weight { get; set; }
        public string Attributes { get; set; }
        public string AdditionalHighlights { get; set; }
        public bool IsDefault { get; set; }
        
        // Flags for updating attachments
        public bool ThumbnailUpdated { get; set; }
        public bool ImagesUpdated { get; set; }
    }

    /// <summary>
    /// Model for variant attachments
    /// </summary>
    public class VariantAttachmentsModel
    {
        public List<AttachmentModel> Thumbnails { get; set; } = new List<AttachmentModel>();
        public List<AttachmentModel> ProductImages { get; set; } = new List<AttachmentModel>();
    }
} 