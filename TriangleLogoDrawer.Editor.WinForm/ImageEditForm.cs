using MultiWinFormCloser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TriangleLogoDrawer.Data;
using TriangleLogoDrawer.Data.Services;
using TriangleLogoDrawer.Editor.FormOpener;
using Point = System.Drawing.Point;

namespace TriangleLogoDrawer.Editor.WinForm
{
    public partial class ImageEditForm : MultiWinFormCloseableForm
    {
        private enum EditState
        {
            none,
            point,
            triangle,
            order
        }

        private const string ToolStripMenuItemPointString = "Point";
        private const string ToolStripMenuItemTriangleString = "Triangle";
        private const string ToolStripMenuItemOrderString = "Order";
        private const string ToolStripItemAddNewShapeString = "New Shape";
        private const string ToolStripItemRemoveActiveShapeString = "Remove Current Shape";
        private const string toolStripMenuItemExitString = "Exit";

        private const string MessageRemoveShapeWrongState = "you must have state active before you can delete it";

        private Color backgroundColour = Color.FromArgb(255, 255, 255, 255);
        private Color pointButtonNotSelectedColour = Color.FromArgb(127, 0, 0, 0);
        private Color pointButtonSelectedColour = Color.FromArgb(255, 255, 0, 0);
        private Color triangleButtonSelectedColour = Color.FromArgb(127, 127, 127, 127);
        private Color triangleButtonNotSelectedColour = Color.FromArgb(127, 170, 170, 170);
        private Color triangleButtonEdgeColour = Color.FromArgb(255, 0, 0, 0);

        private const int menuVisibilityTimerInterval = 500;
        private const int MenuVisibilityMaxMouseHideForVisible = 2;
        private const int orderLineWidth = 3;
        private const int PointButtonRadius = 10;
        private const int shiftPerColourStep = /*8*/ 255;

        private readonly IPointData pointData;
        private readonly IShapeData shapeData;
        private readonly ITriangleData triangleData;
        private readonly ITriangleOrderData triangleOrderData;

        private readonly int imageId;
        private readonly int middleX;
        private readonly int middleY;
        private readonly int distanceModifier;
        private readonly List<Control> drawnShapes;
        private readonly Dictionary<ToolStripItem, int> shapeIdsPerToolStripItem;
        

        private MenuStrip menuStrip;
        private ToolStripMenuItem toolStripMenuItemOrder;
        private PictureBox pictureBox;

        private EditState state;
        private IEnumerable<Data.Point> points;
        private IEnumerable<Triangle> triangles;
        private List<TriangleOrder> order;
        private Triangle workingOnTriangle;
        private int activeShapeId;

