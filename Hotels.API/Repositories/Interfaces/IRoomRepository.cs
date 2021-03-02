using System;

namespace Hotels.API.Repositories.Interfaces
{
    public interface IRoomRepository<T> : IInsertRepository<T>, ISelectRepository<T> where T : class
    {
            // RoomRepository özelindeki methodlar için kullanılır.
    }
}
