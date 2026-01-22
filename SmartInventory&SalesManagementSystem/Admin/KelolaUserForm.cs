using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartInventory_SalesManagementSystem.Admin
{
    public partial class KelolaUserForm : Form
    {
        SmartInventoryDbContext db = new SmartInventoryDbContext();
        public KelolaUserForm()
        {
            InitializeComponent();
        }

        private void KelolaUserForm_Load(object sender, EventArgs e)
        {
            //userBinding1.Clear();
            dataGridView1.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            comboBox1.Enabled = false;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = true;
            userBindingSource.DataSource = db.Users.ToList();
            roleBindingSource.DataSource = db.Roles.ToList();
            FormBorderStyle = FormBorderStyle.None;
            Bounds = Screen.PrimaryScreen.Bounds;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is User user)
            {
                if (e.ColumnIndex == edit_btn.Index)
                {
                    dataGridView1.Enabled = false;
                    textBox1.Enabled = true;
                    textBox2.Enabled = true;
                    textBox3.Enabled = true;
                    textBox4.Enabled = true;
                    comboBox1.Enabled = true;
                    radioButton1.Enabled = true;
                    radioButton2.Enabled = true;
                    button1.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = false;
                    userBinding1.DataSource = db.Users.AsNoTracking().FirstOrDefault(s => s.UserId == user.UserId);
                    if (user.IsActive == true)
                    {
                        radioButton1.Checked = true;
                        radioButton2.Checked = false;
                    }
                    else
                    {
                        radioButton2.Checked = true;
                        radioButton1.Checked = false;
                    }
                }
                if (e.ColumnIndex == del_btn.Index)
                {
                    if (MessageBox.Show("Apakah anda ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                    db.Users.Remove(user);
                    if (db.SaveChanges() > 0)
                    {
                        MessageBox.Show("Data berhasil di hapus");
                        KelolaUserForm_Load(sender, e);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (userBinding1.Current is User check_user && roleBindingSource.Current is Role role)
            {
                check_user.IsActive = radioButton1.Checked ? true : false;
                if (MessageBox.Show("Apakah anda ingin menyimpan data ini?", "Konfirmasi", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
                var user = db.Users.FirstOrDefault(s => s.UserId == check_user.UserId);
                if (user == null)
                {
                    user = new User();
                    user.CreatedAt = DateTime.Now;
                    db.Users.Add(user);
                }
                user.FullName = textBox2.Text;
                user.Username = textBox3.Text;
                user.PasswordHash = textBox4.Text;
                user.IsActive = check_user.IsActive;
                user.RoleId = role.RoleId;
                if (db.SaveChanges() > 0)
                {
                    MessageBox.Show("Data berhasil di simpan");
                    KelolaUserForm_Load(sender, e);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                userBindingSource.DataSource = db.Users.ToList();
            }
            else
            {
                userBindingSource.DataSource = db.Users.Where(s => s.Username.ToLower().Trim().Contains(textBox1.Text.ToLower().Trim())
                && s.FullName.ToLower().Trim().Contains(textBox1.Text.ToLower().Trim())).ToList();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            KelolaUserForm_Load(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            userBinding1.AddNew();
            dataGridView1.Enabled = false;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            comboBox1.Enabled = true;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = false;
        }

        private void userBindingSource_PositionChanged(object sender, EventArgs e)
        {
            if (userBindingSource.Current is User user)
            {
                userBinding1.DataSource = db.Users.AsNoTracking().FirstOrDefault(s => s.UserId == user.UserId);
                if (user.IsActive == true)
                {
                    radioButton1.Checked = true;
                    radioButton2.Checked = false;
                }
                else
                {
                    radioButton2.Checked = true;
                    radioButton1.Checked = false;
                }
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is User user)
            {
                if (e.ColumnIndex == roleIdDataGridViewTextBoxColumn.Index)
                {
                    e.Value = user.Role.RoleName;
                }
            }
        }
    }
}
