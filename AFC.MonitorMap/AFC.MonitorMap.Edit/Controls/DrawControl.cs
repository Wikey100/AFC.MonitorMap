/**********************************************************
** 文件名： DrawControl.cs
** 文件作用:画板基类
** 
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2018-09-14    xwj       增加
**
**********************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DrawTools.Model;

namespace DrawTools
{
    public partial class DrawControl : UserControl
    {

        public virtual void SaveXML(string xmlfile)
        { }

        public virtual bool OpenXML(string xmlfile)
        {
            return false;
        }

        public virtual bool OpenData(List<StationMapModel> modelList,string mapType)
        {
            return false;
        }

        public virtual bool SaveData(string mapType)
        {
            return false;
        }

        public virtual int CheckSCDeviceVertyList()
        {
            return 0;
        }
    }
}
