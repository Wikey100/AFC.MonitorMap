/**********************************************************
** 文件名： Tool.cs
** 文件作用:Base class for all drawing tools
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2018-09-14    xwj       增加
**
**********************************************************/

using System.Windows.Forms;

namespace DrawTools.DeviceTools
{
    public abstract class Tool
    {
        public virtual void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
        }

        public virtual void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
        }

        public virtual void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {
        }
    }
}