using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ZBY_HW_GATE.Container;
using ZBY_HW_GATE.Plate;

namespace ZBY_HW_GATE
{
    public partial class Form1 : Form
    {

        Container.Container_Window Container_ = new Container_Window();
        Plate.Plate_Window Plate_ = new Plate_Window();
        CVR.CVR_Window CVR_ = new CVR.CVR_Window();
        Gate.Gate_Window Gate_ = new Gate.Gate_Window();
        LED.LED_Window LED_ = new LED.LED_Window();
        DataBase.DataBase_Window dataBase_Window = new DataBase.DataBase_Window();
        IEDataBase.InData_Window InData_ = new IEDataBase.InData_Window();
        IEDataBase.OutData_Window OutData_ = new IEDataBase.OutData_Window();
       
        TabPage ContainerTable = new TabPage("集装箱");
        TabPage PlateTable = new TabPage("电子车牌");
        TabPage CvrTable = new TabPage("身份证");
        TabPage GateTable = new TabPage("道闸");
        TabPage LedTable = new TabPage("显示屏");
        TabPage PrintTable = new TabPage("打印机");
        TabPage ScanerTable = new TabPage("扫描仪");
        TabPage ServerTable = new TabPage("服务端");
        TabPage ClientTable = new TabPage("客户端");
        TabPage HttpTable = new TabPage("HTTP");
        TabPage LocalTable = new TabPage("本地数据库");
        TabPage InTable = new TabPage("入闸数据库");
        TabPage OutTable = new TabPage("出闸数据库");
        TabPage AboutTable = new TabPage("系统说明");

        DateTime beginTime = DateTime.Now;

        public Form1()
        {
            InitializeComponent();
            //删除Page按钮
            ClosePagetoolStripButton.Enabled = false;
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
           SetTabPate("LocalTable", LocalTable, dataBase_Window);
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
    }
}