        public ImageEditForm(IPointData pointData, IShapeData shapeData, ITriangleData triangleData, ITriangleOrderData triangleOrderData, Data.Image Image, bool fullscreen, int width, int height)
        {
            state = EditState.none;
            shapeIdsPerToolStripItem = new();
            drawnShapes = new();

            BackColor = backgroundColour;

            this.pointData = pointData;
            this.shapeData = shapeData;
            this.triangleData = triangleData;
            this.triangleOrderData = triangleOrderData;

            this.imageId = Image.Id;

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
            distanceModifier = Math.Min(middleX, middleY);
            InitializeMenuStrip();
            //InitializePictureBox(Image.BackgroundImagePath);

            Click += VoidClick;
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

            toolStripMenuItemOrder = new()
            {
                Text = ToolStripMenuItemOrderString
            };
            ToolStripItem addNewShape = new ToolStripMenuItem()
            {
                Text = ToolStripItemAddNewShapeString
            };
            addNewShape.Click += MenuOrderNewShapeClick;
            toolStripMenuItemOrder.DropDownItems.Add(addNewShape);
            ToolStripItem removeActiveShape = new ToolStripMenuItem()
            {
                Text = ToolStripItemRemoveActiveShapeString
            };
            removeActiveShape.Click += MenuOrderRemoveShapeClick;
            toolStripMenuItemOrder.DropDownItems.Add(removeActiveShape);
            UpdateToolStripMenuItemOrder();

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
            System.Drawing.Point mousePosition = PointToClient(MousePosition);
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
            state = EditState.point;
            Draw();
        }
        private void MenuTriangleClick(object sender, EventArgs args)
        {
            //TODO: initialize triangles functionality
            state = EditState.triangle;
            points = pointData.GetAll(imageId);
            newWorkingOnTriangle();
            Draw();
        }
        private void MenuOrderNewShapeClick(object sender, EventArgs args)
        {
            //TODO: add a new shape and make it active
        }
        private void MenuOrderRemoveShapeClick(object sender, EventArgs args)
        {
            //TODO: remove the currently active shape
            switch (state)
            {
                case EditState.order:
                    break;

                default:
                    MessageBox.Show(MessageRemoveShapeWrongState);
                    break;
            }
        }
        private void MenuOrderShapeClick(object sender, EventArgs args)
        {
            //TODO: initialize order functionality
            int? shapeId = shapeIdsPerToolStripItem[sender as ToolStripItem];
            if(shapeId != null)
            {
                state = EditState.order;
                activeShapeId = (int)shapeId;
                newWorkingOnTriangle();
                points = pointData.GetAll(imageId);
                triangles = triangleData.GetAll(imageId);
                Draw();
            }
            else
            {
                UpdateToolStripMenuItemOrder();
            }
        }
        private void MenuExitClick(object sender, EventArgs args)
        {
            Hide();
            Opener.Open(Opener.Options.Open);
        }
        private void VoidClick(object sender, EventArgs args)
        {
            switch (state)
            {
                case EditState.point:
                    pointData.Create(new Data.Point() { ImageId = imageId, X = GetXConvertedFormData(Cursor.Position.X), Y = GetYConvertedFormData(Cursor.Position.Y) });
                    Draw();
                    break;
            }
        }
        private void PointButtonClick(object sender, EventArgs args)
        {
            //TODO: remove point and redraw
            switch (state)
            {
                case EditState.point:
                    if (sender.GetType() == typeof(RoundButton))
                    {
                        pointData.Delete(((RoundButton)sender).PointId);
                    }
                    Draw();
                    break;

                case EditState.triangle:
                    if (sender.GetType() == typeof(RoundButton))
                    {
                        int buttonId = ((RoundButton)sender).PointId;
                        if (workingOnTriangle.PointIdOne == buttonId)
                        {
                            workingOnTriangle.PointIdOne = default;
                        }
                        else if(workingOnTriangle.PointIdOne == default)
                        {
                            workingOnTriangle.PointIdOne = buttonId;
                        }
                        else if (workingOnTriangle.PointIdTwo == buttonId)
                        {
                            workingOnTriangle.PointIdTwo = default;
                        }
                        else if (workingOnTriangle.PointIdTwo == default)
                        {
                            workingOnTriangle.PointIdTwo = buttonId;
                        }
                        else if (workingOnTriangle.PointIdThree == buttonId)
                        {
                            workingOnTriangle.PointIdThree = default;
                        }
                        else if (workingOnTriangle.PointIdThree == default)
                        {
                            workingOnTriangle.PointIdThree = buttonId;
                        }

                        if(workingOnTriangle.PointIdOne != default && workingOnTriangle.PointIdTwo != default && workingOnTriangle.PointIdThree != default)
                        {
                            triangleData.Create(workingOnTriangle);
                            newWorkingOnTriangle();
                        }
                        Draw();
                    }
                    break;
            }                        
        }
        private void TriangleButtonClick(object sender, EventArgs args)
        {
            switch (state)
            {
                case EditState.triangle:
                    if (sender.GetType() == typeof(TriangleButton))
                    {
                        triangleData.Delete(((TriangleButton)sender).TriangleId);
                    }
                    Draw();
                    break;

                case EditState.order:
                    if (sender.GetType() == typeof(TriangleButton))
                    {
                        TriangleButton button = ((TriangleButton)sender);
                        if (workingOnTriangle.Id == button.TriangleId)
                        {
                            workingOnTriangle.Id = default;
                        }else if(workingOnTriangle.Id == default)
                        {
                            workingOnTriangle.Id = button.TriangleId;
                        }
                        else
                        {
                            OrderAction(button.TriangleId);
                            newWorkingOnTriangle();
                        }
                        Draw();
                    }
                    break;
            }
        }

