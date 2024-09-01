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
		private readonly IUnitOfWork unitOfWork;

		public AddressService(ApplicationDbContext context, IMapper mapper,IUnitOfWork unitOfWork)
        {
            _context = context;
            this.mapper = mapper;
			this.unitOfWork = unitOfWork;
		}

        public async Task<int> AddAddress(AddressDto addressDto)
        {
            var addressModel = mapper.Map<Address>(addressDto);
            if (addressDto.ID != null)
            {
                _context.Addresses.Update(addressModel);
            }
            else 
            {
               await _context.Addresses.AddAsync(addressModel);
            }

            await unitOfWork.Complete();

			return addressModel.Id;
            
        }
    }
}
