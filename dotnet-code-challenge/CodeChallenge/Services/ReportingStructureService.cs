using System;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.Services
{
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly ILogger<ReportingStructureService> _logger;
        IEmployeeService _employeeService;

        public ReportingStructureService(ILogger<ReportingStructureService> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        // Fetch reporting structure for a given employee
        public async Task<ReportingStructure> GetByEmployeeId(string id)
        {
            var currentEmployee = await _employeeService.GetByIdWithAllReports(id);
            var numReports = 0;
            if(!String.IsNullOrEmpty(id) && currentEmployee != null)
            {
                numReports = CountSubordinates(currentEmployee);
                return new ReportingStructure(currentEmployee, numReports);
            }
            return null;
        }
        
        // Recursively counts number of subordinates of a given employee
        private int CountSubordinates(Employee employee) 
        {
            var subordinates = 0;
            if(employee.DirectReports != null) 
                {
                    subordinates += employee.DirectReports.Count;
                    foreach(var subordinate in employee.DirectReports){
                        subordinates += CountSubordinates(subordinate);
                    }
                }
            return subordinates;
        }
    }
}
