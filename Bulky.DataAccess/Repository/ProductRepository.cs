﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;

namespace Bulky.DataAccess.Repository
{
	public class ProductRepository : Repository<Product>, IProductRepository
	{
		private ApplicationDbContext _db;
		public ProductRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		//public Product Get(Func<object, bool> value, string includeProperties)
		//{
		//	throw new NotImplementedException();
		//}

		public List<Product> GetAll(string includeProperties)
		{
			throw new NotImplementedException();
		}

		public void Update(Product obj)
		{
			var objFromDb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
			if (objFromDb != null)
			{
				objFromDb.Title = obj.Title;
				objFromDb.Description = obj.Description;
				objFromDb.CategoryId = obj.CategoryId;
				objFromDb.ISBN = obj.ISBN;
				objFromDb.ListPrice = obj.ListPrice;
				objFromDb.Price = obj.Price;
				objFromDb.Price100 = obj.Price100;
				objFromDb.Price50 = obj.Price50;
				objFromDb.Author = obj.Author;

				if (obj.ImageUrl != null)
				{
					objFromDb.ImageUrl = obj.ImageUrl;
				}
			}

		}
	}
}
