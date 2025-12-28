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
    public partial class KelolaBarangForm : Form
    {
        SmartInventoryDBEntities db = new SmartInventoryDBEntities();
        public KelolaBarangForm()
        {
            InitializeComponent();
        }

        private void KelolaBarangForm_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;
            Bounds = Screen.PrimaryScreen.Bounds;
            productBindingSource.DataSource = db.Products.ToList();
            categoryBindingSource.DataSource = db.Categories.ToList();
            supplierBindingSource.DataSource = db.Suppliers.ToList();   
        }

        private void productBindingSource_PositionChanged(object sender, EventArgs e)
        {
            if(productBindingSource.Current is Product product)
            {
                ProductBinding1.DataSource = db.Products.FirstOrDefault(s => s.ProductId == product.ProductId);
                categoryBindingSource.Position = categoryBindingSource.Find("CategoryID", product.CategoryId);
                supplierBindingSource.Position = supplierBindingSource.Find("SupplierId", product.SupplierId);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ProductBinding1.Clear();
            ProductBinding1.AddNew();
        }
    }
}
