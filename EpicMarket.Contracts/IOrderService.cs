using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface IOrderService
    {
        Task<int> CreateOrder(OrdersDto order,string UserName,string PageSource);

        Task<OrdersDto> GetSingleOrder(int OrderId);

        Task<int> UpdateStatus(int OrderId, string OrderStatus);

        Task<List<DropDownOptions>> GetOrderStatusOptions();

        Task<GetDataResult<List<OrderResult>>> GetAllOrders(OrderParams orderParams,int businessID);
    }
}
