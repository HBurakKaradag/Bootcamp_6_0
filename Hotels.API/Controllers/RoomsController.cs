using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Hotels.API.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Hotels.API.Models.Filters;
using Hotels.API.Extensions;
using Hotels.API.Models.Derived;

namespace Hotels.API.Controllers
{
    [Authorize]
    [Route("/[controller]/[action]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {

        private readonly IRoomService _roomService;
        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpPost(Name= nameof(AddRoom))]
        public async Task<IActionResult> AddRoom(Room room)
        {
            await _roomService.AddRoomAsync(room);
            return Ok();
        }

        [HttpGet(Name = nameof(GetRoomsPaged))]
        public async Task<IActionResult> GetRoomsPaged([FromQuery] RoomFilter roomFilter)
        {
            // validasyon 
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

         
            roomFilter.RouteName = string.Concat(Request.Scheme, "://", Request.Host.Value);
            roomFilter.RouteValues = Request.Query.ToRouteValues();

            var rooms = await _roomService.GetRoomsPagedAsync(roomFilter);

            if (rooms.Data == null || !rooms.Data.Any())  //rooms.Data.Count() <= 0 ) /* 1 2 3 4 ... 10.000*/
                return NoContent();

            return Ok(rooms);


        }
    }
}
