using RestSharp;

namespace HousePrices.Web.Factories
{
    public interface IRestFactory
    {
        IRestClient CreateClient(string baseUrl);
        IRestRequest CreateRequest(string url, Method method);
    }

    public class RestFactory : IRestFactory
    {
        public IRestClient CreateClient(string baseUrl)
        {
            return new RestClient(baseUrl);
        }

        public IRestRequest CreateRequest(string url, Method method)
        {
            return new RestRequest(url, method);
        }
    }
}