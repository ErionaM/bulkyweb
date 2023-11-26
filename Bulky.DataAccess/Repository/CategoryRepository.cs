using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.EntityFrameworkCore;


namespace Bulky.DataAccess.Repository
{
	public class CategoryRepository : Repository<Category> , ICategoryRepository
	{
		private ApplicationDbContext _db;

		public CategoryRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public Category? Get(Func<object, bool> value)
		{
			throw new NotImplementedException();
		}

		public void Update(Category obj)
		{
			_db.Categories.Update(obj);
		}
	}
}
