using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices; // include this to do the round corner trick
using Northwoods.Go;

namespace FlowArt
{
    partial class frmInfo
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
            this.lbName = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Font = new System.Drawing.Font("Arial Rounded MT Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitle.Location = new System.Drawing.Point(125, 14);
            this.lbTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(189, 32);
            this.lbTitle.TabIndex = 9;
            this.lbTitle.Text = "PROPERTIES";
            // 
            // lbName
            // 
            this.lbName.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbName.Location = new System.Drawing.Point(57, 80);
            this.lbName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(122, 19);
            this.lbName.TabIndex = 12;
            this.lbName.Text = "Design Name:";
            // 
            // tbName
            // 
            this.tbName.BackColor = System.Drawing.SystemColors.Info;
            this.tbName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbName.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbName.Location = new System.Drawing.Point(184, 80);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(184, 19);
            this.tbName.TabIndex = 13;
            // 
            // btnOK
            // 
            this.btnOK.AutoSize = true;
            this.btnOK.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnOK.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(168, 128);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(85, 36);
            this.btnOK.TabIndex = 16;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // frmInfo
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DodgerBlue;
            this.ClientSize = new System.Drawing.Size(420, 186);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.lbName);
            this.Controls.Add(this.lbTitle);
            this.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmBlockEdit";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Button btnOK;
    }
}