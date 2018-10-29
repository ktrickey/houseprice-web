using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using HousePrices.Web.Models;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace HousePrices.Web.Controllers
{
    public class PagedResult<T>
    {

        public long TotalRows { get; set; }
        public IEnumerable<T> Results { get; set; }
    }
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
		private string _apiRoot;
		public HomeController()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				
				.AddJsonFile("appsettings.json")
				.AddEnvironmentVariables();

			var configuration = builder.Build();


			_apiRoot = configuration["ApiRoot"];
			Log.Information($"ApiRoot = {_apiRoot}");
		}
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
			var url = $"{_apiRoot}/api/transaction/{search.Postcode}/{search.Radius}";
		
			Log.Information($"accessing {url}");
			try
			{

				WebRequest request = WebRequest.Create(url);
				// If required by the server, set the credentials.
				request.Credentials = CredentialCache.DefaultCredentials;
				//"STUFF".Humanize(LetterCasing.Sentence);
				// Get the response.
				HttpWebResponse response = (HttpWebResponse) request.GetResponse();
				// Display the status.
				Log.Information(response.StatusDescription);
				// Get the stream containing content returned by the server.
				Stream dataStream = response.GetResponseStream();
				// Open the stream using a StreamReader for easy access.
				StreamReader reader = new StreamReader(dataStream);
				// Read the content.
				string responseFromServer = reader.ReadToEnd();
				// Display the content.
				Console.WriteLine(responseFromServer);
				// Cleanup the streams and the response.
				reader.Close();
				dataStream.Close();
				response.Close();

				var stuff = Newtonsoft.Json.JsonConvert.DeserializeObject<PagedResult<Results>>(responseFromServer);
				return View(stuff);
			}
			catch (Exception ex)
			{
				Log.Information($"Error accessing {url}:{ex.Message}");
				throw;
			}
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
