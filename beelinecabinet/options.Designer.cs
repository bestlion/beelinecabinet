namespace tpscabinet
{
    partial class frm_options
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
            this.txt_Login = new System.Windows.Forms.TextBox();
            this.txt_Password = new System.Windows.Forms.TextBox();
            this.lbl_login = new System.Windows.Forms.Label();
            this.lbl_password = new System.Windows.Forms.Label();
            this.lbl_interval = new System.Windows.Forms.Label();
            this.num_Interval = new System.Windows.Forms.NumericUpDown();
            this.btn_Save = new System.Windows.Forms.Button();
            this.num_TraffNotify = new System.Windows.Forms.NumericUpDown();
            this.cb_TraffNotify = new System.Windows.Forms.CheckBox();
            this.cb_PaymentNotify = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.num_Interval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_TraffNotify)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_Login
            // 
            this.txt_Login.Location = new System.Drawing.Point(103, 9);
            this.txt_Login.Name = "txt_Login";
            this.txt_Login.Size = new System.Drawing.Size(147, 20);
            this.txt_Login.TabIndex = 0;
            // 
            // txt_Password
            // 
            this.txt_Password.Location = new System.Drawing.Point(103, 38);
            this.txt_Password.Name = "txt_Password";
            this.txt_Password.Size = new System.Drawing.Size(147, 20);
            this.txt_Password.TabIndex = 1;
            this.txt_Password.UseSystemPasswordChar = true;
            // 
            // lbl_login
            // 
            this.lbl_login.AutoSize = true;
            this.lbl_login.Location = new System.Drawing.Point(12, 12);
            this.lbl_login.Name = "lbl_login";
            this.lbl_login.Size = new System.Drawing.Size(38, 13);
            this.lbl_login.TabIndex = 2;
            this.lbl_login.Text = "Логин";
            // 
            // lbl_password
            // 
            this.lbl_password.AutoSize = true;
            this.lbl_password.Location = new System.Drawing.Point(12, 41);
            this.lbl_password.Name = "lbl_password";
            this.lbl_password.Size = new System.Drawing.Size(45, 13);
            this.lbl_password.TabIndex = 3;
            this.lbl_password.Text = "Пароль";
            // 
            // lbl_interval
            // 
            this.lbl_interval.AutoSize = true;
            this.lbl_interval.Location = new System.Drawing.Point(12, 68);
            this.lbl_interval.Name = "lbl_interval";
            this.lbl_interval.Size = new System.Drawing.Size(148, 13);
            this.lbl_interval.TabIndex = 4;
            this.lbl_interval.Text = "Интервал обновления (мин)";
            // 
            // num_Interval
            // 
            this.num_Interval.Location = new System.Drawing.Point(196, 66);
            this.num_Interval.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
            this.num_Interval.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.num_Interval.Name = "num_Interval";
            this.num_Interval.Size = new System.Drawing.Size(54, 20);
            this.num_Interval.TabIndex = 5;
            this.num_Interval.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(80, 158);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(93, 23);
            this.btn_Save.TabIndex = 6;
            this.btn_Save.Text = "Сохранить";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // num_TraffNotify
            // 
            this.num_TraffNotify.Location = new System.Drawing.Point(179, 96);
            this.num_TraffNotify.Maximum = new decimal(new int[] {
            2147483646,
            0,
            0,
            0});
            this.num_TraffNotify.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_TraffNotify.Name = "num_TraffNotify";
            this.num_TraffNotify.Size = new System.Drawing.Size(73, 20);
            this.num_TraffNotify.TabIndex = 8;
            this.num_TraffNotify.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // cb_TraffNotify
            // 
            this.cb_TraffNotify.AutoSize = true;
            this.cb_TraffNotify.Location = new System.Drawing.Point(15, 90);
            this.cb_TraffNotify.Name = "cb_TraffNotify";
            this.cb_TraffNotify.Size = new System.Drawing.Size(158, 30);
            this.cb_TraffNotify.TabIndex = 10;
            this.cb_TraffNotify.Text = "Уведомлять если \r\nтрафика меньше чем (Мб)";
            this.cb_TraffNotify.UseVisualStyleBackColor = true;
            this.cb_TraffNotify.CheckedChanged += new System.EventHandler(this.cb_TraffNotify_CheckedChanged);
            // 
            // cb_PaymentNotify
            // 
            this.cb_PaymentNotify.AutoSize = true;
            this.cb_PaymentNotify.Location = new System.Drawing.Point(15, 122);
            this.cb_PaymentNotify.Name = "cb_PaymentNotify";
            this.cb_PaymentNotify.Size = new System.Drawing.Size(235, 30);
            this.cb_PaymentNotify.TabIndex = 11;
            this.cb_PaymentNotify.Text = "Уведомлять об оплате в последний день\r\nмесяца";
            this.cb_PaymentNotify.UseVisualStyleBackColor = true;
            // 
            // frm_options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 192);
            this.Controls.Add(this.cb_PaymentNotify);
            this.Controls.Add(this.cb_TraffNotify);
            this.Controls.Add(this.num_TraffNotify);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.num_Interval);
            this.Controls.Add(this.lbl_interval);
            this.Controls.Add(this.lbl_password);
            this.Controls.Add(this.lbl_login);
            this.Controls.Add(this.txt_Password);
            this.Controls.Add(this.txt_Login);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_options";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки";
            this.Load += new System.EventHandler(this.frm_options_Load);
            ((System.ComponentModel.ISupportInitialize)(this.num_Interval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_TraffNotify)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_Login;
        private System.Windows.Forms.TextBox txt_Password;
        private System.Windows.Forms.Label lbl_login;
        private System.Windows.Forms.Label lbl_password;
        private System.Windows.Forms.Label lbl_interval;
        private System.Windows.Forms.NumericUpDown num_Interval;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.NumericUpDown num_TraffNotify;
        private System.Windows.Forms.CheckBox cb_TraffNotify;
        private System.Windows.Forms.CheckBox cb_PaymentNotify;
    }
}