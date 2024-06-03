using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    public class CompensationRepository : ICompensationRepository
    {
        private readonly CompensationContext _compensationContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(ILogger<CompensationRepository> logger, CompensationContext compensationContext)
        {
            _compensationContext = compensationContext;
            _logger = logger;
        }

        // Create a new compensation record for a given employee, or replace if one already exists
        public Compensation AddOrReplace(Compensation compensation)
        {
            if(GetByEmployeeId(compensation.EmployeeId) != null)
            {
                _compensationContext.Entry(GetByEmployeeId(compensation.EmployeeId)).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                _compensationContext.Compensations.Update(compensation);
            } 
            else 
            {
                _compensationContext.Compensations.Add(compensation);
            }
            return compensation;
        }

        // Fetch compensation info for a given employee
        public Compensation GetByEmployeeId(string id)
        {
            return _compensationContext.Compensations.SingleOrDefault(e => e.EmployeeId == id);
        }

        public Task SaveAsync()
        {
            return _compensationContext.SaveChangesAsync();
        }

        // Remove compensation record from DB
        public Compensation Remove(Compensation compensation)
        {
            return _compensationContext.Remove(compensation).Entity;
        }
    }
}
