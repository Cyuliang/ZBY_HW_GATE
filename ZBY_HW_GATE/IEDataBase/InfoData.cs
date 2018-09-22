using System;
using System.Data;

namespace ZBY_HW_GATE.IEDataBase
{
    class InfoData
    {
        /// <summary>
        /// 插入车牌
        /// </summary>
        /// <param name="time"></param>
        /// <param name="Plate"></param>
        public void InsertIN(DateTime time, string Plate)
        {
            string Updatetext = string.Format("INSERT INTO `hw`.`indata` (`Plate`,`Time`) VALUES('{0}','{1}')", Plate,time);
            DataBase.MySqlHelper.ExecuteNonQuery(DataBase.MySqlHelper.Conn, CommandType.Text, Updatetext, null);
        }

        /// <summary>
        /// 插入箱号
        /// </summary>
        /// <param name="time"></param>
        /// <param name="Container"></param>
        public void Update(DateTime time , string Container)
        {
            string Updatetext = string.Format("UPDATE  `hw`.`indata` SET `Container` = '{0}' WHERE (`Time` = '{1}')",  Container, time);
            DataBase.MySqlHelper.ExecuteNonQuery(DataBase.MySqlHelper.Conn, CommandType.Text, Updatetext, null);
        }

        /// <summary>
        /// 插入出闸数据
        /// </summary>
        /// <param name="time"></param>
        /// <param name="Plate"></param>
        public void InserOut(DateTime time,string Plate)
        {
            string Updatetext = string.Format("INSERT INTO `hw`.`outdata` (`Plate`,`Time`) VALUES('{0}','{1}')", Plate, time);
            DataBase.MySqlHelper.ExecuteNonQuery(DataBase.MySqlHelper.Conn, CommandType.Text, Updatetext, null);
        }
    }
}
