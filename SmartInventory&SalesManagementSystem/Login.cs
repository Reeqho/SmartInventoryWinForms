using SmartInventory_SalesManagementSystem.Session;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartInventory_SalesManagementSystem
{
    public partial class Login : Form
    {
        SmartInventoryDbContext db = new SmartInventoryDbContext();
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty || textBox2.Text == string.Empty)
            {
                MessageBox.Show("Please enter username and password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {

                var user = db.Users.FirstOrDefault(u => u.Username == textBox1.Text && u.PasswordHash == textBox2.Text);

                if (user?.Role != null)
                {
                    SessionManager.SetUser(user);
                    MainMenuForm mainMenuForm = new MainMenuForm();
                    mainMenuForm.Show();
                    Hide();
                }
                else
                {
                    MessageBox.Show("Role belum tersedia!");
                    return;
                }

            }

        }

        private void Login_Load(object sender, EventArgs e)
        {
            using (var db = new SmartInventoryDbContext())
            {
                if (!db.Roles.Any())
                {
                    db.Roles.Add(new Role { RoleName = "Admin" });
                    db.Roles.Add(new Role { RoleName = "Kasir" });
                    db.SaveChanges();
                }
            }

            textBox1.Text = "admin";
            textBox2.Text = "admin123";

            // kasir  
            //textBox1.Text = "kasir1";
            //textBox2.Text = "123456";
            textBox2.UseSystemPasswordChar = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }
    }
}
