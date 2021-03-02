using System;
using System.Threading.Tasks;

namespace Hotels.API.Consumers.Services
{
    public interface IRoomConsumeService
    {
        Task<bool> AddRoomEventConsume(string content);
        
    }
}
