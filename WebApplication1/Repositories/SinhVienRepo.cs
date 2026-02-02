using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dto;
using WebApplication1.Models;

namespace WebApplication1.Repositories;

public class SinhVienRepo : ISinhVienRepo
{
    private readonly AppDbContext _context;

    public SinhVienRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<object>> GetAllPaginatedAsync(int pageNumber, int pageSize)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var skip = (pageNumber - 1) * pageSize;

        var totalCount = await _context.SinhViens.CountAsync();

        var data = await _context.SinhViens
            .Include(sv => sv.Khoa)
            .Select(sv => new
            {
                sv.SinhVienId,
                sv.MaSinhVien,
                sv.TenSinhVien,
                sv.NgaySinh,
                sv.GioiTinh,
                sv.KhoaId,
                TenKhoa = sv.Khoa != null ? sv.Khoa.TenKhoa : "Chưa cấp khoa"
            })
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<object>(data.Cast<object>().ToList(), totalCount, pageNumber, pageSize);
    }

    public async Task<PagedResult<object>> SearchSinhVienAsync(SearchSinhVienParams svParams)
    {
        var pageNumber = svParams.PageNumber < 1 ? 1 : svParams.PageNumber;
        var pageSize = svParams.PageSize < 1 ? 10 : (svParams.PageSize > 100 ? 100 : svParams.PageSize);

        var skip = (pageNumber - 1) * pageSize;

        var query = _context.SinhViens
            .Include(sv => sv.Khoa)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(svParams.Keyword))
        {
            var keyword = svParams.Keyword.ToLower();
            query = query.Where(sv => sv.TenSinhVien.ToLower().Contains(keyword));
        }

        var totalCount = await query.CountAsync();

        var data = await query
            .Select(sv => new
            {
                sv.SinhVienId,
                sv.MaSinhVien,
                sv.TenSinhVien,
                sv.NgaySinh,
                sv.GioiTinh,
                sv.KhoaId,
                TenKhoa = sv.Khoa != null ? sv.Khoa.TenKhoa : "Chưa cấp khoa"
            })
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<object>(data.Cast<object>().ToList(), totalCount, pageNumber, pageSize);
    }
}
