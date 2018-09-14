/**********************************************************
** 文件名： StationConfigManager.cs
** 文件作用:实现接口支持设备地图绘制
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2018-09-14    xwj       增加
**
**********************************************************/
using DrawTools.DB;
using DrawTools.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFC.MonitorMap.WPFTest
{
    public class StationConfigManager: IDeviceInterface
    {
        #region 方法

        /// <summary>
        /// 获取车站信息列表
        /// </summary>
        /// <returns></returns>
        public List<FilterItem> GetStationList(string lineId)
        {
            //var stationList = CodeManager.Instance.GetLineStationList(lineId, true);
            //List<FilterItem> stationFilterList = new List<FilterItem>();
            //for (int i = 0; i < stationList.Count; i++)
            //{
            //    stationFilterList.Add(new FilterItem(stationList[i].Tag.ToString(), stationList[i].Description));
            //}
            //return stationFilterList;
            return null;
        }


        public List<LineModel> GetAllLines()
        {
            //return GlobalConfig.Instance.AllLine;
            return null;
        }

        /// <summary>
        /// 获取车站设备列表
        /// </summary>
        /// <param name="stationId">车站编号</param>
        /// <param name="mapType"></param>
        /// <returns></returns>
        public List<StationMapModel> GetStationDeviceList(string stationId, string mapType)
        {
            //var list = ServiceFactory.Instance.DeviceMonitorService.GetStationDeviceList(stationId, mapType);
            //List<StationMapModel> deviceList = new List<StationMapModel>();
            //for (int i = 0; i < list.Count(); i++)
            //{
            //    deviceList.Add(new StationMapModel()
            //    {
            //        RecID = list[i].RecID,
            //        StationID = list[i].StationID,
            //        DeviceID = list[i].DeviceID,
            //        DeviceName = list[i].DeviceName,
            //        DeviceType = list[i].DeviceType,
            //        DeviceSubType = list[i].DeviceSubType,
            //        DeviceSeqInStation = list[i].DeviceSeqInStation,
            //        LobbyId = list[i].LobbyId,
            //        GroupID = list[i].GroupID,
            //        DeviceSeqInGroup = list[i].DeviceSeqInGroup,
            //        XPos = list[i].XPos,
            //        YPos = list[i].YPos,
            //        IpAdd = list[i].IpAdd,
            //        Device_W = list[i].Device_W,
            //        Device_H = list[i].Device_H,
            //        Region_W = list[i].Region_W,
            //        Region_H = list[i].Region_H,
            //        Angle = list[i].Angle,
            //        TextFontSize = list[i].TextFontSize,
            //        TextFonStyle = list[i].TextFonStyle,
            //        TextColor = list[i].TextColor,
            //        Entry = list[i].Entry,
            //        LableId = list[i].LableId,
            //        TextType = list[i].TextType,
            //        MapType = list[i].MapType,
            //        UseFlag = list[i].UseFlag
            //    });
            //}
            //return deviceList;

            return null;
        }

        /// <summary>
        /// 保存车站设备信息
        /// </summary>
        /// <param name="model">设备信息</param>
        /// <returns></returns>
        public bool SaveStationDevice(List<StationMapModel> modelList)
        {
            //bool result = false;
            //Core.DeviceMonitorService.StationMapModel[] deviceList = new Core.DeviceMonitorService.StationMapModel[modelList.Count];
            //for (int i = 0; i < modelList.Count; i++)
            //{
            //    var deviceModel = new Core.DeviceMonitorService.StationMapModel()
            //    {
            //        RecID = modelList[i].RecID,
            //        StationID = modelList[i].StationID,
            //        DeviceID = modelList[i].DeviceID,
            //        DeviceName = modelList[i].DeviceName,
            //        DeviceType = modelList[i].DeviceType,
            //        DeviceSubType = modelList[i].DeviceSubType,
            //        DeviceSeqInStation = modelList[i].DeviceSeqInStation,
            //        LobbyId = modelList[i].LobbyId,
            //        GroupID = modelList[i].GroupID,
            //        DeviceSeqInGroup = modelList[i].DeviceSeqInGroup,
            //        XPos = modelList[i].XPos,
            //        YPos = modelList[i].YPos,
            //        IpAdd = modelList[i].IpAdd,
            //        Device_W = modelList[i].Device_W,
            //        Device_H = modelList[i].Device_H,
            //        Region_W = modelList[i].Region_W,
            //        Region_H = modelList[i].Region_H,
            //        Angle = modelList[i].Angle,
            //        TextFontSize = modelList[i].TextFontSize,
            //        TextFonStyle = modelList[i].TextFonStyle,
            //        TextColor = modelList[i].TextColor,
            //        Entry = modelList[i].Entry,
            //        LableId = modelList[i].LableId,
            //        TextType = modelList[i].TextType,
            //        MapType = modelList[i].MapType,
            //        UseFlag = modelList[i].UseFlag
            //    };
            //    deviceList[i] = deviceModel;
            //}
            //ServiceFactory.Instance.DeviceMonitorService.SaveStationDevice(deviceList);
            //return result;
            return false;
        }
        #endregion
    }
}
