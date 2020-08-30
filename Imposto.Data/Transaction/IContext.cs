using System;
using System.Data;

namespace Imposto.Data.Transaction
{
    public interface IContext : IDisposable
    {
        IDbConnection Connection { get; }
    }
}
