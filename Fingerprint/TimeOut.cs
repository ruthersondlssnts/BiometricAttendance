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
    public partial class TimeOut : Form
    {
        private AxZKFPEngX ZkFprint = new AxZKFPEngX();
        AttendanceDBEntities _db = new AttendanceDBEntities();
        public TimeOut()
        {
            InitializeComponent();
        }
        private void ShowHintInfo(String s)
        {
            prompt.Text = s;
        }
        private void btnTimeOut_Click(object sender, EventArgs e)
        {
            fpicture.Image = null;
            lblName.Text = "";
            if (ZkFprint.IsRegister)
            {
                ZkFprint.CancelEnroll();
            }
            ZkFprint.OnCapture += zkFprint_OnCapture;
            ZkFprint.OnImageReceived += zkFprint_OnImageReceived;
            ZkFprint.BeginCapture();
            ShowHintInfo("Please give fingerprint sample.");
        }
        private bool Check;
        private void zkFprint_OnCapture(object sender, IZKFPEngXEvents_OnCaptureEvent e)
        {
            bool isVerified = false;
            //fingerprint to verify
            string template = ZkFprint.EncodeTemplate1(e.aTemplate);

            foreach (var item in _db.Employees)
            {
                if (ZkFprint.VerFingerFromStr(ref template, item.Fingerprint, false, ref Check))
                {
                    ShowHintInfo("Verified");
                    lblName.Text = "Time Out\nName: " + item.Name + "\n" + DateTime.Now.ToString("dddd, dd MMMM yyyy\nhh:mm tt");
                    ZkFprint.OnCapture -= zkFprint_OnCapture;
                    ZkFprint.OnImageReceived -= zkFprint_OnImageReceived;
                    fpicture.Image = null;
                    //add attendance to database realtime
                    Attendance at = new Attendance();
                    at.EmpID = item.ID;
                    at.DateTime = DateTime.Now;
                    at.Type = "Out";
                    _db.Attendances.Add(at);
                    isVerified = true;

                    break;
                }


            }

            if (!isVerified)
            {
                //BEEP AND RED
                ZkFprint.ControlSensor(12, 1);
                ZkFprint.ControlSensor(13, 1);
                Thread.Sleep(100);
                ZkFprint.ControlSensor(12, 0);
                ZkFprint.ControlSensor(13, 0);
                //Unregister Events
                ShowHintInfo("Not Verified");
                lblName.Text = "";
                ZkFprint.OnCapture -= zkFprint_OnCapture;
                ZkFprint.OnImageReceived -= zkFprint_OnImageReceived;
                fpicture.Image = null;
            }
            else
            {
                //BEEP AND GREEN
                ZkFprint.ControlSensor(11, 1);
                ZkFprint.ControlSensor(13, 1);
                Thread.Sleep(100);
                ZkFprint.ControlSensor(11, 0);
                ZkFprint.ControlSensor(13, 0);
                //Unregister Events
                ZkFprint.OnCapture -= zkFprint_OnCapture;
                ZkFprint.OnImageReceived -= zkFprint_OnImageReceived;
                fpicture.Image = null;

                _db.SaveChanges();
                MessageBox.Show("Saved");
            }



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

        private void TimeOut_Load(object sender, EventArgs e)
        {
            Controls.Add(ZkFprint);
            InitialAxZkfp();
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
    }
}
