using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Models.ViewModels
{
	public class PaginationVM<T>
	{
		public IEnumerable<T> Items { get; set; }
        public int TotalItems { get; set; }
		public int PageIndex { get; set; } = 0;
		public int PageSize { get; set; } = 5;
    }
}
