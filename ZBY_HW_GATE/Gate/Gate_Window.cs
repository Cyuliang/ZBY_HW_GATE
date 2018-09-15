using System;
using System.Drawing;
using System.Windows.Forms;

namespace ZBY_HW_GATE.Gate
{
    public partial class Gate_Window : Form
    {
        private CLog log = new CLog();
        private Gate Gate_ = new Gate();

        /// <summary>
        /// 开门委托类
        /// </summary>
        /// <param name="SN">SN号</param>
        /// <returns></returns>
        public delegate int OpenGateDelete(string Ip, int Port, String SN);
        private OpenGateDelete delegatesOpenGate;

        /// <summary>
        /// 关闭控制器监听委托类
        /// </summary>
        public delegate void StopDoorState();
        public StopDoorState delegatesStopDoorState;
        
        public Gate_Window()
        {
            InitializeComponent();

            textBox1.Text = Properties.Settings.Default.InDoorIp;
            textBox2.Text = Properties.Settings.Default.InDoorPort.ToString();
            textBox3.Text = Properties.Settings.Default.InDoorSN;
            textBox4.Text = Properties.Settings.Default.OutDoorIp;
            textBox5.Text = Properties.Settings.Default.OutDoorPort.ToString();
            textBox6.Text = Properties.Settings.Default.OutDoorSN;

            delegatesOpenGate = Gate.OpenDoor;
            Gate_.NewState += Gate__NewState;
            delegatesStopDoorState = Gate_.StopDoorState;
            Gate_.GetDoorState();
        }

        /// <summary>
        /// log
        /// </summary>
        /// <param name="mes"></param>
        private void GetMessage(string mes)
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

        /// <summary>
        /// 侦听控制器状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Gate__NewState(object sender, DoorStateEventArgs e)
        {
            if (e.State == 1)
            {
                if (e.SN == Int32.Parse(Properties.Settings.Default.InDoorSN))
                {
                    label7.BackColor = Color.DarkGreen;
                    log.logInfo.Info(string.Format("{0} OnLine",e.SN));
                    GetMessage(string.Format("{0} OnLine", e.SN));
                }
                if (e.SN == Int32.Parse(Properties.Settings.Default.OutDoorSN))
                {
                    label8.BackColor = Color.DarkGreen;
                    log.logInfo.Info(string.Format("{0} OnLine", e.SN));
                    GetMessage(string.Format("{0} OnLine", e.SN));
                }
            }
            else
            {
                if (e.SN == Int32.Parse(Properties.Settings.Default.InDoorSN))
                {
                    label7.BackColor = Color.DarkRed;
                    log.logInfo.Info(string.Format("{0} Drop line ", e.SN));
                    GetMessage(string.Format("{0} Drop line ", e.SN));
                }
                if (e.SN == Int32.Parse(Properties.Settings.Default.OutDoorSN))
                {
                    label8.BackColor = Color.DarkRed;
                    log.logInfo.Info(string.Format("{0} Drop line ", e.SN));
                    GetMessage(string.Format("{0} Drop line ", e.SN));
                }
            }
        }

        /// <summary>
        /// 入闸
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void In_Button_Click(object sender, EventArgs e)
        {
            try
            {
                if (delegatesOpenGate(textBox1.Text, int.Parse(textBox2.Text), textBox3.Text) == -1)
                {
                    log.logInfo.Info("Open In Door Error");
                    GetMessage("Open In Door Error");
                }
                else
                {
                    log.logInfo.Info("Open In Door Success");
                    GetMessage("Open In Door Success");
                }
            }
            catch (Exception ex)
            {

                log.logError.Error("Open In Door Error", ex);
                GetMessage("Open In Door Error");
            }
        }

        /// <summary>
        /// 出闸
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Out_Button_Click(object sender, EventArgs e)
        {
            try
            {
                if (delegatesOpenGate(textBox4.Text, int.Parse(textBox5.Text), textBox6.Text) == -1)
                {
                    log.logInfo.Info("Open Out Door Error");
                    GetMessage("Open Out Door Error");
                }
                else
                {
                    log.logInfo.Info("Open Out Door Success");
                    GetMessage("Open Out Door Success");
                }
            }
            catch (Exception ex)
            {
                log.logError.Error("Open Out Door Error", ex);
                GetMessage("Open Out Door Error");
            }
        }
    }
}
