using TriangleLogoDrawer.ApplicationCore.Entities;

namespace TriangleLogoDrawer.Editor.FormOpener
{
    public class EditImageInfo
    {
        public Image Image { get; set; }
        public bool Fullscreen { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}