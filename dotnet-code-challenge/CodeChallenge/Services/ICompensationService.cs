using CodeChallenge.Models;
using System;
using System.Threading.Tasks;

namespace CodeChallenge.Services
{
    public interface ICompensationService
    {
        Compensation GetByEmployeeId(String id);
        Compensation CreateOrReplace(Compensation compensation);
    }
}
