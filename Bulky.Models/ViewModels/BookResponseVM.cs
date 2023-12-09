using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Models.ViewModels;

public class BookListResponseVM
{
    public int NumFound { get; set; }
    public IEnumerable<BookResponseVM> Docs { get; set; }
}

public class BookResponseVM
{
    public string Title { get; set; }
    public double Ratings_average { get; set; }
    public int Ratings_count { get; set; }
}
