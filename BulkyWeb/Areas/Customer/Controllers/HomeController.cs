using System.Diagnostics;
using System.Security.Claims;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
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

		public IActionResult Index()
		{
			List<ProductApiVM> productApis = new List<ProductApiVM>();
			IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");

			foreach (var product in productList)
			{
				productApis.Add(new ProductApiVM
				{
					Product = product,
					RatingAverage = GetRatingAverage(product.Title!)
				});
			}

			return View(productApis);
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
			ShoppingCart cart = new()
			{
				Product = _unitOfWork.Product.GetAll(includeProperties: "Category").FirstOrDefault(),
				Count = 1,
				ProductId = productId,
			};
			return View(cart);
		}

		[HttpPost]
		[Authorize] // the user have to logged into the website, if he want to post
		public IActionResult Details(ShoppingCart shoppingCart)
		{
			//Get the userID of the logged user
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
			}
			else
			{
				//add cart record
				_unitOfWork.ShoppingCart.Add(shoppingCart);


			}
			TempData["success"] = "Cart updated successfully";
			_unitOfWork.Save();



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