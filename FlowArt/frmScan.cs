using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices; // include this to do the round corner trick
using Northwoods.Go;

namespace FlowArt
{
    /// <summary>
    /// Summary description for frmScan
    /// </summary>
    partial class frmScan : Form
    {
        private Label lbType;
        private TextBox tbValue;
        private Button btnOK;

        DataType dataType;
        public VarInfo VarInfoResult = VarInfo.NullVal;

        public frmScan(string VarName, DataType dataType)
        {
            InitializeComponent();
            // round the corner of form
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            
            // init textbox value
            tbName.Text = VarName;
            tbType.Text = dataType.ToString().Remove(0,3);
            this.dataType = dataType;
            
            // select user interacting control
            if(dataType == DataType.FA_Boolean)
            {
                tbValue.Hide();
                cboxValue.Show();
                cboxValue.Focus();

                cboxValue.Items.Add("TRUE");
                cboxValue.Items.Add("FALSE");
                cboxValue.Text = "TRUE";
            }
            else
            {
                tbValue.Show();
                cboxValue.Hide();
                if (dataType == DataType.FA_Number)
                    tbValue.Text = "0";

                tbValue.Focus();
                tbValue.SelectionStart = 0; 
                tbValue.SelectionLength = tbValue.Text.Length;
            }

            InitMoveFormHandlers();
        }

        private bool ValidValue()
        {
            if (dataType == DataType.FA_Boolean)
                return true;

            string value = tbValue.Text;
            return VarInfo.ValidValue(value, dataType);
        }
        
        private void tbValue_UpdateCheckValidValue(object sender, EventArgs e)
        {
            //int selectionStartSaved = tbValue.SelectionStart;
            if( ! ValidValue() )
            {
                tbValue.ForeColor = Color.OrangeRed;
                tbValue.BackColor = Color.FromArgb(255, 217, 215); // pastel red color
            }
            else
            {
                tbValue.ForeColor = Color.Black;
                tbValue.BackColor = Color.White;
            }
            //tbValue.SelectionStart = selectionStartSaved;
        }


        private void btnOK_Click(object sender, System.EventArgs e)
        {
            if (ValidValue())
            {
                string value;
                if (dataType == DataType.FA_Boolean)
                    value = cboxValue.Text;
                else
                    value = tbValue.Text;

                VarInfoResult = new VarInfo(dataType, value);
                this.DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                tbValue.Focus();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


        // User drag to move form /////////////////////////////////////////////////////////////////////////////////////////////////////////// 
        private bool MouseIsDown = false;
        private Point MouseDownMouseLocation;
        private Point MouseDownFormLocation;

        private void frmScan_MouseDownMoveForm(object sender, MouseEventArgs e)
        {
            if( e.Button == MouseButtons.Left )
            {
                MouseIsDown = true;
                MouseDownMouseLocation = Cursor.Position;
                MouseDownFormLocation = this.Location;
            }
        }

        private void frmScan_MouseMoveMoveForm(object sender, MouseEventArgs e)
        {
            if( MouseIsDown && e.Button == MouseButtons.Left )
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
            if( e.Button == MouseButtons.Left )
                MouseIsDown = false;
        }

        private void InitMoveFormHandlers()
        {
            this.MouseDown += frmScan_MouseDownMoveForm;
            this.MouseUp += frmScan_MouseUpMoveForm;
            this.MouseMove += frmScan_MouseMoveMoveForm;

            foreach(Control control in Controls)
                if( control is Label )
                {
                    control.MouseDown += frmScan_MouseDownMoveForm;
                    control.MouseUp += frmScan_MouseUpMoveForm;
                    control.MouseMove += frmScan_MouseMoveMoveForm;
                }
        }

        
    }
}