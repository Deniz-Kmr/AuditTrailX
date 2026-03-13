namespace AuditTrailX.Core.Common;

public class PagedResult<T>
{
    // burada sayfalı veri sonucunu generic olarak taşıyorum
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    // burada toplam sayfa sayısını toplam kayıt ve sayfa boyutuna göre hesaplıyorum
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    // burada ileri geri sayfa var mı kontrolünü kolaylaştırıyorum
    public bool HasNextPage => PageNumber < TotalPages;
    public bool HasPreviousPage => PageNumber > 1;
}