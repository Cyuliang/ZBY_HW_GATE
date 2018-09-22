using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace ZBY_HW_GATE.Plate
{
    public partial class Plate_Window : Form
    {
        private CLog Log_ = new CLog();
        private Plate Plate_ = new Plate();
        private System.Threading.Timer timer_PlateLink;

        private delegate void PlateDataDelegate(string Ip, string Plate, string Color, string Time);
        public delegate void SetPlateDelegate(uint state);
        private delegate void SetRelayCloseDelegate();
        private delegate void UpdateUiDelegate(string mes);
        private delegate void PlateDelegate();
        public delegate void PlateResultDelegate(string resule);
        public event PlateResultDelegate PlateResultEvent;

        public event SetPlateDelegate SetPlateStates;
        private PlateDelegate PlateStar;
        private PlateDelegate PlateSettiger;
        private PlateDelegate PlateQuitDevice;
        private PlateDelegate PlateSearchDevice;
        private SetRelayCloseDelegate setRelayClose;
        private Action<bool> PlayAction;
        private Action<string> Send485Action;
        private Action<string> SetIpAction;
        private Action<string> SetSaveImg;

        public Plate_Window()
        {
            InitializeComponent();

            timer_PlateLink = new System.Threading.Timer(PlateTimeLink, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));
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

            PlateIpTextBox.Text = Properties.Settings.Default.PlateIPAddr;
            PlatePortTextBox.Text = Properties.Settings.Default.PlatePort.ToString();
            PlateSetIp();
            PlateSetPath();

            LinkButton.Hide();
            AbortButton.Hide();
            TiggerButton.Hide();
            LinkButton.Hide();
            LiftingButton.Hide();
            TransmissionButton.Hide();
            SearchButton.Hide();
            SetIpButton.Hide();
            SetPathButton.Hide();
            OpenButton.Hide();
            CloseButton.Hide();
            OnLineLable.Hide();
        }

        /// <summary>
        /// 自动链接
        /// </summary>
        /// <param name="o"></param>
        private void PlateTimeLink(object o)
        {
            PlateStar();
        }

        /// <summary>
        /// Log
        /// </summary>
        /// <param name="mes"></param>
        private void Message(string mes)
        {
            if (LogListBox.InvokeRequired)
            {
                LogListBox.Invoke(new UpdateUiDelegate(Message), new object[] { mes });
            }
            else
            {
                if (LogListBox.Items.Count > 100)
                {
                    LogListBox.Items.RemoveAt(0);
                }
                LogListBox.Items.Add(mes);
                LogListBox.SelectedIndex = LogListBox.Items.Count - 1;
            }
        }

        /// <summary>
        /// 相机状态
        /// </summary>
        /// <param name="str"></param>
        /// <param name="status"></param>
        private void PlateCallBack(string str,uint status)
        {
            if(SetPlateStates!=null)
            {
                SetPlateStates(status);
            }
            if (status==1)
            {
                OnLineLable.BackColor = Color.DarkGreen;
                //Message(string.Format("{0} Plate OnLine",str));
                //Log_.logInfo.Info(string.Format("{0} Plate OnLine", str));
                timer_PlateLink.Change(-1, -1);
            }
            if(status==0)
            {
                OnLineLable.BackColor = Color.DarkRed;
                //Message(string.Format("{0} Plate Drop line", str));
                Log_.logWarn.Warn(string.Format("{0} Plate Drop line", str));
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
        public void PlateLink()
        {       
            timer_PlateLink.Change(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));
            //PlateStar();
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
            if(TimeTextBox.InvokeRequired)
            {
                TimeTextBox.Invoke(new PlateDataDelegate(PlateDataInfoOut), new object[] { Ip, Plate, Color, Time });
            }
            else
            {
                TimeTextBox.Text = Time;
                IpTextBox.Text = Ip;
                PlateTextBox.Text = Plate;
                ColorTextBox.Text = Color;
                PlateResultEvent(string.Format("Plate Result Time：{0} Plate：{1}", Time, Plate));
            } 
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
        public void PlateLifting()
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
        public void PlateTigger()
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
        public void PlateAbort()
        {
            timer_PlateLink.Change(-1, -1);
            PlateQuitDevice();           
            //new System.Threading.Timer(CloseState, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(0));
        }
        //private void CloseState(object o)
        //{
        //    SetPlateStates(0);
        //}

        /// <summary>
        /// 485传输
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button5_Click(object sender, EventArgs e)
        {
            Send485Action(DataTextBox.Text);
        }
        public void PlateSend()
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
        public void PlateOpenPlay()
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
        public void PlateClosePlay()
        {
            PlayAction(false);
            new System.Threading.Timer(ClearPic,null,TimeSpan.FromSeconds(5),TimeSpan.FromSeconds(0));
        }

        /// <summary>
        /// 清除VIDEO
        /// </summary>
        /// <param name="state"></param>
        private void ClearPic(object state)
        {
            ClearPicture();
        }
        delegate void clearDle();
        private void ClearPicture()
        {
            if (pictureBox1.InvokeRequired)
            {
                pictureBox1.Invoke(new clearDle(ClearPicture), new object[] { });
            }
            else
            {
                pictureBox1.Image = null;
                pictureBox1.Refresh();
            }
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
        public void PlateSetIp()
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
        public void PlateSetPath()
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
        public void PlateSera()
        {
            PlateSearchDevice();
        }
    }
}
