/**********************************************************
** 文件名： AGMWallDummy.cs
** 文件作用:无检票闸机门
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2018-09-14    xwj       增加
**
**********************************************************/

using DrawTools.DocToolkit;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DrawTools.Device
{
    public class AGMWallDummy : DrawRectangle
    {
        private int objectID;
        private int flag;
        private int x_axis;
        private int y_axis;
        private int width;
        private int height;
        private HVDirection direction;

        public int Flag
        {
            get { return flag; }
            set { flag = value; }
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

        public override Rectangle GetRectangle()
        {
            return Rectangle;
        }

        public AGMWallDummy()
            : this(0, 0, 1, 1)
        {
        }

        public AGMWallDummy(int x, int y, int width, int height)
            : base()
        {
            Rectangle = new Rectangle(x, y, width, height);
            logicIDTail = "";
            objectID = objIdInc++;
            direction = HVDirection.Horizontal;

            Initialize();
        }

        public override DrawObject Clone()
        {
            AGMWallDummy drawAGMWallDummy = new AGMWallDummy();
            objIdInc--;
            drawAGMWallDummy.Rectangle = this.Rectangle;
            drawAGMWallDummy.tagIDBase = this.tagIDBase;
            drawAGMWallDummy.logicIDTail = logicIDTail;
            drawAGMWallDummy.Direction = this.Direction;
            FillDrawObjectFields(drawAGMWallDummy);
            return drawAGMWallDummy;
        }

        public override DrawObject Clone(int n)
        {
            AGMWallDummy drawAGMWallDummy = new AGMWallDummy();
            drawAGMWallDummy.Rectangle = this.Rectangle;
            drawAGMWallDummy.tagIDBase = this.tagIDBase;
            drawAGMWallDummy.logicIDTail = LogicIDAdd(logicIDTail, n);
            FillDrawObjectFields(drawAGMWallDummy);
            return drawAGMWallDummy;
        }

        public override void Draw(Graphics g)
        {
            Pen pen = new Pen(Color, PenWidth);
            Brush brushout = new SolidBrush(Color.FromArgb(255, 150, 150, 150));
            Brush brushin = new SolidBrush(Color.White);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle frect = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            g.DrawRectangle(pen, Rectangle);
            g.FillRectangle(brushout, frect);
            pen.Dispose();
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
    }
}