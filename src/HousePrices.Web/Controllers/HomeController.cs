using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using HousePrices.Web.Factories;
using Microsoft.AspNetCore.Mvc;
using HousePrices.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using RestSharp;
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
        private readonly IRestFactory _restFactory;
        private readonly string _apiRoot;
        private readonly string _clientRoot;

        public HomeController(IHostingEnvironment env, IConfiguration configuration, IRestFactory restFactory)
        {
            _restFactory = restFactory;

            _apiRoot = configuration["ApiRoot"];

            _clientRoot = $"{_apiRoot}/api/transaction/";

            Log.Information($"ApiRoot = {_apiRoot}");

            Log.Information($"Web root path:{env.WebRootPath}, Content root path:{env.ContentRootPath}");
            this.ViewBag.mappedPath = env.WebRootPath;
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
        public async Task<IActionResult> SearchResults(SearchStructure search)
        {

            var client = _restFactory.CreateClient(_clientRoot);
            var searchRequest = _restFactory.CreateRequest($"{search.Postcode}/{search.Radius}", Method.GET);

            var yearRequest = _restFactory.CreateRequest("years", Method.GET);




            Log.Information($"accessing {_clientRoot}");
            try
            {

                searchRequest.Credentials = CredentialCache.DefaultCredentials;

                var response = await client.ExecuteTaskAsync<PagedResult<Results>>(searchRequest);
                var yearsResponse = await client.ExecuteTaskAsync<int[]>(yearRequest);

                // Display the status.
                Log.Information(response.StatusDescription);


                var stuff = response.Data;
                return View(new SearchResults(stuff, yearsResponse.Data));
            }
            catch (Exception ex)
            {
                Log.Information($"Error accessing {_clientRoot}:{ex.Message}");
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
