using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Drawing;
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
            saleBindingSource.DataSource = db.Sales.ToList();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is Sale sale)
            {
                if(e.ColumnIndex == detail_btn.Index)
                {
                    saleDetailBindingSource.DataSource = db.SaleDetails.Where(s => s.SaleId == sale.SaleId).ToList();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
