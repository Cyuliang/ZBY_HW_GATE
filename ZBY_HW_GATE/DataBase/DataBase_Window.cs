using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZBY_HW_GATE.DataBase
{
    public partial class DataBase_Window : Form
    {
        public static DataBase_Window DataBase_window;
        private CLog Log_ = new CLog();
        private delegate void UpdateUiDelegate();


        public DataBase_Window()
        {
            InitializeComponent();

            DataBase_window = this;
        }

        /// <summary>
        /// 窗口加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataBase_Window_Load(object sender, System.EventArgs e)
        {
            Init_ShowWindow();
            if(FindTextBox.Text==string.Empty)
            {
                FindButton.Enabled = false;
            }
        }

        public void Init_ShowWindow()
        {
            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.Invoke(new UpdateUiDelegate(Init_ShowWindow), new object[] { });
            }
            else
            {
                dataSet1 = DataBase.MySqlHelper.GetDataSet(DataBase.MySqlHelper.Conn, CommandType.Text, "select * from hw.gate", null);
                bindingSource1.DataSource = dataSet1.Tables[0];
                bindingNavigator1.BindingSource = bindingSource1;
                dataGridView1.DataSource = bindingSource1;
                dataGridView1.Columns[0].Visible = false;
            }
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FindButton_Click(object sender, EventArgs e)
        {
            string cmdText = string.Format("SELECT *  FROM `hw`.`gate` WHERE Plate='{0}' or Container='{1}' or Cards='{2}'", FindTextBox.Text, FindTextBox.Text, FindTextBox.Text);
            MySqlDataReader reader = MySqlHelper.ExecuteReader(MySqlHelper.Conn, CommandType.Text, cmdText, null);
            if (reader.Read())
            {
                MessageBox.Show(String.Format("Find Data\n Plate={0}\n Container={1}\n Cards={2}\n", reader[1].ToString(), reader[2].ToString(), reader[7].ToString()));
                if (reader[1].ToString() != string.Empty)
                {
                    ReturnDataView(reader[1].ToString(), 1);
                }
                else if (reader[2].ToString() != string.Empty)
                {
                    ReturnDataView(reader[2].ToString(), 2);
                }
                else
                {
                    if (reader[7].ToString() != string.Empty)
                    {
                        ReturnDataView(reader[7].ToString(), 7);
                    }
                }
            }
            else
            {
                MessageBox.Show("Not Find Data");
            }
        }

        /// <summary>
        /// 循环判断数据
        /// </summary>
        /// <param name="reader"></param>
        private void ReturnDataView(string reader, int index)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (reader == dataGridView1.Rows[i].Cells[index].Value.ToString())
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[index];
                    break;
                }
            }
        }

        /// <summary>
        /// textBox事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FindTextBox_TextChanged(object sender, EventArgs e)
        {
            if(FindTextBox.Text!=string.Empty)
            {
                FindButton.Enabled = true;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            int rowindex = dataGridView1.CurrentCell.RowIndex;
            DialogResult but = MessageBox.Show(string.Format("Confirm deletion Plate={0}  Container={1}  Card={2} ? ", dataGridView1.Rows[rowindex].Cells[1].Value.ToString(), dataGridView1.Rows[rowindex].Cells[2].Value.ToString(), dataGridView1.Rows[rowindex].Cells[7].Value.ToString()), "提示", MessageBoxButtons.YesNo);
            if (but == DialogResult.Yes)
            {
                try
                {
                    string drop = string.Format("DELETE FROM `hw`.`gate` WHERE (`Id` = '{0}')", dataGridView1.Rows[rowindex].Cells[0].Value.ToString());
                    MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, System.Data.CommandType.Text, drop, null);
                    Init_ShowWindow();
                    Log_.logInfo.Info(drop);
                    MessageBox.Show("Delete Success!");
                }
                catch (Exception ex)
                {
                    Log_.logError.Error("Delete Fail", ex);
                }
            }
            else if (but == DialogResult.No)
            {
                MessageBox.Show("Nothing!!!");
            }
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = MySqlHelper.GetDataSet(MySqlHelper.Conn, System.Data.CommandType.Text, "select * from gate", null).Tables[0].DefaultView;
            DataItem dataItem = new DataItem();
            dataItem.ShowDialog();
            Init_ShowWindow();
            dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1];
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            int rowindex = dataGridView1.CurrentCell.RowIndex;
            DataItem dataItem = new DataItem();
            dataItem.UpdataUi(int.Parse(dataGridView1.Rows[rowindex].Cells[0].Value.ToString()));
            dataItem.ShowDialog();
            Init_ShowWindow();
            dataGridView1.CurrentCell = dataGridView1.Rows[rowindex].Cells[1];
        }

        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton3_Click(object sender, EventArgs e)
        {        
            if (dataGridView1.Rows.Count > 0)
            {
                string saveFileName = "";
                //bool fileSaved = false;  
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    DefaultExt = "xls",
                    Filter = "Excel文件|*.xls",
                    FileName = string.Format("{0:yyyyMMddHHmmss}.xls", DateTime.Now)
                };
                saveDialog.ShowDialog();
                saveFileName = saveDialog.FileName;
                if (saveFileName.IndexOf(":") < 0)
                {
                    return; //被点了取消   
                }
                else
                {
                    var t1 = new Task(TaskMethod, saveFileName, TaskCreationOptions.LongRunning);
                    t1.Start();
                }
            }
            else
            {
                MessageBox.Show("报表为空,无表格需要导出", "提示", MessageBoxButtons.OK);
            }
        }

        static object taskMethodLocj = new object();
        static void TaskMethod(object title)
        {
            lock (taskMethodLocj)
            {
                DataGridView dataGridView1 = DataBase_Window.DataBase_window.dataGridView1;

                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                if (xlApp == null)
                {
                    MessageBox.Show("无法创建Excel对象，可能您的机子未安装Excel");
                    return;
                }
                Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
                Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];//取得sheet1  

                //写入标题  
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    worksheet.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
                }
                //写入数值  
                for (int r = 0; r < dataGridView1.Rows.Count; r++)
                {
                    for (int i = 0; i < dataGridView1.ColumnCount; i++)
                    {
                        if (i == 7)
                        {
                            worksheet.Cells[r + 2, i + 1] = "'" + dataGridView1.Rows[r].Cells[i].Value;
                        }
                        else
                        {
                            worksheet.Cells[r + 2, i + 1] = dataGridView1.Rows[r].Cells[i].Value;
                        }
                    }
                    System.Windows.Forms.Application.DoEvents();
                }
                worksheet.Columns.EntireColumn.AutoFit();//列宽自适应  
                                                         //   if (Microsoft.Office.Interop.cmbxType.Text != "Notification")  
                                                         //   {  
                                                         //       Excel.Range rg = worksheet.get_Range(worksheet.Cells[2, 2], worksheet.Cells[ds.Tables[0].Rows.Count + 1, 2]);  
                                                         //      rg.NumberFormat = "00000000";  
                                                         //   }  

                if (title.ToString() != "")
                {
                    try
                    {
                        workbook.Saved = true;
                        workbook.SaveCopyAs(title.ToString());
                        //fileSaved = true;  
                    }
                    catch (Exception ex)
                    {
                        //fileSaved = false;  
                        MessageBox.Show("导出文件时出错,文件可能正被打开！\n" + ex.Message);
                    }

                }
                //else  
                //{  
                //    fileSaved = false;  
                //}  
                xlApp.Quit();
                GC.Collect();//强行销毁   
                             // if (fileSaved && System.IO.File.Exists(saveFileName)) System.Diagnostics.Process.Start(saveFileName); //打开EXCEL  
                MessageBox.Show("导出文件成功", "提示", MessageBoxButtons.OK);
            }
        }

        private void DataBase_Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
