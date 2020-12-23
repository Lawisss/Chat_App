using System;
using System.Windows.Forms;

namespace Server
{
    static class Program
    {
        public static ServerForm ServerForm
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
            }
        }

        //Application launcher
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ServerForm());
        }
    }
}
