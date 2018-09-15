using System;
using System.Windows.Forms;
using System.Drawing;
using System.Text;
using System.Globalization;
using System.IO;

namespace ZBY_HW_GATE.Plate
{
    public partial class Plate_Window : Form
    {
        private CLog log = new CLog();
        private Plate Plate_ = new Plate();

        private delegate void PlateDelgate();
        private PlateDelgate PlateStar;
        private PlateDelgate PlateSettiger;
        private PlateDelgate PlateQuitDevice;
        private PlateDelgate PlateSearchDevice;

        private delegate void SetRelayCloseDelgate();
        private SetRelayCloseDelgate setRelayClose;    

        private Action<bool> PlayAction;
        private Action<string> Send485Action;
        private Action<string> SetIpAction;
        private Action<string> SetSaveImg;

        public Plate_Window()
        {
            InitializeComponent();

            OnLineLable.BackColor = Color.DarkRed;
            Plate_.PlateCallBack = new Action<string, uint>(PlateCallBack);
            PlateStar += Plate_.CallbackFuntion;
            Plate_.GetmessageAction = new Action<string>(Message);
            Plate_.PlateDataCallBack = new Action<string, string, string, string>(PlateDataInfoOut);
            Plate_.JpegCallBack = new Action<byte[]>(JpegCallBack);
            Plate_.DataJpegCallBack = new Action<byte[]>(DataJpeg);
            setRelayClose += Plate_.SetRelayClose;
            PlateSettiger += Plate_.SetTrigger;
            PlateQuitDevice += Plate_.QuitDevice;
            PlayAction = new Action<bool>(Plate_.Play);
            Send485Action = new Action<string>(Plate_.RS485Send);
            SetIpAction = new Action<string>(Plate_.SetIpNetwork);
            SetSaveImg = new Action<string>(Plate_.SetSavrImagePath);
            PlateSearchDevice += Plate_.SearchDeviceList;
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
            LogTextBox.Text += mes+"\r\n";
            LogTextBox.Focus();//获取焦点
            LogTextBox.Select(LogTextBox.TextLength, 0);//光标定位到文本最后
            LogTextBox.ScrollToCaret();//滚动到光标处
        }

        /// <summary>
        /// 相机状态
        /// </summary>
        /// <param name="str"></param>
        /// <param name="status"></param>
        private void PlateCallBack(string str,uint status)
        {
            if(status==1)
            {
                OnLineLable.BackColor = Color.DarkGreen;
                Message(string.Format("{0} Plate OnLine",str));
                log.logInfo.Info(string.Format("{0} Plate OnLine", str));
            }
            if(status==0)
            {
                OnLineLable.BackColor = Color.DarkRed;
                Message(string.Format("{0} Plate Drop line", str));
                log.logWarn.Warn(string.Format("{0} Plate Drop line", str));
            }
        }

        /// <summary>
        /// 链接车牌
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, EventArgs e)
        {
            PlateStar();
        }

        /// <summary>
        /// 车牌结果
        /// </summary>
        /// <param name="Ip"></param>
        /// <param name="Plate"></param>
        /// <param name="Color"></param>
        /// <param name="Time"></param>
        private void PlateDataInfoOut(string Ip,string Plate,string Color,string Time)
        {
            TimeTextBox.Text = Time;
            IpTextBox.Text = Ip;
            PlateTextBox.Text = Plate;
            ColorTextBox.Text = Color;
        }

        /// <summary>
        /// Jpeg流
        /// </summary>
        /// <param name="jpeg"></param>
        private void JpegCallBack(byte[] jpeg)
        {
            pictureBox1.Image = Image.FromStream(new MemoryStream(jpeg));
        }

        /// <summary>
        /// 结果图片
        /// </summary>
        /// <param name="jpeg"></param>
        private void DataJpeg(byte[] jpeg)
        {
            pictureBox2.Image = Image.FromStream(new MemoryStream(jpeg));
        }

        private void Plate_Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        /// <summary>
        /// 抬杆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button4_Click(object sender, EventArgs e)
        {
            setRelayClose();
        }

        /// <summary>
        /// 手动触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button3_Click_1(object sender, EventArgs e)
        {
            PlateSettiger();
        }

        /// <summary>
        /// 断开链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button2_Click(object sender, EventArgs e)
        {
            PlateQuitDevice();
        }

        /// <summary>
        /// 485传输
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button5_Click(object sender, EventArgs e)
        {
            Send485Action(DataTextBox.Text);
        }

        /// <summary>
        /// 打开视频
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenButton_Click(object sender, EventArgs e)
        {
            PlayAction(true);
        }

        /// <summary>
        /// 关闭视频
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, EventArgs e)
        {
            PlayAction(false);
        }

        /// <summary>
        /// 设置IP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetIpButton_Click(object sender, EventArgs e)
        {
            SetIpAction(Properties.Settings.Default.LocalIp);
        }

        /// <summary>
        /// 设置保存路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetSaveButton_Click(object sender, EventArgs e)
        {
            SetSaveImg(Properties.Settings.Default.PlatePicSavrPath);
        }

        /// <summary>
        /// 搜索设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchButton_Click(object sender, EventArgs e)
        {
            PlateSearchDevice();
        }
    }
}
