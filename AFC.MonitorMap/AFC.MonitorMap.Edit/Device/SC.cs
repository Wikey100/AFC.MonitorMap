/**********************************************************
** 文件名： SC.cs
** 文件作用:SC设备
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2018-09-14    xwj       增加
**
**********************************************************/

using AFC.MonitorMap.Edit.Properties;
using DrawTools.DocToolkit;
using DrawTools.Model;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DrawTools.Device
{
    public class SC : DrawRectangle
    {
        private int objectID;
        private int flag;
        private int x_axis;
        private int y_axis;
        private int width;
        private int height;

        public SC() : this(0, 0, 1, 1)
        {
        }

        public SC(int x, int y, int width, int height)
            : base()
        {
            Rectangle = new Rectangle(x, y, width, height);
            logicIDTail = "";
            objectID = objIdInc++;
            Initialize();
        }

        public override DrawObject Clone()
        {
            SC drawSC = new SC();
            objIdInc--;
            drawSC.Rectangle = this.Rectangle;

            drawSC.tagIDBase = this.tagIDBase;
            drawSC.logicIDTail = logicIDTail;
            drawSC.DeviceIP = this.DeviceIP;
            FillDrawObjectFields(drawSC);
            return drawSC;
        }

        public override DrawObject Clone(int n)
        {
            SC drawSC = new SC();
            drawSC.Rectangle = this.Rectangle;
            drawSC.tagIDBase = this.tagIDBase;
            drawSC.DeviceIP = this.DeviceIP;
            drawSC.logicIDTail = LogicIDAdd(logicIDTail, n);
            FillDrawObjectFields(drawSC);
            return drawSC;
        }

        public override void Draw(Graphics g)
        {
            Rectangle rect_down = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            Image img = new Bitmap(Resource.Server);
            g.DrawImage(img, rect_down);
        }

        public override void OnDoubleClick(Object sender, EventArgs e)
        {
            DrawArea drawArea = (DrawArea)sender;
            EditorDialog dlg = new EditorDialog(logicIDTail, this, "", drawArea.GraphicsList.SCVerify, DeviceTypeEnum.SC, DeviceTypeEnum.SC, drawArea.GraphicsList.IPVerify);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (this.tagIDBase != 0)
                {
                    drawArea.GraphicsList.SCVerify.Remove(logicIDTail);
                }
                logicIDTail = dlg.IDvalues;

                this.x_axis = dlg.X_axis;
                this.y_axis = dlg.Y_axis;
                this.width = dlg.RWidth;
                this.height = dlg.RHeight;
                StationID = dlg.StationID;
                DeviceID = dlg.DeviceID;
                DeviceName = dlg.DeviceName;
                IpAdd = dlg.IpAdd;
                DeviceType = dlg.DeviceType;
                DeviceSubType = dlg.DeviceSubType;
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

        public override void AntiClockWiseDirection()
        { }

        public override void ClockWiseDirection()
        { }

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

        public int Flag
        {
            get { return flag; }
            set { flag = value; }
        }

        public override Rectangle GetRectangle()
        {
            return Rectangle;
        }
    }
}