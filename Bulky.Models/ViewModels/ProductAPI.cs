using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Bulky.Models.ViewModels
{
	public class ProductAPI
	{
		public Product Product { get; set; }
		[ValidateNever]
		public double Rating { get; set; }	
	}
}
