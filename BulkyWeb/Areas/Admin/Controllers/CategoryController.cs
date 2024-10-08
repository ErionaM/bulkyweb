﻿using Microsoft.AspNetCore.Mvc;
using Bulky.Models;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Bulky.Utillity;
using System.Drawing.Text;

namespace BulkyWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = SD.Role_Admin)]
	public class CategoryController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public CategoryController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

		}


		private const int ItemsPerPage = 10;
		public IActionResult Index()
		{
        IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll(); 
   return View(objCategoryList);
		


		}


		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Category obj)
		{
			if (obj.Name == obj.DisplayOrder.ToString())
			{
				ModelState.AddModelError("name", "To displayOrder cannot exactly match the Name.");
			}
			if (ModelState.IsValid)
			{
				_unitOfWork.Category.Add(obj);
				_unitOfWork.Save();
				TempData["success"] = "Category created successfully";
				return RedirectToAction("Index", "Category");
			}
			return View();
		}


		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
            var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);


            //_unitOfWork.Category.Get(u => u.Id == id);
            //Category categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //Category categoryFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();
            if (categoryFromDb == null)
			{
				return NotFound();
			}
			return View(categoryFromDb);
		}

		[HttpPost]
		public IActionResult Edit(Category obj)
		{

			if (ModelState.IsValid)
			{
				_unitOfWork.Category.Update(obj);
				_unitOfWork.Save();
				TempData["success"] = "Category updated successfully";
				return RedirectToAction("Index", "Category");
			}
			return View(obj);
		}


		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
            var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);


            if (categoryFromDb == null)
			{
				return NotFound();
			}
			return View(categoryFromDb);
		}

		[HttpPost, ActionName("Delete")]
		public IActionResult DeletePOST(int? id)
		{
            var obj = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);


            if (obj == null)
			{
				return NotFound();
			}
			_unitOfWork.Category.Remove(obj);
			_unitOfWork.Save();
			TempData["success"] = "Category deleted successfully";
			return RedirectToAction("Index");


		}
	}
}

