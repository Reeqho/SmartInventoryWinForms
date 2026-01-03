using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
                Where(x => x.Sale.SaleDate.Value.Date >= dateTimePicker1.Value.Date 
                && x.Sale.SaleDate.Value.Date <= dateTimePicker2.Value.Date).ToList();
        }
    }
}
