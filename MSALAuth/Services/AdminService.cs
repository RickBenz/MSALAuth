using static MSALAuth.Constants;

namespace MSALAuth.Services
{
    public class AdminService(IHttpClientFactory httpClientFactory)
    {
        public IHttpClientFactory _httpClientFactory { get; } = httpClientFactory;

        public async Task<string> GetAnyDataFromClientApi()
        {
            try
            {
                var client = _httpClientFactory.CreateClient(APIType.MainAPI);
                var response = await client.GetStringAsync("https://api.apis.guru/v2/list.json");
                
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }//END GetAnyDataFirst
    }
}
