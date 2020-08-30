using System.Data;

namespace Imposto.Data.Transaction
{
    public class Context : IContext
    {
        public Context(IDbConnection connection)
        {
            Connection = connection;
        }

        public IDbConnection Connection { get; private set; }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}
