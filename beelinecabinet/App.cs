using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace tpscabinet
{
    class App : ApplicationContext
    {
        private NotifyIcon AppIcon;
        private ContextMenu AppIconContextMenu;
        private System.Timers.Timer timer_update;
        private CabinetScraper tps_cabinet;
        private bool flag_UpdateInProgress;
        private frm_notify NotifyFrm;
        private helper W32Helper;
        Dictionary<string, DateTime> InfoNotifyWasShown;

        public App()
        {
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
            W32Helper = new helper();
            InfoNotifyWasShown = new Dictionary<string, DateTime>{
                { "TraffNotify", DateTime.MinValue },
                { "PaymentNotify", DateTime.MinValue }
            };

            InitializeComponent();
            AppIcon.Visible = true;
        }

        private void InitializeComponent()
        {
            AppIcon = new NotifyIcon();
            AppIcon.Icon = Properties.Resources.icon;
            AppIcon.MouseClick += AppIcon_MouseClick;
            AppIcon.BalloonTipText = "Обновление данных...";
            AppIconContextMenu = new ContextMenu();
            AppIconContextMenu.MenuItems.Add("Обновить", new EventHandler(mnu_ForceUpdate_Click));
            AppIconContextMenu.MenuItems.Add("Опции", new EventHandler(mnu_Options_Click));
            AppIconContextMenu.MenuItems.Add("Выход", new EventHandler(mnu_Exit_Click));
            AppIcon.ContextMenu = AppIconContextMenu;

            NotifyFrm = new frm_notify();
            NotifyFrm.CreateControl();

            tps_cabinet = new CabinetScraper();
            tps_cabinet.Login = Properties.Settings.Default.TPSLogin;
            tps_cabinet.Password = Properties.Settings.Default.TPSPassword;

            flag_UpdateInProgress = false;
            timer_update = new System.Timers.Timer();
            timer_update.Interval = Properties.Settings.Default.UpdateInterval * 60 * 1000;
            timer_update.Elapsed += timer_update_Elapsed;
            timer_update_Elapsed(null, null);
            timer_update.Start();
            Microsoft.Win32.SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;
        }

        private void mnu_ForceUpdate_Click(object sender, EventArgs e)
        {
            if (tps_cabinet.LastError != null) tps_cabinet.ClearErrors(); /// if error was before clear it, so user can see new status
            timer_update_Elapsed(null, null); // force update
        }

        void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            if (NotifyFrm.Visible)
                this.SetNotifyWindowsPos();
        }

        void AppIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.ShowNotify();
            }
        }

        void ShowNotify()
        {
            this.ShowNotify(null, false);
        }
        delegate void delegate_ShowNotify(string Info, bool Permanent);
        void ShowNotify(string Info, bool Permanent)
        {
            if (string.IsNullOrEmpty(Info))
                NotifyFrm.FillData(tps_cabinet);
            else
                NotifyFrm.FillData(Info);
            NotifyFrm.Invalidate(true);
            SetNotifyWindowsPos();
            NotifyFrm.Show((Permanent) ? 0 : 4000);
        }

        /// <summary>
        /// /// set notify window position bassed on taskbar position and size
        /// </summary>
        void SetNotifyWindowsPos()
        {
            W32Helper.GetTaskbarInfo();
            switch (W32Helper.TaskbarPosition)
            {
                case helper.TaskbarPos.Top:
                    NotifyFrm.Left = W32Helper.TaskbarBounds.Right - NotifyFrm.Width - 1;
                    NotifyFrm.Top = W32Helper.TaskbarBounds.Bottom + 1;
                    break;
                case helper.TaskbarPos.Left:
                    NotifyFrm.Left = W32Helper.TaskbarBounds.Right + 1;
                    NotifyFrm.Top = W32Helper.TaskbarBounds.Bottom - NotifyFrm.Height - 1;
                    break;
                case helper.TaskbarPos.Right:
                    NotifyFrm.Left = W32Helper.TaskbarBounds.Left - NotifyFrm.Width - 1;
                    NotifyFrm.Top = W32Helper.TaskbarBounds.Bottom - NotifyFrm.Height - 1;
                    break;
                default: //w32helper.TaskbarPosition.Bottom and Unknown position
                    NotifyFrm.Left = W32Helper.TaskbarBounds.Right - NotifyFrm.Width - 1;
                    NotifyFrm.Top = W32Helper.TaskbarBounds.Top - NotifyFrm.Height - 1;
                    break;
            }
        }

        
        void timer_update_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (flag_UpdateInProgress) return;
            flag_UpdateInProgress = true;
            //rewrite those part to BackgroundWorker and move to CabinetScraper
            ThreadPool.QueueUserWorkItem(new WaitCallback(UpdateInfo));
        }

        void UpdateInfo(object state)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
            try
            {
                if (!String.IsNullOrEmpty(tps_cabinet.Login) && !String.IsNullOrEmpty(tps_cabinet.Password))
                {
                    tps_cabinet.LoadDataBeeline();
                    /// notify
                    ////// notify if we need to pay today (show one time in 4h) \ !NotifyFrm.Visible for not override other notify
                    if (!NotifyFrm.Visible && Properties.Settings.Default.PaymentNotify && (DateTime.Now - this.InfoNotifyWasShown["PaymentNotify"]).TotalHours >= 4 && DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) == DateTime.Now.Day)
                    {
                        Program.invokerControl.Invoke(new delegate_ShowNotify(ShowNotify), new object[] { "\r\nСегодня последний день оплаты\r\n\r\n", true });
                        this.InfoNotifyWasShown["PaymentNotify"] = DateTime.Now;
                    }
                    ////// notify if traff less than limits (show one time in 4h)
                    if (!NotifyFrm.Visible && Properties.Settings.Default.TraffNotify && (DateTime.Now - this.InfoNotifyWasShown["TraffNotify"]).TotalHours >= 4 && tps_cabinet.LastUpdate != DateTime.MinValue && tps_cabinet.TraffLeft < Properties.Settings.Default.TraffNotifyLimit && tps_cabinet.TraffLeft != 0)
                    {
                        Program.invokerControl.Invoke(new delegate_ShowNotify(ShowNotify), new object[] { "\r\nОстаток трафика меньше установленного лимита (" + tps_cabinet.TraffLeft + " Мб)\r\n\r\n", true });
                        this.InfoNotifyWasShown["TraffNotify"] = DateTime.Now;
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Program.invokerControl.BeginInvoke(new delegate_ShowNotify(ShowNotify), new object[] { "Ошибка: " + ex.Message, false });
                }
                catch { }
            }
            flag_UpdateInProgress = false;
        }

        private void mnu_Options_Click(object sender, EventArgs e)
        {
            frm_options frmOptions = new frm_options();
            frmOptions.ShowDialog();
            if (frmOptions.DialogResult == DialogResult.OK)
            {
                timer_update.Stop(); // stop update timer
                timer_update.Interval = Properties.Settings.Default.UpdateInterval * 60 * 1000;
                ///set new values
                tps_cabinet.Login = Properties.Settings.Default.TPSLogin;
                tps_cabinet.Password = Properties.Settings.Default.TPSPassword;
                /// reset values
                this.InfoNotifyWasShown["TraffNotify"] = DateTime.MinValue;
                this.InfoNotifyWasShown["PaymentNotify"] = DateTime.MinValue;
                /// update notification
                if (!NotifyFrm.Visible) NotifyFrm.FillData(tps_cabinet);
                mnu_ForceUpdate_Click(null, null); //force update
                timer_update.Start(); // start update timer
            }
            frmOptions.Close();
            frmOptions.Dispose();
        }

        private void mnu_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void OnApplicationExit(object sender, EventArgs e)
        {
            try
            {
                timer_update.Stop();
                timer_update.Dispose();
                NotifyFrm.Close();
                NotifyFrm.Dispose();
                AppIconContextMenu.Dispose();
                AppIcon.Visible = false;
                AppIcon.Dispose();
            }
            catch { }
        }
    }


}