using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hotels.API.Models.Derived;
using Hotels.API.Models.Filters;
using Hotels.API.Models.Paging;

namespace Hotels.API.Services
{
    public interface IRoomService
    {
        Task<bool> AddRoomAsync(Room room);
        Task<PagedResponse<Room>> GetRoomsPagedAsync(RoomFilter filter);
    }
}
