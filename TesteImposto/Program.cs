using SimpleInjector;
using System;
using System.Windows.Forms;

namespace TesteImposto
{
    static class Program
    {
        private static Container Container { get; set; }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Container = new Container();

            Container.RegisterStartup();
            Container.ResolveDependencies();

            Container.Verify();

            Application.Run(Container.RunStartupApplication());
        }
    }
}
