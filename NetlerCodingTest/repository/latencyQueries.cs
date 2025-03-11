using NetlerCodingTest.dtos;
using NetlerCodingTest.models;

namespace NetlerCodingTest.repository
{

    public interface IlatencyQueries
    {
        public  LatenciesResponseDto GetAverageLatencyDto(DateTime startDate, DateTime endDate);

    }
    public class latencyQueries : IlatencyQueries
    {
        private readonly Client client = new Client();


        public List<AverageLatency> ComputeLatencyForOneDay(List<Latency> oneDayData, DateTime startDate, DateTime endDate)
        {
            var day = oneDayData.Where(latency => latency.Date >= startDate && latency.Date <= endDate)
                .GroupBy(latency => latency.ServiceId)
                .Select(latencyGroup => new AverageLatency()
                {
                    ServiceId = latencyGroup.Key,
                    NumberOfRequests = latencyGroup.Count(),
                    AverageResponseTimeMs = (int)latencyGroup.Average(l => l.MilliSecondsDelay)
                }).ToList();

            return day;
        }

        public async Task<LatenciesResponseDto> GetAverageLatencies(DateTime startDate, DateTime endDate)
        {
            List<Task<List<Latency>>> tasks = new List<Task<List<Latency>>>();
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                tasks.Add(client.GetLatenciesForDate(date.ToString("yyyy-MM-dd")));
            }

           var results = await Task.WhenAll(tasks);

           var allLatencies = results.AsParallel().Select(day => ComputeLatencyForOneDay(day, startDate, endDate)).SelectMany(x => x).ToList();

            var latanciesToReturn = allLatencies.GroupBy(latency => latency.ServiceId)
                .Select(latencyGroup => new AverageLatency()
                {
                    ServiceId = latencyGroup.Key,
                    NumberOfRequests = latencyGroup.Sum(l => l.NumberOfRequests),
                    AverageResponseTimeMs = (int)latencyGroup.Average(l => l.AverageResponseTimeMs)
                }).ToList();


            return new LatenciesResponseDto() { Period = new List<string>{ startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyy-MM-dd")}, AverageLatencies = latanciesToReturn };
        }
        public LatenciesResponseDto GetAverageLatencyDto(DateTime startDate, DateTime endDate)
        {
            return GetAverageLatencies(startDate, endDate).Result;
        }
    }


}
