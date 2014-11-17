using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace tpscabinet
{
    static class Program
    {
        public static Control invokerControl;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
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
