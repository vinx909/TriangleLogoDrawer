using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TriangleLogoDrawer.ApplicationCore.Interfaces;
using TriangleLogoDrawer.Data.Services;
using TriangleLogoDrawer.Data.Services.SimpleDependencyProvidedSetup;
using TriangleLogoDrawer.Editor.FormOpener;
using TriangleLogoDrawer.SimpleDependencyProvider;

namespace TriangleLogoDrawer.Editor
{
    static class Program
    {
        private enum selectWindowOptions
        {
            WinForm
        }
        private enum editWindowOptions
        {
            WinForm
        }
        private const selectWindowOptions selectWindowOption = selectWindowOptions.WinForm;
        private const editWindowOptions editWindowOption = editWindowOptions.WinForm;

        private static ISelectImage imageSelector;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SimpleDependencyProviderSetup.Setup();

            Opener.AddOpenAction(Opener.Options.Edit, OpenEditWindow());
            Opener.AddOpenAction(Opener.Options.Open, OpenSelectWindow());

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Opener.Open(Opener.Options.Open);
            Application.Run((Form)imageSelector);
        }

        public static Action OpenSelectWindow()
        {
            Action toReturn = null;
            switch (selectWindowOption)
            {
                case selectWindowOptions.WinForm:
                    toReturn = () => {
                        WinForm.ImageSelectionForm imageSelectionForm = new WinForm.ImageSelectionForm(DependencyProvider.Get<IImageService>());
                        imageSelector = imageSelectionForm;
                        imageSelectionForm.Show();
                        };
                    break;
            }
            return toReturn;
        }
        public static Action OpenEditWindow()
        {
            Action toReturn = null;
            switch (editWindowOption)
            {
                case editWindowOptions.WinForm:
                    toReturn = () =>
                    {
                        EditImageInfo info = imageSelector.GetEditImageInfo();
                        WinForm.ImageEditForm imageEditForm = new WinForm.ImageEditForm(DependencyProvider.Get<IImageService>(), DependencyProvider.Get<IPointService>(), DependencyProvider.Get<IShapeService>(), DependencyProvider.Get<ITriangleService>(), DependencyProvider.Get<IOrderService>(), info.Image, info.Fullscreen, info.Width, info.Height);
                        imageEditForm.Show();
                    };
                    break;
            }
            return toReturn;
        }
    }
}
