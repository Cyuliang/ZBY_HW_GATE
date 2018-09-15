using System;

namespace ZBY_HW_GATE.LED
{
    class LED
    {     
        #region
        private const int CONTROLLER_BX_5E1 = 0x0154;
        private const int CONTROLLER_BX_5E2 = 0x0254;
        private const int CONTROLLER_BX_5E3 = 0x0354;
        private const int CONTROLLER_BX_5Q0P = 0x1056;
        private const int CONTROLLER_BX_5Q1P = 0x1156;
        private const int CONTROLLER_BX_5Q2P = 0x1256;
        private const int CONTROLLER_BX_6Q1 = 0x0166;
        private const int CONTROLLER_BX_6Q2 = 0x0266;
        private const int CONTROLLER_BX_6Q2L = 0x0466;
        private const int CONTROLLER_BX_6Q3 = 0x0366;
        private const int CONTROLLER_BX_6Q3L = 0x0566;

        private const int CONTROLLER_BX_5E1_INDEX = 0;
        private const int CONTROLLER_BX_5E2_INDEX = 1;
        private const int CONTROLLER_BX_5E3_INDEX = 2;
        private const int CONTROLLER_BX_5Q0P_INDEX = 3;
        private const int CONTROLLER_BX_5Q1P_INDEX = 4;
        private const int CONTROLLER_BX_5Q2P_INDEX = 5;
        private const int CONTROLLER_BX_6Q1_INDEX = 6;
        private const int CONTROLLER_BX_6Q2_INDEX = 7;
        private const int CONTROLLER_BX_6Q2L_INDEX = 8;
        private const int CONTROLLER_BX_6Q3_INDEX = 9;
        private const int CONTROLLER_BX_6Q3L_INDEX = 10;

        //==============================================================================
        // 返回状态代码定义
        private const int RETURN_ERROR_NOFIND_DYNAMIC_AREA = 0xE1; //没有找到有效的动态区域。
        private const int RETURN_ERROR_NOFIND_DYNAMIC_AREA_FILE_ORD = 0xE2; //在指定的动态区域没有找到指定的文件序号。
        private const int RETURN_ERROR_NOFIND_DYNAMIC_AREA_PAGE_ORD = 0xE3; //在指定的动态区域没有找到指定的页序号。
        private const int RETURN_ERROR_NOSUPPORT_FILETYPE = 0xE4; //不支持该文件类型。
        private const int RETURN_ERROR_RA_SCREENNO = 0xF8; //已经有该显示屏信息。如要重新设定请先DeleteScreen删除该显示屏再添加；
        private const int RETURN_ERROR_NOFIND_AREA = 0xFA; //没有找到有效的显示区域；可以使用AddScreenProgramBmpTextArea添加区域信息。
        private const int RETURN_ERROR_NOFIND_SCREENNO = 0xFC; //系统内没有查找到该显示屏；可以使用AddScreen函数添加显示屏
        private const int RETURN_ERROR_NOW_SENDING = 0xFD; //系统内正在向该显示屏通讯，请稍后再通讯；
        private const int RETURN_ERROR_OTHER = 0xFF; //其它错误；
        private const int RETURN_NOERROR = 0; //没有错误
        //==============================================================================

        private bool m_bSendBusy = false;//此变量在数据更新中非常重要，请务必保留。

        //添加屏幕
        private readonly int nControlType = Properties.Settings.Default.nControlType;
        private readonly int nScreenNo = Properties.Settings.Default.nScreenNo;
        private readonly int nSendMode = Properties.Settings.Default.nSendMode;
        private readonly int nWidth = Properties.Settings.Default.nWidth;
        private readonly int nHeight = Properties.Settings.Default.nHeight;
        private readonly int nScreenType = Properties.Settings.Default.nScreenType;
        private readonly int nPixelMode = Properties.Settings.Default.nPixelMode;
        private readonly string pCom = Properties.Settings.Default.pCom;
        private readonly int nBaud = Properties.Settings.Default.nBaud;
        public readonly string pSocketIP = Properties.Settings.Default.pSocketIP;
        public readonly int nSocketPort = Properties.Settings.Default.nSocketPort;
        private readonly int nStaticIpMode = Properties.Settings.Default.nStaticIpMode;
        private readonly int nServerMode = Properties.Settings.Default.nServerMode;
        private readonly string pBarcode = Properties.Settings.Default.pBarcode;
        private readonly string pNetworkID = Properties.Settings.Default.pNetworkID;
        private readonly string pServerIP = Properties.Settings.Default.pServerIP;
        private readonly int nServerPort = Properties.Settings.Default.nServerPort;
        private readonly string pServerAccessUser = Properties.Settings.Default.pServerAccessUser;
        private readonly string pServerAccessPassword = Properties.Settings.Default.pServerAccessPassword;
        private readonly string pCommandDataFile = Properties.Settings.Default.pCommandDataFile;

