namespace tpscabinet
{
    partial class frm_notify
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
            this.lbl_Info = new System.Windows.Forms.Label();
            this.pic_Graph = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pic_Graph)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_Info
            // 
            this.lbl_Info.AutoSize = true;
            this.lbl_Info.Location = new System.Drawing.Point(91, 10);
            this.lbl_Info.Name = "lbl_Info";
            this.lbl_Info.Size = new System.Drawing.Size(40, 13);
            this.lbl_Info.TabIndex = 2;
            this.lbl_Info.Text = "InfoPnl";
            // 
            // pic_Graph
            // 
            this.pic_Graph.ErrorImage = null;
            this.pic_Graph.InitialImage = null;
            this.pic_Graph.Location = new System.Drawing.Point(10, 10);
            this.pic_Graph.Name = "pic_Graph";
            this.pic_Graph.Size = new System.Drawing.Size(75, 75);
            this.pic_Graph.TabIndex = 3;
            this.pic_Graph.TabStop = false;
            this.pic_Graph.Visible = false;
            // 
            // frm_notify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(283, 95);
            this.ControlBox = false;
            this.Controls.Add(this.pic_Graph);
            this.Controls.Add(this.lbl_Info);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(220, 28);
            this.Name = "frm_notify";
            this.Padding = new System.Windows.Forms.Padding(7);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Honeydew;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_notify_FormClosing);
            this.Click += new System.EventHandler(this.ctrl_Click);
            ((System.ComponentModel.ISupportInitialize)(this.pic_Graph)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_Info;
        private System.Windows.Forms.PictureBox pic_Graph;
    }
}