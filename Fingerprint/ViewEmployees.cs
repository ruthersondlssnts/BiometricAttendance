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
    public partial class ViewEmployees : Form
    {
        public ViewEmployees()
        {
            InitializeComponent();
        }
        AttendanceDBEntities _db = new AttendanceDBEntities();

        private void ViewEmployees_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = _db.Employees.ToList().AsReadOnly();
        }
    }
}
