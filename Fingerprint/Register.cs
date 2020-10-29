using AxZKFPEngXControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fingerprint
{
    public partial class lblInfo : Form
    {
        private AxZKFPEngX ZkFprint = new AxZKFPEngX();
        AttendanceDBEntities _db = new AttendanceDBEntities();
        public lblInfo()
        {
            InitializeComponent();
            
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (txtName.Text!="")
            {
                fpicture.Image = null;
                ZkFprint.OnImageReceived += zkFprint_OnImageReceived;
                ZkFprint.OnFeatureInfo += zkFprint_OnFeatureInfo;
                ZkFprint.OnEnroll += zkFprint_OnEnroll;
                ZkFprint.CancelEnroll();
                ZkFprint.EnrollCount = 3;
                ZkFprint.BeginEnroll();
                ShowHintInfo("Please give fingerprint sample.");
            }
            else
            {
                MessageBox.Show("Enter Name First!");
            }
           
        }

        private void Register_Load(object sender, EventArgs e)
        {
            Controls.Add(ZkFprint);
            InitialAxZkfp();
            btnRegister.Enabled = false;
        }
        private void InitialAxZkfp()
        {
            try
            {

                if (ZkFprint.InitEngine() == 0)
                {
                    ZkFprint.FPEngineVersion = "9";
                    ZkFprint.EnrollCount = 3;
                    deviceSerial.Text += " " + ZkFprint.SensorSN + " Count: " + ZkFprint.SensorCount.ToString() + " Index: " + ZkFprint.SensorIndex.ToString();
                    ShowHintInfo("Device successfully connected");
                }

            }
            catch (Exception ex)
            {
                ShowHintInfo("Device init err, error: " + ex.Message);
            }
        }
        private void zkFprint_OnEnroll(object sender, IZKFPEngXEvents_OnEnrollEvent e)
        {
            if (e.actionResult)
            {
                Employee emp = new Employee();
                emp.Name = txtName.Text;
                emp.Fingerprint = ZkFprint.EncodeTemplate1(e.aTemplate);
                _db.Employees.Add(emp);
                _db.SaveChanges();
                ShowHintInfo("Registration successful.");
                //BEEP AND GREEN
                ZkFprint.ControlSensor(13, 1);
                ZkFprint.ControlSensor(11, 1);
                Thread.Sleep(100);
                ZkFprint.ControlSensor(13, 0);
                ZkFprint.ControlSensor(11, 0);
                MessageBox.Show("Saved");
                
                
                txtName.Text = "";
                ShowHintInfo("Click Register Button to Register");
                //Unregister Events
                ZkFprint.OnImageReceived -= zkFprint_OnImageReceived;
                ZkFprint.OnFeatureInfo -= zkFprint_OnFeatureInfo;
                ZkFprint.OnEnroll -= zkFprint_OnEnroll;
                btnRegister.Enabled = false;
            }
            else
            {
                //BEEP AND RED
                ZkFprint.ControlSensor(13, 1);
                ZkFprint.ControlSensor(12, 1);
                Thread.Sleep(100);
                ZkFprint.ControlSensor(13, 0);
                ZkFprint.ControlSensor(12, 0);
                ShowHintInfo("Error, please click register again.");
                //Unregister Events
                ZkFprint.OnImageReceived -= zkFprint_OnImageReceived;
                ZkFprint.OnFeatureInfo -= zkFprint_OnFeatureInfo;
                ZkFprint.OnEnroll -= zkFprint_OnEnroll;
            }
        }
        private void ShowHintInfo(String s)
        {
            prompt.Text = s;
        }
        private void zkFprint_OnFeatureInfo(object sender, IZKFPEngXEvents_OnFeatureInfoEvent e)
        {

            String strTemp = string.Empty;
            if (ZkFprint.EnrollIndex != 1)
            {
                if (ZkFprint.IsRegister)
                {
                    if (ZkFprint.EnrollIndex - 1 > 0)
                    {
                        int eindex = ZkFprint.EnrollIndex - 1;
                        strTemp = "Please scan again ..." + eindex;
                    }
                }
            }
            ShowHintInfo(strTemp);
        }
        private void zkFprint_OnImageReceived(object sender, IZKFPEngXEvents_OnImageReceivedEvent e)
        {
            Graphics g = fpicture.CreateGraphics();
            Bitmap bmp = new Bitmap(fpicture.Width, fpicture.Height);
            g = Graphics.FromImage(bmp);
            int dc = g.GetHdc().ToInt32();
            ZkFprint.PrintImageAt(dc, 0, 0, bmp.Width, bmp.Height);
            g.Dispose();
            fpicture.Image = bmp;
        }


        private void txtName_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtName.Text == "")
            {
                btnRegister.Enabled = false;
            }
            else
            {
                btnRegister.Enabled = true;
            }
        }
    }
}
