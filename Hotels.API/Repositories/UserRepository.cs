using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Hotels.API.Entities;
using Hotels.API.Repositories.Interfaces;
using Hotels.API.UnitOfWorks;
using Hotels.API.Queries;

namespace Hotels.API.Repositories
{
    public class UserRepository : IUserRepository<UserEntity>
    {
        private readonly UserQueries _queries = null;
        private readonly IUnitOfWork _unitOfWork;

        public UserRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _queries = new UserQueries();
        }

        public async Task<IEnumerable<UserEntity>> GetAllAsync()
        {
            return await _unitOfWork.Connection.QueryAsync<UserEntity>(_queries.SelectUserAllQueries);
        }

        public async Task<IEnumerable<UserEntity>> GetMany(object filter)
        {
            var whereConditions = QueryBuilder.PrepareCondition(filter);
            var sql = _queries.SelectUserManyQuery + whereConditions.condition;
            return await _unitOfWork.Connection.QueryAsync<UserEntity>(sql, whereConditions.parameters);
        }

    }
}
