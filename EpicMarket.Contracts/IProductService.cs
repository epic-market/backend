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
        Task<List<ProductsMapOptionResult>> GetAllProductForMap(int businessID, int outletID);
        int AddOrUpdateProduct(ProductsDto productsDto,string UserName, int businessID);

        Task<GetDataResult<List<ProductResult>>> GetAllProducts(ProductParams productResult, int businessID);

        Task<ProductsDto> GetProductDetails(int productId);
    }
}
