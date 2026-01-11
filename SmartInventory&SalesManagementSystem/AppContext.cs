using SmartInventory_SalesManagementSystem.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartInventory_SalesManagementSystem
{
    public class AppContext : ApplicationContext
    {
        public AppContext()
        {
            ShowLogin();
        }

        private void ShowLogin()
        {
            Login login = new Login();
            login.FormClosed += (s, e) =>
            {
                if (SessionManager.IsLoggedIn())
                    ShowMain();
                else
                    ExitThread();
            };
            login.Show();
        }

        private void ShowMain()
        {
            MainMenuForm main = new MainMenuForm();
            main.FormClosed += (s, e) =>
            {
                if (!SessionManager.IsLoggedIn())
                    ShowLogin();
                else
                    ExitThread();
            };
            main.Show();
        }
    }
}
