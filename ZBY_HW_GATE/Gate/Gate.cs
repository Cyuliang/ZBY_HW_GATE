using System;
using System.Collections.Generic;
using WG3000_COMM.Core;

namespace ZBY_HW_GATE.Gate
{
    class Gate
    {
        private static  Gate Gate_ = new Gate();
        /// <summary>
        /// 日志类
        /// </summary>
        private CLog log = new CLog();          

        /// <summary>
        /// 控制器监听类
        /// </summary>
        wgWatchingService wgWatching;

        /// <summary>
        /// 控制器类
        /// </summary>
        private wgMjController wgMjController1, wgMjController2;

        /// <summary>
        /// 控制器状态事件
        /// </summary>
        public event EventHandler<DoorStateEventArgs> NewState;

        /// <summary>
        /// 监控的控制表
        /// </summary>
        private Dictionary<Int32, wgMjController> selectedControllers;

        public Gate()
        {
            wgMjController1 = new wgMjController
            {
                IP = Properties.Settings.Default.InDoorIp,
                PORT = Properties.Settings.Default.InDoorPort,
                ControllerSN = Int32.Parse(Properties.Settings.Default.InDoorSN)
            };

            wgMjController2 = new wgMjController
            {
                IP = Properties.Settings.Default.OutDoorIp,
                PORT = Properties.Settings.Default.OutDoorPort,
                ControllerSN = Int32.Parse(Properties.Settings.Default.OutDoorSN)
            };

            selectedControllers = new Dictionary<int, wgMjController>
            {
                { wgMjController1.ControllerSN, wgMjController1 },
                { wgMjController2.ControllerSN, wgMjController2 }
            };

            wgWatching = new wgWatchingService();
            wgWatching.EventHandler += WgWatching_EventHandler;
            wgWatching.WatchingController = selectedControllers;
        }

        private void WgWatching_EventHandler(string message)
        {
            log.logWarn.Warn(message);
        }

        /// <summary>
        /// 查询控制器状态
        /// </summary>
        public void GetDoorState()
        {
            DoorSatus(Properties.Settings.Default.InDoorSN, Properties.Settings.Default.OutDoorSN);          
        }

        /// <summary>
        /// 停止监控
        /// </summary>
        public void StopDoorState()
        {
            try
            {
                if (wgWatching != null)
                {
                    wgWatching.WatchingController = null;
                    wgWatching.StopWatch();
                    log.logInfo.Info("Close Door Watching");
                }
            }
            catch (Exception ex)
            {

                log.logError.Error("Stop Door Watching", ex);
            }
        }

        /// <summary>
        /// 开闸动作
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public static int OpenDoor(string Ip,int Port, string SN)
        {
            using (wgMjController wgMjController1 = new wgMjController())
            {
                try
                {
                    wgMjController1.IP = Ip;
                    wgMjController1.PORT = Port;
                    wgMjController1.ControllerSN = Int32.Parse(SN);
                    return (wgMjController1.RemoteOpenDoorIP(1));
                }
                catch (Exception ex)
                {
                    Gate_.log.logError.Error("Open Door Error",ex);
                }
            }
            return - 1;
        }

        /// <summary>
        /// 查询状态
        /// </summary>
        /// <param name="Ip"></param>
        /// <param name="Port"></param>
        /// <param name="SN"></param>
        /// <returns></returns>
        private  void DoorSatus(string InSN,string OutSN)
        {
            EventHandler<DoorStateEventArgs> newState = NewState;
            if(newState!=null)
            {
                try
                {
                    wgMjControllerRunInformation conRunInfo = null;
                    int commStatus = wgWatching.CheckControllerCommStatus(Int32.Parse(InSN), ref conRunInfo);
                    newState(this, new DoorStateEventArgs(commStatus, Int32.Parse(InSN)));
                    commStatus = wgWatching.CheckControllerCommStatus(Int32.Parse(OutSN), ref conRunInfo);
                    newState(this, new DoorStateEventArgs(commStatus, Int32.Parse(OutSN)));
                }
                catch (Exception ex)
                {
                    log.logError.Error("Query  Door State Error", ex);
                }
            }
        }
    }
}
