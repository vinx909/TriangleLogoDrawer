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
using TriangleLogoDrawer.Data.Services;
using TriangleLogoDrawer.Editor.FormOpener;

namespace TriangleLogoDrawer.Editor.WinForm
{
    public partial class ImageSelectionForm : MultiWinFormCloseableForm, ISelectImage
    {
        private const string stringNextLine = "\r\n";
        private const string ToolStripMenuItemFileString = "File";
        private const string nameLabelString = "name";
        private const string pathLabelString = "path to background image. shown on right.";
        private const string newImageWidthLabelString = "width (for opening the image)";
        private const string newImageHeightLabelString = "height (for opening the image)";
        private const string newImageButtonString = "create";
        private const string fullscreenCheckBoxString = "fullscreen";
        private const string widthLabelString = "width";
        private const string heightLabelString = "height";
        private const string editImageButtonString = "edit";
        private const string errorMessageNoName = "no name was given. background image is optional, name is not.";
        private const string errorMessageNoRadioButtonSelected = "no image selected, if there's no image to select you need create one instead.";
        private const string errorMessageImpossibleWidth = "the screen must have a width above {0}. (the higher the better)";
        private const string errorMessageImpossibleHeight = "the screen must have a height above {0}. (the higher the better)";

        private const bool editImageFullscreenCheckBoxDefault = false;
        private const bool disabled = false;
        private const bool enabled = true;

        private const int newImageOptionsWidth = 237;
        private const int minWidthEdit = 1;
        private const int minHeightEdit = 1;

        private readonly IImageData imageData;

        private TextBox newImageNameTextBox;
        private TextBox newImageBackgroundImagePath;
        private CheckBox newImageFullscreenCheckBox;
        private TextBox newImageWidthTextBox;
        private TextBox newImageHeightTextBox;
        private PictureBox newImageBackgroundExamplePictureBox;

        private ListBox existingImageslistBox;
        private CheckBox editImageFullscreenCheckBox;
        private TextBox editImageWidthTextBox;
        private TextBox editImageHeightTextBox;
        private PictureBox editImageBackgroundExamplePictureBox;

        private readonly List<Control> objectsToHide;
        private readonly List<Control> objectsToShowNewImage;
        private readonly List<Control> objectsToShowEditImage;

        private Dictionary<RadioButton, int> radioButtonValues;

        private EditImageInfo editImageInfo;

        private int menuHeight;
        private int editImageBackgroundExamplePictureBoxMaxHeight;

        public ImageSelectionForm(IImageData imageData)
        {
            this.imageData = imageData;

            editImageInfo = new EditImageInfo();

            objectsToHide = new List<Control>();
            objectsToShowNewImage = new List<Control>();
            objectsToShowEditImage = new List<Control>();

            InitializeComponent();
            InitializeMenuStrip();
            InitializeNewImages();
            InitializeOpenImages();

            DisableAll();
        }

