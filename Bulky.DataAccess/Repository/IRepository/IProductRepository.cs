using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bulky.Models;

namespace Bulky.DataAccess.Repository.IRepository
{
	public interface IProductRepository : IRepository<Product>
	{
		//Product Get(Func<object, bool> value, string includeProperties);

		//	List<Product> GetAll(string includeProperties);
		void Update(Product obj);
	}
}
