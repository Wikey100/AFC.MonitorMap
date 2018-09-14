/**********************************************************
** 文件名： AGMChannelDual.cs
** 文件作用:双向闸机
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2018-09-14    xwj       增加
**
**********************************************************/

using DrawTools.DocToolkit;
using DrawTools.Model;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DrawTools.Device
{
    public class AGMChannelDual : DrawRectangle
    {
        private int objectID;
        private int flag;
        private HVDirection direction;
        private EDirection entry;
        private bool showProperty;
        private string arrayId;
        private int x_axis;
        private int y_axis;
        private int width;
        private int height;

        public string ArrayId
        {
            get { return arrayId; }
            set { arrayId = value; }
        }

        public AGMChannelDual()
            : this(0, 0, 1, 1)
        {
        }

        public AGMChannelDual(int x, int y, int width, int height)
            : base()
        {
            Rectangle = new Rectangle(x, y, width, height);
            logicIDTail = "";
            objectID = objIdInc++;
            tagIDBase = 0;
            flag = objIdInc++;
            direction = HVDirection.Horizontal;
            entry = EDirection.Left;
            showProperty = false;
            Initialize();
        }

        public override DrawObject Clone()
        {
            AGMChannelDual drawAGMChannelDual = new AGMChannelDual();
            drawAGMChannelDual.Rectangle = this.Rectangle;
            drawAGMChannelDual.tagIDBase = this.tagIDBase;
            drawAGMChannelDual.logicIDTail = this.logicIDTail;
            drawAGMChannelDual.DeviceIP = this.DeviceIP;
            drawAGMChannelDual.Direction = this.Direction;

            FillDrawObjectFields(drawAGMChannelDual);
            return drawAGMChannelDual;
        }

        public override DrawObject Clone(int n)
        {
            AGMChannelDual drawAGMChannelDual = new AGMChannelDual();
            drawAGMChannelDual.Rectangle = this.Rectangle;
            drawAGMChannelDual.tagIDBase = this.tagIDBase;
            drawAGMChannelDual.DeviceIP = this.DeviceIP;
            drawAGMChannelDual.logicIDTail = LogicIDAdd(logicIDTail, n);
            drawAGMChannelDual.Direction = this.Direction;
            FillDrawObjectFields(drawAGMChannelDual);
            return drawAGMChannelDual;
        }

        private Point p11 = new Point(0, 0);
        private Point p12 = new Point(0, 0);
        private Point p13 = new Point(0, 0);
        private Point p14 = new Point(0, 0);
        private Point p15 = new Point(0, 0);
        private Point p16 = new Point(0, 0);
        private Point p17 = new Point(0, 0);

        private Point p21 = new Point(0, 0);
        private Point p22 = new Point(0, 0);
        private Point p23 = new Point(0, 0);
        private Point p24 = new Point(0, 0);
        private Point p25 = new Point(0, 0);
        private Point p26 = new Point(0, 0);
        private Point p27 = new Point(0, 0);

        public override void Draw(Graphics g)
        {
            Pen pen = new Pen(Color, PenWidth);

            Brush brushout = null;
            if (tagIDBase == 0)
                brushout = new SolidBrush(Color.FromArgb(255, 0, 255, 255));
            else
                brushout = new SolidBrush(Color.FromArgb(255, 0, 255, 0));

            Brush brushin = new SolidBrush(Color.White);

            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle frect = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);

            g.FillRectangle(brushout, frect);    //填充区域

            if (Angle != null)
            {
                direction = (Angle == "Horizontal" ? HVDirection.Horizontal : HVDirection.Vertical);
            }

            //宽度大于高度，箭头横向
            if (Rectangle.Width > Rectangle.Height)
            {
                direction = HVDirection.Horizontal;
            }
            //宽度小于高度，箭头竖向
            if (Rectangle.Width < Rectangle.Height)
            {
                direction = HVDirection.Vertical;
            }

            switch (direction)
            {
                case HVDirection.Horizontal:
                    {
                        p11.X = Rectangle.X + Rectangle.Width / 2; p11.Y = Rectangle.Y + Rectangle.Height / 2;
                        p12.X = Rectangle.X + Rectangle.Width / 4; p12.Y = Rectangle.Y + Rectangle.Height / 6;
                        p13.X = Rectangle.X + Rectangle.Width / 4; p13.Y = Rectangle.Y + 2 * Rectangle.Height / 6;
                        p14.X = Rectangle.X; p14.Y = Rectangle.Y + 2 * Rectangle.Height / 6;
                        p15.X = Rectangle.X; p15.Y = Rectangle.Y + 4 * Rectangle.Height / 6;
                        p16.X = Rectangle.X + Rectangle.Width / 4; p16.Y = Rectangle.Y + 4 * Rectangle.Height / 6;
                        p17.X = Rectangle.X + Rectangle.Width / 4; p17.Y = Rectangle.Y + 5 * Rectangle.Height / 6;

                        p21.X = Rectangle.X + Rectangle.Width / 2; p21.Y = Rectangle.Y + Rectangle.Height / 2;
                        p22.X = Rectangle.X + 3 * Rectangle.Width / 4; p22.Y = Rectangle.Y + Rectangle.Height / 6;
                        p23.X = Rectangle.X + 3 * Rectangle.Width / 4; p23.Y = Rectangle.Y + 2 * Rectangle.Height / 6;
                        p24.X = Rectangle.X + Rectangle.Width; p24.Y = Rectangle.Y + 2 * Rectangle.Height / 6;
                        p25.X = Rectangle.X + Rectangle.Width; p25.Y = Rectangle.Y + 4 * Rectangle.Height / 6;
                        p26.X = Rectangle.X + 3 * Rectangle.Width / 4; p26.Y = Rectangle.Y + 4 * Rectangle.Height / 6;
                        p27.X = Rectangle.X + 3 * Rectangle.Width / 4; p27.Y = Rectangle.Y + 5 * Rectangle.Height / 6;
                    }
                    break;

                case HVDirection.Vertical:
                    {
                        p11.X = Rectangle.X + Rectangle.Width / 2; p11.Y = Rectangle.Y + Rectangle.Height / 2;
                        p12.X = Rectangle.X + Rectangle.Width / 6; p12.Y = Rectangle.Y + Rectangle.Height / 4;
                        p13.X = Rectangle.X + 2 * Rectangle.Width / 6; p13.Y = Rectangle.Y + Rectangle.Height / 4;
                        p14.X = Rectangle.X + 2 * Rectangle.Width / 6; p14.Y = Rectangle.Y;
                        p15.X = Rectangle.X + 4 * Rectangle.Width / 6; p15.Y = Rectangle.Y;
                        p16.X = Rectangle.X + 4 * Rectangle.Width / 6; p16.Y = Rectangle.Y + Rectangle.Height / 4;
                        p17.X = Rectangle.X + 5 * Rectangle.Width / 6; p17.Y = Rectangle.Y + Rectangle.Height / 4;

                        p21.X = Rectangle.X + Rectangle.Width / 2; p21.Y = Rectangle.Y + Rectangle.Height / 2;
                        p22.X = Rectangle.X + 1 * Rectangle.Width / 6; p22.Y = Rectangle.Y + 3 * Rectangle.Height / 4;
                        p23.X = Rectangle.X + 2 * Rectangle.Width / 6; p23.Y = Rectangle.Y + 3 * Rectangle.Height / 4;
                        p24.X = Rectangle.X + 2 * Rectangle.Width / 6; p24.Y = Rectangle.Y + Rectangle.Height;
                        p25.X = Rectangle.X + 4 * Rectangle.Width / 6; p25.Y = Rectangle.Y + Rectangle.Height;
                        p26.X = Rectangle.X + 4 * Rectangle.Width / 6; p26.Y = Rectangle.Y + 3 * Rectangle.Height / 4;
                        p27.X = Rectangle.X + 5 * Rectangle.Width / 6; p27.Y = Rectangle.Y + 3 * Rectangle.Height / 4;
                    }
                    break;
            }

            //画箭头
            GraphicsPath path = new GraphicsPath();

            path.StartFigure();
            path.AddLine(p11, p12);
            path.AddLine(p12, p13);
            path.AddLine(p13, p14);
            path.AddLine(p14, p15);
            path.AddLine(p15, p16);
            path.AddLine(p16, p17);
            path.AddLine(p17, p11);

            g.FillPath(brushin, path);
            g.DrawPath(pen, path);

            path = new GraphicsPath();

            path.StartFigure();
            path.AddLine(p21, p22);
            path.AddLine(p22, p23);
            path.AddLine(p23, p24);
            path.AddLine(p24, p25);
            path.AddLine(p25, p26);
            path.AddLine(p26, p27);
            path.AddLine(p27, p21);

            g.FillPath(brushin, path);
            g.DrawPath(pen, path);

            g.DrawRectangle(pen, Rectangle);       //画外框

            Brush brushin0 = new SolidBrush(Color.Red);
            StringFormat style = new StringFormat();
            style.Alignment = StringAlignment.Center;
            if (showProperty)
            {
                g.DrawString(
                     logicIDTail,
                     new Font("宋体", 9, FontStyle.Regular),
                     brushin0,
                     Rectangle, style);
            }
            pen.Dispose();
        }

        public override void OnDoubleClick(Object sender, EventArgs e)
        {
            string entryValue;
            if (entry == EDirection.Left)
                entryValue = "Left";
            else if (entry == EDirection.Right)
                entryValue = "Right";
            else if (entry == EDirection.Up)
            {
                entryValue = "Up";
            }
            else
                entryValue = "Down";

            DrawArea drawArea = (DrawArea)sender;

            EditorDialog dlg = new EditorDialog(logicIDTail, this, entryValue, drawArea.GraphicsList.AGMVerify, DeviceTypeEnum.AGM, DeviceTypeEnum.AGM_Sub_AGMChannelDual, drawArea.GraphicsList.IPVerify);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (this.tagIDBase != 0)
                {
                    drawArea.GraphicsList.AGMVerify.Remove(logicIDTail);
                }
                logicIDTail = dlg.IDvalues;
                arrayId = dlg.ArrayId;
                this.x_axis = dlg.X_axis;
                this.y_axis = dlg.Y_axis;
                this.width = dlg.RWidth;
                this.height = dlg.RHeight;

                RecID = dlg.RecID;
                StationID = dlg.StationID;
                DeviceID = dlg.DeviceID;
                DeviceName = dlg.DeviceName;
                IpAdd = dlg.IpAdd;
                DeviceType = dlg.DeviceType;
                DeviceSubType = dlg.DeviceSubType;
                GroupID = dlg.GroupID;
                this.Device_H = dlg.Device_H;
                this.Device_W = dlg.Device_W;

                if (dlg.Entryvalues == "Left")
                    this.Entry = EDirection.Left;
                else if (dlg.Entryvalues == "Right")
                    this.Entry = EDirection.Right;
                else if (dlg.Entryvalues == "Up")
                    this.Entry = EDirection.Up;
                else
                    this.Entry = EDirection.Down;

                if (tagIDBase == 0)
                    flag = objIdInc++;
                if (drawArea.AddText(this))
                {
                    tagIDBase = flag;

                    this.SetRectangle(x_axis, y_axis, width, height);
                    drawArea.SetDirty();
                    drawArea.Refresh();
                }
            }
        }

        public int ObjectID
        {
            get { return objectID; }
            set { objectID = value; }
        }

        public Rectangle RectangleLs
        {
            get { return Rectangle; }
            set { Rectangle = value; }
        }

        public HVDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public EDirection Entry
        {
            get { return entry; }
            set { entry = value; }
        }

        public int Flag
        {
            get { return flag; }
            set { flag = value; }
        }

        public bool ShowProperty
        {
            get { return showProperty; }
            set { showProperty = value; }
        }

        public override void ShowItemProperty(bool IsShow)
        {
            this.showProperty = IsShow;
        }

        public override Rectangle GetRectangle()
        {
            return Rectangle;
        }

        public override void AntiClockWiseDirection()
        {
            base.AntiClockWiseDirection();
            if (this.direction == HVDirection.Horizontal)
                direction = HVDirection.Vertical;
            else if (direction == HVDirection.Vertical)
                direction = HVDirection.Horizontal;
        }

        public override void ClockWiseDirection()
        {
            base.ClockWiseDirection();
            if (this.direction == HVDirection.Horizontal)
                direction = HVDirection.Vertical;
            else if (direction == HVDirection.Vertical)
                direction = HVDirection.Horizontal;
        }

        public override void MoveHandleTo(Point point, int handleNumber)
        {
            base.MoveHandleTo(point, handleNumber);

            if (this.Rectangle.Width > this.Rectangle.Height)
                Direction = HVDirection.Horizontal;
            else
                Direction = HVDirection.Vertical;
        }
    }
}