        private void Draw()
        {
            foreach(Control drawnShape in drawnShapes)
            {
                Controls.Remove(drawnShape);
            }
            drawnShapes.Clear();
            switch (state)
            {
                case EditState.point:
                    points = pointData.GetAll(imageId);
                    triangles = triangleData.GetAll(imageId);
                    DrawPoints(points);
                    DrawTriangles(this.triangles, true);
                    break;

                case EditState.triangle:
                    triangles = triangleData.GetAll(imageId);
                    DrawPoints(this.points);
                    DrawTriangles(triangles);
                    break;

                case EditState.order:
                    List<TriangleOrder> order = triangleOrderData.GetOrder(activeShapeId);
                    DrawLineOrder(order);
                    DrawTriangleOrder(order);
                    break;
            }
        }
        private void DrawPoints(IEnumerable<Data.Point> points)
        {
            foreach (Data.Point point in points)
            {
                Button newButton = new RoundButton(point.Id)
                {
                    Location = new System.Drawing.Point(GetXConvertedDataForm(point.X) - PointButtonRadius / 2, GetYConvertedDataForm(point.Y) - PointButtonRadius / 2),
                    Width = PointButtonRadius,
                    Height = PointButtonRadius,
                    BackColor = pointButtonNotSelectedColour
                };
                if(workingOnTriangle?.PointIdOne == point.Id || workingOnTriangle?.PointIdTwo == point.Id || workingOnTriangle?.PointIdThree == point.Id)
                {
                    newButton.BackColor = pointButtonSelectedColour;
                }
                else
                {
                    newButton.BackColor = pointButtonNotSelectedColour;
                }

                newButton.Click += PointButtonClick;
                Controls.Add(newButton);
                drawnShapes.Add(newButton);
            }
        }
        private void DrawTriangles(IEnumerable<Triangle> triangles, bool transparent = false)
        {
            Color colour;
            if (transparent)
            {
                colour = Color.Transparent;
            }
            else
            {
                colour = triangleButtonNotSelectedColour;
            }
            foreach(Triangle triangle in triangles)
            {
                DrawTriangle(triangle, colour);
            }
        }
        private void DrawTriangleOrder(List<TriangleOrder> order)
        {
            List<Triangle> allTriangles = new(triangles);
            
            if (order.Count > 0)
            {
                ColourShifter shifter = new(shiftPerColourStep);
                for (int i = 0; i < order.Count; i++)
                {
                    DrawTriangle(triangles.FirstOrDefault(t => t.Id == order[i].TriangleId), shifter.GetNextColour(127));
                    allTriangles.RemoveAll(t => t.Id == order[i].TriangleId);
                }
            }
            DrawTriangles(allTriangles);
        }
        private void DrawTriangle(Triangle triangle, Color color)
        {
            Data.Point pointOne = points.FirstOrDefault(p => p.Id == triangle.PointIdOne);
            Data.Point pointTwo = points.FirstOrDefault(p => p.Id == triangle.PointIdTwo);
            Data.Point pointThree = points.FirstOrDefault(p => p.Id == triangle.PointIdThree);
            Button newButton = new TriangleButton(triangle.Id,
                GetXConvertedDataForm(pointOne.X), GetYConvertedDataForm(pointOne.Y),
                GetXConvertedDataForm(pointTwo.X), GetYConvertedDataForm(pointTwo.Y),
                GetXConvertedDataForm(pointThree.X), GetYConvertedDataForm(pointThree.Y),
                color, triangleButtonEdgeColour);
            newButton.Click += TriangleButtonClick;
            Controls.Add(newButton);
            drawnShapes.Add(newButton);
        }
        private void DrawLineOrder(List<TriangleOrder> order)
        {
            List<Point> pointsInOrder = new();
            for (int i = 0; i < order.Count; i++)
            {
                pointsInOrder.Add(getcenterPointOfTriangle(order[i].TriangleId));
            }
            Line newLine = new Line(pointsInOrder, orderLineWidth)
            {
                Location = new Point(0, 0),
                Width = ClientSize.Width,
                Height = ClientSize.Height
            };
            Controls.Add(newLine);
            drawnShapes.Add(newLine);
        }
        private Point getcenterPointOfTriangle(int triangleId)
        {
            Triangle triangle = triangles.FirstOrDefault(t => t.Id == triangleId);
            Data.Point pointOne = points.FirstOrDefault(p => p.Id == triangle.PointIdOne);
            Data.Point pointTwo = points.FirstOrDefault(p => p.Id == triangle.PointIdTwo);
            Data.Point pointThree = points.FirstOrDefault(p => p.Id == triangle.PointIdThree);
            int x = (GetXConvertedDataForm(pointOne.X) + GetXConvertedDataForm(pointTwo.X) + GetXConvertedDataForm(pointThree.X)) / 3;
            int y = (GetYConvertedDataForm(pointOne.Y) + GetYConvertedDataForm(pointTwo.Y) + GetYConvertedDataForm(pointThree.Y)) / 3;
            return new Point(x, y);
        }

