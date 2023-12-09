using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Bulky.Models
{
	public class ShoppingCart
	{
		public int Id { get; set; }

		public int ProductId { get; set; } //will be a foreign key to the product table
		[ForeignKey("ProductId")]
		[ValidateNever]
		public Product? Product { get; set; }


		[Range(1,1000,ErrorMessage ="Please enter a value between 1 and 1000")]
		public int Count { get; set; }


		public string? ApplicationUserId { get; set; }// will be a foreign key of application user table
		[ForeignKey("ApplicationUserId")]
		public ApplicationUser? ApplicationUser { get; set; }


		[NotMapped] // don't add to the database
		public double Price { get; set; }
	}
}
