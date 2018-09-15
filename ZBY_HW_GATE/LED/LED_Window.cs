using System;
using System.Windows.Forms;

namespace ZBY_HW_GATE.LED
{
    public partial class LED_Window : Form
    {
        private LED LED_ = new LED();

        /// <summary>
        /// 初始化委托类
        /// </summary>
        private delegate int LedDelegate();       
        private LedDelegate _Init;
        private LedDelegate _AddScreen;
        private LedDelegate _AddArea;
        private LedDelegate _SendCommand;
        private LedDelegate _UnInit;
        private Func<string[],int> _AddText;

        public LED_Window()
        {
            InitializeComponent();
            
            //Led处理信息委托
            LED_.Led_Return_Message_func += ResultMessage;
            _Init += LED_.Initialize;
            _AddScreen += LED_.AddScreen_Dynamic;
            _AddArea += LED_.AddScreenDynamicArea;
            _AddText = new Func<string[],int>(LED_.AddScreenDynamicAreaText);
            _SendCommand += LED_.SendDynamicAreasInfoCommand;
            _UnInit += LED_.Uninitialize;
        }

        private void LED_Window_Load(object sender, EventArgs e)
        {
            IpTextBox.Text = LED_.pSocketIP;
            PortTextBox.Text = LED_.nSocketPort.ToString();
            InitializeButton.Enabled = true;
            AddScreenButton.Enabled = false;
            AddAreaButton.Enabled = false;
            AddTextButton.Enabled = false;
            SendComButton.Enabled = false;
            UninitializeButton.Enabled = false;
        }

        private void LED_Window_FormClosing(object sender, FormClosingEventArgs e)
        {  
            e.Cancel = true;
            Hide();
        }

        /// <summary>
        /// 回调信息
        /// </summary>
        /// <param name="mes"></param>
        private void ResultMessage(string mes)
        {
            if(mmo_FunResultInfo.Lines.Length>100)
            {
                mmo_FunResultInfo.Clear();
            }
            mmo_FunResultInfo.Text += mes+"\r\n";
            mmo_FunResultInfo.Focus();//获取焦点
            mmo_FunResultInfo.Select(mmo_FunResultInfo.TextLength, 0);//光标定位到文本最后
            mmo_FunResultInfo.ScrollToCaret();//滚动到光标处
        }

        /// <summary>
        /// 初始化动态库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitializeButton_Click(object sender, EventArgs e)
        {
            if(_Init()==0)
            {
                AddScreenButton.Enabled = true;
                UninitializeButton.Enabled = true;
            }
        }

        /// <summary>
        /// 添加显示屏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddScreenButton_Click(object sender, EventArgs e)
        {
            if(_AddScreen()==0)
            {
                AddAreaButton.Enabled = true;
            }
        }

        /// <summary>
        /// 添加动态区域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAreaButton_Click(object sender, EventArgs e)
        {
            if(_AddArea()==0)
            {
                AddTextButton.Enabled = true;
            }
        }

        /// <summary>
        /// 添加动态区域文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTextButton_Click(object sender, EventArgs e)
        {
            string[] pTexts = { PlateTextBox.Text,SupplierTextBox.Text, AppointmentTextBox.Text,ParkedTextBox.Text,OntimeTextBox.Text};
            if(_AddText(pTexts)==0)
            {
                SendComButton.Enabled = true;
            }
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendComButton_Click(object sender, EventArgs e)
        {
            _SendCommand();
        }

        /// <summary>
        /// 释放动态区域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UninitializeButton_Click(object sender, EventArgs e)
        {
            if(_UnInit()==0)
            {
                InitializeButton.Enabled = true;
                AddScreenButton.Enabled = false;
                AddAreaButton.Enabled = false;
                AddTextButton.Enabled = false;
                SendComButton.Enabled = false;
                UninitializeButton.Enabled = false;
            }
        }
    }
}
