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
    public partial class Member : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\vs pro\WindowsFormsApplication3\WindowsFormsApplication3\LibraryDB.mdf;Integrated Security=True");

        public Member()
        {
            InitializeComponent();
        }

        private void Member_Load(object sender, EventArgs e)
        {
            loadm();
        }

        private void ADD_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || comboBox1.SelectedItem == null || comboBox2.SelectedItem == null)
            {
                MessageBox.Show("no fields cannot be empty!");
                return;

            }
            if (!textBox2.Text.All(char.IsDigit))
            {
                MessageBox.Show("contact number must contain only digits");
                return;
            }
            conn.Open();    

            SqlCommand cmd = new SqlCommand
                (
                "insert into [Members](MemberName,MemberType,ContatNumber,Email,Status) values(@MemberName,@MemberType,@ContatNumber,@Email,@Status) ",conn
                );

            cmd.Parameters.AddWithValue("@MemberName",textBox1.Text);
            cmd.Parameters.AddWithValue("@MemberType", comboBox1.SelectedItem);
            cmd.Parameters.AddWithValue("@ContatNumber",textBox2.Text);
            cmd.Parameters.AddWithValue("Email",textBox3.Text);
            cmd.Parameters.AddWithValue("@Status", comboBox2.SelectedItem);

            cmd.ExecuteNonQuery();

            conn.Close();
            loadm();
            MessageBox.Show("member aded succesfully!");

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            textBox1.Focus();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {       
                
                MessageBox.Show("Please select a member.");
                return;
            }

            DialogResult result =
            MessageBox.Show("are you sure?","info",MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)

            {
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["MemberID"].Value);
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand
                        (
                            "delete from [Members] where MemberID = @MemberID", conn
                        );
                    cmd.Parameters.AddWithValue("@MemberID", id);
                    cmd.ExecuteNonQuery();

                    conn.Close();

                    MessageBox.Show("member delete succesfully");
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("please select an member by id");
                return;

            }

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["MemberID"].Value);
            conn.Open();

            SqlCommand cmd = new SqlCommand
                (
                   "update  [Members] set MemberName=@MemberName,MemberType=@MemberType,ContatNumber=@ContatNumber,Email=@Email,Status=@Status where MemberID=@MemberID", conn
                   );


            cmd.Parameters.AddWithValue ("@MemberID" , id);
            cmd.Parameters.AddWithValue("@MemberName",textBox1.Text);
            cmd.Parameters.AddWithValue("@MemberType", comboBox1.Text);
            cmd.Parameters.AddWithValue("@ContatNumber", textBox2.Text);
            cmd.Parameters.AddWithValue("@Email", textBox3.Text);
            cmd.Parameters.AddWithValue("@Status", comboBox2.Text);

            cmd.ExecuteNonQuery();

            conn.Close();
            
            loadm();
            label6.Text = "" +dataGridView1.Rows;

        }

        private void loadm()
        {

            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter
                (
                "select * from [Members]",conn
                );

            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            conn.Close();

            label6.Text = "" + dt.Rows.Count;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand

                    (
                        "select * from Members where MemberName like @s1 or MemberType @s2", conn

                    );

                cmd.Parameters.AddWithValue("@s1", "%" + textBox1.Text + "%");
                cmd.Parameters.AddWithValue("@s2", "%" + comboBox1.SelectedItem + "%");

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
