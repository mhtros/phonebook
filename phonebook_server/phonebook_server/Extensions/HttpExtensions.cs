using System.Text.Json;
using Microsoft.AspNetCore.Http;
using phonebook_server.Classes.Pagination;

namespace phonebook_server.Extensions
{
    /// <summary>
    ///     Extends the <see cref="HttpResponse" /> class logic with multipurpose functionality.
    /// </summary>
    public static class HttpExtensions
    {
        /// <summary>
        ///     Add the pagination information in http response header
        /// </summary>
        public static void AddPaginationHeader(this HttpResponse response, int currentPage, int itemsPerPage,
            int totalItems, int totalPages)
        {
            var paginationHeaders = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeaders, options));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}