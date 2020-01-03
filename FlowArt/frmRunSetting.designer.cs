using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices; // include this to do the round corner trick
using Northwoods.Go;

namespace FlowArt
{
    partial class frmRunSetting
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
            this.lbSpeed = new System.Windows.Forms.Label();
            this.lbAutoRun = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.cbAutoRun = new System.Windows.Forms.CheckBox();
            this.cbClearResult = new System.Windows.Forms.CheckBox();
            this.lbFadingEffect = new System.Windows.Forms.Label();
            this.cbFading = new System.Windows.Forms.CheckBox();
            this.lbClearResult = new System.Windows.Forms.Label();
            this.lbSoundEffect = new System.Windows.Forms.Label();
            this.trbSpeed = new System.Windows.Forms.TrackBar();
            this.cbNotifyOnEnd = new System.Windows.Forms.CheckBox();
            this.lbNotifyOnEnd = new System.Windows.Forms.Label();
            this.cbSound = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.trbSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Font = new System.Drawing.Font("Arial Rounded MT Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitle.Location = new System.Drawing.Point(143, 14);
            this.lbTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(135, 32);
            this.lbTitle.TabIndex = 9;
            this.lbTitle.Text = "SETTING";
            // 
            // lbSpeed
            // 
            this.lbSpeed.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSpeed.Location = new System.Drawing.Point(180, 80);
            this.lbSpeed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbSpeed.Name = "lbSpeed";
            this.lbSpeed.Size = new System.Drawing.Size(136, 19);
            this.lbSpeed.TabIndex = 12;
            this.lbSpeed.Text = "Speed Level:";
            // 
            // lbAutoRun
            // 
            this.lbAutoRun.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbAutoRun.Location = new System.Drawing.Point(22, 80);
            this.lbAutoRun.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbAutoRun.Name = "lbAutoRun";
            this.lbAutoRun.Size = new System.Drawing.Size(108, 19);
            this.lbAutoRun.TabIndex = 17;
            this.lbAutoRun.Text = "Auto Run:";
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = true;
            this.btnCancel.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(90, 219);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(99, 36);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.AutoSize = true;
            this.btnOK.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnOK.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(245, 219);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(85, 36);
            this.btnOK.TabIndex = 19;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cbAutoRun
            // 
            this.cbAutoRun.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbAutoRun.Checked = true;
            this.cbAutoRun.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoRun.Location = new System.Drawing.Point(138, 80);
            this.cbAutoRun.Name = "cbAutoRun";
            this.cbAutoRun.Size = new System.Drawing.Size(20, 20);
            this.cbAutoRun.TabIndex = 21;
            this.cbAutoRun.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbAutoRun.UseVisualStyleBackColor = true;
            this.cbAutoRun.CheckedChanged += new System.EventHandler(this.cbAutoRun_CheckedChanged_EnableSpeed);
            // 
            // cbClearResult
            // 
            this.cbClearResult.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbClearResult.Checked = true;
            this.cbClearResult.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbClearResult.Location = new System.Drawing.Point(138, 156);
            this.cbClearResult.Name = "cbClearResult";
            this.cbClearResult.Size = new System.Drawing.Size(20, 20);
            this.cbClearResult.TabIndex = 23;
            this.cbClearResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbClearResult.UseVisualStyleBackColor = true;
            // 
            // lbFadingEffect
            // 
            this.lbFadingEffect.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFadingEffect.Location = new System.Drawing.Point(22, 118);
            this.lbFadingEffect.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbFadingEffect.Name = "lbFadingEffect";
            this.lbFadingEffect.Size = new System.Drawing.Size(143, 19);
            this.lbFadingEffect.TabIndex = 22;
            this.lbFadingEffect.Text = "Fading Effect:";
            // 
            // cbFading
            // 
            this.cbFading.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbFading.Checked = true;
            this.cbFading.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbFading.Location = new System.Drawing.Point(138, 118);
            this.cbFading.Name = "cbFading";
            this.cbFading.Size = new System.Drawing.Size(20, 20);
            this.cbFading.TabIndex = 25;
            this.cbFading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbFading.UseVisualStyleBackColor = true;
            // 
            // lbClearResult
            // 
            this.lbClearResult.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbClearResult.Location = new System.Drawing.Point(22, 156);
            this.lbClearResult.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbClearResult.Name = "lbClearResult";
            this.lbClearResult.Size = new System.Drawing.Size(133, 19);
            this.lbClearResult.TabIndex = 24;
            this.lbClearResult.Text = "Clear Result:";
            // 
            // lbSoundEffect
            // 
            this.lbSoundEffect.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSoundEffect.Location = new System.Drawing.Point(180, 117);
            this.lbSoundEffect.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbSoundEffect.Name = "lbSoundEffect";
            this.lbSoundEffect.Size = new System.Drawing.Size(136, 19);
            this.lbSoundEffect.TabIndex = 26;
            this.lbSoundEffect.Text = "Sound Effect:";
            // 
            // trbSpeed
            // 
            this.trbSpeed.Location = new System.Drawing.Point(300, 80);
            this.trbSpeed.Name = "trbSpeed";
            this.trbSpeed.Size = new System.Drawing.Size(104, 45);
            this.trbSpeed.TabIndex = 28;
            // 
            // cbNotifyOnEnd
            // 
            this.cbNotifyOnEnd.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbNotifyOnEnd.Checked = true;
            this.cbNotifyOnEnd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbNotifyOnEnd.Location = new System.Drawing.Point(339, 156);
            this.cbNotifyOnEnd.Name = "cbNotifyOnEnd";
            this.cbNotifyOnEnd.Size = new System.Drawing.Size(20, 20);
            this.cbNotifyOnEnd.TabIndex = 30;
            this.cbNotifyOnEnd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbNotifyOnEnd.UseVisualStyleBackColor = true;
            // 
            // lbNotifyOnEnd
            // 
            this.lbNotifyOnEnd.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNotifyOnEnd.Location = new System.Drawing.Point(180, 156);
            this.lbNotifyOnEnd.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbNotifyOnEnd.Name = "lbNotifyOnEnd";
            this.lbNotifyOnEnd.Size = new System.Drawing.Size(133, 19);
            this.lbNotifyOnEnd.TabIndex = 29;
            this.lbNotifyOnEnd.Text = "Notify On End:";
            // 
            // cbSound
            // 
            this.cbSound.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbSound.Checked = true;
            this.cbSound.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSound.Location = new System.Drawing.Point(339, 118);
            this.cbSound.Name = "cbSound";
            this.cbSound.Size = new System.Drawing.Size(20, 20);
            this.cbSound.TabIndex = 31;
            this.cbSound.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbSound.UseVisualStyleBackColor = true;
            // 
            // frmRunSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DodgerBlue;
            this.ClientSize = new System.Drawing.Size(420, 285);
            this.Controls.Add(this.cbSound);
            this.Controls.Add(this.cbClearResult);
            this.Controls.Add(this.cbNotifyOnEnd);
            this.Controls.Add(this.lbNotifyOnEnd);
            this.Controls.Add(this.trbSpeed);
            this.Controls.Add(this.lbSoundEffect);
            this.Controls.Add(this.cbFading);
            this.Controls.Add(this.lbClearResult);
            this.Controls.Add(this.lbFadingEffect);
            this.Controls.Add(this.cbAutoRun);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lbAutoRun);
            this.Controls.Add(this.lbSpeed);
            this.Controls.Add(this.lbTitle);
            this.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRunSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmBlockEdit";
            ((System.ComponentModel.ISupportInitialize)(this.trbSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Label lbSpeed;
        private System.Windows.Forms.Label lbAutoRun;
        private Button btnCancel;
        private Button btnOK;
        private CheckBox cbAutoRun;
        private CheckBox cbClearResult;
        private Label lbFadingEffect;
        private CheckBox cbFading;
        private Label lbClearResult;
        private Label lbSoundEffect;
        private TrackBar trbSpeed;
        private CheckBox cbNotifyOnEnd;
        private Label lbNotifyOnEnd;
        private CheckBox cbSound;
    }
}