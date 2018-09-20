using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZBY_HW_GATE.Plate
{
    class Plate
    {
        private CLog log = new CLog();

        public Action<string> GetmessageAction;
        public Action<string, uint> PlateCallBack;
        public Action<string, string, string, string> PlateDataCallBack;
        public Action<byte[]> JpegCallBack;
        public Action<byte[]> DataJpegCallBack;

        public CLIENT_LPRC_ConnectCallback ConnectCallback = null;
        public CLIENT_LPRC_DataEx2Callback DataEx2Callback = null;
        public CLIENT_LPRC_JpegCallback JpegCallback = null;
        public CLIENT_LPRC_SerialDataCallback SerialDataCallback = null;
        private CLIENT_LPRC_DEVDATA_INFO JpegInfo;
        private CLIENT_LPRC_DeviceInfo DeviceInfo;
        private static CLIENT_LPRC_PLATE_RESULTEX recRes;
        private IntPtr pIP = IntPtr.Zero;
        private bool running = false;
        private byte[] chJpegStream = new byte[NativeConstants.CLIENT_LPRC_BIG_PICSTREAM_SIZE_EX + 312];

        public Plate()
        {
            this.ConnectCallback = new CLIENT_LPRC_ConnectCallback(this.OnConnectCallback);
            this.DataEx2Callback = new CLIENT_LPRC_DataEx2Callback(OnDataEx2Callback);
            this.JpegCallback = new CLIENT_LPRC_JpegCallback(OnJpegCallback);
            this.SerialDataCallback = new CLIENT_LPRC_SerialDataCallback(OnSerialDataCallback);
            pIP = Marshal.StringToHGlobalAnsi(Properties.Settings.Default.PlateIPAddr);

            //注册回调函数
            NativeMethods.CLIENT_LPRC_RegCLIENTConnEvent(this.ConnectCallback);
            NativeMethods.CLIENT_LPRC_RegDataEx2Event(this.DataEx2Callback);
            NativeMethods.CLIENT_LPRC_RegJpegEvent(this.JpegCallback);
            NativeMethods.CLIENT_LPRC_RegSerialDataEvent(this.SerialDataCallback);
        }

        /// <summary>
        /// 开始链接
        /// </summary>
        public void CallbackFuntion()
        {
            if (NativeMethods.CLIENT_LPRC_InitSDK(Properties.Settings.Default.PlatePort, IntPtr.Zero, 0, pIP, 1) != 0)
            {
                GetmessageAction(string.Format("{0} 初始化失败！",pIP.ToString()));
                log.logInfo.Info(string.Format("{0} 初始化失败！", pIP.ToString()));
                running = false;
            }
            else
            {
                GetmessageAction(string.Format("{0} 初始化成功！", pIP.ToString()));
                log.logWarn.Warn(string.Format("{0} 初始化成功！", pIP.ToString()));
                running = true;
            }
        }

        /// <summary>
        /// 通知相机设备通讯状态
        /// </summary>
        /// <param name="chCLIENTIP"></param>
        /// <param name="nStatus"></param>
        /// <param name="dwUser"></param>
        public void OnConnectCallback(System.IntPtr chCLIENTIP, uint nStatus, uint dwUser)
        {
            PlateCallBack(chCLIENTIP.ToString(),nStatus);
        }

        /// <summary>
        /// 识别结果回调函数
        /// </summary>
        /// <param name="recResultEx"></param>
        /// <param name="dwUser"></param>
        private void OnDataEx2Callback(ref CLIENT_LPRC_PLATE_RESULTEX recResultEx, uint dwUser)
        {
            recRes = recResultEx;
            string mes = string.Format("Ip：{0}，Plate：{1}，Color：{2}，Date：{3}",recRes.chCLIENTIP,recRes.chLicense,recRes.chColor,recRes.shootTime);
            log.logInfo.Info(mes);
            GetmessageAction(mes);               
            Data(recRes);            
        }

        /// <summary>
        /// 图像回调函数
        /// </summary>
        /// <param name="JpegInfo"></param>
        /// <param name="dwUser"></param>
        private void OnJpegCallback(ref CLIENT_LPRC_DEVDATA_INFO JpegInfo, uint dwUser)
        {
            if (running == true)
            {
                this.JpegInfo = JpegInfo;
                //把图像数据拷贝到指定内存
                uint nJpegStream = this.JpegInfo.nLen;
                Array.Clear(chJpegStream, 0, chJpegStream.Length);
                Marshal.Copy(this.JpegInfo.pchBuf, chJpegStream, 0, (Int32)nJpegStream);
                JpegCallBack(chJpegStream);                          
            }
        }

        /// <summary>
        /// 显示识别结果中的图片
        /// </summary>
        /// <param name="recResultEx"></param>
        private void Data(CLIENT_LPRC_PLATE_RESULTEX recResultEx)
        {
            byte[] chJpegStream = new byte[NativeConstants.CLIENT_LPRC_BIG_PICSTREAM_SIZE_EX + 312];
            Int32 nJpegStream = recResultEx.pFullImage.nLen;
            Array.Clear(chJpegStream, 0, chJpegStream.Length);
            Marshal.Copy(recResultEx.pFullImage.pBuffer, chJpegStream, 0, nJpegStream);
            DataJpegCallBack(chJpegStream);
        }

        /// <summary>
        /// 设置网卡IP
        /// </summary>
        /// <param name="Ip"></param>
        public void SetIpNetwork(string Ip)
        {
            if(NativeMethods.CLIENT_LPRC_SetNetworkCardBind(Marshal.StringToHGlobalAnsi(Ip))==0)
            {
                GetmessageAction(string.Format("Set Ip Network {0}",Ip));
                log.logInfo.Info(string.Format("Set Ip Network {0}",Ip));
            }
        }

        /// <summary>
        /// 设置保存路径
        /// </summary>
        /// <param name="path"></param>
        public void SetSavrImagePath(string path)
        {
            NativeMethods.CLIENT_LPRC_SetSavePath(Marshal.StringToHGlobalAnsi(path));
            GetmessageAction(string.Format("Set Save Path {0}", path));
            log.logInfo.Info(string.Format("Set Save Path {0}", path));
        }

        /// <summary>
        /// 发送抬杆命令
        /// </summary>
        public void SetRelayClose()
        {
            if (running == true)
            {
                if (NativeMethods.CLIENT_LPRC_SetRelayClose(this.pIP, 9110) == 0)
                {
                    GetmessageAction("发送抬杆命令！");
                    log.logInfo.Info("发送抬杆命令！");
                }
            }
        }

        /// <summary>
        /// 模拟触发命令
        /// </summary>
        public void SetTrigger()
        {
            if (running == true)
            {
                if (NativeMethods.CLIENT_LPRC_SetTrigger(pIP, 8080) == 0)
                {
                    GetmessageAction("模拟触发命令！");
                    log.logInfo.Info("模拟触发命令！");
                }
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void QuitDevice()
        {
            if (running == true)
            {
                running = false;
                NativeMethods.CLIENT_LPRC_SetJpegStreamPlayOrStop(pIP,0);            
                if (NativeMethods.CLIENT_LPRC_QuitDevice(this.pIP) == 0)
                {
                    GetmessageAction("设备断开！");
                    log.logInfo.Info("设备断开！");
                }
            }
        }

        /// <summary>
        /// 视频流
        /// </summary>
        public void Play(bool state)
        {
            if(state)
            {
                if(NativeMethods.CLIENT_LPRC_SetJpegStreamPlayOrStop(pIP, 1)==0)
                {
                    GetmessageAction("Open Plate Video！");
                    log.logInfo.Info("Open Plate Video！");
                }
            }
            else
            {
                if(NativeMethods.CLIENT_LPRC_SetJpegStreamPlayOrStop(pIP, 0)==0)
                {
                    GetmessageAction("Close Plate Video！");
                    log.logInfo.Info("CLose Plate Video！");
                }
            }
        }

        /// <summary>
        /// 搜索设备
        /// </summary>
        public void SearchDeviceList()
        {
            if(NativeMethods.CLIENT_LPRC_SearchDeviceList(ref DeviceInfo)>0)
            {                
                GetmessageAction(DeviceInfo.chIp.ToString());
                log.logInfo.Info(DeviceInfo.chIp.ToString());
            }
            else
            {
                GetmessageAction("Not Find Device");
                log.logInfo.Info("Not Find Device");
            }
        }

        /// <summary>
        /// 获取相机485发送的数据
        /// </summary>
        /// <param name="chCLIENTIP"></param>
        /// <param name="pSerialData"></param>
        /// <param name="dwUser"></param>
        public void OnSerialDataCallback(System.IntPtr chCLIENTIP, ref CLIENT_LPRC_DEVSERIAL_DATA pSerialData, uint dwUser)
        {
            string mes = string.Format("Ip：{0}，Text：{1}",chCLIENTIP,pSerialData.pData.ToString());
            GetmessageAction(mes);
            log.logInfo.Info(mes);            
        }

        /// <summary>
        /// 发送485透明传输
        /// </summary>
        public void RS485Send(string mes)
        {
            byte[] prefix = HexToBytes(Properties.Settings.Default.prefix);
            byte[] data = Encoding.GetEncoding("GB2312").GetBytes(mes);
            byte[] end = new byte[] { 0x0D };
            byte[] dst = new byte[prefix.Length + data.Length + end.Length];
            prefix.CopyTo(dst, 0);
            data.CopyTo(dst, prefix.Length);
            end.CopyTo(dst, prefix.Length + data.Length);
            var x = Encoding.GetEncoding("GB2312").GetString(dst).Replace(" ", "\0");
            if (running == true)
            {
                if (NativeMethods.CLIENT_LPRC_RS485Send(pIP, 9110, Marshal.StringToHGlobalAnsi(x), dst.Length) == 0)
                {
                    GetmessageAction("透明传输成功！");
                    log.logInfo.Info("透明传输成功！");
                }
            }
        }

        /// <summary>
        /// 字符转16进制
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] HexToBytes(string hex)
        {
            hex = hex.Trim();
            byte[] bytes = new byte[hex.Length / 2];
            for (int index = 0; index < bytes.Length; index++)
            {
                bytes[index] = byte.Parse(hex.Substring(index * 2, 2), NumberStyles.HexNumber);
            }
            return bytes;
        }
    }
}
