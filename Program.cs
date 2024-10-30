using System;
using System.Windows.Forms;

namespace Airoport
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SignUp signUp = new SignUp();

            if (signUp.ShowDialog() == DialogResult.OK)
            {
                if (!signUp.IsDisposed)
                {
                    switch (SignUp.roleDb)
                    {
                        case "administrator":
                            Application.Run(new Main());
                            break;
                    }
                }
            }
        }
    }
}
