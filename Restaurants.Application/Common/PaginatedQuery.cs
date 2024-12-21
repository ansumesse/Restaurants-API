using MediatR;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Common
{
    public abstract class PaginatedQuery<TResponse> : IRequest<PagedResult<TResponse>>
    {
        public string? SearchedPhrase { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1; 
        public string? SortBy { get; set; }
        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
    }
}
