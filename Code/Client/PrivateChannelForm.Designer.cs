using System;

namespace Client
{
    partial class PrivateChannelForm
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
            this.messagePrivate = new System.Windows.Forms.TextBox();
            this.process = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // messagePrivate
            // 
            this.messagePrivate.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messagePrivate.ForeColor = System.Drawing.Color.Salmon;
            this.messagePrivate.Location = new System.Drawing.Point(20, 348);
            this.messagePrivate.Multiline = true;
            this.messagePrivate.Name = "messagePrivate";
            this.messagePrivate.Size = new System.Drawing.Size(508, 126);
            this.messagePrivate.TabIndex = 5;
            // 
            // process
            // 
            this.process.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.process.ForeColor = System.Drawing.Color.Salmon;
            this.process.Location = new System.Drawing.Point(20, 126);
            this.process.Multiline = true;
            this.process.Name = "process";
            this.process.ReadOnly = true;
            this.process.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.process.Size = new System.Drawing.Size(508, 216);
            this.process.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Bisque;
            this.button1.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Salmon;
            this.button1.Location = new System.Drawing.Point(209, 508);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(106, 41);
            this.button1.TabIndex = 6;
            this.button1.Text = "Send";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Salmon;
            this.label1.Location = new System.Drawing.Point(153, 47);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(233, 40);
            this.label1.TabIndex = 7;
            this.label1.Text = "Private Channel";
            // 
            // PrivateChannelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Client.Properties.Resources.fond_ecran_1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(548, 572);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.messagePrivate);
            this.Controls.Add(this.process);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "PrivateChannelForm";
            this.Text = "ChatApp - Private Channel";
            this.Load += new System.EventHandler(this.PrivateMessage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox messagePrivate;
        private System.Windows.Forms.TextBox process;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
    }
}