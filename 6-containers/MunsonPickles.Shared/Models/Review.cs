using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunsonPickles.Shared.Models;

public class Review
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string? UserId { get; set; }
    public string Text { get; set; } = string.Empty;
    public Product? Product { get; set; }
    public ICollection<ReviewPhoto>? Photos { get; set; }
}
