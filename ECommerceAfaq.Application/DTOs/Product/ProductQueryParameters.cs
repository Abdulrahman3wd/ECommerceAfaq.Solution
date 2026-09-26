using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Application.DTOs.Product
{
    public class ProductQueryParameters
    {
        public string? Search {  get; set; }
        public int? CategoryId { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public string? SortBy { get; set; } = "name"; // name , price , created add

        public string SortOrder { get; set; } = "asc"; //asc - desc

        private const int MaxPageSize = 50;

        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1; 

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value; 
        }


    }
}