        //添加区域
        private readonly int nDYAreaID = Properties.Settings.Default.nDYAreaID;
        private readonly int nRunMode = Properties.Settings.Default.nRunMode;
        private readonly int nTimeOut = Properties.Settings.Default.nTimeOut;
        private readonly int nAllProRelate = Properties.Settings.Default.nAllProRelate;
        private readonly string pProRelateList = Properties.Settings.Default.pProRelateList;
        private readonly int nPlayImmediately = Properties.Settings.Default.nPlayImmediately;
        private readonly string nAreaX = Properties.Settings.Default.nAreaX;
        private readonly string nAreaY = Properties.Settings.Default.nAreaY;
        private readonly string nAreaWidth = Properties.Settings.Default.nAreaWidth;
        private readonly string nAreaHeight = Properties.Settings.Default.nAreaHeight;
        private readonly int nAreaFMode = Properties.Settings.Default.nAreaFMode;
        private readonly int nAreaFLine = Properties.Settings.Default.nAreaFLine;
        private readonly int nAreaFColor = Properties.Settings.Default.nAreaFColor;
        private readonly int nAreaFStunt = Properties.Settings.Default.nAreaFStunt;
        private readonly int nAreaFRunSpeed = Properties.Settings.Default.nAreaFRunSpeed;
        private readonly int nAreaFMoveStep = Properties.Settings.Default.nAreaFMoveStep;

        //添加文本
        private readonly string pText = Properties.Settings.Default.pText;
        private readonly int nShowSingle = Properties.Settings.Default.nShowSingle;
        private readonly int nAlignment = Properties.Settings.Default.nAlignment;
        private readonly string pFontName = Properties.Settings.Default.pFontName;
        private readonly int nFontSize = Properties.Settings.Default.nFontSize;
        private readonly int nBold = Properties.Settings.Default.nBold;
        private readonly int nFontColor = Properties.Settings.Default.nFontColor;
        private readonly int nStunt = Properties.Settings.Default.nStunt;
        private readonly int nRunSpeed = Properties.Settings.Default.nRunSpeed;
        private readonly int nShowTime = Properties.Settings.Default.nShowTime;

        //发送信息
        private readonly int nDelAllDYArea = Properties.Settings.Default.nDelAllDYArea;
        private readonly string pDYAreaIDList = Properties.Settings.Default.pDYAreaIDList;

        //动态区域
        private readonly string[] nAreaXs;
        private readonly string[] nAreaYs;
        private readonly string[] nAreaWidths;
        private readonly string[] nAreaHeights;
        #endregion

        /// <summary>
        /// 日志类
        /// </summary>
        private CLog log = new CLog();

        public LED()
        {
            nAreaXs = nAreaX.Split(',');
            nAreaYs = nAreaY.Split(',');
            nAreaWidths = nAreaWidth.Split(',');
            nAreaHeights = nAreaHeight.Split(',');
        }

        /// <summary>
        /// 处理信息委托类
        /// </summary>
        public Action<string> Led_Return_Message_func;

        /// <summary>
        /// 函数执行返回信息
        /// </summary>
        /// <param name="szfunctionName"></param>
        /// <param name="nResult"></param>
        public void GetErrorMessage(string szfunctionName, int nResult)
        {        
            string szResult;
            DateTime dt = DateTime.Now;
            szResult = dt.ToString() + "---执行函数：" + szfunctionName + "---返回结果：";
            switch (nResult)
            {
                case RETURN_ERROR_NOFIND_DYNAMIC_AREA:
                    Led_Return_Message_func(szResult + "没有找到有效的动态区域。\r\n");
                    log.logWarn.Warn(szResult + "没有找到有效的动态区域");
                    break;
                case RETURN_ERROR_NOFIND_DYNAMIC_AREA_FILE_ORD:
                    Led_Return_Message_func(szResult + "在指定的动态区域没有找到指定的文件序号。\r\n");
                    log.logWarn.Warn(szResult + "在指定的动态区域没有找到指定的文件序号");
                    break;
                case RETURN_ERROR_NOFIND_DYNAMIC_AREA_PAGE_ORD:
                    Led_Return_Message_func(szResult + "在指定的动态区域没有找到指定的页序号。\r\n");
                    log.logError.Error(szResult + "在指定的动态区域没有找到指定的页序号");
                    break;
                case RETURN_ERROR_NOSUPPORT_FILETYPE:
                    Led_Return_Message_func(szResult + "动态库不支持该文件类型。\r\n");
                    log.logWarn.Warn(szResult + "动态库不支持该文件类型");
                    break;
                case RETURN_ERROR_RA_SCREENNO:
                    Led_Return_Message_func(szResult + "系统中已经有该显示屏信息。如要重新设定请先执行DeleteScreen函数删除该显示屏后再添加。\r\n");
                    log.logWarn.Warn(szResult + "系统中已经有该显示屏信息。如要重新设定请先执行DeleteScreen函数删除该显示屏后再添加");
                    break;
                case RETURN_ERROR_NOFIND_AREA:
                    Led_Return_Message_func(szResult + "系统中没有找到有效的动态区域；可以先执行AddScreenDynamicArea函数添加动态区域信息后再添加。\r\n");
                    log.logWarn.Warn(szResult + "系统中没有找到有效的动态区域；可以先执行AddScreenDynamicArea函数添加动态区域信息后再添加");
                    break;
                case RETURN_ERROR_NOFIND_SCREENNO:
                    Led_Return_Message_func(szResult + "系统内没有查找到该显示屏；可以使用AddScreen函数添加该显示屏。\r\n");
                    log.logWarn.Warn(szResult + "系统内没有查找到该显示屏；可以使用AddScreen函数添加该显示屏");
                    break;
                case RETURN_ERROR_NOW_SENDING:
                    Led_Return_Message_func(szResult + "系统内正在向该显示屏通讯，请稍后再通讯。\r\n");
                    log.logWarn.Warn(szResult + "系统内正在向该显示屏通讯，请稍后再通讯");
                    break;
                case RETURN_ERROR_OTHER:
                    Led_Return_Message_func(szResult + "其它错误。\r\n");
                    log.logWarn.Warn(szResult + "其它错误");
                    break;
                case RETURN_NOERROR:
                    Led_Return_Message_func(szResult + "函数执行成功。\r\n");
                    log.logInfo.Info(szResult + "函数执行成功");
                    break;
            }
        }

