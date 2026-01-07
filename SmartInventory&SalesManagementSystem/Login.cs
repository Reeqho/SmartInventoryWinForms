using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartInventory_SalesManagementSystem
{
    public partial class Login : Form
    {
        SmartInventoryDBEntities db = new SmartInventoryDBEntities();
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
                var user = db.Users.FirstOrDefault(s => s.Username == textBox1.Text && s.PasswordHash == textBox2.Text);
                if(user == null)
                {
                    MessageBox.Show("Invalid user ");
                    return;
                }
                else if (user.Role.RoleName == "kasir")
                {
                    
                }
                else if (user.Role.RoleName == "admin")
                {
                   
                }
            }

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
