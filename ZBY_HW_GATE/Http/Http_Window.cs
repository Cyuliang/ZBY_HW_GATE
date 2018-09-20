using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZBY_HW_GATE.Http
{
    public partial class Http_Window : Form
    {
        CHttp CHttp_ = new CHttp();
        private delegate string CHttpDelegate();
        private CHttpDelegate GetCHttp;

        public Http_Window()
        {
            InitializeComponent();
            GetCHttp += CHttp_.SetJosn;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(GetCHttp());
        }
    }
}
