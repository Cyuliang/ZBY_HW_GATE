using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZBY_HW_GATE.Http
{
    class CHttp
    {
        public string SetJosn(/*string Url,string Id,DateTime Time,string Plate,string Container*/)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Properties.Settings.Default.HttpS);
            request.Method = "POST";
            request.ContentType = "application/json";/*x-www-form-urlencoded";*/
            var Josn="{\"eqId\":\"SPPLS01\",\"arrivedTime\":\"20180912103028\",\"truckNumber\":\"粤S12345\",\"tranNo\":\"WDFU1234567\"}";
            byte[] Josntobyte;
            Josntobyte = System.Text.Encoding.UTF8.GetBytes(Josn);
            request.ContentLength = Josntobyte.Length;
            Stream writer;
            try
            {
                writer = request.GetRequestStream();
            }
            catch (Exception ex)
            {
                writer = null;
                throw;
            }
            if(writer!=null)
            {
                writer.Write(Josntobyte, 0, Josntobyte.Length);
                writer.Close();
            }

            HttpWebResponse respone;
            try
            {
                respone = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                respone = ex.Response as HttpWebResponse;
                throw;
            }
            //HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
            //StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
            //string ret = sr.ReadToEnd();
            Stream s = respone.GetResponseStream();
            StreamReader sreader = new StreamReader(s);
            string postConent = sreader.ReadToEnd();
            sreader.Close();
            return postConent;
        }
    }
}
