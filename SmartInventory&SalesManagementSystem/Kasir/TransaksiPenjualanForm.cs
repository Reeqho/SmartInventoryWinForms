using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartInventory_SalesManagementSystem.Kasir
{
    public partial class TransaksiPenjualanForm : Form
    {
        SmartInventoryDBEntities db = new SmartInventoryDBEntities();
        
        public TransaksiPenjualanForm()
        {
            InitializeComponent();
        }

        private void TransaksiPenjualanForm_Load(object sender, EventArgs e)
        {
            productBindingSource.DataSource = db.Products.ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value == 0)
            {
                MessageBox.Show("Mohon masukan jumlah barang nya", "Peringatan", MessageBoxButtons.OK);
                return;
            }
            else
            {
                if (productBindingSource.Current is Product product)
                {
                    var check_existingProduct = PesananBinding.Cast<Product>().
                        FirstOrDefault(s => s.ProductId == product.ProductId);

                    if (check_existingProduct != null)
                    {
                        // what the fuck is this? 
                        check_existingProduct.SaleDetails.First().Quantity += (int)numericUpDown1.Value;
                        check_existingProduct.SaleDetails.First().SubTotal = 
                            check_existingProduct.SaleDetails.First().Quantity * check_existingProduct.Price;
                    }
                    else
                    {
                        SaleDetail saleDetail = new SaleDetail();
                        saleDetail.ProductId = product.ProductId;
                        saleDetail.Quantity = (int)numericUpDown1.Value;
                        saleDetail.Price = product.Price;
                        saleDetail.SubTotal = saleDetail.Price * saleDetail.Quantity;
                        product.SaleDetails.Add(saleDetail);
                        PesananBinding.Add(product);
                        
                    }

                }
                PesananBinding.ResetBindings(true);
            }

        }

        private void PesananBinding_ListChanged(object sender, ListChangedEventArgs e)
        {
            var sub_Total = (int)PesananBinding.Cast<Product>().Sum(s => s.SaleDetails.Sum(x => x.SubTotal));
            label3.Text = $"Total : {sub_Total.ToString("C", CultureInfo.GetCultureInfo("id-ID"))}";
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is Product product)
            {
                if (priceDataGridViewTextBoxColumn.Index == e.ColumnIndex)
                {
                    e.Value = product.Price.ToString("C", CultureInfo.GetCultureInfo("id-ID"));
                }
                if (categoryIdDataGridViewTextBoxColumn.Index == e.ColumnIndex)
                {
                    e.Value = product.Category.CategoryName;
                }
                if (supplierIdDataGridViewTextBoxColumn.Index == e.ColumnIndex)
                {
                    e.Value = product.Supplier.SupplierName;
                }

            }
        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            if (dataGridView2.Rows[e.RowIndex].DataBoundItem is Product product)
            {
                if (priceDataGridViewTextBoxColumn.Index == e.ColumnIndex)
                {
                    e.Value = product.Price.ToString("C", CultureInfo.GetCultureInfo("ID-id"));
                }
                if (categoryIdDataGridViewTextBoxColumn.Index == e.ColumnIndex)
                {
                    e.Value = product.Category.CategoryName;
                }
                if (supplierIdDataGridViewTextBoxColumn.Index == e.ColumnIndex)
                {
                    e.Value = product.Supplier.SupplierName;
                }
                if (quantity_col.Index == e.ColumnIndex)
                {
                    e.Value = product.SaleDetails.Sum(s => s.Quantity);
                }
                if (subTotal_col.Index == e.ColumnIndex)
                {
                    e.Value = product.SaleDetails.Sum(s => s.SubTotal).Value.ToString("C", CultureInfo.GetCultureInfo("id-ID"));
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
                productBindingSource.DataSource = db.Products.Where(p => p.ProductName.ToLower().Trim().Contains(textBox1.Text.ToLower().Trim())).ToList();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Bayar Dan Validasi Produk Tersedia
            var Product_stok_awal = db.Products.ToList();
            foreach (Product product in PesananBinding.List)
            {
                var produk_db = Product_stok_awal.Find(s => s.ProductId == product.ProductId);
                if (produk_db.Stock <= 0)
                {
                    MessageBox.Show("Stock sudah kosong, mohon untuk cek stock terlebih dahulu", "Konfirmasi", MessageBoxButtons.OK);
                    return;
                }
                produk_db.Stock -= 1;
                SaleDetail saleDetail = new SaleDetail();
                saleDetail.ProductId = product.ProductId;
                //saleDetail.Quantity = product.st
            }

        }
    }
}
