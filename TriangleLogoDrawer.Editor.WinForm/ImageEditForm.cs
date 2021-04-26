using MultiWinFormCloser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TriangleLogoDrawer.Editor.FormOpener;

namespace TriangleLogoDrawer.Editor.WinForm
{
    public partial class ImageEditForm : MultiWinFormCloseableForm
    {
        private const string ToolStripMenuItemPointString = "Point";
        private const string ToolStripMenuItemTriangleString = "Triangle";
        private const string ToolStripMenuItemOrderString = "Order";
        private const string toolStripMenuItemExitString = "Exit";

        private const int menuVisibilityTimerInterval = 500;
        private const int MenuVisibilityMaxMouseHideForVisible = 2;


        private readonly int middleX;
        private readonly int middleY;
        

        private MenuStrip menuStrip;
        private PictureBox pictureBox;


        public ImageEditForm(Data.Image Image, bool fullscreen, int width, int height)
        {
            InitializeComponent();
            if (fullscreen)
            {
                //TopMost = true; /*form does not need to always overlay over everything else*/
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
                ClientSize = Screen.PrimaryScreen.Bounds.Size;
            }
            else
            {
                ClientSize = new Size(width, height);
            }
            middleX = ClientSize.Width / 2;
            middleY = ClientSize.Height / 2;
            InitializeMenuStrip();
            InitializePictureBox(Image.BackgroundImagePath);
        }

        private void InitializeMenuStrip()
        {
            menuStrip = new();
            menuStrip.Items.AddRange(GetToolStripMenuItems());

            Controls.Add(menuStrip);
            menuStrip.Visible = false;

            Timer menuVisibilityTimer = new Timer();
            menuVisibilityTimer.Enabled = true;
            menuVisibilityTimer.Interval = menuVisibilityTimerInterval;
            menuVisibilityTimer.Tick += MenuVisibilityTimerTick;
        }
        private ToolStripMenuItem[] GetToolStripMenuItems()
        {
            ToolStripMenuItem toolStripMenuItemPoint = new()
            {
                Text = ToolStripMenuItemPointString
            };
            toolStripMenuItemPoint.Click += MenuPointClick;

            ToolStripMenuItem toolStripMenuItemTriangle = new()
            {
                Text = ToolStripMenuItemTriangleString
            };
            toolStripMenuItemTriangle.Click += MenuTriangleClick;

            ToolStripMenuItem toolStripMenuItemOrder = new()
            {
                Text = ToolStripMenuItemOrderString
            };
            toolStripMenuItemOrder.Click += MenuOrderClick;
            //toolStripMenuItemFile.DropDownItems.AddRange(new ToolStripItem[] { GetToolStripRadioButtonMenuItemNewImage(), GetToolStripRadioButtonMenuItemOpenImage() });

            ToolStripMenuItem toolStripMenuItemExit = new()
            {
                Text = toolStripMenuItemExitString
            };
            toolStripMenuItemExit.Click += MenuExitClick;

            List<ToolStripMenuItem> toReturn = new List<ToolStripMenuItem>()
            {
                toolStripMenuItemPoint,
                toolStripMenuItemTriangle,
                toolStripMenuItemOrder,
                toolStripMenuItemExit
            };
            return toReturn.ToArray();
        }
        private void InitializePictureBox(string imagePath)
        {
            
            if (string.IsNullOrEmpty(imagePath) == false)
            {
                Bitmap bitmap = null;
                try
                {
                    bitmap = new Bitmap(imagePath);
                }
                catch
                {
                    return;
                }
                pictureBox = new PictureBox()
                {
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Width = ClientSize.Width,
                    Height = ClientSize.Height,
                    Image = bitmap
                };
                Controls.Add(pictureBox);
            }
        }

        private void MenuVisibilityTimerTick(object sender, EventArgs args)
        {
            Point mousePosition = PointToClient(MousePosition);
            if(menuStrip.Visible == false)
            {
                if (ClientRectangle.Contains(mousePosition) && mousePosition.Y <= MenuVisibilityMaxMouseHideForVisible)
                {
                    menuStrip.Visible = true;
                }
            }
            else
            {
                if(ClientRectangle.Contains(mousePosition) && menuStrip.ClientRectangle.Contains(mousePosition) == false)
                {
                    menuStrip.Visible = false;
                }
            }
        }
        private void MenuPointClick(object sender, EventArgs args)
        {
            //TODO: initialize points functionality
        }
        private void MenuTriangleClick(object sender, EventArgs args)
        {
            //TODO: initialize triangles functionality
        }
        private void MenuOrderClick(object sender, EventArgs args)
        {
            //TODO: initialize order functionality
        }
        private void MenuExitClick(object sender, EventArgs args)
        {
            Hide();
            Opener.Open(Opener.Options.Open);
        }
    }
}