        /// <summary>
        /// 初始化动态库
        /// </summary>
        public int Initialize()
        {
            int nResult = SafeNativeMethods.Initialize();
            GetErrorMessage("Initialize", nResult);
            return nResult;
        }

        /// <summary>
        /// 添加显示屏
        /// </summary>
        public int AddScreen_Dynamic()
        {
            try
            {
                int nResult = SafeNativeMethods.AddScreen_Dynamic(nControlType, nScreenNo, nSendMode, nWidth, nHeight,
                            nScreenType, nPixelMode, pCom, nBaud, pSocketIP, nSocketPort, nStaticIpMode, nServerMode,
                            pBarcode, pNetworkID, pServerIP, nServerPort, pServerAccessUser, pServerAccessPassword,
                            pCommandDataFile);
                GetErrorMessage("执行AddScreen函数,", nResult);
                return nResult;
            }
            catch (Exception ex)
            {
                log.logError.Error("AddScreen Error",ex);
                return -1;
            }
        }

        /// <summary>
        /// 添加动态区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public int AddScreenDynamicArea()
        {
            try
            {
                int nResult = -1;
                for (int i = 0; i < nDYAreaID; i++)
                {
                    nResult = SafeNativeMethods.AddScreenDynamicArea(nScreenNo, i, nRunMode,
                                 nTimeOut, nAllProRelate, pProRelateList, nPlayImmediately,
                                 int.Parse(nAreaXs[i]), int.Parse(nAreaYs[i]), int.Parse(nAreaWidths[i]), int.Parse(nAreaHeights[i]),
                                 nAreaFMode, nAreaFLine, nAreaFColor, nAreaFStunt, nAreaFRunSpeed, nAreaFMoveStep);
                    GetErrorMessage(string.Format("执行AddScreenDynamicArea函数,添加 {0} X:{1} Y:{2} W:{3} H:{4} 动态区域",
                                 i, nAreaXs[i], nAreaYs[i], nAreaWidths[i], nAreaHeights[i]), nResult);
                }
                return nResult;
            }
            catch (Exception ex)
            {

                log.logError.Error("AddScreenDynamicArea Error", ex);
                return -1;
            }
        }

        /// <summary>
        /// 添加动态区域文本
        /// </summary>
        public int AddScreenDynamicAreaText(string[] pTexts)
        {
            try
            {
                int nResult = -1;
                int i = 0;
                foreach (string str in pTexts)
                {
                    nResult = SafeNativeMethods.AddScreenDynamicAreaText(nScreenNo, i,
                                str, nShowSingle, nAlignment, pFontName, nFontSize, nBold, nFontColor,
                                nStunt, nRunSpeed, nShowTime);
                    GetErrorMessage(string.Format("执行AddScreenDynamicAreaText函数,添加 {0} 区域，文本 {1}", i, str), nResult);
                    i++;
                }
                return nResult;
            }
            catch (Exception ex)
            {
                log.logError.Error("AddScreenDynamicAreaText Error",ex);
                return -1;
            }
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        public int SendDynamicAreasInfoCommand()
        {
            try
            {
                if (m_bSendBusy == false)
                {
                    m_bSendBusy = true;
                    int nResult = SafeNativeMethods.SendDynamicAreasInfoCommand(nScreenNo, nDelAllDYArea, pDYAreaIDList);//如果发送多个动态区域，动态区域编号间用","隔开。
                    GetErrorMessage("执行SendDynamicAreasInfoCommand函数, ", nResult);
                    m_bSendBusy = false;
                    return nResult;
                }
                return -1;
            }
            catch (Exception ex)
            {
                log.logError.Error("SendDynamicAreasInfoCommand Error",ex);
                return -1;
            }
        }

        /// <summary>
        /// 释放动态库
        /// </summary>
        public int Uninitialize()
        {
            try
            {
                int nResult = SafeNativeMethods.Uninitialize();
                GetErrorMessage("Uninitialize", nResult);
                return nResult;
            }
            catch (Exception ex)
            {
                log.logError.Error("Uninitialize Error", ex); 
                return -1;
            }
        }
    }
}
