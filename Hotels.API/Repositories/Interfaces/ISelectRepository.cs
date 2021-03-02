using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hotels.API.Repositories.Interfaces
{
    public interface ISelectRepository<T> where T  : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetMany(object filter);
    }
}
