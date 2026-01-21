using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;


namespace WebApplication1.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<SinhVien> SinhViens => Set<SinhVien>();
    public DbSet<Khoa> Khoas => Set<Khoa>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Cấu hình tên bảng thủ công
        modelBuilder.Entity<SinhVien>().ToTable("sinh_vien");
        modelBuilder.Entity<Khoa>().ToTable("khoa");

        // Nếu bạn muốn map luôn tên cột (ví dụ SINH_VIEN_ID)
        modelBuilder.Entity<SinhVien>()
            .Property(s => s.SinhVienId)
            .HasColumnName("SINH_VIEN_ID");
    }   
}