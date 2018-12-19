using RestSharp;

namespace HousePrices.Web.Factories
{

    public interface IRestClientFactory
    {
        IRestClient Create(string baseUrl);
    }

    public class RestClientFactory : IRestClientFactory
    {
        public IRestClient Create(string baseUrl)
        {
            return new RestClient(baseUrl);
        }
    }
}