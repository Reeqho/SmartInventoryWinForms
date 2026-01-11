using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartInventory_SalesManagementSystem.Kasir
{
    public partial class RiwayatTransaksiForm : Form
    {
        SmartInventoryDBEntities db = new SmartInventoryDBEntities();
        public RiwayatTransaksiForm()
        {
            InitializeComponent();
        }

        private void RiwayatTransaksiForm_Load(object sender, EventArgs e)
        {
            saleDetailBindingSource.Clear();
            saleBindingSource.Clear();
            SaleBindingTanggal.DataSource = db.Sales.Select(s => DbFunctions.TruncateTime(s.SaleDate)).Distinct().ToList();
            saleBindingSource.DataSource = db.Sales.ToList();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is Sale sale)
            {
                var sales_detail = db.SaleDetails.Where(s => s.SaleId == sale.SaleId);
                if (e.ColumnIndex == detail_btn.Index)
                {
                    saleDetailBindingSource.DataSource = sales_detail.ToList();
                    label4.Text = $"Total Transaksi : {sales_detail.ToList().Count}";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RiwayatTransaksiForm_Load(sender, e);
        }

        private void SaleBindingTanggal_CurrentChanged(object sender, EventArgs e)
        {


        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime date = DateTime.Parse(comboBox2.Text);
            saleBindingSource.DataSource = db.Sales.Where(s => DbFunctions.TruncateTime(s.SaleDate) == DbFunctions.TruncateTime(date.Date)).ToList();
        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView2.Rows[e.RowIndex].DataBoundItem is SaleDetail saledetail)
            {
                if (e.ColumnIndex == productIdDataGridViewTextBoxColumn.Index)
                {
                    e.Value = saledetail.Product.ProductName;
                }
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is Sale sale)
            {
                if (e.ColumnIndex == jam_col.Index)
                {
                    e.Value = sale.SaleDate.Value.ToString("HH:mm:ss");
                }
                if (e.ColumnIndex == saleDateDataGridViewTextBoxColumn.Index)
                {
                    e.Value = sale.SaleDate.Value.ToString("dd MMMM yyyy");
                }
            }
        }
    }
}
