using System;
using System.Data;

namespace Hotels.API.UnitOfWorks
{
    public interface IUnitOfWork
    {
        IDbConnection Connection {get;}
        IDbTransaction Transaction {get;}

        Guid Id {get;}

        void Begin();
        void Commit();
        void Rollback();
    }
}