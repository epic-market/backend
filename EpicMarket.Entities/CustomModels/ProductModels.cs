using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities.CustomModels
{
    public class ProductDetailsV2Result
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int Rating { get; set; }
        public int RatingCount { get; set; }
        public CategoryDto Category { get; set; }
        public bool RequiresRefrigeration { get; set; }
        public bool IsRecommended { get; set; }
        public List<VarientOptionDto> VariantOptions { get; set; }
        public List<HighlightDto> BaseHightlights { get; set; }
        public List<ProductVariantV2Result> Variants { get; set; }
        public List<SimilarProductResult> SimilarProducts { get; set; }
    }

    public class ProductVariantV2Result
    {
        public int VariantId { get; set; }
        public List<AttributeDto> Attributes { get; set; }
        public string SKU { get; set; }
        public double SalePrice { get; set; }
        public double? CompareAtPrice { get; set; }
        public List<HighlightDto> AdditionalHightlights { get; set; }
        public int IsInStock { get; set; }
        public int? MaximumOrderQuantity { get; set; }
        public int? MinimumOrderQuantity { get; set; }
        public double? PackedHeight { get; set; }
        public double? PackedWidth { get; set; }
        public double? PackedDepth { get; set; }
        public string WeightUnit { get; set; }
        public double? Weight { get; set; }
        public List<string> Images { get; set; }
        public string Thumbnail { get; set; }
        public bool IsDefaultVariant { get; set; }
    }

    public class SimilarProductResult
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double SalePrice { get; set; }
         public double? CompareAtPrice { get; set; }
        public string Thumbnail { get; set; }
    }
} 