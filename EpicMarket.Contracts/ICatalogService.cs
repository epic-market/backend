using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface IProductService
    {
        Task<int> AddProduct(AddProductsParams productsDto,string UserName, int businessID,string PageSource);
        Task AddOrUpdateProductInventoryDetails(InventoryResult productAdvanced);
        Task<int> UpdateProducts(AddProductsParams productsDto,int id, string UserName, int businessID, string PageSource);
	    Task<GetDataResult<List<ProductResult>>> GetAllProducts(ProductListParams productResult, int businessID);
        Task<List<ProductsMapOptionResult>> GetAllProductForMap(int businessID, int outletID);
         Task<GetDataResult<List<CustomerProductResult>>> GetAllProductsForMobile(ProductMobileParams parameters);
        Task<GetDataResult<List<ProductForPOSResult>>> GetAllProductsForPOS(ProductPOSParams productParams, int outletId);
        Task<ProductDetailsResult> GetProductDetails(int productId);
        Task<ProductDetailsV2Result> GetProductDetailsV2(int productId);
        Task<CustomerProductDetailsResult> GetCustomerProductDetails(int productId);
        Task<InventoryResult> GetProductInventoryDetails(int productId, int branchId);
        Task<int> VerifyProduct(VerifyProductDto verifyProductDto, string UserName, int AdminPersonID, string PageSource);
        Task<int> QuickActions(QuickActionsParams quickActionsParams ,string UserName);
        Task deleteCatelog(int id,string UserName);
        // Task<int> AddProductVariant(int productId, ProductVariantDto variantDto, string userName);
        Task<List<SingleProductVariantsResult>> GetProductVariants(int productId);
        // Task<ProductVariantResponse> GetProductVariant(int variantId);
        // Task UpdateProductVariant(int variantId, ProductVariantDto variantDto, string userName);
	}
}
