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

        Task<OrdersDetailsResult> GetSingleOrder(int OrderId);

        Task<int> UpdateStatus(int OrderId, int statusID);

        Task<GetDataResult<List<OrderResult>>> GetAllOrders(OrderParams orderParams,int businessID);
        Task<GetDataResult<List<OrderMobileResult>>> GetAllOrdersForMobile(OrderParams orderParams,int businessID);
        Task<GetDataResult<OrderMobileDeatilsResult>> GetOrdersDetailsForMobile(int OrderId, int businessID);

        Task<bool> AnyNewOrders(DateTime ordered_after, int businessid , int? outlet_id);
    }
}
