using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
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

        public Task<List<ProductsMapOptionResult>> GetAllProductForMap(int BusinessID)
        {
            throw new NotImplementedException();
        }
    }
}
