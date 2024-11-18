using EpicMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface ISearchService
    {
        Task<SearchResult> SearchAsync(SearchRequest request);

        Task<List<TrendingOutletDto>> GetTop3TrendingStoresNearbyAsync(
        double latitude,
        double longitude,
        double radiusKm = 10);
    }
}
