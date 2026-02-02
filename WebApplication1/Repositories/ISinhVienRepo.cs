using WebApplication1.Dto;

namespace WebApplication1.Repositories;

public interface ISinhVienRepo
{
    Task<PagedResult<object>> GetAllPaginatedAsync(int pageNumber, int pageSize);
    Task<PagedResult<object>> SearchSinhVienAsync(SearchSinhVienParams searchParams);
}
