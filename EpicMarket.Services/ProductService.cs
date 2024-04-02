using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
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

        public int AddProduct(ProductsDto productsDto, string UserName)
        {
            var product = mapper.Map<Catalog>(productsDto);
            product.CreateBy = UserName;
            product.CreateDate = DateTime.Now;
            _context.Catalogs.Add(product);
            _context.SaveChanges();

            return product.ID;
        }

        public async Task<List<ProductsMapOptionResult>> GetAllProductForMap(int BusinessID,int BranchId)
        {

            var _ = await (from catalogItem in _context.Catalogs
                    join outletProduct in (_context.OutletProducts.Where(a => a.OutletID == 1))
                    on catalogItem.ID equals outletProduct.ProductID into joinedProducts
                    from matchedProduct in joinedProducts.DefaultIfEmpty()
                    where catalogItem.BusinessID == 6
                    select new ProductsMapOptionResult
                    {
                        Name = catalogItem.Name,
                        Description = catalogItem.Description,
                        ImageURL = catalogItem.Images,
                        Rate = catalogItem.Rate,
                        Selected = matchedProduct == null ? 0 : 1,
                    }).ToListAsync();

            return _;
        }
    }
}
