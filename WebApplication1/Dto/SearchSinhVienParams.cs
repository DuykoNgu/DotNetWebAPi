namespace WebApplication1.Dto;

public class SearchSinhVienParams
{
    public string? Keyword { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
