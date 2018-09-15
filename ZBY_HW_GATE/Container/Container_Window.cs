using System;
using System.Windows.Forms;
using System.Drawing;

namespace ZBY_HW_GATE.Container
{
    public partial class Container_Window : Form
    {
        CLog log = new CLog();
        System.Threading.Timer timer_connect2server;
        public delegate void ContainerDelegate(string mes);
        public ContainerDelegate MessageDelegate;
        public ContainerDelegate StatusDelegate;

        public Container_Window()
        {
            InitializeComponent();

            Linkbutton.Hide();
            LastRbutton.Hide();
            Abortbutton.Hide();
            axVECONclient1.Hide();

            timer_connect2server = new System.Threading.Timer(Connect2server, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));
            //初始化按钮颜色
            Linkbutton.BackColor = Color.DarkRed;        
            //property
            axVECONclient1.ServerIPAddr = Properties.Settings.Default.ServerIPAddr;
            axVECONclient1.ServerPort = Properties.Settings.Default.ServerPort;
            //Result Event
            axVECONclient1.OnCombinedRecognitionResultISO += AxVECONclient1_OnCombinedRecognitionResultISO;
            axVECONclient1.OnIntermediateRecognitionResultISO += AxVECONclient1_OnIntermediateRecognitionResultISO;
            axVECONclient1.OnUpdateLPNEvent += AxVECONclient1_OnUpdateLPNEvent;
            axVECONclient1.OnNewLPNEvent += AxVECONclient1_OnNewLPNEvent;            
            //Connect Event
            axVECONclient1.OnServerConnected += AxVECONclient1_OnServerConnected;
            axVECONclient1.OnServerDisconnected += AxVECONclient1_OnServerDisconnected;
            axVECONclient1.OnServerError += AxVECONclient1_OnServerError;
        }

        /// <summary>
        /// 中间结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AxVECONclient1_OnIntermediateRecognitionResultISO(object sender, AxVeconclientProj.IVECONclientEvents_OnIntermediateRecognitionResultISOEvent e)
        {
            Message(string.Format("Container:triggerTime：{0} laneNum：{1} containerNum：{2} checkSum：{3}", e.triggerTime.ToLongDateString(), e.laneNum, e.containerNum, e.checkSum));
        }

