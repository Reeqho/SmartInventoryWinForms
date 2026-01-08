using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartInventory_SalesManagementSystem.Admin
{
    public partial class KelolaSupplierForm : Form
    {
        SmartInventoryDBEntities db = new SmartInventoryDBEntities();
        public KelolaSupplierForm()
        {
            InitializeComponent();
        }

        private void KelolaSupplierForm_Load(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = true;
            dataGridView1.Enabled = true;

            supplierBindingSource.DataSource = db.Suppliers.ToList();
            dataGridView1.Focus();
            FormBorderStyle = FormBorderStyle.None;
            Bounds = Screen.PrimaryScreen.Bounds;
        }

        private void supplierBindingSource_PositionChanged(object sender, EventArgs e)
        {
            if (supplierBindingSource.Current is Supplier supplier)
            {
                supplierbinding1.DataSource = db.Suppliers.AsNoTracking().FirstOrDefault(s => s.SupplierId == supplier.SupplierId);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is Supplier supplier)
            {
                if (e.ColumnIndex == edit_btn.Index)
                {
                    textBox1.Enabled = false;
                    textBox2.Enabled = true;
                    textBox3.Enabled = true;
                    textBox4.Enabled = true;
                    button1.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = false;
                    dataGridView1.Enabled = false;
                    supplierbinding1.DataSource = db.Suppliers.AsNoTracking().FirstOrDefault(s => s.SupplierId == supplier.SupplierId);
                }
                if (e.ColumnIndex == del_btn.Index)
                {
                    if (MessageBox.Show("Apakah anda ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                    db.Suppliers.Remove(supplier);
                    if (db.SaveChanges() > 0)
                    {
                        MessageBox.Show("Data berhasil di hapus");
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (supplierbinding1.Current is Supplier supplier)
            {
                db.Suppliers.AddOrUpdate(supplier);
                if (MessageBox.Show("Apakah anda ingin menyimpan data ini?", "Konfirmasi", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
                if (db.SaveChanges() > 0)
                {
                    MessageBox.Show("Data berhasil di simpan");
                    KelolaSupplierForm_Load(sender, e);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            supplierbinding1.Clear();
            KelolaSupplierForm_Load(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = false;
            dataGridView1.Enabled = false;
            supplierbinding1.Clear();
            supplierbinding1.AddNew();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                supplierBindingSource.DataSource = db.Suppliers.ToList();
            }
            else
            {
                supplierBindingSource.DataSource = db.Suppliers.Where(s => s.SupplierName.ToLower().Trim().Contains(textBox1.Text.ToLower().Trim())).ToList();
            }
        }
    }
}
