/**********************************************************
** 文件名： Switch.cs
** 文件作用:Switch设备
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2018-09-14    xwj       增加
**
**********************************************************/

using DrawTools.DocToolkit;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DrawTools.Device
{
    public class Switch : DrawRectangle
    {
        private string switchID;
        private bool showProperty;
        private HVDirection direction;

        public Switch()
            : this(0, 0, 1, 1)
        {
        }

        public Switch(int x, int y, int width, int height)
            : base()
        {
            Rectangle = new Rectangle(x, y, width, height);
            int id = objIdInc++;
            switchID = string.Format("SW{0}", id.ToString("D2"));
            direction = HVDirection.Horizontal;
            Initialize();
        }

        public override DrawObject Clone()
        {
            Switch drawSwitch = new Switch();
            objIdInc--;
            drawSwitch.Rectangle = this.Rectangle;

            FillDrawObjectFields(drawSwitch);
            return drawSwitch;
        }

        public override DrawObject Clone(int n)
        {
            Switch drawSwitch = new Switch();
            drawSwitch.Rectangle = this.Rectangle;

            FillDrawObjectFields(drawSwitch);
            return drawSwitch;
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
            Rectangle zrect = new Rectangle(frect.X, frect.Y, frect.Width, frect.Height);
            Brush brushin0 = new SolidBrush(Color.Black);
            StringFormat style = new StringFormat();
            style.Alignment = StringAlignment.Near;
            if (true)
            {
                g.DrawString(
                     string.Format("交换机:{0}", SwitchID),
                     new Font("宋体", 10, FontStyle.Regular),
                     brushin0,
                     zrect, style);
            }

            pen.Dispose();
        }

        public override void OnDoubleClick(Object sender, EventArgs e)
        {
            DrawArea drawArea = (DrawArea)sender;
            SwitchEditDialog dlg = new SwitchEditDialog(switchID, this);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                SwitchID = dlg.MonitorID;

                if (drawArea.AddText(this))
                {
                    drawArea.SetDirty();
                }
            }
        }

        public override void AntiClockWiseDirection()
        { }

        public override void ClockWiseDirection()
        { }

        public string SwitchID
        {
            get { return switchID; }
            set { switchID = value; }
        }

        public HVDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public bool ShowProperty
        {
            get { return showProperty; }
            set { showProperty = value; }
        }

        public Rectangle RectangleLs
        {
            get { return Rectangle; }
            set { Rectangle = value; }
        }

        public override void ShowItemProperty(bool IsShow)
        {
            this.showProperty = IsShow;
        }

        public override Rectangle GetRectangle()
        {
            return Rectangle;
        }
    }
}