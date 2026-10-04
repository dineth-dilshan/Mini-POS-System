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

    public partial class Book : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\vs pro\WindowsFormsApplication3\WindowsFormsApplication3\LibraryDB.mdf;Integrated Security=True");

        public Book()
        {
            InitializeComponent();
        }

        private void Book_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {

                MessageBox.Show("no fields can be empty");

            }

            int copies;
            try
            {
                copies = Convert.ToInt32(textBox4.Text);
                

            }

            catch
            {
                MessageBox.Show("insert only numbers");
                return;

            }

            if (copies < 0)
            {
                MessageBox.Show("enter non negaitive value");
                return;
            }

            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand
                    (
                        "insert into Books (BookTitle,Author,ShelfLocation,AvailableCopies) values (@bt,@auth,@sl,@ac) ", conn
                    );

                cmd.Parameters.AddWithValue("@bt", textBox1.Text);
                cmd.Parameters.AddWithValue("@auth", textBox2.Text);
                cmd.Parameters.AddWithValue("@sl", textBox3.Text);
                cmd.Parameters.AddWithValue("@ac", textBox4.Text);

                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("insert completed!");
                loadm();

            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if(conn.State == ConnectionState.Open)
                    conn.Close();

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            DialogResult result = 
            MessageBox.Show("are you sure?","info",MessageBoxButtons.YesNo);
            if(result==DialogResult.Yes)
                if (result == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["BookID"].Value);

                    conn.Open();

                    SqlCommand cmd = new SqlCommand(
                        "DELETE FROM Books WHERE BookID=@BookID", conn);

                    cmd.Parameters.AddWithValue("@BookID", id);

                    cmd.ExecuteNonQuery();

                    conn.Close();

                    MessageBox.Show("Book deleted.");

                    loadm();
                }

        }

        private void loadm()
        {

            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter
                (
                "select * from [Books]", conn
                );

            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            conn.Close();

            label6.Text = "" + dt.Rows.Count;
            label5.Text = "" + dt.Rows.Count;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Please select a book.");
                return;

            }

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["BookID"].Value);

            conn.Open();

            SqlCommand cmd = new SqlCommand(
            "UPDATE [Books] SET BookTitle=@BookTitle,Author=@Author,ShelfLocation=@Shelf,AvailableCopies=@Copies WHERE BookID=@BookID", conn);

            cmd.Parameters.AddWithValue("@BookID", id);
            cmd.Parameters.AddWithValue("@BookTitle", textBox1.Text);
            cmd.Parameters.AddWithValue("@Author", textBox2.Text);
            cmd.Parameters.AddWithValue("@Shelf", textBox3.Text);
            cmd.Parameters.AddWithValue("@Copies", textBox4.Text);

            cmd.ExecuteNonQuery();

            conn.Close();

            MessageBox.Show("Book updated successfully.");

            loadm();


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                textBox1.Text = row.Cells["BookTitle"].Value.ToString();
                textBox2.Text = row.Cells["Author"].Value.ToString();
                textBox3.Text = row.Cells["ShelfLocation"].Value.ToString();
                textBox4.Text = row.Cells["AvailableCopies"].Value.ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand

                    (
                        "select * from [Books] where BookTitle like @s1 or Author @s2", conn

                    );

                cmd.Parameters.AddWithValue("@s1", "%" + textBox1.Text + "%");
                cmd.Parameters.AddWithValue("@s2", "%" + textBox2.Text + "%");

                cmd.ExecuteNonQuery();
                conn.Close();
                loadm();


            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
    }
}
