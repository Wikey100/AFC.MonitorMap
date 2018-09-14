/**********************************************************
** 文件名： PaidArea.cs
** 文件作用:PainArea设备
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
    public class PaidArea : DrawRectangle
    {
        private int objectID;

        public PaidArea() : this(0, 0, 1, 1)
        {
        }

        public PaidArea(int x, int y, int width, int height)
            : base()
        {
            Rectangle = new Rectangle(x, y, width, height);

            objectID = objIdInc++;

            Initialize();
        }

        public override DrawObject Clone()
        {
            PaidArea drawPaidArea = new PaidArea();
            objIdInc--;
            drawPaidArea.Rectangle = this.Rectangle;

            FillDrawObjectFields(drawPaidArea);
            return drawPaidArea;
        }

        public override void Draw(Graphics g)
        {
            Pen pen = new Pen(Color, PenWidth);
            Brush brushout = new SolidBrush(Color.FromArgb(255, 160, 190, 180));
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle frect = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            g.FillRectangle(brushout, frect);
            g.DrawRectangle(pen, Rectangle);
            pen.Dispose();
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

        public override void AntiClockWiseDirection()
        { }

        public override void ClockWiseDirection()
        { }

        public override Rectangle GetRectangle()
        {
            return Rectangle;
        }
    }
}