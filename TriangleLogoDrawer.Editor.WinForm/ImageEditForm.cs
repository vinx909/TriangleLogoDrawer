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
using TriangleLogoDrawer.ApplicationCore.Entities;
using TriangleLogoDrawer.ApplicationCore.Interfaces;
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

        private readonly IImageService imageService;
        private readonly IPointService pointService;
        private readonly IShapeService shapeService;
        private readonly ITriangleService triangleService;
        private readonly IOrderService orderService;

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
        private ApplicationCore.Entities.Image image;
        private List<Order> order;
        private Triangle workingOnTriangle;
        private int activeShapeId;

        public ImageEditForm(IImageService imageService, IPointService pointService, IShapeService shapeService, ITriangleService triangleService, IOrderService orderService, ApplicationCore.Entities.Image Image, bool fullscreen, int width, int height)
        {
            state = EditState.none;
            shapeIdsPerToolStripItem = new();
            drawnShapes = new();

            BackColor = backgroundColour;

            this.imageService = imageService;
            this.pointService = pointService;
            this.shapeService = shapeService;
            this.triangleService = triangleService;
            this.orderService = orderService;

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
            image = imageService.Get(imageId).Result;
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
                RefreshImage();
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
                    pointService.Create(new ApplicationCore.Entities.Point() { ImageId = imageId, X = GetXConvertedFormData(Cursor.Position.X), Y = GetYConvertedFormData(Cursor.Position.Y) });
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
                        pointService.Remove(((RoundButton)sender).Point);
                    }
                    Draw();
                    break;

                case EditState.triangle:
                    if (sender.GetType() == typeof(RoundButton))
                    {
                        ApplicationCore.Entities.Point point = ((RoundButton)sender).Point;
                        if (workingOnTriangle.PointOne == point)
                        {
                            workingOnTriangle.PointOne = default;
                        }
                        else if (workingOnTriangle.PointTwo == point)
                        {
                            workingOnTriangle.PointTwo = default;
                        }
                        else if (workingOnTriangle.PointThree == point)
                        {
                            workingOnTriangle.PointThree = default;
                        }
                        else if(workingOnTriangle.PointOne == default)
                        {
                            workingOnTriangle.PointOne = point;
                        }
                        else if (workingOnTriangle.PointTwo == default)
                        {
                            workingOnTriangle.PointTwo = point;
                        }
                        else if (workingOnTriangle.PointThree == default)
                        {
                            workingOnTriangle.PointThree = point;
                        }

                        if(workingOnTriangle.PointOne != default && workingOnTriangle.PointTwo != default && workingOnTriangle.PointThree != default)
                        {
                            triangleService.Create(workingOnTriangle);
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
                        triangleService.Remove(((TriangleButton)sender).Triangle);
                    }
                    Draw();
                    break;

                case EditState.order:
                    if (sender.GetType() == typeof(TriangleButton))
                    {
                        TriangleButton button = ((TriangleButton)sender);
                        if (workingOnTriangle == button.Triangle)
                        {
                            workingOnTriangle.Id = default;
                        }else if(workingOnTriangle.Id == default)
                        {
                            workingOnTriangle = button.Triangle;
                        }
                        else
                        {
                            OrderAction(button.Triangle);
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
                    image = imageService.Get(imageId).Result;
                    DrawPoints(image.Points);
                    DrawTriangles(image.Triangles, true);
                    break;

                case EditState.triangle:
                    image = imageService.Get(imageId).Result;
                    DrawPoints(image.Points);
                    DrawTriangles(image.Triangles);
                    break;

                case EditState.order:
                    IOrderedEnumerable<Order> order = image.GetOrder(activeShapeId);
                    DrawLineOrder(order);
                    DrawTriangleOrder(order);
                    break;
            }
        }
        private void DrawPoints(IEnumerable<ApplicationCore.Entities.Point> points)
        {
            foreach (ApplicationCore.Entities.Point point in points)
            {
                Button newButton = new RoundButton(point)
                {
                    Location = new System.Drawing.Point(GetXConvertedDataForm(point.X) - PointButtonRadius / 2, GetYConvertedDataForm(point.Y) - PointButtonRadius / 2),
                    Width = PointButtonRadius,
                    Height = PointButtonRadius,
                    BackColor = pointButtonNotSelectedColour
                };
                if(workingOnTriangle?.PointOne == point || workingOnTriangle?.PointTwo == point || workingOnTriangle?.PointThree == point)
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
        private void DrawTriangleOrder(IOrderedEnumerable<Order> order)
        {
            Order[] orderArray = order.ToArray();
            List<Triangle> allTriangles = new(image.Triangles);
            
            if (order.Any())
            {
                ColourShifter shifter = new(shiftPerColourStep);
                for (int i = 0; i < orderArray.Length; i++)
                {
                    DrawTriangle(orderArray[i].Triangle, shifter.GetNextColour(127));
                    allTriangles.RemoveAll(t => t.Id == orderArray[i].TriangleId);
                }
            }
            DrawTriangles(allTriangles);
        }
        private void DrawTriangle(Triangle triangle, Color color)
        {
            Button newButton = new TriangleButton(triangle, GetXConvertedDataForm, GetYConvertedDataForm, color, triangleButtonEdgeColour);
            newButton.Click += TriangleButtonClick;
            Controls.Add(newButton);
            drawnShapes.Add(newButton);
        }
        private void DrawLineOrder(IOrderedEnumerable<Order> order)
        {
            List<Point> pointsInOrder = new();
            foreach (Order orderItem in order)
            {
                pointsInOrder.Add(getcenterPointOfTriangle(orderItem.Triangle));
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
        private Point getcenterPointOfTriangle(Triangle triangle)
        {
            int x = (GetXConvertedDataForm(triangle.PointOne.X) + GetXConvertedDataForm(triangle.PointTwo.X) + GetXConvertedDataForm(triangle.PointThree.X)) / 3;
            int y = (GetYConvertedDataForm(triangle.PointOne.Y) + GetYConvertedDataForm(triangle.PointTwo.Y) + GetYConvertedDataForm(triangle.PointThree.Y)) / 3;
            return new Point(x, y);
        }

        private void RefreshImage()
        {
            image = imageService.Get(imageId).Result;
        }
        private void UpdateToolStripMenuItemOrder()
        {
            foreach (KeyValuePair< ToolStripItem, int> shapeIdPerToolStripItem in shapeIdsPerToolStripItem)
            {
                toolStripMenuItemOrder.DropDownItems.Remove(shapeIdPerToolStripItem.Key);
            }
            shapeIdsPerToolStripItem.Clear();
            foreach (Shape shape in image.Shapes)
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
            workingOnTriangle = new Triangle()
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
        private void OrderAction(Triangle triangle)
        {
            if (!image.HasOrders(activeShapeId))
            {
                orderService.Create(new() { OrderNumber = 0, ShapeId = activeShapeId, TriangleId = workingOnTriangle.Id });
                orderService.Create(new() { OrderNumber = 1, ShapeId = activeShapeId, TriangleId = triangle.Id });
            }
            else
            {
                orderService.Create(new() { OrderNumber = orderService.GetOrderNumber(workingOnTriangle), ShapeId = activeShapeId, TriangleId = triangle.Id });
            }

            Order newOrder;

            if (order.Count > 0)
            {
                for (int i = 0; i < order.Count; i++)
                {
                    if (order[i].TriangleId == workingOnTriangle.Id)
                    {
                        if( i + 1 < order.Count && order[i + 1].TriangleId == triangleId)
                        {
                            orderService.Remove(activeShapeId, triangleId);
                            return;
                        }
                        newOrder = new() { ShapeId = activeShapeId, TriangleId = triangleId, OrderNumber = order[i].OrderNumber + 1 };
                        orderService.Create(newOrder);
                        return;
                    }
                }

            }

            newOrder = new() { ShapeId = activeShapeId, TriangleId = workingOnTriangle.Id, OrderNumber = order.Count };
            orderService.Create(newOrder);
            newOrder = new() { ShapeId = activeShapeId, TriangleId = triangleId, OrderNumber = order.Count + 1 };
            orderService.Create(newOrder);
            return;
        }

        private class RoundButton : Button
        {
            internal ApplicationCore.Entities.Point Point { get; set; }
            internal RoundButton(ApplicationCore.Entities.Point point) : base()
            {
                Point = point;
                FlatStyle = FlatStyle.Flat;
            }
            protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
            {
                GraphicsPath grPath = new GraphicsPath();
                grPath.AddEllipse(0, 0, Width-1, Height-1);
                Region = new Region(grPath);
                base.OnPaint(e);
            }
        }
        private class TriangleButton : Button
        {
            private const int borderWidth = 2;

            public Triangle Triangle { get; set; }

            private Point[] points;
            private Color shapeColour;
            private Color edgeColour;

            internal TriangleButton(Triangle triangle, int x1, int y1, int x2, int y2, int x3, int y3, Color shapeColour, Color edgeColour)
            {
                FlatStyle = FlatStyle.Flat;

                this.shapeColour = shapeColour;
                this.edgeColour = edgeColour;
                Triangle = triangle;

                Location = new Point(Math.Min(Math.Min(x1, x2), x3),Math.Min(Math.Min(y1, y2), y3));
                Width = Math.Max(Math.Max(x1, x2), x3) - Location.X;
                Height = Math.Max(Math.Max(y1, y2), y3) - Location.Y;
                points = new Point[3]
                {
                    new Point(x1 - Location.X, y1 - Location.Y),
                    new Point(x2 - Location.X, y2 - Location.Y),
                    new Point(x3 - Location.X, y3 - Location.Y)
                };
            }

            public TriangleButton(Triangle triangle, Func<decimal, int> getXConvertedDataForm, Func<decimal, int> getYConvertedDataForm, Color shapeColour, Color edgeColour) : this(triangle, getXConvertedDataForm(triangle.PointOne.X), getYConvertedDataForm(triangle.PointOne.Y), getXConvertedDataForm(triangle.PointTwo.X), getYConvertedDataForm(triangle.PointTwo.Y), getXConvertedDataForm(triangle.PointThree.X), getYConvertedDataForm(triangle.PointThree.Y), shapeColour, edgeColour) { }

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
