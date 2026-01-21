
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Cần để dùng .ToListAsync()
using WebApplication1.Data;
using WebApplication1.Dto;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SinhVienController: ControllerBase
{
        private readonly AppDbContext _context;

        public SinhVienController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.SinhViens
                .Select(sv => new
                {
                    sv.SinhVienId,
                    sv.MaSinhVien,
                    sv.TenSinhVien,
                    sv.NgaySinh,
                    sv.GioiTinh,
                    sv.KhoaId,
                    TenKhoa = sv.Khoa!.TenKhoa
                })
                .ToListAsync();

            return Ok(data);
        }


        [HttpPost]
        public async Task<IActionResult> Create(SinhVien sv)
        {
            _context.SinhViens.Add(sv);
            await _context.SaveChangesAsync();
            return Ok(sv);
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
            return Ok(entity);
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