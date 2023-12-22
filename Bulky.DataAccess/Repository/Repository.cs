using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Bulky.DataAccess.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext _db;
		internal DbSet<T> dbSet;

		public Repository(ApplicationDbContext db)
		{
			_db = db;
			//_db.ShoppingCarts.Include(u => u.Product).Include(u=>u.CoverType);
			this.dbSet = _db.Set<T>();
		}
		public void Add(T entity)
		{
			dbSet.Add(entity);
		}
		//includeProp - "Category,CoverType"
		public IEnumerable<T> GetAll(
			Expression<Func<T, bool>>? filter= null, 
			string? includeProperties = null,
			int? index = null,
			int? size = null)
		{
			IQueryable<T> query = dbSet;
			if (filter != null)
			{
				query = query.Where(filter);
			}
			if (includeProperties != null)
			{
				foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeProp);
				}
			}

			if(index.HasValue && size.HasValue)
			{
				var totalProductListItems = query.Count();

				int itemsToSkip = Math.Min(index.Value * size.Value, totalProductListItems);

				query = query.Skip(itemsToSkip).Take(size.Value);
			}

			return query.ToList();
		}

		public int CountItems()
		{
			IQueryable<T> query = dbSet;

			return query.Count();
		}


		public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
		{
			IQueryable<T> query;
			if(tracked){
				query = dbSet;

			}
			else
			{
				query = dbSet.AsNoTracking();

			}

			query = query.Where(filter);
			if (!string.IsNullOrEmpty(includeProperties))
			{
				foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query.AsQueryable().Include(includeProp);
				}
			}
			return query.FirstOrDefault();
		}

		public void Remove(T entity)
		{
			dbSet.Remove(entity);
		}

		public void RemoveRange(IEnumerable<T> entity)
		{
			dbSet.RemoveRange(entity);
		}
	}
}