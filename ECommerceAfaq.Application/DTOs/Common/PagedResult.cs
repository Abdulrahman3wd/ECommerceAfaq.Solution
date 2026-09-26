using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Application.DTOs.Common
{
    public class PagedResult<T>
    {

        public List<T> Items { get; set; } = new(); 

        public int PageNumber { get; set; }
        public int PageSize { get; set; } // 10
        public int TotalCount { get; set; } // 47 
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize); // 47 / 10 = 5

        public bool HasPreviousPage => PageNumber > 1 ;
        public bool HasNextPage => PageNumber < TotalPages;



    }
}
