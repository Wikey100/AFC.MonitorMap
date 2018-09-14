/**********************************************************
** 文件名： PersistWindowState.cs
** 文件作用:
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2018-09-14    xwj       增加
**
**********************************************************/

#region Using directives

using Microsoft.Win32;
using System.Drawing;
using System.Windows.Forms;

#endregion Using directives

namespace DrawTools.DocToolkit
{
    public class PersistWindowState
    {
        #region Members

        private Form ownerForm;
        private string registryPath;
        private int normalLeft;
        private int normalTop;
        private int normalWidth;
        private int normalHeight;

        private FormWindowState windowState = FormWindowState.Normal;

        private bool allowSaveMinimized = false;

        #endregion Members

        #region Constructor

        public PersistWindowState(string path, Form owner)
        {
            if (path == null ||
                path.Length == 0)
            {
                registryPath = "Software\\Unknown";
            }
            else
            {
                registryPath = path;
            }

            if (!registryPath.EndsWith("\\"))
                registryPath += "\\";

            registryPath += "MainForm";

            ownerForm = owner;

            ownerForm.Closing += OnClosing;
            ownerForm.Resize += OnResize;
            ownerForm.Move += OnMove;
            ownerForm.Load += OnLoad;

            normalWidth = ownerForm.Width;
            normalHeight = ownerForm.Height;
        }

        #endregion Constructor

        #region Properties

        public bool AllowSaveMinimized
        {
            get
            {
                return allowSaveMinimized;
            }
            set
            {
                allowSaveMinimized = value;
            }
        }

        #endregion Properties

        #region Event Handlers

        private void OnResize(object sender, System.EventArgs e)
        {
            if (ownerForm.WindowState == FormWindowState.Normal)
            {
                normalWidth = ownerForm.Width;
                normalHeight = ownerForm.Height;
            }
        }

        private void OnMove(object sender, System.EventArgs e)
        {
            if (ownerForm.WindowState == FormWindowState.Normal)
            {
                normalLeft = ownerForm.Left;
                normalTop = ownerForm.Top;
            }

            windowState = ownerForm.WindowState;
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(registryPath);
            key.SetValue("Left", normalLeft);
            key.SetValue("Top", normalTop);
            key.SetValue("Width", normalWidth);
            key.SetValue("Height", normalHeight);

            if (!allowSaveMinimized)
            {
                if (windowState == FormWindowState.Minimized)
                    windowState = FormWindowState.Normal;
            }

            key.SetValue("WindowState", (int)windowState);
        }

        private void OnLoad(object sender, System.EventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath);
            if (key != null)
            {
                int left = (int)key.GetValue("Left", ownerForm.Left);
                int top = (int)key.GetValue("Top", ownerForm.Top);
                int width = (int)key.GetValue("Width", ownerForm.Width);
                int height = (int)key.GetValue("Height", ownerForm.Height);
                FormWindowState windowState = (FormWindowState)key.GetValue("WindowState", (int)ownerForm.WindowState);

                ownerForm.Location = new Point(left, top);
                ownerForm.Size = new Size(width, height);
                ownerForm.WindowState = windowState;
            }
        }

        #endregion Event Handlers
    }
}