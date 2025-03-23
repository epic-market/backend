using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface ICatalogService
    {
        Task<int> AddProduct(AddProductsDto productsDto,string UserName, int businessID,string PageSource);
        Task AddOrUpdateProductInventoryDetails(InventoryResult productAdvanced);
        Task<int> UpdateProducts(AddProductsDto productsDto,int id, string UserName, int businessID, string PageSource);
	    Task<GetDataResult<List<ProductResult>>> GetAllProducts(ProductParams productResult, int businessID);
        Task<List<ProductsMapOptionResult>> GetAllProductForMap(int businessID, int outletID);
         Task<GetDataResult<List<CustomerResultBaseOnCategory>>> GetAllProductsForMobile(ProductMobileParams parameters);
        Task<GetDataResult<List<ProductForPOSResult>>> GetAllProductsForPOS(ProductPOSParams productParams, int outletId);
        Task<ProductsDto> GetProductDetails(int productId);
        Task<InventoryResult> GetProductInventoryDetails(int productId, int branchId);
        Task<int> VerifyCatalog(VerifyCatalogDto verifyCatalogDto, string UserName, int AdminPersonID, string PageSource);
        Task<int> QuickActions(QuickActionsParams quickActionsParams ,string UserName);
        Task deleteCatelog(int id,string UserName);
        // Task<int> AddProductVariant(int productId, ProductVariantDto variantDto, string userName);
        Task<List<ProductVariantResponse>> GetProductVariants(int productId);
        // Task<ProductVariantResponse> GetProductVariant(int variantId);
        // Task UpdateProductVariant(int variantId, ProductVariantDto variantDto, string userName);
	}
}