        private void UpdateToolStripMenuItemOrder()
        {
            foreach (KeyValuePair< ToolStripItem, int> shapeIdPerToolStripItem in shapeIdsPerToolStripItem)
            {
                toolStripMenuItemOrder.DropDownItems.Remove(shapeIdPerToolStripItem.Key);
            }
            shapeIdsPerToolStripItem.Clear();
            foreach (Shape shape in shapeData.GetAll(imageId))
            {
                ToolStripItem newItem = new ToolStripMenuItem()
                {
                    Text = shape.Name
                };
                shapeIdsPerToolStripItem.Add(newItem, shape.Id);
                toolStripMenuItemOrder.DropDownItems.Add(newItem);
                newItem.Click += MenuOrderShapeClick;
            }
        }

        private void newWorkingOnTriangle()
        {
            workingOnTriangle = new Data.Triangle()
            {
                ImageId = imageId
            };
        }
        private int GetXConvertedDataForm(decimal dataX)
        {
            return middleX + (int)(dataX * distanceModifier);
        }
        private int GetYConvertedDataForm(decimal dataY)
        {
            return middleY + (int)(dataY * distanceModifier);
        }
        private decimal GetXConvertedFormData(int formX)
        {
            return 1m * (formX - middleX) / distanceModifier;
        }
        private decimal GetYConvertedFormData(int formY)
        {
            return 1m * (formY - middleY) / distanceModifier;
        }
        private void OrderAction(int triangleId)
        {
            TriangleOrder newOrder;

            if (order.Count > 0)
            {
                for (int i = 0; i < order.Count; i++)
                {
                    if (order[i].TriangleId == workingOnTriangle.Id)
                    {
                        if( i + 1 < order.Count && order[i + 1].TriangleId == triangleId)
                        {
                            triangleOrderData.Delete(activeShapeId, triangleId);
                            return;
                        }
                        newOrder = new() { ShapeId = activeShapeId, TriangleId = triangleId, OrderNumber = order[i].OrderNumber + 1 };
                        triangleOrderData.Create(newOrder);
                        return;
                    }
                }

            }

            newOrder = new() { ShapeId = activeShapeId, TriangleId = workingOnTriangle.Id, OrderNumber = order.Count };
            triangleOrderData.Create(newOrder);
            newOrder = new() { ShapeId = activeShapeId, TriangleId = triangleId, OrderNumber = order.Count + 1 };
            triangleOrderData.Create(newOrder);
            return;
        }

