using System;

namespace Hotels.API.Queries
{
    public class RoomQueries
    {
        public string AddRoomQuery => @"Insert Into Rooms(Name, Rate, IsMigrate)
                                                   Values(@Name,@Rate,@IsMigrate)";

        public string SelectRoomAllQuery => "Select * From Rooms";

        public string SelectRoomManyQuery => @"Select * From Rooms r
                                                Where 1 = 1";
    }
}
