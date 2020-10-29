using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fingerprint
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            lblInfo register = new lblInfo();
            register.ShowDialog();
        }

        private void btnViewEmployees_Click(object sender, EventArgs e)
        {
            ViewEmployees viewEmployees = new ViewEmployees();
            viewEmployees.ShowDialog();
        }

        private void btnTimeIn_Click(object sender, EventArgs e)
        {
            TimeIn timeIn = new TimeIn();
            timeIn.ShowDialog();
        }

        private void btnTimeOut_Click(object sender, EventArgs e)
        {
            TimeOut timeOut = new TimeOut();
            timeOut.ShowDialog();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }
    }
}
