using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartInventory_SalesManagementSystem.Admin
{
    public partial class LaporanPenjualanForm : Form
    {
        SmartInventoryDBEntities db = new SmartInventoryDBEntities();
        public LaporanPenjualanForm()
        {
            InitializeComponent();
        }

        private void LaporanPenjualanForm_Load(object sender, EventArgs e)
        {
            saleDetailBindingSource.DataSource = db.SaleDetails.ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            saleDetailBindingSource.DataSource = db.SaleDetails.
                Where(x => DbFunctions.TruncateTime(x.Sale.SaleDate) >= DbFunctions.TruncateTime(dateTimePicker1.Value)
                && DbFunctions.TruncateTime(x.Sale.SaleDate) <= DbFunctions.TruncateTime(dateTimePicker2.Value)).ToList();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is SaleDetail saledetail)
            {
                if (e.ColumnIndex == saleIdDataGridViewTextBoxColumn.Index)
                {
                    e.Value = saledetail.Sale.User.Username;
                }
                if (e.ColumnIndex == productIdDataGridViewTextBoxColumn.Index)
                {
                    e.Value = saledetail.Product.ProductName;
                }
                if (e.ColumnIndex == tanggal_col.Index)
                {
                    e.Value = saledetail.Sale.SaleDate.Value.Date.ToString("yyyy/MM/dd");
                }
                if (e.ColumnIndex == jam_col.Index)
                {
                    e.Value = saledetail.Sale.SaleDate.Value.ToString("HH:mm:ss");
                }
                //if (e.ColumnIndex == totalOmzet_col.Index)
                //{
                //    e.Value = saledetail.Quantity * saledetail.Price;
                //}
            }
        }

        private void saleDetailBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            label5.Text = "Total Omzet : " + saleDetailBindingSource.Cast<SaleDetail>().Sum(x => x.Quantity * x.Price).ToString("C", CultureInfo.GetCultureInfo("id-ID"));
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                LaporanPenjualanForm_Load(sender, e);
            }
            else
            {
                saleDetailBindingSource.DataSource = db.SaleDetails
                .Where(x => x.Product.ProductName.ToLower().Trim().Contains(textBox1.Text.Trim().ToLower()))
                .ToList();
            }
        }
    }
}
