using System.Net.Http.Headers;
using System.Data;
using System;

namespace Hotels.API.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection _connection = null;
        private IDbTransaction _transaction = null;

        Guid _id = Guid.Empty;

        public UnitOfWork(IDbConnection connection)
        {
            _connection = connection;
        }

        Guid IUnitOfWork.Id
        {
            get { return _id; }
        }

        IDbConnection IUnitOfWork.Connection
        {
            get { return _connection; }
        }

        IDbTransaction IUnitOfWork.Transaction
        {
            get { return _transaction; }
        }

        public void Begin()
        {
            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
            Close();
        }

        public void Rollback()
        {
            _transaction.Rollback();
            Close();
        }

        public void Close()
        {
            if (_transaction != null)
                _transaction.Dispose();
            _transaction = null;
        }

    }
}
