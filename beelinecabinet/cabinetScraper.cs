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
                post_vars.Add("AuthData.ReturnUrl", "");
                post_vars.Add("AuthData.Login", this.Login);
                post_vars.Add("AuthData.Password", this.Password);
                this.CabinetHTML = UTF8Encoding.UTF8.GetString(wc.UploadValues("https://clientsnew.beeline.uz/RU/Account/LogOn?ReturnUrl=%2f", "POST", post_vars));
                System.IO.File.WriteAllText("out" + DateTime.Now.Ticks + ".html", this.CabinetHTML, Encoding.UTF8);

                /*if (String.IsNullOrEmpty(this.CabinetHTML))
                {
                    this.LastError = new Error("Данные не получены", false);
                    return;
                }
                if (this.CabinetHTML.Contains("Вход в персональный кабинет"))
                {
                    this.LastError = new Error("Логин\\пароль не верны", true);
                    return;
                }
                if (!String.IsNullOrEmpty(this.CabinetHTML))
                {
                    float.TryParse(GetCabinetVal(@"traffic\s*:\s*'Использовано',\s*value:\s*(\d+(?:\.\d{1,2})?)\s*}"), out this.TraffUsed);
                    float.TryParse(GetCabinetVal(@"traffic\s*:\s*'Осталось',\s*value:\s*(-?\d+(?:\.\d{1,2})?)\s*}"), out this.TraffLeft);
                    if (TraffLeft < 0) TraffLeft = 0;
                    float.TryParse(GetCabinetVal(@"<strong\s+class=""balance""\s+data-accid=""\d+"">\s*(-?\d+(?:\.\d{1,2})?)\s*</strong>"), out this.Balance);
                    int.TryParse(GetCabinetVal(@"/pppoe_session\?id=(\d+)"), out this.CabinetID);
                    float.TryParse(GetCabinetVal(@"<th>Статус</th>[\s\S]+?<td>\s*\$(\d+(?:\.\d{1,2})?)\s*</td>"), out this.AbonentPlata);
                    float.TryParse(GetCabinetVal(@"<p>1\s*доллар\s*США\s*=\s*(\d+(?:\.\d{1,2})?)"), out this.Kurs);
                    this.LastError = null;
                    this.LastUpdate = DateTime.Now;
                }*/
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
    }
}
