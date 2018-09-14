/**********************************************************
** 文件名： DocManager.cs
** 文件作用:
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2018-09-14    xwj       增加
**
**********************************************************/

#region Using directives

using DrawTools.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Security;
using System.Windows.Forms;

#endregion Using directives

namespace DrawTools.DocToolkit
{
    #region Class DocManager

    public class DocManager
    {
        #region Events

        public event SaveEventHandler SaveEvent;

        public event LoadEventHandler LoadEvent;

        public event OpenFileEventHandler OpenEvent;

        public event EventHandler ClearEvent;

        public event EventHandler DocChangedEvent;

        #endregion Events

        #region Members

        private string fileName = "";
        private bool dirty = false;
        private Form frmOwner;
        private string newDocName;
        private string fileDlgFilter;
        private string registryPath;
        private bool updateTitle;
        private const string registryValue = "Path";
        private string fileDlgInitDir = "";
        private int childFormNumber;
        private string filetitlename;

        #endregion Members

        #region Enum

        public enum SaveType
        {
            Save,
            SaveAs
        }

        #endregion Enum

        #region Constructor

        public DocManager(DocManagerData data)
        {
            frmOwner = data.FormOwner;
            frmOwner.Closing += OnClosing;

            updateTitle = data.UpdateTitle;

            newDocName = data.NewDocName;

            fileDlgFilter = data.FileDialogFilter;

            registryPath = data.RegistryPath;

            childFormNumber = data.num;

            filetitlename = newDocName + childFormNumber;

            if (!registryPath.EndsWith("\\"))
                registryPath += "\\";

            registryPath += "FileDir";

            RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath);

            if (key != null)
            {
                string s = (string)key.GetValue(registryValue);

                if (!Empty(s))
                    fileDlgInitDir = s;
            }
        }

        #endregion Constructor

        #region Public functions and Properties

        public bool Dirty
        {
            get
            {
                return dirty;
            }
            set
            {
                dirty = value;
                SetCaption();
            }
        }

        public bool NewDocument()
        {
            SetFileName("");
            Dirty = false;
            return true;
        }

        public bool CloseDocument()
        {
            if (!this.dirty)
                return true;

            DialogResult res = MessageBox.Show(
                frmOwner,
                "Save changes " + filetitlename + " ?",
                Application.ProductName,
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Exclamation);

            switch (res)
            {
                case DialogResult.Yes: return SaveDocument(SaveType.Save);
                case DialogResult.No: return true;
                case DialogResult.Cancel: return false;
                default: Debug.Assert(false); return false;
            }
        }

        public bool OpenDocument(List<StationMapModel> modelList, string mapType)
        {
            Control drawdontrol = frmOwner.Controls[0];
            if (!((DrawControl)drawdontrol).OpenData(modelList, mapType)) return false;
            return true;
        }

        public bool SaveDocument(SaveType type)
        {
            string newFileName = this.fileName;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = fileDlgFilter;

            if ((type == SaveType.SaveAs) ||
                Empty(newFileName))
            {
                if (!Empty(newFileName))
                {
                    saveFileDialog1.InitialDirectory = Path.GetDirectoryName(newFileName);
                    saveFileDialog1.FileName = Path.GetFileName(newFileName);
                }
                else
                {
                    saveFileDialog1.InitialDirectory = fileDlgInitDir;
                    saveFileDialog1.FileName = filetitlename + ".xml";
                }

                DialogResult res = saveFileDialog1.ShowDialog(frmOwner);

                if (res != DialogResult.OK)
                    return false;

                newFileName = saveFileDialog1.FileName;
                fileDlgInitDir = new FileInfo(newFileName).DirectoryName;
            }

            try
            {
                Control drawdontrol = frmOwner.Controls[0];
                ((DrawControl)drawdontrol).SaveXML(newFileName);
            }
            catch (Exception ex) { return HandleSaveException(ex, newFileName); }
            Dirty = false;
            SetFileName(newFileName);
            filetitlename = newFileName;

            if (OpenEvent != null)
            {
                OpenEvent(this, new OpenFileEventArgs(newFileName, true));
            }
            return true;
        }

        /// <summary>
        /// 以数据库方式保存
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool SaveDocumentData(string mapType)
        {
            try
            {
                Control drawdontrol = frmOwner.Controls[0];
                ((DrawControl)drawdontrol).SaveData(mapType);
            }
            catch (Exception ex) { }
            return true;
        }

