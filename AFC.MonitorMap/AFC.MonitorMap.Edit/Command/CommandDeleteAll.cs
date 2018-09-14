/**********************************************************
** 文件名： CommandDeleteAll.cs
** 文件作用:Delete All command
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2018-09-14    xwj       增加
**
**********************************************************/

using DrawTools.DocToolkit;
using System.Collections.Generic;

namespace DrawTools.Command
{
    public class CommandDeleteAll : Command
    {
        private List<DrawObject> cloneList;

        public CommandDeleteAll(GraphicsList graphiList)
            : base(graphiList)
        {
            cloneList = new List<DrawObject>();
            int n = graphicsList.Count;

            for (int i = n - 1; i >= 0; i--)
            {
                cloneList.Add(graphicsList[i].Clone());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Execute()
        {
            graphicsList.BomVerify.Clear();
            graphicsList.AGMVerify.Clear();
            graphicsList.TCMVerify.Clear();
            graphicsList.TVMVerify.Clear();
            graphicsList.SCVerify.Clear();
            DrawObject.objIdInc = 1;
            graphicsList.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Undo()
        {
            foreach (DrawObject o in cloneList)
            {
                graphicsList.Add(o);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Redo()
        {
            graphicsList.BomVerify.Clear();
            graphicsList.AGMVerify.Clear();
            graphicsList.TCMVerify.Clear();
            graphicsList.TVMVerify.Clear();
            DrawObject.objIdInc = 1;
            graphicsList.Clear();
        }
    }
}