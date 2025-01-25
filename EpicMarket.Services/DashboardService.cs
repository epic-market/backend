using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EpicMarket.Data.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Entities;

namespace EpicMarket.Services
{
    public class DashboardService : IDashboardService
    {
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<DashboardService> _logger;

    public DashboardService(ApplicationDbContext dbContext, ILogger<DashboardService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<ActiveUserChartResponse> GetActiveUsers(int outletId)
    {
        var response = new ActiveUserChartResponse();
        try
        {
            var monthlyData = new List<ActiveUserChart>();
            var weeklyData = new List<ActiveUserChart>();
            
            // Get the earliest order date
            var earliestOrder = await _dbContext.Orders
                .Where(o => o.OutletID == outletId)
                .OrderBy(o => o.OrderAt)
                .IgnoreQueryFilters()
                .Select(o => o.OrderAt)
            
                .FirstOrDefaultAsync();

            if (earliestOrder == default)
            {
                _logger.LogInformation($"No orders found for outlet {outletId}");
                response.Monthly = new List<ActiveUserChart>();
                response.Weekly = new List<ActiveUserChart>();
                return response;
            }

            var today = DateTime.UtcNow.Date;
            var startDate = earliestOrder.Date;
            
            // Calculate months between earliest order and now
            var monthsToShow = Math.Min(12, ((today.Year - startDate.Year) * 12) + today.Month - startDate.Month + 1);
            
            // Monthly Data
            for (int i = monthsToShow - 1; i >= 0; i--)
            {
                var date = today.AddMonths(-i);
                var startOfMonth = new DateTime(date.Year, date.Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
                
                var activeUsers = await _dbContext.Orders
                    .Where(o => o.OutletID == outletId &&
                               o.OrderAt.Date >= startOfMonth && 
                               o.OrderAt.Date <= endOfMonth)
                    .IgnoreQueryFilters()
                    .Select(o => o.PersonID)
                    .Distinct()
                    .CountAsync();
                    
                monthlyData.Add(new ActiveUserChart
                {
                    X = date.ToString("MMMM"),
                    Customer = activeUsers
                });
            }

            // Weekly Data - Last 7 days
            for (int i = 6; i >= 0; i--)
            {
                var date = today.AddDays(-i);
                var activeUsers = await _dbContext.Orders
                    .Where(o => o.OutletID == outletId &&
                               o.OrderAt.Date == date)
                    .Select(o => o.OrderAt)
                    .Distinct()
                    .CountAsync();
                    
                weeklyData.Add(new ActiveUserChart
                {
                    X = date.ToString("ddd"),
                    Customer = activeUsers
                });
            }

            _logger.LogInformation($"Retrieved active users data for outlet {outletId}. Monthly entries: {monthlyData.Count}, Weekly entries: {weeklyData.Count}");
            response.Monthly = monthlyData;
            response.Weekly = weeklyData;
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while getting active users data for outlet {outletId}");
            throw;
        }
    }

        //GMV=Sales Price of Goods�Number of Goods Sold
        public async Task<List<GMVChart>> GetGrossMerchandiseValue(int outletId)
    {
        try
        {
            _logger.LogInformation($"Getting gross merchandise value (GMV) by month for outlet {outletId}");
            
            var today = DateTime.Today;
            var startDate = await _dbContext.Orders
                .Where(o => o.OutletID == outletId)
                .OrderBy(o => o.OrderAt)
                .IgnoreQueryFilters()
                .Select(o => o.OrderAt)
                .FirstOrDefaultAsync();

            if (startDate == default)
            {
                return new List<GMVChart>();
            }

            var monthlyGMV = new List<GMVChart>();
            var monthsToShow = Math.Min(12, ((today.Year - startDate.Year) * 12) + today.Month - startDate.Month + 1);

            for (int i = monthsToShow - 1; i >= 0; i--)
            {
                var date = today.AddMonths(-i);
                var startOfMonth = new DateTime(date.Year, date.Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                var merchandiseValue = await _dbContext.Orders
                    .Where(o => o.OutletID == outletId &&
                               o.OrderAt >= startOfMonth && 
                               o.OrderAt <= endOfMonth)
                    .IgnoreQueryFilters()
                    .SumAsync(o => o.TotalPrice);

                monthlyGMV.Add(new GMVChart
                {
                    Month = date.ToString("MMMM"),
                    Value = Math.Round((decimal)merchandiseValue, 2)
                });
            }

            _logger.LogInformation($"Retrieved monthly GMV values for outlet {outletId}. Entries: {monthlyGMV.Count}");
            return monthlyGMV;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while getting monthly GMV values for outlet {outletId}");
            throw;
        }
    }

    public async Task<decimal> GetCustomerRetentionRate(int outletId)
    {
        try
        {
            _logger.LogInformation($"Calculating customer retention rate for outlet {outletId}");

            var today = DateTime.Today;
            var lastMonth = today.AddMonths(-1);
            var twoMonthsAgo = today.AddMonths(-2);

            // Get customers who ordered in previous month
            var previousMonthCustomers = await _dbContext.Orders
                .Where(o => o.OutletID == outletId &&
                           o.OrderAt >= twoMonthsAgo && o.OrderAt < lastMonth)
                .IgnoreQueryFilters()
                .Select(o => o.PersonID)
                .Distinct()
                .ToListAsync();

            if (!previousMonthCustomers.Any())
            {
                _logger.LogInformation($"No customers found in previous month period for outlet {outletId}");
                return 0;
            }

            // Get customers from previous month who also ordered in current month
            var retainedCustomers = await _dbContext.Orders
                .Where(o => o.OutletID == outletId &&
                           o.OrderAt >= lastMonth && o.OrderAt < today)
                .Where(o => previousMonthCustomers.Contains(o.PersonID))
                .IgnoreQueryFilters()
                .Select(o => o.PersonID)
                .Distinct()
                .CountAsync();

            var retentionRate = (decimal)retainedCustomers / previousMonthCustomers.Count * 100;
            
            _logger.LogInformation($"Customer retention rate for outlet {outletId}: {retentionRate}%. " +
                                 $"Previous month customers: {previousMonthCustomers.Count}, " +
                                 $"Retained customers: {retainedCustomers}");

            return Math.Round(retentionRate, 2);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while calculating customer retention rate for outlet {outletId}");
            throw;
        }
    }

    public async Task<List<PopularProductChart>> GetTopSellingProducts(int outletId)
    {
        try
        {
            _logger.LogInformation($"Getting top 10 selling products for outlet {outletId}");

            var topProducts = await _dbContext.OrderDetails
                .Where(od => od.Order.OutletID == outletId)
                .GroupBy(od => od.CatalogVariants.Catalog.ID)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalQuantity = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(x => x.TotalQuantity)
                .Take(10)
                .ToListAsync();

            var productIds = topProducts.Select(p => p.ProductId).ToList();

            var products = await _dbContext.Catalogs
                .Where(p => productIds.Contains(p.ID))
                .Select(p => new
                {
                    p.ID,
                    p.Name,
                })
                .ToDictionaryAsync(p => p.ID);

            var result = topProducts.Select(tp => new PopularProductChart
            {
                ProductName = products[tp.ProductId].Name,
                Quantity = tp.TotalQuantity
            }).ToList();

            _logger.LogInformation($"Retrieved top {result.Count()} selling products for outlet {outletId}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while getting top selling products for outlet {outletId}");
            throw;
        }
    }

    public async Task<Dictionary<string, int>> GetOrderStatusDistribution(DateTime date, int outletId, string period = "day")
    {
        try
        {
            _logger.LogInformation($"Getting order status distribution for outlet {outletId}, {period}: {date}");

            var query = _dbContext.Orders.Where(o => o.OutletID == outletId);

            // Filter based on period
            if (period.ToLower() == "day")
            {
                query = query.Where(o => o.OrderAt.Date == date.Date);
            }
            else if (period.ToLower() == "month")
            {
                query = query.Where(o => o.OrderAt.Month == date.Month && o.OrderAt.Year == date.Year);
            }

            var distribution = await query
                .Include(c=>c.OrderStatusOptions)
                .GroupBy(o => o.OrderStatusOptions.OrderStatus)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToDictionaryAsync(x => x.Status, x => x.Count);

            _logger.LogInformation($"Retrieved order status distribution for outlet {outletId} with {distribution.Count} statuses");
            return distribution;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while getting order status distribution for outlet {outletId}");
            throw;
        }
    }

    }
}