﻿using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace ZBY_HW_GATE.DataBase
{
    public partial class DataItem : Form
    {
        /// <summary>
        /// 更新选区项
        /// </summary>
        private int SelectIndex=-1;

        CLog log = new CLog();

        public DataItem()
        {
            InitializeComponent();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
        }

        /// <summary>
        /// 编辑对话框显示原来信息
        /// </summary>
        /// <param name="index">查询ID</param>
        public void UpdataUi(int index)
        {
            SelectIndex = index;
            string cmdText = string.Format("SELECT *  FROM `hw`.`gate` WHERE Id={0}",index);
            MySqlDataReader reader =MySqlHelper.ExecuteReader(MySqlHelper.Conn, CommandType.Text, cmdText, null);
            reader.Read();
            textBox1.Text = reader[1].ToString();
            textBox2.Text = reader[2].ToString();
            textBox4.Text = reader[3].ToString();
            textBox5.Text = reader[4].ToString();
            textBox6.Text = reader[5].ToString();

            textBox3.Text = reader[7].ToString();
            if(reader[6].ToString()=="1")
            {
                radioButton1.Checked = true;
            }
            else if(reader[6].ToString()=="0")
            {
                radioButton2.Checked = true;
            }
            if(reader[8].ToString()!="")
            {
                string[] dt = (reader[8].ToString().Split(' ')[0]).Split('/');
                DateTime ds = new DateTime(int.Parse(dt[0]), int.Parse(dt[1]), int.Parse(dt[2]));
                dateTimePicker1.Value = ds;
            }
            log.logInfo.Info("Read DataSuccess");
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, EventArgs e)
        {
            int onTime = 0;
            if(radioButton1.Checked)
            {
                onTime = 1;
            }          
            if(SelectIndex !=-1)
            {
                string Updatetext = string.Format("UPDATE `hw`.`gate` SET `Plate` = '{0}',  `Container`='{1}',`Supplier`='{2}', `Appointment`='{3}', `Parked`='{4}', `Ontime`='{5}', `Cards`='{6}', `Truetime`='{7}' WHERE (`Id` = '{8}')",
    textBox1.Text, textBox2.Text, textBox4.Text, textBox5.Text, textBox6.Text, onTime, textBox3.Text, dateTimePicker1.Value.ToShortDateString(), SelectIndex);
                MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, Updatetext, null);
                log.logInfo.Info("Update Data Success");
            }
            else
            {
                string cmdText = string.Format("INSERT INTO `hw`.`gate` (`Plate`, `Container`, `Supplier`, `Appointment`, `Parked`, `Ontime`, `Cards`, `Truetime`) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')",
    textBox1.Text, textBox2.Text, textBox4.Text, textBox5.Text, textBox6.Text, onTime, textBox3.Text, dateTimePicker1.Value.ToShortDateString());
                MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, cmdText, null);
                log.logInfo.Info("Insert into Data Success");
            }
            Close();
            Dispose();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }
    }
}
