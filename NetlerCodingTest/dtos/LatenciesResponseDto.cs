namespace NetlerCodingTest.dtos;

public class AverageLatency
{
    public int ServiceId { get; set; }
    public int NumberOfRequests { get; set; }
    public int AverageResponseTimeMs { get; set; }
}
public class LatenciesResponseDto
{
    public required List<string> Period { get; set; }
    public List<AverageLatency> AverageLatencies { get; set; }
}
