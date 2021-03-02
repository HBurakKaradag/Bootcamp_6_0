using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Hotels.API.Entities;
using Hotels.API.Models.Filters;
using Hotels.API.Queries;
using Hotels.API.Repositories.Interfaces;
using Hotels.API.UnitOfWorks;

namespace Hotels.API.Repositories
{
    public class RoomRepository : IRoomRepository<RoomEntity>
    {

        private readonly IUnitOfWork _unitofWork;
        private readonly RoomQueries _queries;
        public RoomRepository(IUnitOfWork unitOfWork)
        {
            _unitofWork = unitOfWork;
            _queries =  new RoomQueries();
        }

        public async Task<int> AddAsync(RoomEntity entity)
        {
            return await _unitofWork.Connection.ExecuteAsync(_queries.AddRoomQuery ,entity,_unitofWork.Transaction);
        }


        public async Task<IEnumerable<RoomEntity>> GetAllAsync()
        {
            return await _unitofWork.Connection.QueryAsync<RoomEntity>(_queries.SelectRoomAllQuery);
        }

        public async Task<IEnumerable<RoomEntity>> GetMany(object filter)
        {
            var whereConditions = QueryBuilder.PrepareCondition(filter);
            var sql = _queries.SelectRoomManyQuery + whereConditions.condition;
            return await _unitofWork.Connection.QueryAsync<RoomEntity>(sql, whereConditions.parameters);
        }
    }
}