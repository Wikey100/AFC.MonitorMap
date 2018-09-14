/**********************************************************
** 文件名： MruManager.cs
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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows.Forms;

#endregion Using directives

namespace DrawTools.DocToolkit
{
    using StringEnumerator = IEnumerator<String>;
    using StringList = List<String>;

    public class MruManager
    {
        #region Members

        public event MruFileOpenEventHandler MruOpenEvent;

        private Form ownerForm;

        private ToolStripMenuItem menuItemMRU;
        private ToolStripMenuItem menuItemParent;

        private string registryPath;

        private int maxNumberOfFiles = 10;

        private int maxDisplayLength = 40;

        private string currentDirectory;

        private StringList mruList;

        private const string regEntryName = "file";

        #endregion Members

        #region Windows API

        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        private static extern bool PathCompactPathEx(
            StringBuilder pszOut,
            string pszPath,
            int cchMax,
            int reserved);

        #endregion Windows API

        #region Constructor

        public MruManager()
        {
            mruList = new StringList();
        }

        #endregion Constructor

        #region Public Properties

        public int MaxDisplayNameLength
        {
            set
            {
                maxDisplayLength = value;

                if (maxDisplayLength < 10)
                    maxDisplayLength = 10;
            }

            get
            {
                return maxDisplayLength;
            }
        }

        public int MaxMruLength
        {
            set
            {
                maxNumberOfFiles = value;

                if (maxNumberOfFiles < 1)
                    maxNumberOfFiles = 1;

                if (mruList.Count > maxNumberOfFiles)
                    mruList.RemoveRange(maxNumberOfFiles - 1, mruList.Count - maxNumberOfFiles);
            }

            get
            {
                return maxNumberOfFiles;
            }
        }

        public string CurrentDir
        {
            set
            {
                currentDirectory = value;
            }

            get
            {
                return currentDirectory;
            }
        }

        #endregion Public Properties

        #region Public Functions

        public void Initialize(Form owner, ToolStripMenuItem mruItem, ToolStripMenuItem mruItemParent, string regPath)
        {
            ownerForm = owner;

            menuItemMRU = mruItem;

            menuItemParent = mruItemParent;

            registryPath = regPath;
            if (registryPath.EndsWith("\\"))
                registryPath += "MRU";
            else
                registryPath += "\\MRU";

            currentDirectory = Directory.GetCurrentDirectory();

            menuItemParent.DropDownOpening += new EventHandler(this.OnMRUParentPopup);

            ownerForm.Closing += OnOwnerClosing;

            LoadMRU();
        }

        public void Add(string file)
        {
            Remove(file);

            if (mruList.Count == maxNumberOfFiles)
                mruList.RemoveAt(maxNumberOfFiles - 1);

            mruList.Insert(0, file);
        }

        public void Remove(string file)
        {
            int i = 0;

            StringEnumerator myEnumerator = mruList.GetEnumerator();

            while (myEnumerator.MoveNext())
            {
                if ((string)myEnumerator.Current == file)
                {
                    mruList.RemoveAt(i);
                    return;
                }

                i++;
            }
        }

        #endregion Public Functions

        #region Event Handlers

        private void OnMRUParentPopup(object sender, EventArgs e)
        {
            if (mruList.Count == 0)
            {
                menuItemMRU.Enabled = false;
                return;
            }

            menuItemMRU.Enabled = true;

            ToolStripMenuItem item;

            StringEnumerator myEnumerator = mruList.GetEnumerator();
            int i = 0;

            while (myEnumerator.MoveNext())
            {
                item = new ToolStripMenuItem();
                item.Text = GetDisplayName((string)myEnumerator.Current);
                item.Tag = i++;

                item.Click += OnMRUClicked;

                menuItemMRU.DropDownItems.Add(item);
            }
        }

        private void OnMRUClicked(object sender, EventArgs e)
        {
            string s;

            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            if (item != null)
            {
                s = (string)mruList[(int)item.Tag];

                if (s.Length > 0)
                {
                    if (MruOpenEvent != null)
                    {
                        MruOpenEvent(this, new MruFileOpenEventArgs(s));
                    }
                }
            }
        }

        private void OnOwnerClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int i, n;

            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(registryPath);

                if (key != null)
                {
                    n = mruList.Count;

                    for (i = 0; i < maxNumberOfFiles; i++)
                    {
                        key.DeleteValue(regEntryName +
                            i.ToString(CultureInfo.InvariantCulture), false);
                    }

                    for (i = 0; i < n; i++)
                    {
                        key.SetValue(regEntryName +
                            i.ToString(CultureInfo.InvariantCulture), mruList[i]);
                    }
                }
            }
            catch (ArgumentNullException ex) { HandleWriteError(ex); }
            catch (SecurityException ex) { HandleWriteError(ex); }
            catch (ArgumentException ex) { HandleWriteError(ex); }
            catch (ObjectDisposedException ex) { HandleWriteError(ex); }
            catch (UnauthorizedAccessException ex) { HandleWriteError(ex); }
        }

        #endregion Event Handlers

        #region Private Functions

        private void LoadMRU()
        {
            string sKey, s;

            try
            {
                mruList.Clear();

                RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath);

                if (key != null)
                {
                    for (int i = 0; i < maxNumberOfFiles; i++)
                    {
                        sKey = regEntryName + i.ToString(CultureInfo.InvariantCulture);

                        s = (string)key.GetValue(sKey, "");

                        if (s.Length == 0)
                            break;

                        mruList.Add(s);
                    }
                }
            }
            catch (ArgumentNullException ex) { HandleReadError(ex); }
            catch (SecurityException ex) { HandleReadError(ex); }
            catch (ArgumentException ex) { HandleReadError(ex); }
            catch (ObjectDisposedException ex) { HandleReadError(ex); }
            catch (UnauthorizedAccessException ex) { HandleReadError(ex); }
        }

        private void HandleReadError(Exception ex)
        {
            Trace.WriteLine("Loading MRU from Registry failed: " + ex.Message);
        }

        private void HandleWriteError(Exception ex)
        {
            Trace.WriteLine("Saving MRU to Registry failed: " + ex.Message);
        }

        private string GetDisplayName(string fullName)
        {
            FileInfo fileInfo = new FileInfo(fullName);

            if (fileInfo.DirectoryName == currentDirectory)
                return GetShortDisplayName(fileInfo.Name, maxDisplayLength);

            return GetShortDisplayName(fullName, maxDisplayLength);
        }

        private string GetShortDisplayName(string longName, int maxLen)
        {
            StringBuilder pszOut = new StringBuilder(maxLen + maxLen + 2);  // for safety

            if (PathCompactPathEx(pszOut, longName, maxLen, 0))
            {
                return pszOut.ToString();
            }
            else
            {
                return longName;
            }
        }

        #endregion Private Functions
    }

    public delegate void MruFileOpenEventHandler(object sender, MruFileOpenEventArgs e);

    public class MruFileOpenEventArgs : System.EventArgs
    {
        private string fileName;

        public MruFileOpenEventArgs(string fileName)
        {
            this.fileName = fileName;
        }

        public string FileName
        {
            get
            {
                return fileName;
            }
        }
    }
}