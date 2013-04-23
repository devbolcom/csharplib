using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Bol.OpenAPI
{
    public class ListResultRequest
    {
        public ListResultRequest(ListType type, string categoryId)
        {
            this.Type = type;
            this.CategoryId = categoryId;
            this.NrProducts = 10;            
        }

        public ListType Type { get; set; }
        public string CategoryId { get; set; }
        public List<string> RefinementIds { get; set; }
        public Boolean? IncludeProducts { get; set; }
        public Boolean? IncludeCategories { get; set; }
        public Boolean? IncludeRefinements { get; set; }
        public Boolean? IncludeAttributes { get; set; }
        public ListSortingMethod? SortingMethod { get; set; }
        public Boolean? SortingAscending { get; set; }
        public Int32? NrProducts { get; set; }
        public Int64? Offset { get; set; }
        public string ListId { get; set; }

        public enum ListType
        {
            [DescriptionAttribute("toplist_default")]
            TOPLIST_DEFAULT,
            [DescriptionAttribute("toplist_overall")]
            TOPLIST_OVERALL,
            [DescriptionAttribute("toplist_last_week")]
            TOPLIST_LAST_WEEK,
            [DescriptionAttribute("toplist_last_two_months")]
            TOPLIST_LAST_TWO_MONTHS,
            [DescriptionAttribute("new")]
            NEW,
            [DescriptionAttribute("preorder")]
            PREORDER
        }

        public enum ListSortingMethod
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
