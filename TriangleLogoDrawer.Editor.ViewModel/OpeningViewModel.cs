using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TriangleLogoDrawer.Editor.ViewModel
{
    public class OpeningViewModel : ViewModelBase
    {
        private const string visibilityVisible = "Visible";
        private const string visibilityCollapsed = "Collapsed";

        public enum OpeningStages
        {
            createNew,
            OpenImage,
        }

        private OpeningStages stage;
        private bool fullscreen;
        private int width;

        public OpenImageViewModel WorkingOneNewViewModel { get; init; }
        public ObservableCollection<OpenImageViewModel> Images { get; set; }
        public OpenImageViewModel SelectedImage { get; set; }
        public bool FullScreen
        {
            get => fullscreen;
            set
            {
                if (FullScreen != value)
                {
                    fullscreen = value;
                    RaiseProppertyChanged();
                    RaiseProppertyChanged(nameof(WidthHeigthVisibility));
                }
            }
        }
        public int Width { get => width; set { if (width != value) { width = value; RaiseProppertyChanged(); } } }
        public int Height { get; set; }
        public ICommand CommandSwitchStageNewImage { get; init; }
        public ICommand CommandSwitchStageOpenImage { get; init; }
        public string NewImageVisibility
        {
            get
            {
                if (stage == OpeningStages.createNew)
                {
                    return visibilityVisible;
                }
                else
                {
                    return visibilityCollapsed;
                }
            }
        }
        public string OpenImageVisibility
        {
            get
            {
                if (stage == OpeningStages.OpenImage)
                {
                    return visibilityVisible;
                }
                else
                {
                    return visibilityCollapsed;
                }
            }
        }
        public string WidthHeigthVisibility
        {
            get
            {
                if (FullScreen)
                {
                    return visibilityVisible;
                }
                else
                {
                    return visibilityCollapsed;
                }
            }
        }

        public OpeningViewModel()
        {
            CommandSwitchStageNewImage = new Command(SwitchStageNewImage);
            CommandSwitchStageOpenImage = new Command(SwitchStageOpenImage);
            FullScreen = true;
            WorkingOneNewViewModel = new();
        }

        private void SwitchStageNewImage()
        {
            stage = OpeningStages.createNew;
            SwitchStageRaiseProppertyChanged();
        }
        private void SwitchStageOpenImage()
        {
            stage = OpeningStages.OpenImage;
            SwitchStageRaiseProppertyChanged();
        }
        private void SwitchStageRaiseProppertyChanged()
        {
            RaiseProppertyChanged(nameof(NewImageVisibility));
            RaiseProppertyChanged(nameof(OpenImageVisibility));
        }
    }
}
