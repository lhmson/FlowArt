using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices; // include this to do the round corner trick
using Northwoods.Go;

namespace FlowArt
{
    partial class frmScan : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        public frmScan()
        {
            InitializeComponent();
            // round the corner of form
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }



        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbValue = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.lbType = new System.Windows.Forms.Label();
            this.lbName = new System.Windows.Forms.Label();
            this.lbValue = new System.Windows.Forms.Label();
            this.tbType = new System.Windows.Forms.TextBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lbTitle = new System.Windows.Forms.Label();
            this.cboxValue = new System.Windows.Forms.ComboBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbValue
            // 
            this.tbValue.BackColor = System.Drawing.SystemColors.Info;
            this.tbValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbValue.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbValue.Location = new System.Drawing.Point(162, 135);
            this.tbValue.Name = "tbValue";
            this.tbValue.Size = new System.Drawing.Size(200, 19);
            this.tbValue.TabIndex = 1;
            this.tbValue.TextChanged += new System.EventHandler(this.tbValue_UpdateCheckValidValue);
            // 
            // btnOK
            // 
            this.btnOK.AutoSize = true;
            this.btnOK.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnOK.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(245, 170);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(99, 36);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lbType
            // 
            this.lbType.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbType.Location = new System.Drawing.Point(51, 94);
            this.lbType.Name = "lbType";
            this.lbType.Size = new System.Drawing.Size(72, 20);
            this.lbType.TabIndex = 0;
            this.lbType.Text = "Type:";
            // 
            // lbName
            // 
            this.lbName.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbName.Location = new System.Drawing.Point(51, 54);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(72, 20);
            this.lbName.TabIndex = 4;
            this.lbName.Text = "Name:";
            // 
            // lbValue
            // 
            this.lbValue.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbValue.Location = new System.Drawing.Point(51, 134);
            this.lbValue.Name = "lbValue";
            this.lbValue.Size = new System.Drawing.Size(72, 20);
            this.lbValue.TabIndex = 5;
            this.lbValue.Text = "Value:";
            // 
            // tbType
            // 
            this.tbType.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.tbType.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbType.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbType.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tbType.Location = new System.Drawing.Point(162, 95);
            this.tbType.Name = "tbType";
            this.tbType.ReadOnly = true;
            this.tbType.Size = new System.Drawing.Size(200, 19);
            this.tbType.TabIndex = 6;
            // 
            // tbName
            // 
            this.tbName.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.tbName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbName.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tbName.Location = new System.Drawing.Point(162, 55);
            this.tbName.Name = "tbName";
            this.tbName.ReadOnly = true;
            this.tbName.Size = new System.Drawing.Size(200, 19);
            this.tbName.TabIndex = 7;
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Font = new System.Drawing.Font("Arial Rounded MT Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitle.Location = new System.Drawing.Point(161, 9);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(100, 32);
            this.lbTitle.TabIndex = 8;
            this.lbTitle.Text = "INPUT";
            // 
            // cboxValue
            // 
            this.cboxValue.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.cboxValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxValue.FormattingEnabled = true;
            this.cboxValue.Location = new System.Drawing.Point(162, 135);
            this.cboxValue.Name = "cboxValue";
            this.cboxValue.Size = new System.Drawing.Size(200, 21);
            this.cboxValue.TabIndex = 9;
            // 
            // btnExit
            // 
            this.btnExit.AutoSize = true;
            this.btnExit.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(90, 170);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(99, 36);
            this.btnExit.TabIndex = 19;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // frmScan
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.DodgerBlue;
            this.ClientSize = new System.Drawing.Size(420, 220);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.cboxValue);
            this.Controls.Add(this.lbTitle);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.tbType);
            this.Controls.Add(this.lbValue);
            this.Controls.Add(this.lbName);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tbValue);
            this.Controls.Add(this.lbType);
            this.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmScan";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Properties";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lbName;
        private Label lbValue;
        private TextBox tbType;
        private TextBox tbName;
        private Label lbTitle;
        private ComboBox cboxValue;
        private Button btnExit;
    }
}