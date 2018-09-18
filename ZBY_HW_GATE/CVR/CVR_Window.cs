using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZBY_HW_GATE.CVR
{
    public partial class CVR_Window : Form
    {
        CLog Log_ = new CLog();
        CVR CVR_ = new CVR();
        private bool ReadForBooen = true;

        public Action<string> FillDataUi;
        private delegate void AsynUpdateUi(string mes);
        private delegate void CVRDelegate();
        private delegate void CVRvolatile(bool state);
        private CVRDelegate Authenticatefor;
        private CVRDelegate InitDelegate;
        private CVRDelegate AuthenticateDelegate;
        private CVRDelegate BeginInvokeAuthentiacte;
        private CVRDelegate CloseDelegate;
        private CVRvolatile SetCVRvolatile;

        public CVR_Window()
        {
            InitializeComponent();

            InitDelegate += CVR_.InitComm;
            AuthenticateDelegate += CVR_.Authenticate;
            BeginInvokeAuthentiacte += CVR_.AuthenticateWhile;
            CloseDelegate += CVR_.CloseComm;
            CVR_.FillDataActive = new Action<byte[], byte[], byte[], byte[], byte[], byte[], byte[], byte[],byte[]>(FillData);
            CVR_.FillDataBmpActive = new Action<byte[], int>(FillDataBmp);
            CVR_.MessageDelegate = GetMessage;
            SetCVRvolatile += CVR_.GetStarted;
            Authenticatefor += CVR_.AuthenticateFor;

            InitButton.Hide();
            buttonReadCard.Hide();
            CloseButton.Hide();
            ForReadButton.Hide();
        }
        #region
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitButton_Click(object sender, EventArgs e)
        {
            InitDelegate();
        }
        public void Init()
        {
            InitDelegate();
        }

        /// <summary>
        /// 读卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonReadCard_Click(object sender, EventArgs e)
        {
            AuthenticateDelegate();
        }
        public void Read()
        {
            AuthenticateDelegate();
        }

        /// <summary>
        /// 关闭COM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, EventArgs e)
        {
            CloseDelegate();
        }
        public void CloseC()
        {
            CloseDelegate();
        }
        #endregion

        /// <summary>
        /// log
        /// </summary>
        /// <param name="mes"></param>
        private void GetMessage(string mes)
        {
            if (InvokeRequired)
            {
                LogListBox.Invoke(new AsynUpdateUi(GetMessage), new object[] { mes });
            }
            else
            {
                if (LogListBox.Items.Count > 100)
                {
                    LogListBox.Items.Clear();
                }
                LogListBox.Items.Add(mes);
                LogListBox.SelectedIndex = LogListBox.Items.Count - 1;
            }
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sex"></param>
        /// <param name="peopleNation"></param>
        /// <param name="birthday"></param>
        /// <param name="number"></param>
        /// <param name="address"></param>
        /// <param name="signdate"></param>
        /// <param name="validtermOfStart"></param>
        /// <param name="bCivic"></param>
        public void FillData(byte[] name,byte[] sex,byte[] peopleNation,byte[] birthday,byte[] number,byte[] address,byte[] signdate,byte[] validtermOfStart,byte[] validtermOfEnd)
        {
            FillDataUi(string.Format("姓名：{0} ID：{1}" ,System.Text.Encoding.GetEncoding("GB2312").GetString(name), System.Text.Encoding.GetEncoding("GB2312").GetString(number).Replace("\0", "").Trim()));
            textBox1.Text= System.Text.Encoding.GetEncoding("GB2312").GetString(name);
            textBox2.Text= System.Text.Encoding.GetEncoding("GB2312").GetString(sex).Replace("\0", "").Trim();
            textBox8.Text= System.Text.Encoding.GetEncoding("GB2312").GetString(peopleNation).Replace("\0", "").Trim();
            textBox3.Text= System.Text.Encoding.GetEncoding("GB2312").GetString(birthday).Replace("\0", "").Trim();
            textBox4.Text= System.Text.Encoding.GetEncoding("GB2312").GetString(number).Replace("\0", "").Trim();
            textBox5.Text= System.Text.Encoding.GetEncoding("GB2312").GetString(address).Replace("\0", "").Trim();
            textBox6.Text= System.Text.Encoding.GetEncoding("GB2312").GetString(signdate).Replace("\0", "").Trim();
            textBox7.Text= System.Text.Encoding.GetEncoding("GB2312").GetString(validtermOfStart).Replace("\0", "").Trim() + "-" + System.Text.Encoding.GetEncoding("GB2312").GetString(validtermOfEnd).Replace("\0", "").Trim();                    
        }       

        /// <summary>
        /// 读取头像
        /// </summary>
        /// <param name="imgData"></param>
        public void FillDataBmp(byte[] imgData,int length)
        {
            MemoryStream myStream = new MemoryStream();
            for (int i = 0; i < length; i++)
            {
                myStream.WriteByte(imgData[i]);
            }
            Image myImage = Image.FromStream(myStream);
            pictureBoxPhoto.Image = myImage;
        }

        /// <summary>
        /// 重写关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CVR_Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        /// <summary>
        /// 自动读取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {           
            if(checkBox1.Checked)
            {
                SetCVRvolatile(true);
                BeginInvokeAuthentiacte.BeginInvoke(new AsyncCallback(CallWhileDone), null);
            }
            else
            {
                SetCVRvolatile(false);
            }
        }
        private void CallWhileDone(IAsyncResult ar)
        {
            MessageBox.Show("停止循环");
        }

        /// <summary>
        /// 定时读取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            ForRead();
        }

        /// <summary>
        /// 定时读取
        /// </summary>
        public void ForRead()
        {
            if (ReadForBooen)
            {
                Authenticatefor.BeginInvoke(new AsyncCallback(CallForDone), null);
                ReadForBooen = false;
            }
        }

        private void CallForDone(IAsyncResult ar)
        {
            MessageBox.Show("调用完成");
            ReadForBooen = true;
        }
    }
}
