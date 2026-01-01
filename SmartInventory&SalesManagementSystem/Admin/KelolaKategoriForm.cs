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
    public partial class KelolaKategoriForm : Form
    {
        SmartInventoryDBEntities db = new SmartInventoryDBEntities();
        public KelolaKategoriForm()
        {
            InitializeComponent();
        }

        private void KelolaKategoriForm_Load(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            textBox2.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = true;
            dataGridView1.Enabled = true;
            categoryBindingSource.DataSource = db.Categories.ToList();

            FormBorderStyle = FormBorderStyle.None;
            Bounds = Screen.PrimaryScreen.Bounds;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                categoryBindingSource.DataSource = db.Categories.ToList();
            }
            else
            {
                categoryBindingSource.DataSource = db.Categories.Where(x => x.CategoryName.Trim().ToLower().Contains(textBox1.Text.ToLower().Trim())).ToList();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is Category category)
            {
                if (e.ColumnIndex == edit_btn.Index)
                {
                    textBox1.Enabled = false;
                    textBox2.Enabled = true;
                    button1.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = false;
                    dataGridView1.Enabled = false;
                    categorybinding1.DataSource = db.Categories.AsNoTracking().FirstOrDefault(s => s.CategoryId == category.CategoryId);
                }
                if (e.ColumnIndex == del_btn.Index)
                {

                    db.Categories.Remove(category);
                    if (MessageBox.Show("Apakah anda ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                    if (db.SaveChanges() > 0)
                    {
                        MessageBox.Show("Data berhasil di hapus");
                        KelolaKategoriForm_Load(sender, e);
                    }

                }
            }
        }

        private void categoryBindingSource_PositionChanged(object sender, EventArgs e)
        {
            if (categoryBindingSource.Current is Category category)
            {
                categorybinding1.DataSource = db.Categories.AsNoTracking().FirstOrDefault(s => s.CategoryId == category.CategoryId);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (categorybinding1.Current is Category category)
            {
                db.Categories.AddOrUpdate(category);
                if (MessageBox.Show("Apakah anda ingin menyimpan data ini?", "Konfirmasi", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
                if (db.SaveChanges() > 0)
                {
                    MessageBox.Show("Data berhasil di simpan");
                    KelolaKategoriForm_Load(sender, e);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            categorybinding1.Clear();
            categorybinding1.AddNew();
            textBox1.Enabled = false;
            textBox2.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = false;
            dataGridView1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            categorybinding1.Clear();
            KelolaKategoriForm_Load(sender, e);
        }
    }
}
