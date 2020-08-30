using Imposto.Common.Util.Contract;
using Imposto.Common.Util.Implementation;
using Imposto.Core.Data;
using Imposto.Core.Service.Contract;
using Imposto.Core.Service.Implementation;
using Imposto.Data.Repository;
using Imposto.Data.Transaction;
using SimpleInjector;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TesteImposto
{
    public static class Bootstrapper
    {
        public static void RegisterStartup(this Container container) =>
            container.Register<FormImposto>(Lifestyle.Singleton);

        public static void ResolveDependencies(this Container container)
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString);

            container.RegisterInstance<IContext>(new Context(connection));

            container.Register<INotaFiscalService, NotaFiscalService>(Lifestyle.Singleton);

            container.Register<INotaFiscalRepository, NotaFiscalRepository>(Lifestyle.Singleton);

            container.Register(typeof(IXmlWriter<>), typeof(XmlWriter<>), Lifestyle.Singleton);
        }

        public static Form RunStartupApplication(this Container container) => container.GetInstance<FormImposto>();
    }
}
