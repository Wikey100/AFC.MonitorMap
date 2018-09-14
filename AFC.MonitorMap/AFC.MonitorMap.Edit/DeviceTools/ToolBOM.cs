/**********************************************************
** 文件名： ToolBOM.cs
** 文件作用:BOM  Tool
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2018-09-14    xwj       增加
**
**********************************************************/

using DrawTools.Device;
using System.Windows.Forms;

namespace DrawTools.DeviceTools
{
    public class ToolBOM : ToolRectangle
    {
        public ToolBOM()
        {
            Cursor = new Cursor(GetType(), "Ellipse.cur");
        }

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new BOM(e.X, e.Y, 38, 43));
        }
    }
}