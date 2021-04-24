using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TriangleLogoDrawer.Data.Services;

namespace TriangleLogoDrawer.Editor
{
    public partial class ImageSelectionForm : Form
    {
        private IImageData imageData;

        public ImageSelectionForm(IImageData imageData)
        {
            this.imageData = imageData;

            InitializeComponent();
            InitializeMenuStrip();
        }

        private void InitializeMenuStrip()
        {
            MenuStrip menuStrip = new MenuStrip();

            //this.MainMenuStrip = menuStrip;
            Controls.Add(menuStrip);

            menuStrip.Items.Add(GetToolStripMenuItemFile());
        }
        private ToolStripMenuItem GetToolStripMenuItemFile()
        {
            ToolStripMenuItem toolStripMenuItemFile = new ToolStripMenuItem("File");
            toolStripMenuItemFile.Text = "File";
            toolStripMenuItemFile.DropDownItems.AddRange(new ToolStripItem[] { GetToolStripRadioButtonMenuItemNewImage(), GetToolStripRadioButtonMenuItemOpenImage()});
            return toolStripMenuItemFile;
        }
        private ToolStripItem GetToolStripRadioButtonMenuItemOpenImage()
        {
            ToolStripItem OpenImage = new ToolStripMenuItem();
            OpenImage.Text = "Open";
            OpenImage.Click += MenuOpenImageClick;
            return OpenImage;
        }
        private ToolStripItem GetToolStripRadioButtonMenuItemNewImage()
        {
            ToolStripItem NewImage = new ToolStripMenuItem();
            NewImage.Text = "New";
            NewImage.Click += MenuNewImageClick;
            return NewImage;
        }

        private void MenuNewImageClick(object sender, EventArgs args)
        {
            // TODO: toggle on new image options
            MessageBox.Show("new image");
        }
        private void MenuOpenImageClick(object sender, EventArgs args)
        {
            // TODO: toggle on open image options
            string toReturn = "";
            foreach(TriangleLogoDrawer.Data.Image image in imageData.GetAll())
            {
                toReturn += image.Name+"\r\n";
            }
            MessageBox.Show(toReturn);
        }
    }
}
