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
        Task<int> CreateOrder(OrdersDto order,string UserName);

        Task<OrdersDto> GetSingleOrder(int OrderId);

        Task<int> UpdateStatus(int OrderId, string OrderStatus);

        Task<DropDownOptions> GetOrderStatusOptions();
    }
}
