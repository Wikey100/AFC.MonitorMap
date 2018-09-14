/**********************************************************
** 文件名： ToolEllipse.cs
** 文件作用:Ellipse Tool
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2018-09-14    xwj       增加
**
**********************************************************/

using DrawTools.DocToolkit;
using System.Windows.Forms;

namespace DrawTools.DeviceTools
{
    public class ToolEllipse : ToolRectangle
    {
        public ToolEllipse()
        {
            Cursor = new Cursor(GetType(), "Ellipse.cur");
        }

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new DrawEllipse(e.X, e.Y, 1, 1));
        }
    }
}