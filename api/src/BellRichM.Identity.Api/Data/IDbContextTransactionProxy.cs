using System;

namespace BellRichM.Identity.Api.Data
{
public interface IDbContextTransactionProxy : IDisposable
    {
        void Commit();

        void Rollback();
    }
}