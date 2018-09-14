/**********************************************************
** 文件名： ToolAGMChannel.cs
** 文件作用:AGMChannel tool
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
    public class ToolAGMChannel : ToolRectangle
    {
        public ToolAGMChannel()
        {
            Cursor = new Cursor(GetType(), "Ellipse.cur");
        }

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new AGMChannel(e.X, e.Y, 71, 27));
        }
    }
}