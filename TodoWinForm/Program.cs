using System;
using System.Configuration;
using System.Windows.Forms;

namespace TodoWinForm
{
    static class Program
    {
        /// <summary>
        /// Variável somente leitura que recupera a URL da WebAPI
        /// </summary>
        public static readonly string API = ConfigurationManager.AppSettings["BaseAddress"];

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}
