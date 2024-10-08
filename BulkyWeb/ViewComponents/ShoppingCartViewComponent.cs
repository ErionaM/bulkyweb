﻿using System.Security.Claims;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Utillity;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.ViewComponents
{
	public class ShoppingCartViewComponent : ViewComponent
	{
		private readonly IUnitOfWork _unitOfWork;
		public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
			if (userId != null)
			{
				if (HttpContext.Session.GetInt32(SD.SessionCart) == null)
				{
					HttpContext.Session.SetInt32(SD.SessionCart,
						_unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId.Value).Count());
				}
				return View(HttpContext.Session.GetInt32(SD.SessionCart));

			}
			else
			{
				HttpContext.Session.Clear();
				return View(0);
			}
		}
	}
}
