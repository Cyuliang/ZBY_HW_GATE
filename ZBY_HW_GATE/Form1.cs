using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ZBY_HW_GATE
{
    public partial class Form1 : Form
    {
        private Container.Container_Window Container_ = new Container.Container_Window();
        private Plate.Plate_Window Plate_ = new Plate.Plate_Window();
        private CVR.CVR_Window CVR_ = new CVR.CVR_Window();
        private Gate.Gate_Window Gate_ = new Gate.Gate_Window();
        private LED.LED_Window LED_ = new LED.LED_Window();
        private DataBase.DataBase_Window DataBase_ = new DataBase.DataBase_Window();
        private IEDataBase.InData_Window InData_ = new IEDataBase.InData_Window();
        private IEDataBase.OutData_Window OutData_ = new IEDataBase.OutData_Window();

        private TabPage ContainerTable = new TabPage("集装箱");
        private TabPage PlateTable = new TabPage("电子车牌");
        private TabPage CvrTable = new TabPage("身份证");
        private TabPage GateTable = new TabPage("道闸");
        private TabPage LedTable = new TabPage("显示屏");
        private TabPage PrintTable = new TabPage("打印机");
        private TabPage ScanerTable = new TabPage("扫描仪");
        private TabPage ServerTable = new TabPage("服务端");
        private TabPage ClientTable = new TabPage("客户端");
        private TabPage HttpTable = new TabPage("HTTP");
        private TabPage LocalTable = new TabPage("本地数据库");
        private TabPage InTable = new TabPage("入闸数据库");
        private TabPage OutTable = new TabPage("出闸数据库");
        private TabPage AboutTable = new TabPage("系统说明");

        /// <summary>
        /// page也from对象
        /// </summary>
        private Dictionary<TabPage, Form> tablItem = new Dictionary<TabPage, Form>();

        /// <summary>
        /// 启动时间
        /// </summary>
        DateTime beginTime = DateTime.Now;

        public Form1()
        {
            InitializeComponent();
            //删除Page按钮
            ClosePagetoolStripButton.Enabled = false;
            Container_.StatusDelegate += ContainerStatuesDelegate;
            Container_.ContainerEvent += Container__ContainerEvent;
        }

        /// <summary>
        /// 集装箱数据Log
        /// </summary>
        /// <param name="mes"></param>
        private void Container__ContainerEvent(string mes)
        {
            if (MainListBox.Items.Count > 300)
            {
                MainListBox.Items.Clear();
            }
            MainListBox.Items.Add(mes);
            MainListBox.SelectedIndex = MainListBox.Items.Count - 1;
        }



        /// <summary>
        /// 箱号链接状态
        /// </summary>
        /// <param name="status"></param>
        private void ContainerStatuesDelegate(string status)
        {
            if(status=="1")
            {
                toolStripStatusLabel2.BackColor = Color.DarkOrange;
            }
            if(status=="0")
            {
                toolStripStatusLabel2.BackColor = Color.DarkRed;
            }
        }

        /// <summary>
        /// 箱号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContianerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetTabPate("ContainerTable", ContainerTable, Container_);
        }

        /// <summary>
        /// 车牌
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetTabPate("PlateTable", PlateTable, Plate_);
        }

        /// <summary>
        /// 道闸
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetTabPate("GateTable", GateTable, Gate_);
        }

        /// <summary>
        /// 读卡器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReaderCardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetTabPate("CvrTable", CvrTable, CVR_);
        }

        /// <summary>
        /// 本地数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LocalToolStripMenuItem_Click(object sender, EventArgs e)
        {
           SetTabPate("LocalTable", LocalTable, DataBase_);
        }

        /// <summary>
        /// 入闸数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetTabPate("InTable", InTable, InData_);
        }

        /// <summary>
        /// 窗口关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Container_.Disconnect();
            Gate_.delegatesStopDoorState();
            Application.ExitThread();
        }

        /// <summary>
        /// LED
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetTabPate("LedTable", LedTable, LED_);
        }

        /// <summary>
        /// 出闸数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutSluiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetTabPate("OutTable", OutTable, OutData_);
        }

        /// <summary>
        /// 删除page页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_DoubleClick_1(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex != 0)
            {
                tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
            }
        }

        /// <summary>
        /// 删除页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex != 0)
            {
                foreach (Control form in tabControl1.SelectedTab.Controls)
                {
                    Form f = (Form)form;
                    f.Close();
                }
                tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
            }
        }

        /// <summary>
        /// 显示当前选取页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void tabControl1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                ClosePagetoolStripButton.Enabled = false;
            }
            else
            {
                ClosePagetoolStripButton.Enabled = true;
            }
            PagetoolStripStatusLabel2.Text = tabControl1.SelectedTab.Text;

        }

        /// <summary>
        /// 运行时长
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            var endTime = DateTime.Now.Subtract(beginTime);
            int Hours = (int)endTime.TotalHours;
            int Minutes = (int)endTime.TotalMinutes%60;
            int Seconds = (int)endTime.TotalSeconds%60;
            TimetoolStripStatusLabel.Text = string.Format("系统运行：{0:d}小时{1}分钟{2}秒", Hours,Minutes,Seconds);
        }

        /// <summary>
        /// 添加Page页面
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="tabPage"></param>
        /// <param name="form"></param>
        private void SetTabPate(string Name,TabPage tabPage,Form form)
        {
            if (ErgodicModiForm(Name, tabControl1))
            {
                tabPage.Name = Name;
                tabControl1.Controls.Add(tabPage);
                form.TopLevel = false;
                form.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Show();
                tabPage.Controls.Add(form);
            }
            tabControl1.SelectedTab = tabPage;
        }

        /// </summary>  
        /// <param name="MainTabControlKey">选项卡的键值</param>  
        /// <param name="objTabControl">要添加到的TabControl对象</param>  
        /// <returns></returns>  
        private Boolean ErgodicModiForm(string MainTabControlKey, TabControl objTabControl)
        {
            //遍历选项卡判断是否存在该子窗体  
            foreach (Control con in objTabControl.Controls)
            {
                TabPage tab = (TabPage)con;
                if (tab.Name == MainTabControlKey)
                {
                    return false;//存在  
                }
            }
            return true;//不存在  
        }

        #region//集装箱
        /// <summary>
        /// 链接集装箱号码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Container_.ContainerLink();
        }

        /// <summary>
        /// 断开集装箱号码链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Container_.ContainerClose();
        }

        /// <summary>
        /// 获取最后一次结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Container_.ContainerLastR();
        }
        #endregion
    }
}
