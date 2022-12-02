namespace MunsonPickles.Web.Models;

public class Review
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int UserId { get; set; }
    public string Text { get; set; } = string.Empty;
    public Product? Product { get; set; }
    public IEnumerable<ReviewPhoto>? Photos { get; set; }
}
