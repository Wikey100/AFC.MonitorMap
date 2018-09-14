/**********************************************************
** 文件名： Command.cs
** 文件作用:操作命令基类
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2018-09-14    xwj       增加
**
**********************************************************/

namespace DrawTools.Command
{
    public abstract class Command
    {
        protected GraphicsList graphicsList;

        public Command(GraphicsList graphicsList)
        {
            this.graphicsList = graphicsList;
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// 
        /// </summary>
        public abstract void Undo();

        /// <summary>
        /// 
        /// </summary>
        public abstract void Redo();
    }
}