using System;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CodeChallenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;
        private readonly ILogger<CompensationService> _logger;
        IEmployeeService _employeeService;

        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository, IEmployeeService employeeService)
        {
            _compensationRepository = compensationRepository;
            _logger = logger;
            _employeeService = employeeService;
        }

        // Create new compensation record for existing employee, or replace existing record
        public Compensation CreateOrReplace(Compensation compensation)
        {
            // Check if the POJO is null, and if the employee id contained within is valid
            if(compensation != null && _employeeService.GetById(compensation.EmployeeId) != null)
            {
                _compensationRepository.AddOrReplace(compensation);
                _compensationRepository.SaveAsync().Wait();
            } 
            else 
            { 
                throw new Exception("Invalid Employee ID");
            }

            return compensation;
        }

        // Fetch compensation record by employee id
        public Compensation GetByEmployeeId(string id)
        {
            var comp = _compensationRepository.GetByEmployeeId(id);
            if(comp != null)
            {
                comp.Employee = _employeeService.GetById(id);
                return comp;
            }

            return null;
        }
    }
}
