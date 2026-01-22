using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace SmartInventory_SalesManagementSystem.Kasir
{
    public partial class TransaksiPenjualanForm : Form
    {
        SmartInventoryDbContext db = new SmartInventoryDbContext();

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

            if (MessageBox.Show("Apakah transaksi sudah selesai?",
                "Konfirmasi", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var carts = cartBindingSource.List.Cast<Cart>().ToList();

                    // ===== CEK & UPDATE STOK =====
                    foreach (Cart cart in carts)
                    {
                        var product = db.Products.First(p => p.ProductId == cart.ProductId);

                        if (product.Stock < cart.Quantity)
                        {
                            MessageBox.Show($"Stock {product.ProductName} kurang");
                            return;
                        }

                        product.Stock -= cart.Quantity;
                    }

                    // ===== SALE =====
                    Sale sale = new Sale
                    {
                        SaleDate = DateTime.Now,
                        UserId = 1,
                        TotalAmount = carts.Sum(c => c.SubTotal)
                    };

                    db.Sales.Add(sale);
                    db.SaveChanges(); 

                    // ===== SALE DETAIL =====
                    foreach (Cart cart in carts)
                    {
                        SaleDetail detail = new SaleDetail
                        {
                            SaleId = sale.SaleId,
                            ProductId = cart.ProductId,
                            Quantity = cart.Quantity,
                            Price = cart.Price,
                            SubTotal = cart.SubTotal
                        };

                        db.SaleDetails.Add(detail);
                    }

                    db.SaveChanges();
                    transaction.Commit();

                    MessageBox.Show("Pesanan berhasil dibayar");

                    // ===== CETAK STRUK =====
                    if (MessageBox.Show("Cetak struk?", "Cetak",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        CetakStruk(carts);
                    }

                    cartBindingSource.Clear();
                    TransaksiPenjualanForm_Load(sender, e);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Transaksi gagal: " + ex.Message);
                }
            }
        }

        private void CetakStruk(List<Cart> carts)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                FileName = "Struk_Bayar.pdf"
            };

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            Document doc = new Document(PageSize.A4, 25, 25, 25, 25);
            PdfWriter.GetInstance(doc, new FileStream(sfd.FileName, FileMode.Create));
            doc.Open();

            Paragraph title = new Paragraph("STRUK PEMBAYARAN\n\n",
                FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16))
            { Alignment = Element.ALIGN_CENTER };

            doc.Add(title);
            doc.Add(new Paragraph($"Tanggal: {DateTime.Now:dd MMMM yyyy HH:mm:ss}\n\n"));

            PdfPTable table = new PdfPTable(4) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 40f, 15f, 20f, 25f });

            AddCell(table, "Produk", true);
            AddCell(table, "Qty", true);
            AddCell(table, "Harga", true);
            AddCell(table, "Subtotal", true);

            decimal total = 0;

            foreach (var cart in carts)
            {
                decimal subtotal = cart.Quantity * cart.Price;
                total += subtotal;

                AddCell(table, cart.ProductName);
                AddCell(table, cart.Quantity.ToString());
                AddCell(table, cart.Price.ToString("C", CultureInfo.GetCultureInfo("id-ID")));
                AddCell(table, subtotal.ToString("C", CultureInfo.GetCultureInfo("id-ID")));
            }

            doc.Add(table);
            doc.Add(new Paragraph($"\nTOTAL: {total:C}",
                FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));

            doc.Close();
        }

        private void AddCell(PdfPTable table, string text, bool isHeader = false)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, FontFactory.GetFont(FontFactory.HELVETICA, isHeader ? 10 : 9)));

            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Padding = 5;
            cell.BackgroundColor = isHeader ? BaseColor.LIGHT_GRAY : BaseColor.WHITE;

            table.AddCell(cell);
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