        private class RoundButton : Button
        {
            internal int PointId { get; set; }
            internal RoundButton(int pointId) : base()
            {
                PointId = pointId;
                FlatStyle = FlatStyle.Flat;
            }
            protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
            {
                GraphicsPath grPath = new GraphicsPath();
                grPath.AddEllipse(0, 0, Width-1, Height-1);
                this.Region = new Region(grPath);
                base.OnPaint(e);
            }
        }
        private class TriangleButton : Button
        {
            private const int borderWidth = 2;

            internal int TriangleId { get; set; }
            private Point[] points;
            private Color shapeColour;
            private Color edgeColour;

            internal TriangleButton(int triangleId, int x1, int y1, int x2, int y2, int x3, int y3, Color shapeColour, Color edgeColour)
            {
                FlatStyle = FlatStyle.Flat;  

                this.shapeColour = shapeColour;
                this.edgeColour = edgeColour;
                TriangleId = triangleId;

                Location = new Point(Math.Min(Math.Min(x1, x2), x3),Math.Min(Math.Min(y1, y2), y3));
                Width = Math.Max(Math.Max(x1, x2), x3) - Location.X;
                Height = Math.Max(Math.Max(y1, y2), y3) - Location.Y;
                points = new Point[3]
                {
                    new System.Drawing.Point(x1 - Location.X, y1 - Location.Y),
                    new System.Drawing.Point(x2 - Location.X, y2 - Location.Y),
                    new System.Drawing.Point(x3 - Location.X, y3 - Location.Y)
                };
            }
            protected override void OnPaint(PaintEventArgs e)
            {
                GraphicsPath graphicsPath = new GraphicsPath();
                graphicsPath.AddPolygon(points);


                Region = new Region(graphicsPath);
                
                base.OnPaint(e);
                
                Graphics graphics = e.Graphics;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.FillPolygon(new SolidBrush(shapeColour), points);
                graphics.DrawPolygon(new Pen(edgeColour, borderWidth), points);
            }
        }
        private class Line : Control
        {
            private readonly List<Point> order;
            private readonly int width;

            public Line(List<Point> order, int width)
            {
                this.order = order;
                this.width = width;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                GraphicsPath graphicsPath = new GraphicsPath();
                int pathExtra = width / 2;
                
                //base.OnPaint(e);
                ColourShifter colourShifter = new(shiftPerColourStep);
                Point[] polygonPoints = new Point[4]
                {
                    new Point(order[0].X - pathExtra, order[0].Y - pathExtra),
                    new Point(order[0].X + pathExtra, order[0].Y - pathExtra),
                    new Point(order[0].X + pathExtra, order[0].Y + pathExtra),
                    new Point(order[0].X - pathExtra, order[0].Y + pathExtra),
                };
                graphicsPath.AddPolygon(polygonPoints);
                for (int i = 1; i < order.Count; i++)
                {
                    //e.Graphics.DrawLine(new Pen(new LinearGradientBrush(order[i - 1], order[i], previousColour, nowColour)), order[i - 1], order[i]);
                    e.Graphics.DrawLine(new Pen(new SolidBrush(colourShifter.GetNextColour()), width), order[i-1], order[i]);

                    polygonPoints = new Point[4]
                    {
                        new Point(order[i].X - pathExtra, order[i].Y - pathExtra),
                        new Point(order[i].X + pathExtra, order[i].Y - pathExtra),
                        new Point(order[i].X + pathExtra, order[i].Y + pathExtra),
                        new Point(order[i].X - pathExtra, order[i].Y + pathExtra),
                    };
                    graphicsPath.AddPolygon(polygonPoints);
                    polygonPoints = new Point[4]
                    {
                        new Point(order[i-1].X - pathExtra, order[i-1].Y - pathExtra),
                        new Point(order[i].X - pathExtra, order[i].Y - pathExtra),
                        new Point(order[i].X + pathExtra, order[i].Y + pathExtra),
                        new Point(order[i-1].X + pathExtra, order[i-1].Y + pathExtra),
                    };
                    graphicsPath.AddPolygon(polygonPoints);
                }

                Region = new Region(graphicsPath);
            }
        }

