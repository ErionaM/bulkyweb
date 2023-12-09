using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;

namespace Bulky.DataAccess.Repository
{
	internal class OrderHeaderRepository :Repository<OrderHeader>, IOrderHeaderRepository
	{
		private ApplicationDbContext _db;
		public OrderHeaderRepository(ApplicationDbContext db): base(db)
		{
			_db = db;
		}

		public void Update(OrderHeader obj)
		{
			_db.OrderHeaders.Update(obj);
		}
	}
}
