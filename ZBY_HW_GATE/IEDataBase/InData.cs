using System;
using System.Data;

namespace ZBY_HW_GATE.IEDataBase
{
    class InData
    {
        public void Insert(DateTime time, string Plate, string Container)
        {
            string Updatetext = string.Format("INSERT INTO `hw`.`indata` (`Plate`,`Container`,`Time`) VALUES('{0}','{1}','{2}')", Plate,Container,time);
            DataBase.MySqlHelper.ExecuteNonQuery(DataBase.MySqlHelper.Conn, CommandType.Text, Updatetext, null);
        }

    }
}
