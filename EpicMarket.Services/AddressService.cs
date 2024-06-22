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
    public class AddressService : IAddressService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public AddressService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        public int AddAddress(AddressDto addressDto)
        {
            var addressModel = mapper.Map<Address>(addressDto);
            if (addressDto.ID != null)
            {
                _context.Addresses.Update(addressModel);
            }
            else 
            {
                _context.Addresses.Add(addressModel);
            }
            
             _context.SaveChanges();
            return addressModel.Id;
            
        }
    }
}
