using System;

namespace ZBY_HW_GATE.CVR
{
    class CVR
    {
        CLog Log_ = new CLog();

        private volatile bool STATE;
        public Action<byte[], byte[], byte[], byte[], byte[], byte[], byte[], byte[],byte[]> FillDataActive;
        public Action<byte[], int> FillDataBmpActive;
        public delegate void CVRDelegate(string mes);
        public CVRDelegate MessageDelegate;

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitComm()
        {
            try
            {
                int ret = SafeNativeMethods.CVR_InitComm(Properties.Settings.Default.CVRCom);
                if (ret == 1)
                {
                    Log_.logInfo.Info("CVR InitComm Success");
                    MessageDelegate("CVR InitComm Success");       
                }
                else
                {
                    Log_.logWarn.Warn("CVR InitComm Fail");
                    MessageDelegate("CVR InitComm Fail");
                }
            }
            catch (Exception ex)
            {
                Log_.logError.Error("CVR InitComm error", ex);
                MessageDelegate("CVR InitComm error");
            }
        }

        /// <summary>
        /// 读卡
        /// </summary>
        public void Authenticate()
        {
            try
            {
                int authenticate = SafeNativeMethods.CVR_Authenticate();
                if (authenticate == 1)
                {
                    int readContent = SafeNativeMethods.CVR_Read_FPContent();
                    if (readContent == 1)
                    {
                        Log_.logInfo.Info("CVR Read Cards Success");
                        MessageDelegate("CVR Read Cards Success");
                        FillData();
                    }
                    else
                    {
                        Log_.logWarn.Warn("CVR Read Cards Faile");
                        MessageDelegate("CVR Read Cards Faile");
                    }
                }
                else
                {
                    Log_.logWarn.Warn("CVR Authenticate error");
                    MessageDelegate("CVR Authenticate error");
                }
            }
            catch (Exception ex)
            {

                Log_.logError.Error("CVR Read Data Error", ex);
                MessageDelegate("CVR Read Data Error");
            }
        }

        public void GetStarted(bool state)
        {
            STATE = state;
        }

        /// <summary>
        /// 循环读卡
        /// </summary>
        /// <param name="state"></param>
        public void AuthenticateWhile()
        {
            while (STATE)
            { 
                int authenticate = SafeNativeMethods.CVR_Authenticate();
                if (authenticate == 1)
                {
                    int readContent = SafeNativeMethods.CVR_Read_FPContent();
                    if (readContent == 1)
                    {
                        Log_.logInfo.Info("CVR Read Cards Success");
                        MessageDelegate("CVR Read Cards Success");
                        FillData();
                    }
                }
                System.Threading.Thread.Sleep(3000);
                MessageDelegate("CVR Read Cards While");
            }            
        }

        /// <summary>
        /// 定时读取
        /// </summary>
        public void AuthenticateFor()
        {
            int i = 0;
            while(i<Properties.Settings.Default.CVRReadWhile)
            {
                int authenticate = SafeNativeMethods.CVR_Authenticate();
                if (authenticate == 1)
                {
                    int readContent = SafeNativeMethods.CVR_Read_FPContent();
                    if (readContent == 1)
                    {
                        Log_.logInfo.Info("CVR Read Cards Success");
                        MessageDelegate("CVR Read Cards Success");
                        FillData();
                    }
                }
                i++;
                System.Threading.Thread.Sleep(2000);
                MessageDelegate("CVR Read Cards For");
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void CloseComm()
        {
            if(SafeNativeMethods.CVR_CloseComm()==1)
            {
                Log_.logInfo.Info("CVR Close Success");
                MessageDelegate("CVR Close Success");
            }
        }

        /// <summary>
        /// 数据
        /// </summary>
        public void FillData()
        {
            try
            {
                byte[] imgData = new byte[40960];               
                int Imglength = 40960;
                SafeNativeMethods.GetBMPData(ref imgData[0], ref Imglength);   

                byte[] name = new byte[128];
                int length = 128;
                SafeNativeMethods.GetPeopleName(ref name[0], ref length);

                byte[] cnName = new byte[128];
                length = 128;
                SafeNativeMethods.GetPeopleChineseName(ref cnName[0], ref length);

                byte[] number = new byte[128];
                length = 128;
                SafeNativeMethods.GetPeopleIDCode(ref number[0], ref length);

                byte[] peopleNation = new byte[128];
                length = 128;
                SafeNativeMethods.GetPeopleNation(ref peopleNation[0], ref length);

                byte[] peopleNationCode = new byte[128];
                length = 128;
                SafeNativeMethods.GetNationCode(ref peopleNationCode[0], ref length);

                byte[] validtermOfStart = new byte[128];
                length = 128;
                SafeNativeMethods.GetStartDate(ref validtermOfStart[0], ref length);

                byte[] birthday = new byte[128];
                length = 128;
                SafeNativeMethods.GetPeopleBirthday(ref birthday[0], ref length);

                byte[] address = new byte[128];
                length = 128;
                SafeNativeMethods.GetPeopleAddress(ref address[0], ref length);

                byte[] validtermOfEnd = new byte[128];
                length = 128;
                SafeNativeMethods.GetEndDate(ref validtermOfEnd[0], ref length);

                byte[] signdate = new byte[128];
                length = 128;
                SafeNativeMethods.GetDepartment(ref signdate[0], ref length);

                byte[] sex = new byte[128];
                length = 128;
                SafeNativeMethods.GetPeopleSex(ref sex[0], ref length);

                byte[] samid = new byte[128];
                SafeNativeMethods.CVR_GetSAMID(ref samid[0]);

                bool bCivic = true;
                byte[] certType = new byte[32];
                length = 32;
                SafeNativeMethods.GetCertType(ref certType[0], ref length);

                string strType = System.Text.Encoding.ASCII.GetString(certType);
                int nStart = strType.IndexOf("I");
                if (nStart != -1) bCivic = false;

                Log_.logInfo.Info(string.Format("CVR Data Name：{0}   Number：{1}", System.Text.Encoding.GetEncoding("GB2312").GetString(name), System.Text.Encoding.GetEncoding("GB2312").GetString(number).Replace("\0", "").Trim()));
                MessageDelegate(string.Format("CVR Data Name：{0}   Number：{1}", System.Text.Encoding.GetEncoding("GB2312").GetString(name), System.Text.Encoding.GetEncoding("GB2312").GetString(number).Replace("\0", "").Trim()));

                if (bCivic)
                {
                    FillDataBmpActive(imgData, Imglength);
                    FillDataActive(name, sex, peopleNation, birthday, number, address, signdate, validtermOfStart, validtermOfEnd);
                }
                STATE = false;
            }
            catch (Exception ex)
            {
                Log_.logError.Error("CVR Read Data error", ex);
                MessageDelegate("CVR Read Data error");
            }
        }
    }
}
