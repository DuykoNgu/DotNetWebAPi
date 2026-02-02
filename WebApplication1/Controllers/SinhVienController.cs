using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dto;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers;

[ApiController]

[Route("api/[controller]")]
[Authorize]
public class SinhVienController: ControllerBase
{
        private readonly ISinhVienRepo _sinhVienRepo;
        private readonly AppDbContext _context;

        public SinhVienController(ISinhVienRepo sinhVienRepo, AppDbContext context)
        {
            _sinhVienRepo = sinhVienRepo;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _sinhVienRepo.GetAllPaginatedAsync(page, pageSize);
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] SearchSinhVienParams searchParams)
        {
            var result = await _sinhVienRepo.SearchSinhVienAsync(searchParams);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SinhVien sv)
        {
            _context.SinhViens.Add(sv);
            await _context.SaveChangesAsync();
            await _context.Entry(sv).Reference(s => s.Khoa).LoadAsync();
    
            return Ok(new
            {
                sv.SinhVienId,
                sv.MaSinhVien,
                sv.TenSinhVien,
                sv.NgaySinh,
                sv.GioiTinh,
                sv.KhoaId,
                TenKhoa = sv.Khoa!.TenKhoa
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SinhVienUpdateDto sv)
        {
            if ((sv) == null) return BadRequest();
            var entity = await _context.SinhViens.FindAsync(id);
            if (entity == null) return NotFound();
            
            var khoaExists = await _context.Khoas.AnyAsync(k => k.KhoaId == sv.KhoaId);
            if (!khoaExists)
                return BadRequest("Khoa không tồn tại");

            entity.TenSinhVien = sv.TenSinhVien;
            entity.MaSinhVien = sv.MaSinhVien;
            entity.GioiTinh = sv.GioiTinh;
            entity.NgaySinh = sv.NgaySinh;
            entity.KhoaId = sv.KhoaId;
            
            await _context.SaveChangesAsync();
            
            await _context.Entry(entity).Reference(s => s.Khoa).LoadAsync();
            return Ok(new
            {
                entity.SinhVienId,
                entity.MaSinhVien,
                entity.TenSinhVien,
                entity.NgaySinh,
                entity.GioiTinh,
                entity.KhoaId,
                TenKhoa = entity.Khoa!.TenKhoa
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sv = await _context.SinhViens.FindAsync(id);
            if (sv == null) return NotFound();

            _context.SinhViens.Remove(sv);
            await _context.SaveChangesAsync();
            return Ok();
        }
}
