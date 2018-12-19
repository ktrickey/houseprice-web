using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
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
        private readonly IRestClientFactory _restClient;
        private readonly IRestRequestFactory _request;
        private readonly string _apiRoot;

        public HomeController(IHostingEnvironment env, IConfiguration configuration, IRestClientFactory restClient, IRestRequestFactory request)
        {
            _restClient = restClient;
            _request = request;

            _apiRoot = configuration["ApiRoot"];

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
        public IActionResult SearchResults(SearchStructure search)
        {
            var url = $"{_apiRoot}/api/transaction/";

            var client = _restClient.Create($"{_apiRoot}/api/transaction/");
            var request = _request.Create($"{search.Postcode}/{search.Radius}", Method.GET);

            Log.Information($"accessing {url}");
            try
            {

                request.Credentials = CredentialCache.DefaultCredentials;

                var response = client.Execute<PagedResult<Results>>(request);

                // Display the status.
                Log.Information(response.StatusDescription);


                var stuff = response.Data;
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
