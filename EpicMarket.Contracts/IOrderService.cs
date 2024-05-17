using EpicMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface IOrderService
    {
        int CreateOrder(OrdersDto order,string UserName,int businessId);

        Task<OrdersDto> GetSingleOrder(int OrderId);

        int UpdateStatus(int OrderId, string OrderStatus);

        Task<List<DropDownOptions>> GetOrderStatusOptions();

        Task<List<OrderResult>> GetAllOrders(OrderParams orderParams,int businessID);
    }
}
