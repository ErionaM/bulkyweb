using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utillity;
using Bulky.DataAccess;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bulky.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace BulkyBookWeb.Controllers;
[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class UserController : Controller
{
	private readonly ApplicationDbContext _db;

	public UserController(ApplicationDbContext db)
	{
		_db = db;
	}

	public IActionResult Index()
	{
		return View();
	}

	

	#region API CALLS
	[HttpGet]
	public IActionResult GetAll()
	{
		List <ApplicationUser>objUserList = _db.ApplicationUsers.Include(u => u.Company).ToList();

		var userRoles = _db.UserRoles.ToList();
		var roles = _db.Roles.ToList();

		foreach(var user in objUserList)
		{
			var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
			user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;

			if(user.Company == null)
			{
				user.Company = new()
				{
					Name = ""

				};

			}
		}
		return Json(new { data = objUserList });
	}

	//POST
	[HttpPost]
	public IActionResult LockUnlock([FromBody]string id)
	{
		var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
		if(objFromDb == null)
		{
		return Json(new { success = false, message = "Error while Locking/Unlocking" });

		}
		if(objFromDb.LockoutEnd!= null && objFromDb.LockoutEnd > DateTime.Now)   //user is currently locked we have to unlock them
		{
			objFromDb.LockoutEnd = DateTime.Now;
		}else
		{
			objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
		}
		_db.SaveChanges();
		return Json(new { success = true, message = "Operation successful" });
		
	}
	#endregion
}