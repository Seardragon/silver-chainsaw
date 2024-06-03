using System;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CodeChallenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        // Add employee to DB
        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        // Fetch employee by ID
        public Employee GetById(string id)
        {
            var employee = _employeeContext.Employees
                .SingleOrDefault(e => e.EmployeeId == id);

            return employee;
        }

        // Fetch employee + all their reports by ID
        public async Task<Employee> GetByIdWithAllReports(string id) {
            var employee = _employeeContext.Employees
                .Include(e => e.DirectReports)
                .SingleOrDefault(e => e.EmployeeId == id);

         
            await LoadDirectReportsRecursive(employee);

            return employee;
        }

        // Recursively loads subordinates of direct reports
        private async Task LoadDirectReportsRecursive(Employee employee)
        {
            if (employee != null && employee.DirectReports != null && employee.DirectReports.Count > 0)
            {
                foreach (var directReport in employee.DirectReports)
                {
                    var loadedReport = await _employeeContext.Employees
                        .Include(e => e.DirectReports)
                        .SingleOrDefaultAsync(e => e.EmployeeId == directReport.EmployeeId);

                    if (loadedReport != null)
                    {
                        directReport.DirectReports = loadedReport.DirectReports;

                        await LoadDirectReportsRecursive(directReport);
                    }
                }
            }
        }


        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        // Remove employee from DB
        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }
    }
}
