using SmartInventory_SalesManagementSystem.Admin;
using SmartInventory_SalesManagementSystem.Kasir;
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
        public bool IsAdmin = true;
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

            if (IsAdmin == false)
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
    }
}
