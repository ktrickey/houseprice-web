using RestSharp;

namespace HousePrices.Web.Factories
{
    public interface IRestRequestFactory
    {

        IRestRequest Create(string url, Method method);
    }

    public class RestRequestFactory : IRestRequestFactory
    {
            public IRestRequest Create(string url, Method method)
            {
                return new RestRequest(url, method);
            }
        }

}