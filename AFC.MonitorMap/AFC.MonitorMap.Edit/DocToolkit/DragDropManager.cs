/**********************************************************
** 文件名： DragDropManager.cs
** 文件作用:
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2018-09-14    xwj       增加
**
**********************************************************/

#region Using directives

using System;
using System.Windows.Forms;

#endregion Using directives

namespace DrawTools.DocToolkit
{
    public class DragDropManager
    {
        private Form frmOwner;

        public event FileDroppedEventHandler FileDroppedEvent;

        public DragDropManager(Form owner)
        {
            frmOwner = owner;

            frmOwner.AllowDrop = true;

            frmOwner.DragEnter += OnDragEnter;
            frmOwner.DragDrop += OnDragDrop;
        }

        private void OnDragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void OnDragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            Array a = (Array)e.Data.GetData(DataFormats.FileDrop);

            if (a != null)
            {
                if (FileDroppedEvent != null)
                {
                    FileDroppedEvent.BeginInvoke(this, new FileDroppedEventArgs(a), null, null);

                    frmOwner.Activate();
                }
            }
        }
    }

    public delegate void FileDroppedEventHandler(object sender, FileDroppedEventArgs e);

    public class FileDroppedEventArgs : System.EventArgs
    {
        private Array fileArray;

        public FileDroppedEventArgs(Array array)
        {
            this.fileArray = array;
        }

        public Array FileArray
        {
            get
            {
                return fileArray;
            }
        }
    }
}