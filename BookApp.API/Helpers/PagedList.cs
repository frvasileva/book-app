using System;
using System.Collections.Generic;

namespace BookApp.API.Helpers {
    public class PagedList<T> : List<T> {

        public List<T> Items { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PagedList (List<T> items, int count, int pageNumber, int pageSize) {
            Items = items;
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int) Math.Ceiling (count / (double) pageSize);
        }
    }
}