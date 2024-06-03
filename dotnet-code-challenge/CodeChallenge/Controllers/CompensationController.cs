using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;
using System.Threading.Tasks;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/compensation")]
    public class CompensationController : ControllerBase
    {
        private readonly ILogger<CompensationController> _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        // Create/update a compensation record for a specific employee
        [HttpPost]
        public IActionResult CreateCompensationRecord([FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation record create request for employee: '{compensation.EmployeeId}'");

            try 
            {
                _compensationService.CreateOrReplace(compensation);
            } 
            catch
            {
                return NotFound();
            }
            return CreatedAtRoute("getCompensationByEmployeeId", new { id = compensation.EmployeeId }, compensation);
        }

        // Fetch the compensation record for a specific employee
        [HttpGet("{id}", Name = "getCompensationByEmployeeId")]
        public IActionResult GetCompensationByEmployeeId(String id)
        {
            _logger.LogDebug($"Received compensation get request for employee '{id}'");

            var compRecord = _compensationService.GetByEmployeeId(id);

            if (compRecord == null)
                return NotFound();

            return Ok(compRecord);
        }
    }
}
