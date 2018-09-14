/**********************************************************
** 文件名： UndoManager.cs
** 文件作用:动态加载云服务接口动态库
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2018-09-14    xwj       增加
**
**********************************************************/

using System.Collections.Generic;

namespace DrawTools.DocToolkit
{
    public class UndoManager
    {
        #region Class Members

        private List<DrawTools.Command.Command> historyList;

        private int nextUndo;

        #endregion Class Members

        #region Constructor

        public UndoManager(GraphicsList graphicsList)
        {
            ClearHistory();
        }

        #endregion Constructor

        #region Properties

        public bool CanUndo
        {
            get
            {
                if (nextUndo < 0 ||
                    nextUndo > historyList.Count - 1)   // precaution
                {
                    return false;
                }

                return true;
            }
        }

        public bool CanRedo
        {
            get
            {
                if (nextUndo == historyList.Count - 1)
                {
                    return false;
                }

                return true;
            }
        }

        #endregion Properties

        #region Public Functions

        public void ClearHistory()
        {
            historyList = new List<DrawTools.Command.Command>();
            nextUndo = -1;
        }

        public void AddCommandToHistory(DrawTools.Command.Command command)
        {
            this.TrimHistoryList();

            historyList.Add(command);

            nextUndo++;
        }

        public void Undo()
        {
            if (!CanUndo)
            {
                return;
            }
            DrawTools.Command.Command command = historyList[nextUndo];

            command.Undo();

            nextUndo--;
        }

        public void Redo()
        {
            if (!CanRedo)
            {
                return;
            }

            int itemToRedo = nextUndo + 1;
            DrawTools.Command.Command command = historyList[itemToRedo];

            command.Redo();

            nextUndo++;
        }

        #endregion Public Functions

        #region Private Functions

        private void TrimHistoryList()
        {
            if (historyList.Count == 0)
            {
                return;
            }

            if (nextUndo == historyList.Count - 1)
            {
                return;
            }

            for (int i = historyList.Count - 1; i > nextUndo; i--)
            {
                historyList.RemoveAt(i);
            }
        }

        #endregion Private Functions
    }
}