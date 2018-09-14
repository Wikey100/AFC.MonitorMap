/**********************************************************
** 文件名： ToolPointer.cs
** 文件作用:Pointer Tool
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2018-09-14    xwj       增加
**
**********************************************************/

using DrawTools.Command;
using DrawTools.DocToolkit;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DrawTools.DeviceTools
{
    public class ToolPointer : Tool
    {
        private enum SelectionMode
        {
            None,
            NetSelection,
            Move,
            Size
        }

        private SelectionMode selectMode = SelectionMode.None;

        private DrawObject resizedObject;

        private int resizedObjectHandle;

        private Point lastPoint = new Point(0, 0);

        private Point startPoint = new Point(0, 0);

        private CommandChangeState commandChangeState;
        private bool wasMove;

        public ToolPointer()
        {
        }

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            commandChangeState = null;
            wasMove = false;

            selectMode = SelectionMode.None;
            Point point = new Point(e.X, e.Y);

            foreach (DrawObject o in drawArea.GraphicsList.Selection)
            {
                int handleNumber = o.HitTest(point);

                if (handleNumber > 0)
                {
                    selectMode = SelectionMode.Size;
                    resizedObject = o;
                    resizedObjectHandle = handleNumber;
                    drawArea.GraphicsList.UnselectAll();
                    o.Selected = true;
                    commandChangeState = new CommandChangeState(drawArea.GraphicsList);
                    break;
                }
            }

            if (selectMode == SelectionMode.None)
            {
                int n1 = drawArea.GraphicsList.Count;
                DrawObject o = null;

                for (int i = 0; i < n1; i++)
                {
                    if (drawArea.GraphicsList[i].HitTest(point) == 0)
                    {
                        o = drawArea.GraphicsList[i];
                        break;
                    }
                }

                if (o != null)
                {
                    selectMode = SelectionMode.Move;

                    if ((Control.ModifierKeys & Keys.Control) == 0 && !o.Selected)
                        drawArea.GraphicsList.UnselectAll();

                    o.Selected = true;

                    commandChangeState = new CommandChangeState(drawArea.GraphicsList);

                    drawArea.Cursor = Cursors.SizeAll;
                }
            }

            if (selectMode == SelectionMode.None)
            {
                if ((Control.ModifierKeys & Keys.Control) == 0)
                    drawArea.GraphicsList.UnselectAll();

                selectMode = SelectionMode.NetSelection;
            }

            lastPoint.X = e.X;
            lastPoint.Y = e.Y;
            startPoint.X = e.X;
            startPoint.Y = e.Y;

            drawArea.Capture = true;

            drawArea.Refresh();

            if (selectMode == SelectionMode.NetSelection)
            {
                ControlPaint.DrawReversibleFrame(
                    drawArea.RectangleToScreen(DrawRectangle.GetNormalizedRectangle(startPoint, lastPoint)),
                    Color.Black,
                    FrameStyle.Dashed);
            }
        }

        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            Point point = new Point(e.X, e.Y);
            Point oldPoint = lastPoint;
            wasMove = true;

            if (e.Button == MouseButtons.None)
            {
                Cursor cursor = null;

                for (int i = 0; i < drawArea.GraphicsList.Count; i++)
                {
                    int n = drawArea.GraphicsList[i].HitTest(point);

                    if (n > 0)
                    {
                        cursor = drawArea.GraphicsList[i].GetHandleCursor(n);
                        break;
                    }
                }

                if (cursor == null)
                    cursor = Cursors.Default;

                drawArea.Cursor = cursor;

                return;
            }

            if (e.Button != MouseButtons.Left)
                return;

            int dx = e.X - lastPoint.X;
            int dy = e.Y - lastPoint.Y;

            lastPoint.X = e.X;
            lastPoint.Y = e.Y;

            if (selectMode == SelectionMode.Size)
            {
                if (resizedObject != null)
                {
                    resizedObject.MoveHandleTo(point, resizedObjectHandle);
                    drawArea.SetDirty();
                    drawArea.Refresh();
                }
            }

            if (selectMode == SelectionMode.Move)
            {
                List<DrawObject> selectList = new List<DrawObject>();
                int n = drawArea.GraphicsList.Count;
                for (int i = 0; i < n; i++)
                {
                    if (drawArea.GraphicsList[i].Selected)
                    {
                        drawArea.GraphicsList.SelectAlist(drawArea.GraphicsList[i].TagIDBase);
                        selectList.Add(drawArea.GraphicsList[i]);
                    }
                }
                foreach (DrawObject o in selectList)
                {
                    o.Move(dx, dy);
                }

                drawArea.Cursor = Cursors.SizeAll;
                drawArea.SetDirty();
                drawArea.Refresh();
            }

            if (selectMode == SelectionMode.NetSelection)
            {
                ControlPaint.DrawReversibleFrame(
                    drawArea.RectangleToScreen(DrawRectangle.GetNormalizedRectangle(startPoint, oldPoint)),
                    Color.Black,
                    FrameStyle.Dashed);

                ControlPaint.DrawReversibleFrame(
                    drawArea.RectangleToScreen(DrawRectangle.GetNormalizedRectangle(startPoint, point)),
                    Color.Black,
                    FrameStyle.Dashed);

                return;
            }
        }

        public override void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {
            if (selectMode == SelectionMode.NetSelection)
            {
                ControlPaint.DrawReversibleFrame(
                    drawArea.RectangleToScreen(DrawRectangle.GetNormalizedRectangle(startPoint, lastPoint)),
                    Color.Black,
                    FrameStyle.Dashed);

                drawArea.GraphicsList.SelectInRectangle(
                    DrawRectangle.GetNormalizedRectangle(startPoint, lastPoint));

                selectMode = SelectionMode.None;
            }

            if (resizedObject != null)
            {
                resizedObject.Normalize();
                resizedObject = null;
            }

            drawArea.Capture = false;
            drawArea.Refresh();

            if (commandChangeState != null && wasMove)
            {
                commandChangeState.NewState(drawArea.GraphicsList);
                drawArea.AddCommandToHistory(commandChangeState);
                commandChangeState = null;
            }
        }
    }
}