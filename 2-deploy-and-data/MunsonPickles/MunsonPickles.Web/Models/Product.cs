namespace MunsonPickles.Web.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? PhotoUrl { get; set; }
    public ProductType? ProductType { get; set; }
    public ICollection<Review>? Reviews { get; set; }
}
