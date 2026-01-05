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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

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
            cartBindingSource.AddNew();
            cartBindingSource.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (productBindingSource.Current is Product product)
            {
                var carts = cartBindingSource.List.Cast<Cart>().FirstOrDefault(s => s.ProductId == product.ProductId);
                if (numericUpDown1.Value == 0)
                {
                    MessageBox.Show("Mohon masukan jumlah barang");
                    return;
                }
                else
                {
                    if (carts != null)
                    {
                        carts.Quantity += (int)numericUpDown1.Value;
                    }
                    else
                    {
                        Cart cart = new Cart();
                        cart.ProductId = product.ProductId;
                        cart.ProductName = product.ProductName;
                        cart.Price = product.Price;
                        cart.Quantity = (int)numericUpDown1.Value;
                        cart.CategoryName = product.Category.CategoryName;
                        cart.SupplierName = product.Supplier.SupplierName;
                        cartBindingSource.Add(cart);
                    }
                    numericUpDown1.Value = 0;
                    cartBindingSource.ResetBindings(true);
                }
            }
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
            if (cartBindingSource.Count == 0)
            {
                MessageBox.Show("Mohon masukan pesanan");
                return;
            }
            else
            {
                var carts = cartBindingSource.List.Cast<Cart>().ToList();
                var product = db.Products;

                // Add Update Stock
                foreach (Cart cart in carts)
                {
                    var stock_produk = product.FirstOrDefault(s => s.ProductId == cart.ProductId);
                    if (stock_produk.Stock < cart.Quantity)
                    {
                        MessageBox.Show("Stock produk kurang");
                        return;
                    }
                    else
                    {
                        stock_produk.Stock -= cart.Quantity;
                    }
                    db.Products.AddOrUpdate(stock_produk);
                }

                // Add Sale
                Sale sale = new Sale();
                sale.SaleDate = DateTime.Now;
                sale.UserId = 1;
                sale.TotalAmount = carts.Sum(c => c.SubTotal);
                db.Sales.Add(sale);

                // Add Sales Detail
                foreach (Cart cart in carts)
                {
                    SaleDetail saleDetail = new SaleDetail();
                    saleDetail.Price = cart.Price;
                    saleDetail.ProductId = cart.ProductId;
                    saleDetail.Quantity = cart.Quantity;
                    saleDetail.SubTotal = cart.SubTotal;
                    saleDetail.Quantity = cart.Quantity;
                    db.SaleDetails.Add(saleDetail);
                }

                if (MessageBox.Show("Apakah transaksi sudah selesai?", "Konfirmasi", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
                if (db.SaveChanges() > 0)
                {
                    MessageBox.Show("Pesanan berhasil di bayar");
                    cartBindingSource.Clear();
                    TransaksiPenjualanForm_Load(sender, e);
                }
            }


        }

        private void cartBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            label3.Text = $"Total : {cartBindingSource.Cast<Cart>().Sum(c => c.SubTotal).ToString("C", CultureInfo.GetCultureInfo("ID-id"))}";
        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView2.Rows[e.RowIndex].DataBoundItem is Cart cart)
            {
                if (priceCart_col.Index == e.ColumnIndex)
                {
                    e.Value = cart.Price.ToString("C", CultureInfo.GetCultureInfo("id-ID"));
                }
                if (subtotalCart_col.Index == e.ColumnIndex)
                {
                    e.Value = cart.SubTotal.ToString("C", CultureInfo.GetCultureInfo("id-ID"));
                }
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
