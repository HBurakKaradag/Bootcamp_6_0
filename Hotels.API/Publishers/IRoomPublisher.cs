using System;
using Hotels.API.Entities;

namespace Hotels.API.Publishers
{
    public interface IRoomPublisher
    {
        void PublishRoomAdd(RoomEntity entity);
    }
}
