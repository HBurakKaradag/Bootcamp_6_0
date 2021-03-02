using System.Data;
using System;
using Hotels.API.UnitOfWorks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Hotels.API
{
    public class DbSessionManager : IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;
        private readonly UnitOfWork _unitOfWork;
        private readonly RepositoryWrapper _repoWrapper;
        public RepositoryWrapper RepoWrapper => _repoWrapper;
        public UnitOfWork UnitOfWork => _unitOfWork;

        public DbSessionManager(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            _dbConnection.Open();
            _unitOfWork = new UnitOfWork(_dbConnection);
            _repoWrapper = new RepositoryWrapper(_unitOfWork);
        }

        public void Dispose()
        {
            if(_dbConnection != null)
            {
                _unitOfWork.Close();
                _dbConnection.Close();
            }
        }
    }
}
