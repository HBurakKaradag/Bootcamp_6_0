using System.Resources;
using System.Collections.ObjectModel;
using System.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hotels.API.Models.Derived;
using AutoMapper;
using System.Linq;
using Hotels.API.Models.Paging;
using Hotels.API.Models.Filters;
using Hotels.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Hotels.API.Caches;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Hotels.API.Extensions;
using Hotels.API.Publishers;

namespace Hotels.API.Services
{
    public class RoomService : IRoomService
    {
        private readonly IDistributedCache _cache;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;
        private readonly IConfiguration _configuration;
        private readonly IRoomPublisher _roomPublisher;

        public RoomService(IMapper mapper, IUrlHelper urlHelper, IDistributedCache cache, IConfiguration configuration, IRoomPublisher roomPublisher)
        {
            _mapper = mapper;
            _urlHelper = urlHelper;
            _cache = cache;
            _configuration = configuration;
            _roomPublisher = roomPublisher;
        }

        public async Task<bool> AddRoomAsync(Room room)
        {
            var entity = _mapper.Map<RoomEntity>(room);
            var affectedRows = 0;
            using (DbSessionManager dbManager = new DbSessionManager(_configuration))
            {
                
                affectedRows = await dbManager.RepoWrapper.RoomRepository.AddAsync(entity);
            }

            if(affectedRows> 0 )
                _roomPublisher.PublishRoomAdd(entity);

            return affectedRows > 0;
        }

        public async Task<PagedResponse<Room>> GetRoomsPagedAsync(RoomFilter filter)
        {

            List<Room> rooms = null;
            var _key = $"name={filter.Name}&rate={filter.Rate}&pageindex={filter.PageIndex}&rowsperpage={filter.RowsPerPage}";

            var cacheKey = CacheKeys.GetCacheKeyRoomList(_key);
            var cacheValue = await _cache.GetStringAsync(cacheKey);


            // if(!string.IsNullOrWhiteSpace(cacheValue))
            //     rooms = JsonConvert.DeserializeObject<List<Room>>(cacheValue);

            // if(rooms == null)
            // {
            //    using(DbSessionManager dbManager = new DbSessionManager(_configuration))
            //    {
            //         var dbRecords = await dbManager.RepoWrapper.RoomRepository.GetAllAsync();
            //         rooms = dbRecords.Select(room => _mapper.Map<Room>(room)).ToList();
            //         await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(rooms));
            //    }
            // }

            rooms = await _cache.GetOrSetCacheAsync<List<Room>>(cacheKey, () =>
             {
                 var rooms = new List<Room>();
                 using (DbSessionManager dbManager = new DbSessionManager(_configuration))
                 {
                     var dbRecords = dbManager.RepoWrapper.RoomRepository.GetAllAsync().Result;
                     rooms = dbRecords.Select(room => _mapper.Map<Room>(room)).ToList();
                 }
                 return rooms;
             });

            PagedResponse<Room> pageResponse = new PagedResponse<Room>
            {
                Data = rooms,
                Links = new Links
                {
                    NextPage = string.Concat(filter.RouteName, _urlHelper.RouteUrl(filter.NextRouteValues)),
                    PreviousePage = string.Concat(filter.RouteName, _urlHelper.RouteUrl(filter.PreviousRouteValues))
                }
            };

            return pageResponse;
        }
    }
}