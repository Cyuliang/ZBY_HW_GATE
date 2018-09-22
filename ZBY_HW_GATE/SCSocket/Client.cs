using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ZBY_HW_GATE.SCSocket
{
    class Client
    {
        private CLog Log_ = new CLog();
        public delegate void SetMessageDelegate(string mes);
        public SetMessageDelegate SetMessage;

        private Socket socket = null;
        private byte[] buffer = new byte[4096];
        private System.Threading.Timer Timer_;

        public Client()
        {
            Timer_ = new System.Threading.Timer(LinkCallback, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// 定时回调函数
        /// </summary>
        /// <param name="state"></param>
        private void LinkCallback(object state)
        {
            Link();
        }

        /// <summary>
        /// 链接
        /// </summary>
        /// <param name="Ip"></param>
        /// <param name="Port"></param>
        public void Link()
        {
            try
            {
                IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(Properties.Settings.Default.SockerClientIp), Properties.Settings.Default.SocketClientPort);
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket = client;
                client.Connect(ipe);
                if(client.Connected)
                {
                    Timer_.Change(-1, -1);
                }
                client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(receiveMessage), client);
                SetMessage(string.Format("Link Sucess：{0} {1}", Properties.Settings.Default.SockerClientIp, Properties.Settings.Default.SocketClientPort));
                Log_.logInfo.Info(string.Format("Link Sucess：{0} {1}", Properties.Settings.Default.SockerClientIp, Properties.Settings.Default.SocketClientPort));
            }
            catch (Exception ex)
            {
                Timer_.Change(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
                Log_.logError.Error(string.Format("Link Error：{0} {1}", Properties.Settings.Default.SockerClientIp, Properties.Settings.Default.SocketClientPort),ex);
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        public void SendData(string mes)
        {
            if(socket!=null)
            {
                Send(socket, mes);
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="ar"></param>
        private void receiveMessage(IAsyncResult ar)
        {
            socket = ar.AsyncState as Socket;
            try
            {
                int length = socket.EndReceive(ar);
                if(length==0)
                {                  
                    socket.Close();
                    socket = null;
                    Timer_.Change(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
                    SetMessage("Server Close");
                    Log_.logWarn.Warn("Server Close");
                    return;
                }
                string message = Encoding.GetEncoding("GB2312").GetString(buffer, 0, length);
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(receiveMessage), socket);
                SetMessage(string.Format("Get Data：{0} {1} {2}", message, Properties.Settings.Default.SockerClientIp, Properties.Settings.Default.SocketClientPort));
                Log_.logInfo.Info(string.Format("Get Data：{0} {1} {2}",message, Properties.Settings.Default.SockerClientIp, Properties.Settings.Default.SocketClientPort));
            }
            catch (Exception ex)
            {
                Log_.logError.Error(string.Format("Get Data Error：{0} {1}", Properties.Settings.Default.SockerClientIp, Properties.Settings.Default.SocketClientPort), ex);
            }
        }

        private void Send(Socket client, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
            SetMessage(string.Format("Send Data：{0} {1} {2}", data, Properties.Settings.Default.SockerClientIp, Properties.Settings.Default.SocketClientPort));
            Log_.logInfo.Info(string.Format("Send Data：{0} {1} {2}", data, Properties.Settings.Default.SockerClientIp, Properties.Settings.Default.SocketClientPort));
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);
            }
            catch (Exception ex)
            {
                Log_.logError.Error(string.Format("Send Data Error：{0} {1}", Properties.Settings.Default.SockerClientIp, Properties.Settings.Default.SocketClientPort), ex);
            }
        }
    }
}
