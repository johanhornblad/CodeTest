using Microsoft.AspNetCore.Mvc;
using NetlerCodingTest.dtos;
using NetlerCodingTest.repository;

namespace NetlerCodingTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class latenciesController : Controller
    {

        private IlatencyQueries _latencyQueries;

        public latenciesController(IlatencyQueries latencyQueries)
        {
            _latencyQueries = latencyQueries;
        }

        [HttpGet]
        [ProducesResponseType(typeof(LatenciesResponseDto),200)]
        [ProducesResponseType(500)]  
        public IActionResult GetLatencies([FromQuery]string startDate, [FromQuery]string endDate)
        {
            try
            {
                var startDateObj = DateTime.Parse(startDate);
                var endDateObj = DateTime.Parse(endDate);
                var latencyDto = _latencyQueries.GetAverageLatencyDto(startDateObj, endDateObj);
                return Ok(latencyDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
