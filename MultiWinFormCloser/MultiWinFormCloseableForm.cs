using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiWinFormCloser
{
    public abstract class MultiWinFormCloseableForm : Form, IMultiWinFormCloser
    {
        protected MultiWinFormCloseableForm()
        {
            WinFormsToClose.Add(this);
            FormClosing += Close;
        }

        public new void Close()
        {
            WinFormsToClose.Close(this);
        }

        public new void Close(object sender, EventArgs args)
        {
            Close();
        }
        
        void IMultiWinFormCloser.TrueClose()
        {
            base.Close();
        }
    }
}
