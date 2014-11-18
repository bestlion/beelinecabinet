using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace tpscabinet
{
    public class CookieWebClient : WebClient
    {
        public CookieContainer CookieContainer { get; set; }
        public CookieWebClient()
        {
            this.CookieContainer = new CookieContainer();
        }
        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address) as HttpWebRequest;
            if (request == null) return base.GetWebRequest(address);
            request.CookieContainer = CookieContainer;
            request.Timeout = 10000; // 10 sec
            request.ReadWriteTimeout = 120000; // 120 sec
            request.AllowAutoRedirect = true;
            request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.7 (KHTML, like Gecko) Chrome/7.0.517.41 Safari/534.7"; /// google chrome
            return request;
        }
    }

    public class Error
    {
        public string Message;
        public bool isCritical;
        public Error(string Message, bool isCritical)
        {
            this.Message = Message;
            this.isCritical = isCritical;
        }
    }

    public class CabinetScraper
    {
        public string Login
        {
            get;set;
        }
        public string Password
        {
            get;
            set;
        }
        public Error LastError
        {
            get;
            private set;
        }
        public DateTime LastUpdate;
        public float TraffUsed;
        public float TraffLeft;
        public float Balance;
        public int CabinetID;
        public float AbonentPlata;
        public float Kurs;
        private string CabinetHTML;
        private string PacketsHTML;
        private string Plan;

        public CabinetScraper()
        {
            this.Login = "";
            this.Password = "";
            LastUpdate = DateTime.MinValue;
            TraffUsed = 0;
            TraffLeft = 0;
            Balance = 0;
            CabinetID = 0;
            AbonentPlata = 0;
            Kurs = 0;
            CabinetHTML = "";
            PacketsHTML = "";
            Plan = "";
        }

        public void ClearErrors()
        {
            if (this.LastError != null) this.LastError.Message = "";
            this.LastError = null;
        }

        public void LoadDataBeeline()
        {
            using (CookieWebClient wc = new CookieWebClient())
            {
                //wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                NameValueCollection post_vars = new NameValueCollection();
                post_vars.Add("login", "");
                post_vars.Add("password", "");
                post_vars.Add("submit", "");
                post_vars.Add("submit", "Войти");
                post_vars.Add("AuthData.RememberMe", "false");//false
                post_vars.Add("AuthData.ReturnUrl", "/RU/cabinet/internet/info");
                post_vars.Add("AuthData.Login", this.Login);
                post_vars.Add("AuthData.Password", this.Password);
                this.CabinetHTML = UTF8Encoding.UTF8.GetString(wc.UploadValues("https://clientsnew.beeline.uz/RU/Account/LogOn?ReturnUrl=/RU/cabinet/internet/info", "POST", post_vars));
                this.PacketsHTML = UTF8Encoding.UTF8.GetString(wc.DownloadData("https://clientsnew.beeline.uz/ru/cabinet/internet/trafficpacks"));
#if DEBUG
                System.IO.File.WriteAllText("main" + DateTime.Now.Ticks + ".html", this.CabinetHTML, Encoding.UTF8);
                System.IO.File.WriteAllText("packet" + DateTime.Now.Ticks + ".html", this.PacketsHTML, Encoding.UTF8);
#endif
                if (String.IsNullOrEmpty(this.CabinetHTML))
                {
                    this.LastError = new Error("Данные не получены", false);
                    return;
                }
                if (this.CabinetHTML.Contains("Пользователь с таким логином не существует"))
                {
                    this.LastError = new Error("Логин\\пароль не верны", true);
                    return;
                }
                if (!String.IsNullOrEmpty(this.CabinetHTML))
                {
                    //// Traff Left
                    float.TryParse(GetCabinetVal(@"Остаток трафика\s*<span[\s\S]+?>(-?\d+(?:\.\d{1,2})?)</span>"), out this.TraffLeft);
                    //// Traff Used
                    if (!String.IsNullOrEmpty(this.PacketsHTML))
                    {
                        this.Plan = GetCabinetVal(@"Тарифный план:\s*([\s\S]+?)</div>").Trim();
                        if (this.Plan != "")
                        {
                            float TotalTraff = 0;
                            float.TryParse(this.FindTraffLeftInActivePlan(), out TotalTraff);
                            this.TraffUsed = TotalTraff - this.TraffLeft;
                        }
                    }
                    if (this.TraffLeft < 0) this.TraffLeft = 0;
                    if (this.TraffUsed < 0) this.TraffUsed = 0;

                    float.TryParse(GetCabinetVal(@"<span\s+id=""BalanceSpan""[\s\S]+?>(-?\d+(?:\.\d{1,2})?)</span>"), out this.Balance);
                    int.TryParse(GetCabinetVal(@"class=""icn""\s*>\s*(\d+)\s*</td>"), out this.CabinetID);
                    float.TryParse(GetCabinetVal(@"Абонентская плата:\s*(\d+(?:\.\d{1,2})?)"), out this.AbonentPlata);
                    this.Kurs = -1;
                    this.LastError = null;
                    this.LastUpdate = DateTime.Now;
                }
            }
        }

        private string GetCabinetVal(string pattern)
        {
            Match CabinetValMatch = Regex.Match(this.CabinetHTML, pattern, RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            if (CabinetValMatch.Success)
                return CabinetValMatch.Groups[1].Value;
            else
                return "";
        }
        
        private string FindTraffLeftInActivePlan()
        {
            Match PlanValMatch = Regex.Match(this.PacketsHTML, this.Plan + @"[\s\S]+?</td>[\s\S]+?</td>[\s\S]+?</td>[\s\S]+?<div class=""aservice"">\s*(\d+(?:\.\d{1,2})?)\s*</div>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            if (PlanValMatch.Success)
                return PlanValMatch.Groups[1].Value;
            else
                return "";
        }

    }
}
