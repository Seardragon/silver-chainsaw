using CodeChallenge.Models;
using System;
using System.Threading.Tasks;

namespace CodeChallenge.Services
{
    public interface IReportingStructureService
    {
        Task<ReportingStructure> GetByEmployeeId(String id);
    }
}
