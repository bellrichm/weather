using BellRichM.Attribute.CodeCoverage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace BellRichM.Identity.Api.Data
{
    [ExcludeFromCodeCoverage]
    public class DbContextTransactionProxy : IDbContextTransactionProxy
    {
        private readonly IDbContextTransaction _transaction;
        private bool disposed = false;

        public DbContextTransactionProxy(DbContext context)
        {
            _transaction = context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
         {
            if (!disposed)
            {
                if (disposing)
                {
                    _transaction.Dispose();
                }

                disposed = true;
            }
        }
    }
}