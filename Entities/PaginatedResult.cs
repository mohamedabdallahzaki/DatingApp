using Microsoft.EntityFrameworkCore;

namespace API.Entities
{
    public class PaginatedResult<T>
    {
        public PaginatedMetadata Metadata { get; set; } = default!;

        public IReadOnlyList<T> Items { get; set; } = [];
    }

    public class PaginatedMetadata
    {

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }
    }


    public static class paginatedHelper
    {

        public async static Task<PaginatedResult<T>> CreateAsync<T>(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            var metadata = new PaginatedMetadata
            {
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
                PageSize = pageSize,
                TotalCount = count
            };
            return new PaginatedResult<T>
            {
                Metadata = metadata,
                Items = items
            };
        }
    }
}


