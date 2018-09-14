/**********************************************************
** 文件名： GraphicsProperties.cs
** 文件作用:
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2018-09-14    xwj       增加
**
**********************************************************/

#region Using directives

using System.Drawing;

#endregion Using directives

namespace DrawTools.Model
{
    public class GraphicsProperties
    {
        private Color? color;
        private int? penWidth;

        public GraphicsProperties()
        {
            color = null;
            penWidth = null;
        }

        public Color? Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        public int? PenWidth
        {
            get
            {
                return penWidth;
            }
            set
            {
                penWidth = value;
            }
        }
    }
}