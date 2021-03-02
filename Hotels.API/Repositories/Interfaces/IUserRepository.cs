using System;

namespace Hotels.API.Repositories.Interfaces
{
    public interface IUserRepository<T> : ISelectRepository<T> where T : class
    {
        
    }
}
