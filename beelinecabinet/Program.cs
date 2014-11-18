using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace tpscabinet
{
    static class Program
    {
        public static Control invokerControl;
        public static bool DebugMode = false;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
            Program.DebugMode = false;
            if (args.Length > 0 && args[0].StartsWith("/debug"))
                Program.DebugMode = true;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            invokerControl = new Control(); /// helper control what allow Invoke to UI thread
            invokerControl.CreateControl();
            Application.Run(new App());
        }
    }

    class myprof : IFormatProvider
    {
        public object GetFormat(Type formatType)
        {
            throw new NotImplementedException();
        }
    }


}
