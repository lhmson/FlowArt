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
    public partial class frmInfo : Form
    {
        public frmInfo()
        {
            InitializeComponent();

            // round the corner of form
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            InitMoveFormHandlers();
        }

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


        // Event Handlers //////////////////////////////////////////////////////////////////////////////////////////////////// 
        private void btnOK_Click(object sender, EventArgs e)
        {
            UpdateObject();
            this.Close();
        }


        // User drag to move form /////////////////////////////////////////////////////////////////////////////////////////////////////////// 
        private bool MouseIsDown = false;
        private Point MouseDownMouseLocation;
        private Point MouseDownFormLocation;

        private void frmScan_MouseDownMoveForm(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseIsDown = true;
                MouseDownMouseLocation = Cursor.Position;
                MouseDownFormLocation = this.Location;
            }
        }

        private void frmScan_MouseMoveMoveForm(object sender, MouseEventArgs e)
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

        private void frmScan_MouseUpMoveForm(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                MouseIsDown = false;
        }

        private void InitMoveFormHandlers()
        {
            this.MouseDown += frmScan_MouseDownMoveForm;
            this.MouseUp += frmScan_MouseUpMoveForm;
            this.MouseMove += frmScan_MouseMoveMoveForm;

            foreach (Control control in Controls)
                if (control is Label)
                {
                    control.MouseDown += frmScan_MouseDownMoveForm;
                    control.MouseUp += frmScan_MouseUpMoveForm;
                    control.MouseMove += frmScan_MouseMoveMoveForm;
                }
        }

        //-----------------


        protected void EnableControls(bool enable)
        {
            this.tbName.Enabled = enable;
            this.btnOK.Enabled = enable;
        }

        protected void UpdateDialog()
        {
            if (this.Doc == null) return;
            this.tbName.Text = this.Doc.Name;
            EnableControls(!this.Doc.IsReadOnly);
        }

        protected void UpdateObject()
        {
            if (this.Doc == null) return;
            this.Doc.StartTransaction();

            //---------------
            this.Doc.Name = this.tbName.Text;

            this.Doc.FinishTransaction("Change Design Properties");

        }

        public FlowDocument Doc
        {
            get { return myDoc; }
            set
            {
                if (value == null) return;
                myDoc = value;
                UpdateDialog();
            }
        }

        private FlowDocument myDoc = null;

        
    }


}
