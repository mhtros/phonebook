using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace phonebook_server.Classes.Pagination
{
    public class PagedList<T> : List<T>
    {
        private PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            TotalCount = count;
            AddRange(items);
        }

        public int CurrentPage { get; }

        public int TotalPages { get; }

        public int PageSize { get; }

        public int TotalCount { get; }

        /// <summary>
        ///     Creates asynchronously a pagination list, given a <see cref="IQueryable{T}" />,
        ///     the current pageNumber and the total size of elements for each page
        /// </summary>
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> sourceQuery, int pageNumber, int pageSize)
        {
            var count = await sourceQuery.CountAsync();
            var items = await sourceQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}