using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Services
{
    public class ProductService : IProductService
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private readonly IAddressService addressService;

        public ProductService(ApplicationDbContext context, IMapper mapper, IAddressService addressService)
        {
            _context = context;
            this.mapper = mapper;
            this.addressService = addressService;
        }

        public int AddOrUpdateProduct(ProductsDto productsDto, string UserName, int businessID)
        {
            var product = mapper.Map<Catalog>(productsDto);
            product.BusinessID = businessID;
            if (productsDto.Id == null || product.ID == 0)
            {
                product.CreateBy = UserName;
                product.CreateDate = DateTime.Now;
                product.StatusId = _context.StatusOptionSets.FirstOrDefault(c => c.Status == Business_Status.BUSINESS_UNVERIFIED).Id;
                _context.Catalogs.Add(product);
            }
            else 
            {
                product.ModifiedBy = UserName;
                product.ModifiedDate = DateTime.Now;
                _context.Catalogs.Update(product);
            }
            _context.SaveChanges();

            return product.ID;
        }

        public async Task<List<ProductsMapOptionResult>> GetAllProductForMap(int BusinessID,int BranchId)
        {


            var _ = await (from catalogItem in _context.Catalogs
                    join outletProduct in (_context.OutletProducts.Where(a => a.OutletID == BranchId))
                    on catalogItem.ID equals outletProduct.ProductID into joinedProducts
                    from matchedProduct in joinedProducts.DefaultIfEmpty()
                    where catalogItem.BusinessID == BusinessID
                    select new ProductsMapOptionResult
                    {
                        Id = catalogItem.ID,
                        Name = catalogItem.Name,
                        Description = catalogItem.Description,
                        ImageURL = catalogItem.Images,
                        Rate = catalogItem.Rate,
                        Selected = matchedProduct == null ? 0 : 1,
                    }).ToListAsync();

            return _;
        }

        public async Task<GetDataResult<List<ProductResult>>> GetAllProducts(ProductParams productParams, int businessID)
        {


            var getResult = new GetDataResult<List<ProductResult>>();


            //1 . filter with BusinessID
            var Products = _context.Catalogs
                                .Where(c => c.BusinessID == businessID);


            //2 . Appling Searching
            var sortedProducts = Products.Where(row => row.Name.Contains(productParams.searchTerm.Trim()) || row.Description.Contains(productParams.searchTerm.Trim()));


            // 3 .Appying Sorting
            switch (productParams.sortColumn)
            {
                case "ProductID":
                    sortedProducts = productParams.ascending ? sortedProducts.OrderBy(c => c.ID) : sortedProducts.OrderByDescending(c => c.ID);
                    break;
                case "Name":
                    sortedProducts = productParams.ascending ? sortedProducts.OrderBy(c => c.Name) : sortedProducts.OrderByDescending(c => c.Name);
                    break;
                default:
                    break;
            }

            //getting the total count
            int totalCount = sortedProducts.Count();


            // 4. Apply pagination (skip and take)
            var pagedProducts = sortedProducts
                .Skip((productParams.PageIndex - 1) * productParams.pageSize) // Skip items for previous pages
                .Take(productParams.pageSize); // Take items for the current page

            // 5. Select data and add SRNO
            var results = await pagedProducts.Select(c => new ProductResult()
            {
                ProductId = c.ID,
                Name = c.Name,
                Description = c.Description,
                Rate = c.Rate,
                InStock = c.InStock,
                IsActive = c.IsActive,  
                Count = totalCount
            }).ToListAsync();

            getResult.items = results;
            getResult.Count = totalCount;

            return getResult;
        }

        public async Task<ProductsDto> GetProductDetails(int productId)
        {
            return await _context.Catalogs.Where(c=> c.ID == productId).Select(c => new ProductsDto
            { 
                Id=c.ID,
                Name = c.Name,
                Description = c.Description,
                Rate = c.Rate,
                InStock = c.InStock,
                IsActive = c.IsActive,
                Category = c.Category,
                Images = c.Images,
                MaximumOrderPurchase = (int)c.MaximumOrderPurchase,
                IsRecommended = c.IsRecommended
            }
            
            ).FirstOrDefaultAsync();
        }
    }
}
