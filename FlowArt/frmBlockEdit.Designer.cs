namespace FlowArt
{
    partial class frmBlockEdit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
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
            this.lbTitle = new System.Windows.Forms.Label();
            this.lbExpression = new System.Windows.Forms.Label();
            this.lbType = new System.Windows.Forms.Label();
            this.tbType = new System.Windows.Forms.TextBox();
            this.tbExpression = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.lstbxAutoComplete = new System.Windows.Forms.ListBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitle.Location = new System.Drawing.Point(126, 10);
            this.lbTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(168, 37);
            this.lbTitle.TabIndex = 9;
            this.lbTitle.Text = "BLOCK EDIT";
            // 
            // lbExpression
            // 
            this.lbExpression.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbExpression.Location = new System.Drawing.Point(51, 100);
            this.lbExpression.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbExpression.Name = "lbExpression";
            this.lbExpression.Size = new System.Drawing.Size(107, 41);
            this.lbExpression.TabIndex = 12;
            this.lbExpression.Text = "Expression:";
            // 
            // lbType
            // 
            this.lbType.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbType.Location = new System.Drawing.Point(51, 62);
            this.lbType.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbType.Name = "lbType";
            this.lbType.Size = new System.Drawing.Size(107, 41);
            this.lbType.TabIndex = 10;
            this.lbType.Text = "Type:";
            // 
            // tbType
            // 
            this.tbType.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.tbType.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbType.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbType.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tbType.Location = new System.Drawing.Point(162, 61);
            this.tbType.Name = "tbType";
            this.tbType.ReadOnly = true;
            this.tbType.Size = new System.Drawing.Size(200, 22);
            this.tbType.TabIndex = 14;
            // 
            // tbExpression
            // 
            this.tbExpression.BackColor = System.Drawing.SystemColors.Info;
            this.tbExpression.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbExpression.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbExpression.Location = new System.Drawing.Point(162, 101);
            this.tbExpression.Name = "tbExpression";
            this.tbExpression.Size = new System.Drawing.Size(200, 22);
            this.tbExpression.TabIndex = 13;
            this.tbExpression.TextChanged += new System.EventHandler(this.tbExpression_ShowLstBxAutoComplete);
            // 
            // btnOK
            // 
            this.btnOK.AutoSize = true;
            this.btnOK.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnOK.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(245, 156);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(85, 36);
            this.btnOK.TabIndex = 16;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lstbxAutoComplete
            // 
            this.lstbxAutoComplete.BackColor = System.Drawing.SystemColors.MenuBar;
            this.lstbxAutoComplete.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstbxAutoComplete.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstbxAutoComplete.FormattingEnabled = true;
            this.lstbxAutoComplete.Location = new System.Drawing.Point(162, 123);
            this.lstbxAutoComplete.Name = "lstbxAutoComplete";
            this.lstbxAutoComplete.Size = new System.Drawing.Size(200, 67);
            this.lstbxAutoComplete.TabIndex = 17;
            this.lstbxAutoComplete.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstbxAutoComplete_ApplyAutoCompleteByClick);
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = true;
            this.btnCancel.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(90, 156);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(99, 36);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // frmBlockEdit
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DodgerBlue;
            this.ClientSize = new System.Drawing.Size(420, 210);
            this.Controls.Add(this.lstbxAutoComplete);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tbType);
            this.Controls.Add(this.tbExpression);
            this.Controls.Add(this.lbExpression);
            this.Controls.Add(this.lbType);
            this.Controls.Add(this.lbTitle);
            this.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBlockEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "l";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Label lbExpression;
        private System.Windows.Forms.Label lbType;
        private System.Windows.Forms.TextBox tbType;
        private System.Windows.Forms.TextBox tbExpression;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ListBox lstbxAutoComplete;
        private System.Windows.Forms.Button btnCancel;
    }
}