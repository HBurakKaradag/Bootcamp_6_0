using System;
using System.Threading.Tasks;
using Hotels.API.Entities;
using Newtonsoft.Json;

namespace Hotels.API.Consumers.Services
{
    public class RoomConsumeService : IRoomConsumeService
    {
        public Task<bool> AddRoomEventConsume(string content)
        {
            var rooms = JsonConvert.DeserializeObject<RoomEntity>(content);
         
           // Do Work 

           return Task.FromResult<bool>(true);
        }
    }
}