using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Bol.OpenAPI
{
    public class SearchResultsRequest
    {
        public SearchResultsRequest(string term)
        {
            this.Term = term;
            this.NrProducts = 10;
        }

        public string Term { get; set; }
        public string CategoryId { get; set; }
        public List<string> RefinementIds { get; set; }
        public Boolean? IncludeProducts { get; set; }
        public Boolean? IncludeCategories { get; set; }
        public Boolean? IncludeRefinements { get; set; }
        public Boolean? IncludeAttributes { get; set; }
        public SearchSortingMethod? SortingMethod { get; set; }
        public Boolean? SortingAscending { get; set; }
        public Int32? NrProducts { get; set; }
        public Int64? Offset { get; set; }
        public string ListId { get; set; }

        public enum SearchSortingMethod
        {
            [DescriptionAttribute("sales_ranking")]
            SALES_RANKING,
            [DescriptionAttribute("price")]
            PRICE,
            [DescriptionAttribute("title")]
            TITLE,
            [DescriptionAttribute("publishing_date")]
            PUBLISHING_DATE,
            [DescriptionAttribute("customer_rating")]
            CUSTOMER_RATING
        }
    }
}
