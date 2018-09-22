using System;
using System.Windows.Forms;

namespace ZBY_HW_GATE.SCSocket
{
    public partial class Client_Window : Form
    {
        private Client Client_ = new Client();
        private delegate void SetMessageDelegate(string mes);
        private delegate void ClientDelegate();
        private delegate void ClientDataDelegate(string mes);
        private ClientDelegate GetClienLink;
        private ClientDataDelegate GetClientSend;

        public Client_Window()
        {
            InitializeComponent();

            textBox1.Text = Properties.Settings.Default.SockerClientIp;
            textBox2.Text = Properties.Settings.Default.SocketClientPort.ToString();
            GetClienLink += Client_.Link;
            GetClientSend += Client_.SendData;
            Client_.SetMessage += Message;
        }

        private void Client_Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        /// <summary>
        /// 链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LinkToolStripButton_Click(object sender, EventArgs e)
        {
            GetClienLink();
        }


        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestToolStripButton_Click(object sender, EventArgs e)
        {
            GetClientSend(textBox3.Text);
        }

        public void Message(string mes)
        {
            if (LogListBox.InvokeRequired)
            {
                LogListBox.Invoke(new SetMessageDelegate(Message), new object[] { mes });
            }
            else
            {
                if (LogListBox.Items.Count > 100)
                {
                    LogListBox.Items.Clear();
                }
                LogListBox.Items.Add(mes);
            }
        }
    }
}
