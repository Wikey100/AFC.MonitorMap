/**********************************************************
** 文件名： BOM.cs
** 文件作用:BOM设备
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
    public class BOM : DrawRectangle
    {
        private int objectID;
        private int flag;
        private int x_axis;
        private int y_axis;
        private int width;
        private int height;
        private bool showProperty;

        public BOM()
            : this(0, 0, 1, 1)
        {
        }

        public BOM(int x, int y, int width, int height)
            : base()
        {
            Rectangle = new Rectangle(x, y, width, height);
            logicIDTail = "";
            objectID = objIdInc++;
            tagIDBase = 0;
            showProperty = false;
            flag = objIdInc++;
            Initialize();
        }

        public override DrawObject Clone()
        {
            BOM drawBOM = new BOM();
            drawBOM.Rectangle = this.Rectangle;
            drawBOM.tagIDBase = this.tagIDBase;
            drawBOM.logicIDTail = logicIDTail;
            drawBOM.DeviceIP = this.DeviceIP;
            FillDrawObjectFields(drawBOM);
            return drawBOM;
        }

        public override DrawObject Clone(int n)
        {
            BOM drawBOM = new BOM();
            drawBOM.Rectangle = this.Rectangle;
            drawBOM.tagIDBase = this.tagIDBase;
            drawBOM.DeviceIP = this.DeviceIP;
            drawBOM.logicIDTail = LogicIDAdd(logicIDTail, n);

            FillDrawObjectFields(drawBOM);
            return drawBOM;
        }

        public override void Draw(Graphics g)
        {
            try
            {
                Rectangle rect_down = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
                Image img = new Bitmap(Resource.BOM);
                g.DrawImage(img, rect_down);
                Brush brushin0 = new SolidBrush(Color.Red);
                StringFormat style = new StringFormat();
                style.Alignment = StringAlignment.Center;
                if (showProperty)
                {
                    g.DrawString(
                         logicIDTail,
                         new Font("宋体", 9, FontStyle.Regular),
                         brushin0,
                         rect_down, style);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override void OnDoubleClick(Object sender, EventArgs e)
        {
            DrawArea drawArea = (DrawArea)sender;
            EditorDialog dlg = new EditorDialog(logicIDTail, this, "", drawArea.GraphicsList.BomVerify, DeviceTypeEnum.BOM, DeviceTypeEnum.BOM, drawArea.GraphicsList.IPVerify);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (this.tagIDBase != 0)
                {
                    drawArea.GraphicsList.BomVerify.Remove(logicIDTail);
                }
                logicIDTail = dlg.IDvalues;

                this.x_axis = dlg.X_axis;
                this.y_axis = dlg.Y_axis;
                this.width = dlg.RWidth;
                this.height = dlg.RHeight;
                RecID = dlg.RecID;
                DeviceID = dlg.DeviceID;
                DeviceName = dlg.DeviceName;
                IpAdd = dlg.IpAdd;
                this.Device_H = dlg.Device_H;
                this.Device_W = dlg.Device_W;
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
    }
}