        private void InitializeMenuStrip()
        {
            MenuStrip menuStrip = new();
            menuStrip.Items.Add(GetToolStripMenuItemFile());

            Controls.Add(menuStrip);

            menuHeight = menuStrip.Height;
        }
        private ToolStripMenuItem GetToolStripMenuItemFile()
        {
            ToolStripMenuItem toolStripMenuItemFile = new()
            {
                Text = ToolStripMenuItemFileString
            };
            //TODO: add alter image background image path option. low priority
            toolStripMenuItemFile.DropDownItems.AddRange(new ToolStripItem[] { GetToolStripRadioButtonMenuItemNewImage(), GetToolStripRadioButtonMenuItemOpenImage()});
            return toolStripMenuItemFile;
        }
        private ToolStripItem GetToolStripRadioButtonMenuItemOpenImage()
        {
            ToolStripItem OpenImage = new ToolStripMenuItem
            {
                Text = "Open"
            };
            OpenImage.Click += MenuOpenImageClick;
            return OpenImage;
        }
        private ToolStripItem GetToolStripRadioButtonMenuItemNewImage()
        {
            ToolStripItem NewImage = new ToolStripMenuItem
            {
                Text = "New"
            };
            NewImage.Click += MenuNewImageClick;
            return NewImage;
        }
        private void InitializeNewImages()
        {
            Label nameLabel = new()
            {
                Text = nameLabelString,
                Width = newImageOptionsWidth
            };

            newImageNameTextBox = new TextBox();

            Label pathLabel = new()
            {
                Text = pathLabelString,
                Width = newImageOptionsWidth
            };

            newImageBackgroundImagePath = new TextBox()
            {
                Width = newImageOptionsWidth
            };
            newImageBackgroundImagePath.TextChanged += NewImagePathUpdate;

            newImageFullscreenCheckBox = new CheckBox
            {
                Text = fullscreenCheckBoxString
            };

            Label widthLabel = new()
            {
                Text = newImageWidthLabelString,
                Width = newImageOptionsWidth
            };

            newImageWidthTextBox = new TextBox();

            Label heightLabel = new()
            {
                Text = newImageHeightLabelString,
                Width = newImageOptionsWidth
            };

            newImageHeightTextBox = new TextBox();

            Button button = new()
            {
                Text = newImageButtonString
            };
            button.Click += NewImageButtonClick;

            int optionsWidth = Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(nameLabel.Width, newImageNameTextBox.Width), pathLabel.Width), newImageBackgroundImagePath.Width), widthLabel.Width), newImageWidthTextBox.Width), heightLabel.Width), newImageHeightTextBox.Width);
            
            newImageBackgroundExamplePictureBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = ClientSize.Width - optionsWidth,
                Height = ClientSize.Height - menuHeight
            };


            int x = 0;
            int y = menuHeight;
            nameLabel.Location = new Point(x, y);
            y += nameLabel.Height;
            newImageNameTextBox.Location = new Point(x, y);
            y += newImageNameTextBox.Height;
            pathLabel.Location = new Point(x, y);
            y += pathLabel.Height;
            newImageBackgroundImagePath.Location = new Point(x, y);
            y += newImageBackgroundImagePath.Height;
            newImageFullscreenCheckBox.Location = new Point(x, y);
            y += newImageFullscreenCheckBox.Height;
            widthLabel.Location = new Point(x, y);
            y += widthLabel.Height;
            newImageWidthTextBox.Location = new Point(x, y);
            y += newImageWidthTextBox.Height;
            heightLabel.Location = new Point(x, y);
            y += heightLabel.Height;
            newImageHeightTextBox.Location = new Point(x, y);
            y += newImageHeightTextBox.Height;
            button.Location = new Point(x, y);
            newImageBackgroundExamplePictureBox.Location = new Point(optionsWidth, menuHeight);


            Controls.Add(nameLabel);
            Controls.Add(newImageNameTextBox);
            Controls.Add(pathLabel);
            Controls.Add(newImageBackgroundImagePath);
            Controls.Add(newImageFullscreenCheckBox);
            Controls.Add(widthLabel);
            Controls.Add(newImageWidthTextBox);
            Controls.Add(heightLabel);
            Controls.Add(newImageHeightTextBox);
            Controls.Add(button);
            Controls.Add(newImageBackgroundExamplePictureBox);
            objectsToHide.Add(nameLabel);
            objectsToHide.Add(newImageNameTextBox);
            objectsToHide.Add(pathLabel);
            objectsToHide.Add(newImageBackgroundImagePath);
            objectsToHide.Add(newImageFullscreenCheckBox);
            objectsToHide.Add(widthLabel);
            objectsToHide.Add(newImageWidthTextBox);
            objectsToHide.Add(heightLabel);
            objectsToHide.Add(newImageHeightTextBox);
            objectsToHide.Add(button);
            objectsToHide.Add(newImageBackgroundExamplePictureBox);
            objectsToShowNewImage.Add(nameLabel);
            objectsToShowNewImage.Add(newImageNameTextBox);
            objectsToShowNewImage.Add(pathLabel);
            objectsToShowNewImage.Add(newImageBackgroundImagePath);
            objectsToShowNewImage.Add(newImageFullscreenCheckBox);
            objectsToShowNewImage.Add(widthLabel);
            objectsToShowNewImage.Add(newImageWidthTextBox);
            objectsToShowNewImage.Add(heightLabel);
            objectsToShowNewImage.Add(newImageHeightTextBox);
            objectsToShowNewImage.Add(button);
            objectsToShowNewImage.Add(newImageBackgroundExamplePictureBox);
        }
        private void InitializeOpenImages()
        {
            editImageFullscreenCheckBox = new CheckBox
            {
                Text = fullscreenCheckBoxString
            };

            Label widthLabel = new()
            {
                Text = widthLabelString
            };

            editImageWidthTextBox = new TextBox();

            Label heightLabel = new()
            {
                Text = heightLabelString
            };

            editImageHeightTextBox = new TextBox();

            Button editImageButton = new()
            {
                Text = editImageButtonString
            };
            editImageButton.Click += EditImageButtonClick;

            int optionWith = Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(editImageFullscreenCheckBox.Width, widthLabel.Width), editImageWidthTextBox.Width), heightLabel.Width), editImageHeightTextBox.Width), editImageButton.Width);

            editImageBackgroundExamplePictureBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = optionWith
            };

            int x = ClientSize.Width - optionWith;
            int y = menuHeight;
            editImageFullscreenCheckBox.Location = new Point(x, y);
            y += editImageFullscreenCheckBox.Height;
            widthLabel.Location = new Point(x, y);
            y += widthLabel.Height;
            editImageWidthTextBox.Location = new Point(x, y);
            y += editImageWidthTextBox.Height;
            heightLabel.Location = new Point(x, y);
            y += heightLabel.Height;
            editImageHeightTextBox.Location = new Point(x, y);
            y += editImageHeightTextBox.Height;
            editImageButton.Location = new Point(x, y);
            y += editImageButton.Height;
            editImageBackgroundExamplePictureBox.Location = new Point(x, y);
            
            editImageBackgroundExamplePictureBoxMaxHeight = ClientSize.Height - y;

            existingImageslistBox = new ListBox
            {
                Location = new Point(0, menuHeight),
                Height = ClientSize.Height - menuHeight,
                Width = ClientSize.Width - optionWith
            };


            Controls.Add(existingImageslistBox);
            Controls.Add(editImageFullscreenCheckBox);
            Controls.Add(widthLabel);
            Controls.Add(editImageWidthTextBox);
            Controls.Add(heightLabel);
            Controls.Add(editImageHeightTextBox);
            Controls.Add(editImageButton);
            Controls.Add(editImageBackgroundExamplePictureBox);
            objectsToHide.Add(existingImageslistBox);
            objectsToHide.Add(editImageFullscreenCheckBox);
            objectsToHide.Add(widthLabel);
            objectsToHide.Add(editImageWidthTextBox);
            objectsToHide.Add(heightLabel);
            objectsToHide.Add(editImageHeightTextBox);
            objectsToHide.Add(editImageButton);
            objectsToHide.Add(editImageBackgroundExamplePictureBox);
            objectsToShowEditImage.Add(existingImageslistBox);
            objectsToShowEditImage.Add(editImageFullscreenCheckBox);
            objectsToShowEditImage.Add(widthLabel);
            objectsToShowEditImage.Add(editImageWidthTextBox);
            objectsToShowEditImage.Add(heightLabel);
            objectsToShowEditImage.Add(editImageHeightTextBox);
            objectsToShowEditImage.Add(editImageButton);
            objectsToShowEditImage.Add(editImageBackgroundExamplePictureBox);

            radioButtonValues = new Dictionary<RadioButton, int>();
        }

        private void MenuNewImageClick(object sender, EventArgs args)
        {
            ActivateNewImage();
        }
        private void MenuOpenImageClick(object sender, EventArgs args)
        {
            ActivateOpenImage();
        }
        private void NewImagePathUpdate(object sender, EventArgs args)
        {
            if (string.IsNullOrEmpty(newImageBackgroundImagePath.Text) == false)
            {
                try
                {
                    newImageBackgroundExamplePictureBox.Image = new Bitmap(newImageBackgroundImagePath.Text);
                    return;
                }
                catch { }
            }
        }
        private void NewImageButtonClick(object sender, EventArgs ergs)
        {
            string errorMessage = "";
            int width = default;
            int height = default;
            bool validated = string.IsNullOrEmpty(newImageNameTextBox.Text) == false;
            if(validated == false)
            {
                errorMessage += errorMessageNoName;
            }
            string errorMessageAddition = "";
            validated = validated && ValidateInput(newImageFullscreenCheckBox.Checked, newImageWidthTextBox.Text, newImageHeightTextBox.Text, out width, out height, out errorMessageAddition);
            if (validated)
            {
                Data.Image newImage = new()
                { 
                    Name = newImageNameTextBox.Text,
                    BackgroundImagePath = newImageBackgroundImagePath.Text
                };
                int newImageId = imageData.Create(newImage);
                OpenEditImageWindow(newImageId, newImageFullscreenCheckBox.Checked, width, height);
            }
            else
            {
                errorMessage += errorMessageAddition;
                MessageBox.Show(errorMessage);
            }
        }
        private void EditImageRadioButtonClick(object sender, EventArgs args)
        {
            int? imageId = GetIdSelectedRadioButton();
            if (imageId != null)
            {
                Data.Image image = imageData.Get((int)imageId);
                if (string.IsNullOrEmpty(image.BackgroundImagePath) == false)
                {
                    try
                    {
                        editImageBackgroundExamplePictureBox.Image = new Bitmap(image.BackgroundImagePath);
                        double widthZoomFactor = 1.0 * editImageBackgroundExamplePictureBox.Width / editImageBackgroundExamplePictureBox.Image.Width;
                        editImageBackgroundExamplePictureBox.Height = Math.Min((int)(editImageBackgroundExamplePictureBox.Image.Height * widthZoomFactor), editImageBackgroundExamplePictureBoxMaxHeight);
                        return;
                    }
                    catch { }
                }
            }
            editImageBackgroundExamplePictureBox.Image = null;
        }
        private void EditImageButtonClick(object sender, EventArgs args)
        {
            int? imageId = GetIdSelectedRadioButton();
            string errorMessage = "";
            int width = default;
            int height = default;
            bool validated = imageId != null;
            if (validated == false)
            {
                errorMessage += errorMessageNoRadioButtonSelected + stringNextLine;
            }
            string errorMessageAddition = "";
            validated = validated && ValidateInput(editImageFullscreenCheckBox.Checked, editImageWidthTextBox.Text, editImageHeightTextBox.Text, out width, out height, out errorMessageAddition);
            if (validated)
            {
                
                OpenEditImageWindow((int)imageId, editImageFullscreenCheckBox.Checked, width, height);
            }
            else
            {
                errorMessage += errorMessageAddition;
                MessageBox.Show(errorMessage);
            }
        }

        private int? GetIdSelectedRadioButton()
        {
            foreach (KeyValuePair<RadioButton, int> radioButtonValue in radioButtonValues)
            {
                if (radioButtonValue.Key.Checked)
                {
                    return radioButtonValue.Value;
                }
            }

            return null;
        }

        private bool ValidateInput(bool fullscreen, string widthString, string heightString, out int width, out int height, out string errorMessage)
        {
            bool widthParseSuccessfull = int.TryParse(widthString, out width);
            bool heightParseSuccessfull = int.TryParse(heightString, out height);

            if (fullscreen || (widthParseSuccessfull && width >= minWidthEdit && heightParseSuccessfull && height >= minHeightEdit))
            {
                errorMessage = "";
                return true;
            }
            else
            {
                errorMessage = "";
                
                if (editImageFullscreenCheckBox.Checked == false && (widthParseSuccessfull == false || width < minWidthEdit))
                {
                    editImageWidthTextBox.Text = default;
                    errorMessage += string.Format(errorMessageImpossibleWidth, minWidthEdit) + stringNextLine;
                }
                if (editImageFullscreenCheckBox.Checked == false && (heightParseSuccessfull == false || height < minHeightEdit))
                {
                    editImageHeightTextBox.Text = default;
                    errorMessage += string.Format(errorMessageImpossibleHeight, minHeightEdit) + stringNextLine;
                }
                return false;
            }
        }

        private void ActivateNewImage()
        {
            DisableAll();

            newImageNameTextBox.Text = default;
            newImageBackgroundImagePath.Text = default;
            newImageWidthTextBox.Text = default;
            newImageHeightTextBox.Text = default;

            foreach (Control control in objectsToShowNewImage)
            {
                control.Visible = enabled;
            }
        }
        private void ActivateOpenImage()
        {
            DisableAll();

            existingImageslistBox.Controls.Clear();
            radioButtonValues.Clear();

            int height = 0;
            foreach (Data.Image image in imageData.GetAll())
            {
                RadioButton newImageRadioButton = new()
                {
                    Text = image.Name,
                    Location = new Point(0, height)
                };
                newImageRadioButton.Click += EditImageRadioButtonClick;

                radioButtonValues.Add(newImageRadioButton, image.Id);

                height += newImageRadioButton.Height;

                existingImageslistBox.Controls.Add(newImageRadioButton);
            }
            editImageFullscreenCheckBox.Checked = editImageFullscreenCheckBoxDefault;

            editImageWidthTextBox.Text = default;
            editImageHeightTextBox.Text = default;

            foreach (Control control in objectsToShowEditImage)
            {
                control.Visible = enabled;
            }
        }
        private void DisableAll()
        {
            foreach(Control control in objectsToHide)
            {
                control.Visible = disabled;
            }
        }

        private void OpenEditImageWindow(int imageId, bool fullscreen, int width, int height)
        {
            editImageInfo.Image = imageData.Get(imageId);
            editImageInfo.Fullscreen = fullscreen;
            editImageInfo.Width = width;
            editImageInfo.Height = height;

            Hide();

            Opener.Open(Opener.Options.Edit);
        }

        public EditImageInfo GetEditImageInfo()
        {
            return editImageInfo;
        }
    }
}
