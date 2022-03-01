using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Models
{
    public class PagenetedList<T> : List<T>
    {
        public PagenetedList(List<T> items, int count, int pageindex, int pagesize)
        {
            this.AddRange(items);
            PageIndex = pageindex;
            TotalPages = (int)(Math.Ceiling(count / (double)pagesize));
        }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }

        public static PagenetedList<T> Create(IQueryable<T> query, int pageindex, int pagesize)
        {
            var items = query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            return new PagenetedList<T>(items, query.Count(), pageindex, pagesize);
        }
    }
}
