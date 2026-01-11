using SmartInventory_SalesManagementSystem.Admin;
using SmartInventory_SalesManagementSystem.Kasir;
using SmartInventory_SalesManagementSystem.Session;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartInventory_SalesManagementSystem
{
    public partial class MainMenuForm : Form
    {
        int test = 1;
        public bool IsAdmin = false;
        public MainMenuForm()
        {
            InitializeComponent();
        }

        private void CloseAllMdiChild()
        {
            foreach (Form child in MdiChildren)
            {
                child.Close();
            }
        }


        private void OpenForm(Form form)
        {
            CloseAllMdiChild();
            form.MdiParent = this;
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }



        private void MainMenuForm_Load(object sender, EventArgs e)
        {

            if (SessionManager.CurrentUser.Role.RoleName != "Admin")
            {
                tableLayoutPanel1.Visible = false;
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel2.Location = new Point(3, 0);
            }
            else
            {
                tableLayoutPanel2.Visible = false;
                tableLayoutPanel2.Controls.Clear();
                tableLayoutPanel1.Location = new Point(3, 0);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenForm(new KelolaUserForm());

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenForm(new KelolaBarangForm());
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenForm(new KelolaKategoriForm());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenForm(new KelolaSupplierForm());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenForm(new LaporanPenjualanForm());
        }

        private void button10_Click(object sender, EventArgs e)
        {
            OpenForm(new TransaksiPenjualanForm());

        }

        private void button9_Click(object sender, EventArgs e)
        {
            OpenForm(new RiwayatTransaksiForm());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
       "Apakah anda yakin ingin logout?",
       "Konfirmasi",
       MessageBoxButtons.YesNo,
       MessageBoxIcon.Question);

            if (confirm == DialogResult.No)
                return;

            // 1. Clear session
            SessionManager.Clear();

            // 2. Kembali ke LoginForm
            Login login = new Login();
            login.Show();

            // 3. Tutup MainForm
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
      "Apakah anda yakin ingin logout?",
      "Konfirmasi",
      MessageBoxButtons.YesNo,
      MessageBoxIcon.Question);

            if (confirm == DialogResult.No)
                return;


            SessionManager.Clear();


            Login login = new Login();
            login.Show();


            this.Close();
        }

        private void MainMenuForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SessionManager.Clear();
        }
    }
}