        /// <summary>
        /// 链接错误事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AxVECONclient1_OnServerError(object sender, EventArgs e)
        {
            log.logWarn.Warn("Container Server Error");
            Message("Container Server Error");
            Linkbutton.Text = "Link";
            Linkbutton.BackColor = Color.DarkRed;            
            timer_connect2server.Change(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// 链接断开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AxVECONclient1_OnServerDisconnected(object sender, EventArgs e)
        {
            log.logWarn.Warn("Container Link Disconnected ");
            Message("Container Link Disconnected ");
            StatusDelegate("0");
            Linkbutton.Text = "Link";
            Linkbutton.BackColor = Color.DarkRed;
            timer_connect2server.Change(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// 链接成功事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AxVECONclient1_OnServerConnected(object sender, EventArgs e)
        {
            log.logInfo.Info("Container Link Success ");
            Message("Container Link Success ");
            StatusDelegate("1");
            Linkbutton.Text = "Abort";
            Linkbutton.BackColor = Color.DarkGreen;
            timer_connect2server.Change(-1, -1);
        }

        /// <summary>
        /// 链接箱号
        /// </summary>
        /// <param name="o"></param>
        public  void Connect2server(object o)
        {
            try
            {
                axVECONclient1.Connect2Server();
            }
            catch (Exception ex)
            {
                log.logError.Error("Container link error", ex);
                Message("Container link error");
            }
        }

        /// <summary>
        /// 重车车牌结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AxVECONclient1_OnUpdateLPNEvent(object sender, AxVeconclientProj.IVECONclientEvents_OnUpdateLPNEventEvent e)
        {
            log.logInfo.Info("Container UpdateLPN "+e.triggerTime.ToString()+" "+e.lPN);
            Message("Container UpdateLPN " + e.triggerTime.ToString() + " " + e.lPN);
            MessageDelegate("Container UpdateLPN " + e.triggerTime.ToString() + " " + e.lPN);
            textBox10.Text = e.triggerTime.ToString();
            textBox11.Text = e.lPN;
            textBox12.Text = e.colorCode.ToString();
            textBox13.Text = e.laneNum.ToString();
        }

        /// <summary>
        /// 空车车牌结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AxVECONclient1_OnNewLPNEvent(object sender, AxVeconclientProj.IVECONclientEvents_OnNewLPNEventEvent e)
        {
            log.logInfo.Info("Container NewPLN " + e.triggerTime.ToString() + " " + e.lPN);
            Message("Container NewPLN " + e.triggerTime.ToString() + " " + e.lPN);
            MessageDelegate("Container NewPLN " + e.triggerTime.ToString() + " " + e.lPN);
            textBox10.Text = e.triggerTime.ToString();
            textBox11.Text = e.lPN;
            textBox12.Text = e.colorCode.ToString();
            textBox13.Text = e.laneNum.ToString();
        }

        /// <summary>
        /// 集装箱结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AxVECONclient1_OnCombinedRecognitionResultISO(object sender, AxVeconclientProj.IVECONclientEvents_OnCombinedRecognitionResultISOEvent e)
        {
            log.logInfo.Info("Container Result " + e.triggerTime.ToString() + " " + e.checkSum1);
            Message("Container Result " + e.triggerTime.ToString() + " " + e.checkSum1);
            MessageDelegate("Container Result " + e.triggerTime.ToString() + " " + e.checkSum1);
            textBox1.Text = e.laneNum.ToString();
            textBox2.Text = e.triggerTime.ToString();
            textBox3.Text = e.containerNum1;
            textBox5.Text = e.iSO1;
            textBox6.Text = e.checkSum1;
            textBox4.Text = e.containerNum2;
            textBox8.Text = e.iSO2;
            textBox9.Text = e.checkSum2;
            switch(e.containerType)
            {
                case -1:textBox7.Text = "未知";break;
                case 0:textBox7.Text = "20 吋集装箱";break;
                case 1: textBox7.Text = "40 吋集装箱";break;
                case 2:textBox7.Text = "两个 20 吋集装箱";break;
            }
        }

        /// <summary>
        /// 重写窗口关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Container_Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
        #region//链接箱号
        /// <summary>
        /// 链接箱号按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, EventArgs e)
        {
            axVECONclient1.Connect2Server();
        }
        /// <summary>
        /// 主界面链接箱号识别
        /// </summary>
        public void ContainerLink()
        {
            axVECONclient1.Connect2Server();
        }
        #endregion
        #region//获取最后一次结果
        /// <summary>
        /// 获取最后一次结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button2_Click(object sender, EventArgs e)
        {
            axVECONclient1.SendLastResults(Properties.Settings.Default.LaneNum);
            log.logInfo.Info("Get Last Number");
            Message("Get Last Number");
        }
        /// <summary>
        /// 主界面获取结果
        /// </summary>
        public void ContainerLastR()
        {
            axVECONclient1.SendLastResults(Properties.Settings.Default.LaneNum);
            log.logInfo.Info("Get Last Number");
            Message("Get Last Number");
        }
        #endregion
        #region//断开链接
        /// <summary>
        /// 断开链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button3_Click(object sender, EventArgs e)
        {
            axVECONclient1.Disconnect();
            timer_connect2server.Change(-1, -1);          
        }
        /// <summary>
        /// 主界面断开箱号链接
        /// </summary>
        public void ContainerClose()
        {
            axVECONclient1.Disconnect();
            timer_connect2server.Change(-1, -1);
        }
        #endregion
        /// <summary>
        /// 主动关闭
        /// </summary>
        public void Disconnect()
        {
            axVECONclient1.Disconnect();
            timer_connect2server.Change(-1, -1);
        }
        /// <summary>
        /// Log
        /// </summary>
        /// <param name="mes"></param>
        private void Message(string mes)
        {
            if (LogTextBox.Lines.Length > 100)
            {
                LogTextBox.Clear();
            }
            LogTextBox.Text += mes + "\r\n";
            LogTextBox.Focus();//获取焦点
            LogTextBox.Select(LogTextBox.TextLength, 0);//光标定位到文本最后
            LogTextBox.ScrollToCaret();//滚动到光标处
        }
    }
}
