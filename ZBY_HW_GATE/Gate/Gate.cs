using System;
using System.Collections.Generic;
using System.Threading;
using WG3000_COMM.Core;

namespace ZBY_HW_GATE.Gate
{
    class Gate
    {
        private static  Gate Gate_ = new Gate();
        private System.Threading.Timer GetTimer;

        /// <summary>
        /// 日志类
        /// </summary>
        private CLog Log_ = new CLog();          

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
            GetTimer = new System.Threading.Timer(GetStatus, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));

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
            Log_.logWarn.Warn(message);
        }

        /// <summary>
        /// 查询控制器状态
        /// </summary>
        //public void GetDoorState()
        //{
        //    //DoorSatus(Properties.Settings.Default.InDoorSN, Properties.Settings.Default.OutDoorSN);          
        //}

        /// <summary>
        /// 定时查询
        /// </summary>
        /// <param name="state"></param>
        private  void GetStatus(object state)
        {
            DoorSatus(Properties.Settings.Default.InDoorSN, Properties.Settings.Default.OutDoorSN);
        }

        /// <summary>
        /// 停止监控
        /// </summary>
        public void StopDoorState()
        {
            //GetTimer.Change(-1, -1);
            try
            {
                if (wgWatching != null)
                {
                    wgWatching.WatchingController = null;
                    wgWatching.StopWatch();
                    Log_.logInfo.Info("Close Door Watching");
                    GetTimer.Change(-1, -1);
                }
            }
            catch (Exception ex)
            {

                Log_.logError.Error("Stop Door Watching", ex);
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
                    wgMjController1.IP = Ip;
                    wgMjController1.PORT = Port;
                    wgMjController1.ControllerSN = Int32.Parse(SN);
                    return wgMjController1.RemoteOpenDoorIP(1);
                }
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
                    int InStatus = wgWatching.CheckControllerCommStatus(Int32.Parse(InSN), ref conRunInfo);
                    newState(this, new DoorStateEventArgs(InStatus, Int32.Parse(InSN)));
                    int OutStatus = wgWatching.CheckControllerCommStatus(Int32.Parse(OutSN), ref conRunInfo);
                    newState(this, new DoorStateEventArgs(OutStatus, Int32.Parse(OutSN)));
                }
                catch (Exception ex)
                {
                    Log_.logError.Error("Query  Door State Error", ex);
                }
            }
        }
    }
}
