using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace tpscabinet
{
    public partial class frm_notify : Form
    {
        private delegate void delegate_Hide();
        private System.Timers.Timer FadeOutTimer;
        private System.Timers.Timer DelayTimer;
        public int HideDelay = 4000;
        private GraphicsPath BorderPath;
        private GraphicsPath RegionPath;
        private Pen BorderPen;

        public frm_notify()
        {
            InitializeComponent();
            BorderPen = new Pen(Color.FromArgb(118, 118, 118));
            this.SetStyle(ControlStyles.Opaque, true);

            FadeOutTimer = new System.Timers.Timer(120);
            DelayTimer = new System.Timers.Timer(this.HideDelay);
            FadeOutTimer.Elapsed += (o, e) => {
                try {
                    this.Invoke(new delegate_FadeOutAnimation(FadeOutAnimation));
                }
                catch {}
            };
            DelayTimer.Elapsed += (o, e) => {
                try {
                    this.Invoke(new delegate_StartFadeOutAnimation(StartFadeOutAnimation));
                }
                catch {}
            };
            
            /// set for all controls transparent BG, and click handler
            foreach (Control ctrl in this.Controls)
            {
                ctrl.BackColor = Color.Transparent;
                ctrl.Click += ctrl_Click;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.BorderPath = helper.CreateRoundRect(new Rectangle(0, 0, this.Width - 1, this.Height - 1), 4, helper.RectangleCorners.All);
            this.RegionPath = helper.CreateRoundRect(new Rectangle(0, 0, this.Width, this.Height), 4, helper.RectangleCorners.All);
            this.Region = new System.Drawing.Region(RegionPath);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (BorderPath != null && RegionPath != null)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.CompositingQuality = CompositingQuality.AssumeLinear;
                e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                e.Graphics.FillPath(new LinearGradientBrush(new Rectangle(0, 0, this.Width, this.Height), Color.FromArgb(254, 254, 254), Color.FromArgb(228, 229, 240), LinearGradientMode.Vertical), BorderPath);
                e.Graphics.DrawPath(BorderPen, BorderPath);
            }
        }

        /// <summary>
        /// Hide window on control click 
        /// </summary>
        void ctrl_Click(object sender, EventArgs e)
        {
            this.DelayTimer.Stop();
            this.FadeOutTimer.Stop();
            this.Hide();
        }

        public void Show(int Delay)
        {
            this.HideDelay = Delay;
            this.DelayedFadeOut();
            base.Show();
        }
        
        public new void Show()
        {
            this.Show(this.HideDelay);
        }

        /// <summary>
        /// show some info
        /// </summary>
        /// <param name="Info">info string</param>
        public void FillData(string Info)
        {
            lbl_Info.Text = Info;
            lbl_Info.Left = 7;
            pic_Graph.Visible = false;
        }

        /// <summary>
        /// Fill form data
        /// </summary>
        /// <param name="tps_cabinet">scraped cabinet data</param>
        public void FillData(CabinetScraper tps_cabinet)
        {
            if (tps_cabinet.Login.Trim() == "" || tps_cabinet.Password.Trim() == "")
                this.FillData("\r\nВы не ввели свои логин\\пароль\r\n\r\n");
            else if (tps_cabinet.LastError != null && tps_cabinet.LastError.isCritical)
                this.FillData("\r\n" + tps_cabinet.LastError.Message + "\r\n\r\n");
            else if (tps_cabinet.LastUpdate == DateTime.MinValue)
                this.FillData("\r\nОбновление...\r\n\r\n");
            else
            {
                lbl_Info.Left = 91;
                ///// draw traf use piechart ////
                if ((tps_cabinet.TraffUsed + tps_cabinet.TraffLeft) != 0)
                {
                    Bitmap bmp = new Bitmap(pic_Graph.Width, pic_Graph.Height);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.CompositingQuality = CompositingQuality.AssumeLinear;
                        g.InterpolationMode = InterpolationMode.Bicubic;
                        g.Clear(Color.Transparent);
                        g.FillEllipse(Brushes.LightBlue, 0, 0, bmp.Width - 1, bmp.Height - 1);
                        g.FillPie(Brushes.Red, 0, 0, bmp.Width - 1, bmp.Height - 1, 0, tps_cabinet.TraffUsed * 360 / (tps_cabinet.TraffUsed + tps_cabinet.TraffLeft));
                    }
                    pic_Graph.Image = bmp;
                    pic_Graph.Visible = true;
                }
                else
                    pic_Graph.Visible = false;
                ////////////////////////////////

                int MinFromLastUpdate = (int)(DateTime.Now - tps_cabinet.LastUpdate).TotalMinutes;
                lbl_Info.Text = "Обновлено: " + tps_cabinet.LastUpdate.ToString("d MMM HH:mm") + " (" + ((MinFromLastUpdate == 0) ? "только что" : MinFromLastUpdate + " мин. назад") + ")\r\n";

                string DayUsed = "";
                if (tps_cabinet.TraffUsed > 0)
                    DayUsed = " (" + (tps_cabinet.TraffUsed / DateTime.Now.Day).ToString("f2") + " Мб/день)";

                lbl_Info.Text += "Использовано: " + tps_cabinet.TraffUsed + " Мб" + DayUsed + "\r\n";

                string PlaningDayUse = "";
                if (tps_cabinet.TraffLeft > 0 && (DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - DateTime.Now.Day) > 0)
                    PlaningDayUse = " (" + (tps_cabinet.TraffLeft / (DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - DateTime.Now.Day)).ToString("f2") + " Мб/день)";

                lbl_Info.Text += "Остаток: " + tps_cabinet.TraffLeft + " Мб" + PlaningDayUse + "\r\n";
                lbl_Info.Text += "Баланс: $ " + tps_cabinet.Balance + " / Курс: " + tps_cabinet.Kurs.ToString("f2") + "\r\n";
                lbl_Info.Text += "К оплате (1-го): " + ((tps_cabinet.AbonentPlata - tps_cabinet.Balance) * tps_cabinet.Kurs).ToString("f2") + "\r\n";
                //AppIcon.Text += "CabinetID = " + tps_cabinet.CabinetID + "\r\n";
                ///AppIcon.Text += "AbonentPlata = " + tps_cabinet.AbonentPlata + "\r\n";
            }
        }



        /// <summary>
        /// Start fadeout animation
        /// </summary>
        delegate void delegate_StartFadeOutAnimation();
        void StartFadeOutAnimation()
        {
            if (this.ClientRectangle.Contains(this.PointToClient(MousePosition))) return;
            DelayTimer.Stop();
            FadeOutTimer.Start();
        }

        /// <summary>
        /// Process fadeout animation
        /// </summary>
        delegate void delegate_FadeOutAnimation();
        void FadeOutAnimation()
        {
            if (this.ClientRectangle.Contains(this.PointToClient(MousePosition)))
            {
                DelayedFadeOut();
                return;
            }
            this.Opacity -= .04;
            if (this.Opacity <= 0.2)
            {
                FadeOutTimer.Stop();
                this.Hide();
            }
        }

        /// <summary>
        /// Delayed fadeout
        /// </summary>
        public void DelayedFadeOut()
        {
            FadeOutTimer.Stop();
            this.Opacity = 1;
            DelayTimer.Stop();

            if (this.HideDelay > 0)
            {
                DelayTimer.Interval = this.HideDelay;
                DelayTimer.Start();
            }
        }

        private void frm_notify_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                FadeOutTimer.Stop();
                DelayTimer.Stop();
                FadeOutTimer.Dispose();
                DelayTimer.Dispose();
                if (BorderPath != null) BorderPath.Dispose();
                if (RegionPath != null) RegionPath.Dispose();
                if (BorderPen != null) BorderPen.Dispose();
            }
            catch { }
        }

        

    }
}
