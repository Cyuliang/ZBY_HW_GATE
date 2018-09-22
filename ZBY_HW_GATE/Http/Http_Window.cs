using System;
using System.Windows.Forms;

namespace ZBY_HW_GATE.Http
{
    public partial class Http_Window : Form
    {
        private CLog Log_ = new CLog();
        private CHttp CHttp_ = new CHttp();

        private delegate string CHttpDelegate(string Id, string Time, string Plate, string Container);
        private delegate void LogDelegate(string mes);
        public delegate void SetLogDelegate(string mes);
        private CHttpDelegate GetCHttp;        

        public Http_Window()
        {
            InitializeComponent();
            GetCHttp += CHttp_.SetJosn;
            CHttp_.SetMessage += Message;

            IDtextBox.Text = Properties.Settings.Default.eqid;            
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            ResuleData(GetCHttp(IDtextBox.Text, TimetextBox.Text, PlatetextBox.Text, ContainertextBox.Text));
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        public void SendData(string Id, string Time, string Plate, string Container)
        {
            ResuleData(GetCHttp(Id, Time, Plate, Container));
        }

        /// <summary>
        /// 结果
        /// </summary>
        /// <param name="es"></param>
        private void ResuleData(string result)
        {
            if (result != "null")
            {
                SplitString(result);
            }
            Message(string.Format("Result:{0}", result));
            Log_.logInfo.Info(string.Format("Result:{0}", result));
        }

        /// <summary>
        /// 分割数据
        /// </summary>
        /// <param name="mes"></param>
        private void SplitString(string mes)
        {
            //string tmp = @"{{""error_code"":""AE0001"",""error_desc"":""the input is empty."",""result"":{""resultList"":""N"",""众百源"",""员工停车位"",""Y""}}}";
            string[] list = mes.Replace("{", "").Replace("}", "").Split(',');
            int i = 0;
            foreach(string s in list)
            {
                string[] n = s.Replace("\"", "").Split(':');
                if(i==0)
                {
                    textBox5.Text = n[1];                       
                }
                if (i == 1)
                {
                    textBox6.Text = n[1];
                }
                if(i==2)
                {
                    textBox7.Text = n[2];
                }
                if(i==3)
                {
                    textBox8.Text = n[0];
                }
                if(i==4)
                {
                    textBox9.Text = n[0];
                }
                if(i==5)
                {
                    textBox10.Text = n[0];
                }
                i++;            
            }
        }

        /// <summary>
        /// log
        /// </summary>
        /// <param name="mes"></param>
        private void Message(string mes)
        {
            if(LoglistBox.InvokeRequired)
            {
                LoglistBox.Invoke(new LogDelegate(Message), new object[] { mes });
            }
            else
            {
                if(LoglistBox.Items.Count>100)
                {
                    LoglistBox.Items.Clear();
                }
                LoglistBox.Items.Add(mes);
            }
        }
    }
}
