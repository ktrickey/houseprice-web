using System.Collections.Generic;
using HousePrices.Web.Controllers;

namespace HousePrices.Web.Models
{
    public class SearchResults
    {
        public SearchResults(PagedResult<Results> results, IEnumerable<int> years)
        {
            Transactions = results;
            Years = years;
        }
        public PagedResult<Results> Transactions { get; }
        public IEnumerable<int> Years { get; }

    }
}