using EpicMarket.Entities;

public interface IDashboardService
{
    Task<ActiveUserChartResponse> GetActiveUsers(int outletId);
    Task<List<GMVChart>> GetGrossMerchandiseValue(int outletId);
    Task<decimal> GetCustomerRetentionRate(int outletId);
    Task<List<PopularProductChart>> GetTopSellingProducts(int outletId);
    Task<Dictionary<string, int>> GetOrderStatusDistribution(DateTime date, int outletId, string period = "day");
}
