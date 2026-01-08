using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartInventory_SalesManagementSystem.Admin
{
    public partial class KelolaBarangForm : Form
    {
        SmartInventoryDBEntities db = new SmartInventoryDBEntities();
        public KelolaBarangForm()
        {
            InitializeComponent();
        }

        private void KelolaBarangForm_Load(object sender, EventArgs e)
        {
            dataGridView1.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = false;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            numericUpDown1.Enabled = false;
            numericUpDown2.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = true;

            FormBorderStyle = FormBorderStyle.None;
            Bounds = Screen.PrimaryScreen.Bounds;
            productBindingSource.DataSource = db.Products.ToList();
            categoryBindingSource.DataSource = db.Categories.ToList();
            supplierBindingSource.DataSource = db.Suppliers.ToList();


        }

        private void productBindingSource_PositionChanged(object sender, EventArgs e)
        {
            if (productBindingSource.Current is Product product)
            {
                ProductBinding1.DataSource = db.Products.AsNoTracking().FirstOrDefault(s => s.ProductId == product.ProductId);
                categoryBindingSource.Position = db.Categories.ToList().FindIndex(s => s.CategoryId == product.CategoryId);
                supplierBindingSource.Position = db.Suppliers.ToList().FindIndex(s => s.SupplierId == product.SupplierId);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ProductBinding1.Clear();
            ProductBinding1.AddNew();
            dataGridView1.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = true;
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            numericUpDown1.Enabled = true;
            numericUpDown2.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = false;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ProductBinding1.Current is Product product
                && categoryBindingSource.Current is Category category
                && supplierBindingSource.Current is Supplier supplier)
            {
                product.CategoryId = category.CategoryId;
                product.SupplierId = supplier.SupplierId;
                //product.Price += .00;
                var check_product = db.Products.FirstOrDefault(s => s.ProductId == product.ProductId);
                if (check_product == null)
                {
                    product.CreatedAt = DateTime.Now;
                    db.Products.Add(product);
                }
                else
                {
                    db.Products.AddOrUpdate(product);
                }


                if (MessageBox.Show("Apakah anda ingin menyimpan data tersebut?", "Konfirmasi", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
                if (db.SaveChanges() > 0)
                {
                    MessageBox.Show("Data berhasil di simpan");
                    KelolaBarangForm_Load(sender, e);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            KelolaBarangForm_Load(sender, e);
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (dataGridView1.Rows[e.RowIndex].DataBoundItem is Product product)
                {
                    if (e.ColumnIndex == categoryIdDataGridViewTextBoxColumn.Index)
                    {
                        e.Value = product.Category.CategoryName;
                    }
                    if (e.ColumnIndex == supplierIdDataGridViewTextBoxColumn.Index)
                    {
                        e.Value = product.Supplier.SupplierName;
                    }
                    if (e.ColumnIndex == priceDataGridViewTextBoxColumn.Index)
                    {
                        e.Value = product.Price.ToString("C", CultureInfo.GetCultureInfo("id-ID"));
                    }
                }
            }
            catch
            {

            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is Product product)
            {
                if (e.ColumnIndex == edit_btn.Index)
                {

                    ProductBinding1.DataSource = db.Products.FirstOrDefault(s => s.ProductId == product.ProductId);
                    categoryBindingSource.Position = db.Categories.ToList().FindIndex(s => s.CategoryId == product.CategoryId);
                    supplierBindingSource.Position = db.Suppliers.ToList().FindIndex(s => s.SupplierId == product.SupplierId);
                    dataGridView1.Enabled = false;
                    textBox1.Enabled = false;
                    textBox2.Enabled = true;
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;
                    numericUpDown1.Enabled = true;
                    numericUpDown2.Enabled = true;
                    button1.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = false;
                }
                if (e.ColumnIndex == del_btn.Index)
                {
                    if (MessageBox.Show("Apakah anda ingin menghapus data tersebut?", "Konfirmasi", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                    db.Products.Remove(product);
                    if (db.SaveChanges() > 0)
                    {
                        MessageBox.Show("Data berhasil di hapus");
                        KelolaBarangForm_Load(sender, e);
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                productBindingSource.DataSource = db.Products.ToList();
            }
            else
            {
                productBindingSource.DataSource = db.Products.Where(s => s.ProductName.ToLower().Trim().Contains(textBox1.Text.ToLower().Trim())
                || s.Category.CategoryName.ToLower().Trim().Contains(textBox1.Text.ToLower().Trim())
                || s.Supplier.SupplierName.ToLower().Trim().Contains(textBox1.Text.ToLower().Trim())).ToList();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
