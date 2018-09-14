/**********************************************************
** 文件名： CommandChangeState.cs
** 文件作用: 改变设备状态命令
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2018-09-14    xwj       增加
**
**********************************************************/

using DrawTools.DocToolkit;
using System;
using System.Collections.Generic;

namespace DrawTools.Command
{
    public class CommandChangeState : Command
    {
        private List<DrawObject> listBefore;

        private List<DrawObject> listAfter;

        public CommandChangeState(GraphicsList graphicsList)
            : base(graphicsList)
        {
            FillList(graphicsList, ref listBefore);
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
        /// <param name="graphicsList"></param>
        public void NewState(GraphicsList graphicsList)
        {
            FillList(graphicsList, ref listAfter);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Undo()
        {
            ReplaceObjects(graphicsList, listBefore);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Redo()
        {
            ReplaceObjects(graphicsList, listAfter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphicsList"></param>
        /// <param name="list"></param>
        private void ReplaceObjects(GraphicsList graphicsList, List<DrawObject> list)
        {
            for (int i = 0; i < graphicsList.Count; i++)
            {
                DrawObject replacement = null;

                foreach (DrawObject o in list)
                {
                    if (o.ID == graphicsList[i].ID)
                    {
                        replacement = o;
                        break;
                    }
                }

                if (replacement != null)
                {
                    graphicsList.Replace(i, replacement);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphicsList"></param>
        /// <param name="listToFill"></param>
        private void FillList(GraphicsList graphicsList, ref List<DrawObject> listToFill)
        {
            listToFill = new List<DrawObject>();

            foreach (DrawObject o in graphicsList.Selection)
            {
                listToFill.Add(o.Clone());
            }
        }
    }
}