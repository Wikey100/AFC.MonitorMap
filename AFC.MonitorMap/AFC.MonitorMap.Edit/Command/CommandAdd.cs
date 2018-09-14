/**********************************************************
** 文件名： CommandAdd.cs
** 文件作用:添加命令
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2018-09-14    xwj       增加
**
**********************************************************/

using DrawTools.DocToolkit;
using System;

namespace DrawTools.Command
{
    public class CommandAdd : Command
    {
        private DrawObject drawObject;

        public CommandAdd(GraphicsList graphiList)
            : base(graphiList)
        {
            this.drawObject = (DrawObject)graphicsList[0].Clone();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Execute()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Undo()
        {
            graphicsList.DeleteLastAddedObject();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Redo()
        {
            graphicsList.UnselectAll();
            graphicsList.Add(drawObject);
        }
    }
}