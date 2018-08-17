using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HousePrices.Web.Models;

namespace HousePrices.Web.Controllers
{
	public class Results
	{
		public double Price { get; set; }
		public DateTime TransferDate { get; set; }
		public string Postcode { get; set; }
		public string PropertyType { get; set; }
		public string IsNew { get; set; }
		public string Duration { get; set; }
		public string PAON { get; set; }
		public string SAON { get; set; }
		public string Street { get; set; }
		public string Locality { get; set; }
		public string City { get; set; }
		public string District { get; set; }
		public string County { get; set; }
		public string CategoryType { get; set; }
		public string Status { get; set; }
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
		    WebRequest request = WebRequest.Create ($"https://localhost:6001/api/transaction/{search.Postcode}/{search.Radius}");
		    // If required by the server, set the credentials.
		    request.Credentials = CredentialCache.DefaultCredentials;
		
		    // Get the response.
		    HttpWebResponse response = (HttpWebResponse)request.GetResponse ();
		    // Display the status.
		    Console.WriteLine (response.StatusDescription);
		    // Get the stream containing content returned by the server.
		    Stream dataStream = response.GetResponseStream ();
		    // Open the stream using a StreamReader for easy access.
		    StreamReader reader = new StreamReader (dataStream);
		    // Read the content.
		    string responseFromServer = reader.ReadToEnd ();
		    // Display the content.
		    Console.WriteLine (responseFromServer);
		    // Cleanup the streams and the response.
		    reader.Close ();
		    dataStream.Close ();
		    response.Close ();

			var stuff = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Results>>(responseFromServer);
			return View(stuff);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
