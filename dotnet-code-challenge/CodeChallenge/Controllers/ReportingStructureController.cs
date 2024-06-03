using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using System.Threading.Tasks;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/reporting")]
    public class ReportingStructureController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IReportingStructureService _reportingStructureService;

        public ReportingStructureController(ILogger<ReportingStructureController> logger, IReportingStructureService reportingStructureService)
        {
            _logger = logger;
            _reportingStructureService = reportingStructureService;
        }

        // Fetch reporting structure of employee by employeeId
        [HttpGet("{id}", Name = "getReportingStructureByEmployeeId")]
        public async Task<IActionResult> GetReportingStructureByEmployeeId(String id)
        {
            _logger.LogDebug($"Received reporting structure get request for employee '{id}'");

            var reportingStructure = await _reportingStructureService.GetByEmployeeId(id);

            if (reportingStructure == null)
                return NotFound();

            return Ok(reportingStructure);
        }
    }
}
