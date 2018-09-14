/**********************************************************
** 文件名： TVM.cs
** 文件作用:TVM设备
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
    public class TVM : DrawRectangle
    {
        private int objectID;
        private int flag;
        private int x_axis;
        private int y_axis;
        private int width;
        private int height;

        private bool showProperty;
        private string arrayId;

        public string ArrayId
        {
            get { return arrayId; }
            set { arrayId = value; }
        }

        public TVM()
            : this(0, 0, 1, 1)
        {
        }

        public TVM(int x, int y, int width, int height)
            : base()
        {
            Rectangle = new Rectangle(x, y, width, height);
            logicIDTail = "";
            objectID = objIdInc++;
            TagIDBase = 0;
            showProperty = false;
            flag = objIdInc++;
            Initialize();
        }

        public override DrawObject Clone()
        {
            TVM drawTVM = new TVM();
            drawTVM.Rectangle = this.Rectangle;
            drawTVM.TagIDBase = TagIDBase;
            drawTVM.logicIDTail = this.logicIDTail;
            drawTVM.DeviceIP = this.DeviceIP;
            FillDrawObjectFields(drawTVM);
            return drawTVM;
        }

        public override DrawObject Clone(int n)
        {
            TVM drawTVM = new TVM();
            drawTVM.Rectangle = this.Rectangle;
            drawTVM.TagIDBase = TagIDBase;
            drawTVM.DeviceIP = this.DeviceIP;
            drawTVM.logicIDTail = LogicIDAdd(logicIDTail, n);
            FillDrawObjectFields(drawTVM);
            return drawTVM;
        }

        public override void Draw(Graphics g)
        {
            Rectangle rectdown = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            Image img = new Bitmap(Resource.TVM);
            g.DrawImage(img, rectdown);
            Rectangle frect = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            Brush brushin0 = new SolidBrush(Color.Red);
            StringFormat style = new StringFormat();
            style.Alignment = StringAlignment.Center;
            if (showProperty)
            {
                g.DrawString(
                     logicIDTail,
                     new Font("宋体", 9, FontStyle.Regular),
                     brushin0,
                     frect, style);
            }
        }

        public override void OnDoubleClick(Object sender, EventArgs e)
        {
            DrawArea drawArea = (DrawArea)sender;
            EditorDialog dlg = new EditorDialog(logicIDTail, this, "", drawArea.GraphicsList.TVMVerify, DeviceTypeEnum.TVM, DeviceTypeEnum.TVM, drawArea.GraphicsList.IPVerify);
            dlg.ArrayId = arrayId;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (this.TagIDBase != 0)
                {
                    drawArea.GraphicsList.TVMVerify.Remove(logicIDTail);
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
                if (TagIDBase == 0)
                    flag = objIdInc++;
                if (drawArea.AddText(this))
                {
                    TagIDBase = flag;//与颜色变化有关

                    this.SetRectangle(x_axis, y_axis, width, height);
                    drawArea.SetDirty();
                    drawArea.SetDirty();
                }
            }
        }

        public override void AntiClockWiseDirection()
        { }

        public override void ClockWiseDirection()
        { }

        public new int TagIDBase
        {
            get { return tagIDBase; }
            set { tagIDBase = value; }
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