using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZBY_HW_GATE.SCSocket
{
    public partial class Server_Window : Form
    {
        public Server_Window()
        {
            InitializeComponent();
        }

        private void Server_Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
