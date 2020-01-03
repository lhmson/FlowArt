using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices; // include this to do the round corner trick
using Northwoods.Go;

namespace FlowArt
{
    public partial class frmRunSetting : Form
    {
        public frmRunSetting(bool AutoRun, int Speed, bool FadingEffect, bool SoundEffect, bool ClearResult, bool NotifyOnEnd)
        {
            InitializeComponent();

            cbAutoRun.Checked = AutoRun;
            trbSpeed.Value = Speed;
            cbFading.Checked = FadingEffect;
            cbSound.Checked = SoundEffect;
            cbClearResult.Checked = ClearResult;
            cbNotifyOnEnd.Checked = NotifyOnEnd;

            // round the corner of form
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            InitMoveFormHandlers();
        }


        // round corners //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")] // this one rounds the corner
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );


        // User drag to move form /////////////////////////////////////////////////////////////////////////////////////////////////////////// 
        private bool MouseIsDown = false;
        private Point MouseDownMouseLocation;
        private Point MouseDownFormLocation;

        private void frmRunSetting_MouseDownMoveForm(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseIsDown = true;
                MouseDownMouseLocation = Cursor.Position;
                MouseDownFormLocation = this.Location;
            }
        }

        private void frmRunSetting_MouseMoveMoveForm(object sender, MouseEventArgs e)
        {
            if (MouseIsDown && e.Button == MouseButtons.Left)
            {
                this.Location = new Point
                (
                    MouseDownFormLocation.X + Cursor.Position.X - MouseDownMouseLocation.X,
                    MouseDownFormLocation.Y + Cursor.Position.Y - MouseDownMouseLocation.Y
                );
            }
        }

        private void frmRunSetting_MouseUpMoveForm(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                MouseIsDown = false;
        }

        private void InitMoveFormHandlers()
        {
            this.MouseDown += frmRunSetting_MouseDownMoveForm;
            this.MouseUp += frmRunSetting_MouseUpMoveForm;
            this.MouseMove += frmRunSetting_MouseMoveMoveForm;

            foreach (Control control in Controls)
                if (control is Label)
                {
                    control.MouseDown += frmRunSetting_MouseDownMoveForm;
                    control.MouseUp += frmRunSetting_MouseUpMoveForm;
                    control.MouseMove += frmRunSetting_MouseMoveMoveForm;
                }
        }

        
        // get form result //////////////////////////////////////////////////////////////////////////////////////////////////// 
        public bool AutoRunResult;
        public bool FadingEffectResult;
        public bool ClearResultResult;
        public int SpeedResult;
        public bool SoundEffectResult;
        public bool NotificationResult;

        private void GetResult()
        {
            AutoRunResult = cbAutoRun.Checked;
            FadingEffectResult = cbFading.Checked;
            ClearResultResult = cbClearResult.Checked;
            NotificationResult = cbNotifyOnEnd.Checked;
            SoundEffectResult = cbSound.Checked;

            SpeedResult = trbSpeed.Value;
        }

        // Event Handlers //////////////////////////////////////////////////////////////////////////////////////////////////// 
        private void btnOK_Click(object sender, EventArgs e)
        {
            GetResult();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cbAutoRun_CheckedChanged_EnableSpeed(object sender, EventArgs e)
        {
            trbSpeed.Enabled = cbAutoRun.Checked;
        }
    }
}
