using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HousePrices.Web.Models;

namespace HousePrices.Web.Controllers
{
	public class Results
	{
		public string Postcode { get; set; }
	}
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult About()
		{
			ViewData["Message"] = "Your application description page.";

			return View();
		}

		public IActionResult Contact()
		{
			ViewData["Message"] = "Your contact page.";

			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		public class SearchStructure
		{
			public string Postcode { get; set; }
			public double Radius { get; set; }
		}



		[HttpGet]
		public IActionResult SearchResults(SearchStructure search)
		{
			return View(new List<Results>(){new Results(){Postcode = "CB23 3NY"}});
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
