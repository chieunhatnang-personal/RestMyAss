using System;
using System.Threading;
using System.Windows.Forms;

namespace RestMyAss
{
    static class Program
    {
        private const string SingleInstanceMutexName = "RestMyAss_SingleInstance_Mutex";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool isFirstInstance;
            using (Mutex mutex = new Mutex(true, SingleInstanceMutexName, out isFirstInstance))
            {
                if (!isFirstInstance)
                {
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frm_Setting());
            }
        }
    }
}
