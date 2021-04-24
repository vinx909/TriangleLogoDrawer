using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TriangleLogoDrawer.Data.Services;
using TriangleLogoDrawer.Data.Services.SimpleDependencyProvidedSetup;

namespace TriangleLogoDrawer.Editor
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SimpleDependencyProviderSetup.Setup();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ImageSelectionForm(DependencyProvider.Provide<IImageData>()));
        }
    }
}
