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
    public partial class Register : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\vs pro\WindowsFormsApplication3\WindowsFormsApplication3\LibraryDB.mdf;Integrated Security=True");

        public Register()
        {
            InitializeComponent();
        }

        private void Register_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("please insert all fields", "information", MessageBoxButtons.OKCancel);
                return;
            }

            if (textBox2.Text != textBox3.Text)
            {
                MessageBox.Show("paswords are not matching");
                textBox2.Clear();
                textBox2.Focus();
                return;

            }

            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM [user] WHERE Username=@Username ", conn);

            cmd.Parameters.AddWithValue("@Username", textBox1.Text);
            int count = (int)cmd.ExecuteScalar();
            if (count > 0)
            {
                MessageBox.Show("username already exist!");
                conn.Close();
                return;

            }
            

            SqlCommand cmd2 =new SqlCommand("INSERT INTO [user](Username,PassWord)VALUES(@Username,@PassWord)",conn);
            cmd2.Parameters.AddWithValue("@Username",textBox1.Text);
            cmd2.Parameters.AddWithValue("@PassWord",textBox2.Text); 

            cmd2.ExecuteNonQuery();
            MessageBox.Show("registration succes!");
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox1.Focus();

            conn.Close();

            Login l = new Login();
            l.Show();
            this.Hide();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Login re = new Login();
            re.Show();
            this.Hide();
        }
    }
}
