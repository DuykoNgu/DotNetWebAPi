using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models;

public class Khoa
{
    [Column("KHOA_ID")]
    public int KhoaId { get; set; }
    [Column("TEN_KHOA")]
    public string TenKhoa { get; set; }
    
    [Column("SO_HIEU_KHOA")]
    public string SoHieuKhoa { get; set; }

    public ICollection<SinhVien> SinhViens { get; set; } = new List<SinhVien>();
}