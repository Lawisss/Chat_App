using System;
using System.Windows.Forms;

namespace Client
{
    static class Program
    {
        public static HomePageForm HomePageForm
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

            HomePageForm formOne = new HomePageForm();
            Application.Run(formOne);

        }
    }
}
