using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiWinFormCloser
{
    internal static class WinFormsToClose
    {
        private static List<Form> formsToClose = new List<Form>();

        internal static void Add(Form formToCloseAtTheEnd)
        {
            formsToClose.Add(formToCloseAtTheEnd);
        }
        internal static void Close(Form startForm)
        {
            formsToClose.Remove(startForm);
            formsToClose.FirstOrDefault()?.Close();
        }
    }
}
