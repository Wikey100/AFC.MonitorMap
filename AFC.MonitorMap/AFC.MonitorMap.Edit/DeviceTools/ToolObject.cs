/**********************************************************
** 文件名： ToolObject.cs
** 文件作用:ToolObject
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2018-09-14    xwj       增加
**
**********************************************************/

using DrawTools.Command;
using DrawTools.Controls;
using DrawTools.DocToolkit;
using System.Windows.Forms;

namespace DrawTools.DeviceTools
{
    public abstract class ToolObject : Tool
    {
        private Cursor cursor;

        protected Cursor Cursor
        {
            get
            {
                return cursor;
            }
            set
            {
                cursor = value;
            }
        }

        public override void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {
            try
            {
                drawArea.GraphicsList[0].Normalize();
                drawArea.AddCommandToHistory(new CommandAdd(drawArea.GraphicsList));
                drawArea.ActiveTool = DrawToolType.Pointer;

                drawArea.Capture = false;
                drawArea.Refresh();
            }
            catch { }
        }

        protected void AddNewObject(DrawArea drawArea, DrawObject o)
        {
            drawArea.GraphicsList.UnselectAll();

            o.Selected = true;
            drawArea.GraphicsList.Add(o);
            string oName = o.GetType().Name;
            if (oName == "Switch" || oName == "SwitchPort")
            {
                drawArea.mapType = "NetDataSet";
            }
            drawArea.Capture = true;
            drawArea.Refresh();

            drawArea.SetDirty();
        }
    }
}