        public int CheckSCDevieVerty()
        {
            try
            {
                Control drawdontrol = frmOwner.Controls[0];
                return ((DrawControl)drawdontrol).CheckSCDeviceVertyList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool RegisterFileType(
            string fileExtension,
            string progId,
            string typeDisplayName)
        {
            try
            {
                string s = String.Format(CultureInfo.InvariantCulture, ".{0}", fileExtension);

                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(s))
                {
                    key.SetValue(null, progId);
                }

                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(progId))
                {
                    key.SetValue(null, typeDisplayName);
                }

                using (RegistryKey key =
                           Registry.ClassesRoot.CreateSubKey(progId + @"\DefaultIcon"))
                {
                    key.SetValue(null, Application.ExecutablePath + ",0");
                }

                string cmdkey = progId + @"\shell\open\command";
                using (RegistryKey key =
                           Registry.ClassesRoot.CreateSubKey(cmdkey))
                {
                    key.SetValue(null, Application.ExecutablePath + " \"%1\"");
                }

                string appkey = "Applications\\" +
                    new FileInfo(Application.ExecutablePath).Name +
                    "\\shell";
                using (RegistryKey key =
                           Registry.ClassesRoot.CreateSubKey(appkey))
                {
                    key.SetValue("FriendlyCache", Application.ProductName);
                }
            }
            catch (ArgumentNullException ex)
            {
                return HandleRegistryException(ex);
            }
            catch (SecurityException ex)
            {
                return HandleRegistryException(ex);
            }
            catch (ArgumentException ex)
            {
                return HandleRegistryException(ex);
            }
            catch (ObjectDisposedException ex)
            {
                return HandleRegistryException(ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                return HandleRegistryException(ex);
            }

            return true;
        }

        #endregion Public functions and Properties

        #region Other Functions

        private bool HandleRegistryException(Exception ex)
        {
            Trace.WriteLine("Registry operation failed: " + ex.Message);
            return false;
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(registryPath);
            key.SetValue(registryValue, fileDlgInitDir);
        }

        private void SetFileName(string fileName)
        {
            this.fileName = fileName;
            SetCaption();
        }

        private void SetCaption()
        {
            if (!updateTitle)
                return;

            frmOwner.Text = string.Format(
                CultureInfo.InvariantCulture,
                "{0} - {1}{2}",
                Application.ProductName,
                Empty(this.fileName) ? filetitlename : this.fileName,
                this.dirty ? "*" : "");
        }

        private bool HandleOpenException(Exception ex, string fileName)
        {
            MessageBox.Show(frmOwner,
                "Open File operation failed. File name: " + fileName + "\n" +
                "Reason: " + ex.Message,
                Application.ProductName);

            if (OpenEvent != null)
            {
                OpenEvent(this, new OpenFileEventArgs(fileName, false));
            }

            return false;
        }

        private bool HandleSaveException(Exception ex, string fileName)
        {
            MessageBox.Show(frmOwner,
                "Save File operation failed. File name: " + fileName + "\n" +
                "Reason: " + ex.Message,
                Application.ProductName);

            return false;
        }

        private static bool Empty(string s)
        {
            return s == null || s.Length == 0;
        }

        #endregion Other Functions
    }

    #endregion Class DocManager

    #region Delegates

    public delegate void SaveEventHandler(object sender, SerializationEventArgs e);

    public delegate void LoadEventHandler(object sender, SerializationEventArgs e);

    public delegate void OpenFileEventHandler(object sender, OpenFileEventArgs e);

    #endregion Delegates

    #region Class SerializationEventArgs

    public class SerializationEventArgs : System.EventArgs
    {
        private IFormatter formatter;
        private Stream stream;
        private string fileName;
        private bool errorFlag;

        public SerializationEventArgs(IFormatter formatter, Stream stream,
            string fileName)
        {
            this.formatter = formatter;
            this.stream = stream;
            this.fileName = fileName;
            errorFlag = false;
        }

        public bool Error
        {
            get
            {
                return errorFlag;
            }
            set
            {
                errorFlag = value;
            }
        }

        public IFormatter Formatter
        {
            get
            {
                return formatter;
            }
        }

        public Stream SerializationStream
        {
            get
            {
                return stream;
            }
        }

        public string FileName
        {
            get
            {
                return fileName;
            }
        }
    }

    #endregion Class SerializationEventArgs

    #region Class OpenFileEventArgs

    public class OpenFileEventArgs : System.EventArgs
    {
        private string fileName;
        private bool success;

        public OpenFileEventArgs(string fileName, bool success)
        {
            this.fileName = fileName;
            this.success = success;
        }

        public string FileName
        {
            get
            {
                return fileName;
            }
        }

        public bool Succeeded
        {
            get
            {
                return success;
            }
        }
    }

    #endregion Class OpenFileEventArgs

    #region class DocManagerData

    public class DocManagerData
    {
        public DocManagerData()
        {
            frmOwner = null;
            updateTitle = true;
            newDocName = "Untitled";
            fileDlgFilter = "All Files (*.*)|*.*";
            registryPath = "Software\\Unknown";
            num = 1;
        }

        private Form frmOwner;
        private bool updateTitle;
        private string newDocName;
        private string fileDlgFilter;
        private string registryPath;
        public int num;

        public Form FormOwner
        {
            get
            {
                return frmOwner;
            }
            set
            {
                frmOwner = value;
            }
        }

        public bool UpdateTitle
        {
            get
            {
                return updateTitle;
            }
            set
            {
                updateTitle = value;
            }
        }

        public string NewDocName
        {
            get
            {
                return newDocName;
            }
            set
            {
                newDocName = value;
            }
        }

        public string FileDialogFilter
        {
            get
            {
                return fileDlgFilter;
            }
            set
            {
                fileDlgFilter = value;
            }
        }

        public string RegistryPath
        {
            get
            {
                return registryPath;
            }
            set
            {
                registryPath = value;
            }
        }
    };

    #endregion class DocManagerData
}