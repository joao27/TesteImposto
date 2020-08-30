using Dapper;
using Imposto.Data.Transaction;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Imposto.Data.Repository
{
    public abstract class RepositoryBase
    {
        public RepositoryBase(IContext context)
        {
            Context = context;
        }

        public IContext Context { get; }

        protected int ExecuteProcedure(string procedure, DynamicParameters parameters) =>
            Context.Connection.Execute(procedure, parameters, commandType: CommandType.StoredProcedure);

        protected T Query<T>(string sql) =>
            Context.Connection.ExecuteScalar<T>(sql);
    }
}
