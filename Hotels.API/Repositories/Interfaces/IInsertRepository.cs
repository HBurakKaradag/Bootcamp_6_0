using System;
using System.Threading.Tasks;

namespace Hotels.API.Repositories.Interfaces
{
    public interface IInsertRepository<T> where T : class
    {
        Task<int> AddAsync(T entity);
    }
}
