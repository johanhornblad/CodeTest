using NetlerCodingTest.models;
using System.Collections.Specialized;
using System.Web;

namespace NetlerCodingTest
{
    public class Client
    {
        private static readonly HttpClient _client = new HttpClient();
        string baseUrl = "http://latencyapi-env.eba-kqb2ph3i.eu-west-1.elasticbeanstalk.com/latencies";
        private UriBuilder _uriBuilder;
        private NameValueCollection _query; 
        public Client()
        {
            _uriBuilder = new UriBuilder(baseUrl);
            _query = HttpUtility.ParseQueryString(_uriBuilder.Query);


        }


        public async Task<List<Latency>> GetLatenciesForDate(string date)
        {
            try
            {
                _query["date"] = date;
                _uriBuilder.Query = _query.ToString();
                string url = _uriBuilder.ToString();
                HttpResponseMessage response = await _client.GetAsync(url);
                var latencies = await response.Content.ReadFromJsonAsync<IEnumerable<Latency>>();
                response.EnsureSuccessStatusCode();
                return latencies.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

    }
}
