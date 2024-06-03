using CodeChallenge.Models;
using System;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    public interface ICompensationRepository
    {
        Compensation GetByEmployeeId(String id);
        Compensation AddOrReplace(Compensation employee);
        Compensation Remove(Compensation employee);
        Task SaveAsync();
    }
}