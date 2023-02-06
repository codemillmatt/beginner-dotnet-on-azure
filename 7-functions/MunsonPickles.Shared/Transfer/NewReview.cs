using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunsonPickles.Shared.Transfer;

public class NewReview
{
    public string ReviewText { get; set; } = string.Empty;
    public int ProductId { get; set; } = 0;
    public List<string> PhotoUrls { get; set; }= new List<string>();
}