        private class ColourShifter
        {
            private enum colourPart
            {
                red,
                green,
                blue
            };
            private enum stage
            {
                /*
                 * 000 - 0
                 * 010 - 2
                 * 011 - 3
                 * 111 - 7
                 * 110 - 6
                 * 100 - 4
                 * 101 - 5
                 * 001 - 1
                 */
                redLowGreenLowBlueLow,
                redLowGreenHighBlueLow,
                redLowGreenHighBlueHigh,
                redHighGreenHighBlueHigh,
                redHighGreenHighBlueLow,
                redHighGreenLowBlueLow,
                redHighGreenLowBlueHigh,
                redLowGreenLowBlueHigh
            }
            private stage currentStage; 

            private const int minValue = 0;
            private const int maxValue = 255;

            private Dictionary<colourPart, int> valuePerColourPart;

            private int shiftPerStep;

            private colourPart activeColour;
            private bool goingUp;

            internal ColourShifter (int shiftPerStep)
            {
                valuePerColourPart = new();
                valuePerColourPart.Add(colourPart.red, minValue);
                valuePerColourPart.Add(colourPart.green, minValue);
                valuePerColourPart.Add(colourPart.blue, minValue);

                currentStage = stage.redLowGreenHighBlueLow;
                activeColour = colourPart.green;
                goingUp = true;

                this.shiftPerStep = shiftPerStep;
            }

            internal Color GetNextColour(int alpha = 255)
            {
                Color toReturn = Color.FromArgb(alpha, valuePerColourPart[colourPart.red], valuePerColourPart[colourPart.green], valuePerColourPart[colourPart.blue]);
                bool shift = false;
                if (goingUp)
                {
                    if(valuePerColourPart[activeColour] + shiftPerStep > maxValue)
                    {
                        shift = true;
                    }
                }
                else
                {
                    if (valuePerColourPart[activeColour] - shiftPerStep < minValue)
                    {
                        shift = true;
                    }
                }
                if (shift)
                {
                    switch (currentStage)
                    {
                        case stage.redLowGreenLowBlueLow:
                            currentStage = stage.redLowGreenHighBlueLow;
                            activeColour = colourPart.green;
                            goingUp = true;
                            break;
                        case stage.redLowGreenHighBlueLow:
                            currentStage = stage.redLowGreenHighBlueHigh;
                            activeColour = colourPart.blue;
                            break;
                        case stage.redLowGreenHighBlueHigh:
                            currentStage = stage.redHighGreenHighBlueHigh;
                            activeColour = colourPart.red;
                            break;
                        case stage.redHighGreenHighBlueHigh:
                            currentStage = stage.redHighGreenHighBlueLow;
                            activeColour = colourPart.blue;
                            goingUp = false;
                            break;
                        case stage.redHighGreenHighBlueLow:
                            currentStage = stage.redHighGreenLowBlueLow;
                            activeColour = colourPart.green;
                            break;
                        case stage.redHighGreenLowBlueLow:
                            currentStage = stage.redHighGreenLowBlueHigh;
                            activeColour = colourPart.blue;
                            goingUp = true;
                            break;
                        case stage.redHighGreenLowBlueHigh:
                            currentStage = stage.redLowGreenLowBlueHigh;
                            activeColour = colourPart.red;
                            goingUp = false;
                            break;
                        case stage.redLowGreenLowBlueHigh:
                            currentStage = stage.redLowGreenLowBlueLow;
                            activeColour = colourPart.blue;
                            break;
                    }
                }
                /*
                 * 000 - 0
                 * 010 - 2
                 * 011 - 3
                 * 111 - 7
                 * 110 - 6
                 * 100 - 4
                 * 101 - 5
                 * 001 - 1
                 */
                if (goingUp)
                {
                    valuePerColourPart[activeColour] += shiftPerStep;
                }
                else
                {
                    valuePerColourPart[activeColour] -= shiftPerStep;
                }
                return toReturn;
            }
        }
    }
}
