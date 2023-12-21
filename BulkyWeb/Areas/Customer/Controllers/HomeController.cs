using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utillity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace BulkyWeb.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class HomeController : Controller
	{
		private const string BOOKS_URL_API = "https://openlibrary.org/search.json?title={0}";
		private readonly ILogger<HomeController> _logger;
		private readonly IUnitOfWork _unitOfWork;

		public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
		{
			_logger = logger;
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index(int index = 0, int size = 2)
		{
			IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
			var totalProductListItems = productList.Count();

			int itemsToSkip = Math.Min(index * size, totalProductListItems);

			productList = productList.Skip(itemsToSkip).Take(size);


			List<ProductApiVM> productApis = new List<ProductApiVM>();

			foreach (var product in productList)
			{
				productApis.Add(new ProductApiVM
				{
					Product = product,
					//RatingAverage = GetRatingAverage(product.Title!)
					RatingAverage = 0
				});
			}
			
			var pagination = new PaginationVM<ProductApiVM>
			{
				Items = productApis,
				PageIndex = index,
				PageSize = size,
				TotalItems = totalProductListItems
			};

			return View(pagination);
		}

		private double GetRatingAverage(string productTitle)
		{
			double ratingAverage = 0;
			string searchBookUrl = string.Format(BOOKS_URL_API, productTitle);
			Uri baseUrl = new Uri(searchBookUrl);

			RestClient client = new RestClient(baseUrl);
			var request = new RestRequest("", Method.Get);

			var response = client.Execute<BookListResponseVM>(request);

			if (response.IsSuccessful)
			{
				Console.WriteLine(response.Content);

				var bookListResponse = response.Data;

				if (bookListResponse!.NumFound > 0)
				{
					ratingAverage = bookListResponse.Docs
						.FirstOrDefault()!.Ratings_average;
				}
			}
			else
			{
				Console.WriteLine($"Error: {response.StatusCode} - {response.ErrorMessage}");
			}

			return ratingAverage;
		}

		public IActionResult Details(int productId)
		{
			ShoppingCart cartObj = new()
			{
				Product = _unitOfWork.Product
									.GetAll(includeProperties: "Category")
									.FirstOrDefault(p => p.Id == productId),
				Count = 1,
				ProductId = productId
			};
			return View(cartObj);
		}

		[HttpPost]
		[Authorize] 
		public IActionResult Details(ShoppingCart shoppingCart)
		{
			//Get the userID of the user
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
			shoppingCart.ApplicationUserId = userId;

			ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.ApplicationUserId == userId &&
			u.ProductId == shoppingCart.ProductId);

			if (cartFromDb != null)
			{
				//shopping cart exist
				cartFromDb.Count += shoppingCart.Count;
				_unitOfWork.ShoppingCart.Update(cartFromDb);
				_unitOfWork.Save();
			}
			else
			{
				//add cart record
				_unitOfWork.ShoppingCart.Add(shoppingCart);
				_unitOfWork.Save();

				//method setint32 is used to store an integer value in the session
				HttpContext.Session.SetInt32(SD.SessionCart,
					_unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());

			}
			TempData["success"] = "Cart updated successfully";
			



			return RedirectToAction(nameof(Index));
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}