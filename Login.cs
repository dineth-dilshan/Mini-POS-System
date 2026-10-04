using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApplication3
{
    public partial class Login : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\vs pro\WindowsFormsApplication3\WindowsFormsApplication3\LibraryDB.mdf;Integrated Security=True");

        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)

            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Register r = new Register();
            r.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand
                (
                "select count (*) from [user] where Username=@Username and PassWord=@PassWord",conn
                
                );
            cmd.Parameters.AddWithValue("@Username",textBox1.Text);
            cmd.Parameters.AddWithValue("@PassWord",textBox2.Text);
            int count = (int)cmd.ExecuteScalar();
            if (count > 0)
            {
                MessageBox.Show("login succeed");
                librarydashboard dash = new librarydashboard();
                dash.Show();
                this.Hide();

            }
            else
            { MessageBox.Show("the entered username or password not matching ");
                return;
                textBox1.Clear();
                textBox2.Clear();

            }


            conn.Close();


            
        }
    }
}
