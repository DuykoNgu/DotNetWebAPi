using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models;

public class SinhVien
{
    [Column("SINH_VIEN_ID")]
    public int SinhVienId { get; set; }
    [Column("TEN_SINH_VIEN")]
    public string TenSinhVien { get; set; } = null!;
    [Column("MA_SINH_VIEN")] 
    public string MaSinhVien { get; set; } = null!;
    [Column("NGAY_SINH")]
    public DateTime NgaySinh { get; set; }  
    [Column("GIOI_TINH")] 
    public string GioiTinh { get; set; } = null!;
    [Column("KHOA_ID")]
    public int KhoaId { get; set; }

    public Khoa? Khoa { get; set; }
}
