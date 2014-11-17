using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace tpscabinet
{
    public partial class frm_options : Form
    {
        public frm_options()
        {
            InitializeComponent();
        }

        private void frm_options_Load(object sender, EventArgs e)
        {
            txt_Login.Text = Properties.Settings.Default.TPSLogin;
            txt_Password.Text = Properties.Settings.Default.TPSPassword;
            num_Interval.Value = Properties.Settings.Default.UpdateInterval;
            num_TraffNotify.Value = Properties.Settings.Default.TraffNotifyLimit;
            cb_TraffNotify.Checked = Properties.Settings.Default.TraffNotify;
            cb_PaymentNotify.Checked = Properties.Settings.Default.PaymentNotify;
            num_TraffNotify.Enabled = cb_TraffNotify.Checked;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TPSLogin = txt_Login.Text;
            Properties.Settings.Default.TPSPassword = txt_Password.Text;
            Properties.Settings.Default.UpdateInterval = (int)num_Interval.Value;
            Properties.Settings.Default.TraffNotifyLimit = (int)num_TraffNotify.Value;
            Properties.Settings.Default.TraffNotify = cb_TraffNotify.Checked;
            Properties.Settings.Default.PaymentNotify = cb_PaymentNotify.Checked;
            Properties.Settings.Default.Save();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void cb_TraffNotify_CheckedChanged(object sender, EventArgs e)
        {
            num_TraffNotify.Enabled = cb_TraffNotify.Checked;
        }
    }
}
