using System;
using System.IO;
using System.Net;
using System.Text;

namespace ZBY_HW_GATE.Http
{
    class CHttp
    {
        private CLog Log_ = new CLog();
        public delegate void MessageDelegate(string mes);
        public MessageDelegate SetMessage;

        public CHttp()
        {
        }

        public string SetJosn(string Id,string Time,string Plate,string Container)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Properties.Settings.Default.HttpS);
            request.Method = "POST";
            request.Timeout = Properties.Settings.Default.HttpTimeOut;
            request.ReadWriteTimeout = Properties.Settings.Default.HttpReadWriteTimeout;
            request.ContentType = "application/json";/*x-www-form-urlencoded";*/
            string Json =string.Format(@"{{""eqId"":""{0}"",""arrivedTime"":""{1}"",""truckNumber"":""{2}"",""tranNo"":""{3}""}}",
                Id,Time,Plate,Container);
            if(Plate==string.Empty)
            {
                if (Container != string.Empty)
                {
                    Json = string.Format(@"{{""eqId"":""{0}"",""arrivedTime"":""{1}"",""truckNumber"":"""",""tranNo"":""{2}""}}",
                        Id,Time,Container);
                }
            }
            else
            {
                if(Container==string.Empty)
                {
                    Json = string.Format(@"{{""eqId"":""{0}"",""arrivedTime"":""{1}"",""truckNumber"":""{2}"",""tranNo"":""""}}",
                        Id,Time,Plate);
                }
            }
            if(Plate==string.Empty&&Container==string.Empty)
            {
                Json = string.Format(@"{{""eqId"":""SPPLS01"",""arrivedTime"":""{0}"",""truckNumber"":""粤S12345"",""tranNo"":""WDFU1234567""}}",
                    string.Format("{0:yyyyMMddHHmmss}", DateTime.Now));
            }
            byte[] Josntobyte = Encoding.UTF8.GetBytes(Json);
            request.ContentLength = Josntobyte.Length;
            Stream writer=null;
            try
            {
                writer = request.GetRequestStream();
            }
            catch (Exception ex)
            {
                Log_.logError.Error("Send Data Error", ex);
            }
            if (writer != null)
            {
                writer.Write(Josntobyte, 0, Josntobyte.Length);
                writer.Close();

                HttpWebResponse respone;
                try
                {
                    respone = (HttpWebResponse)request.GetResponse();
                }
                catch (WebException ex)
                {
                    respone = ex.Response as HttpWebResponse;
                    Log_.logError.Error("Result Data Error", ex);
                }
                //HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                //StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                //string ret = sr.ReadToEnd();
                Stream s = respone.GetResponseStream();
                StreamReader sreader = new StreamReader(s);
                string postConent = sreader.ReadToEnd();
                sreader.Close();
                SetMessage(string.Format("Post Data：{0}", Json));
                Log_.logError.Error(string.Format("Post Data：{0}", Json));
                return postConent;
            }
            SetMessage(string.Format("Post Error：{0}", Json));
            Log_.logWarn.Warn(string.Format("Post Error：{0}",Json));
            return "null";
        }
    }
}
