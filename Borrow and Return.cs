using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Borrow_and_Return : Form
    {
        // Connection string
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\vs pro\WindowsFormsApplication3\WindowsFormsApplication3\LibraryDB.mdf;Integrated Security=True");

        public Borrow_and_Return()
        {
            InitializeComponent();
        }

        // Consolidated Load Event
        private void Borrow_and_Return_Load(object sender, EventArgs e)
        {
            LoadMembers();
            LoadBooks();
            LoadBorrowRecords();
        }

        private void LoadMembers()
        {
            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT MemberID, MemberName FROM Members", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "MemberName";
                comboBox1.ValueMember = "MemberID";
                comboBox1.SelectedIndex = -1; // Keep it blank initially
            }
            catch (Exception ex) { MessageBox.Show("Error loading members: " + ex.Message); }
            finally { conn.Close(); }
        }

        private void LoadBooks()
        {
            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT BookID, BookTitle FROM Books", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                comboBox2.DataSource = dt;
                comboBox2.DisplayMember = "BookTitle";
                comboBox2.ValueMember = "BookID";
                comboBox2.SelectedIndex = -1; // Keep it blank initially
            }
            catch (Exception ex) { MessageBox.Show("Error loading books: " + ex.Message); }
            finally { conn.Close(); }
        }

        private void LoadBorrowRecords()
        {
            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(
                    @"SELECT BorrowID, Members.MemberName, Books.BookTitle, BorrowDate, DueDate, ReturnDate, ReturnStatus 
                      FROM BorrowRecords 
                      INNER JOIN Members ON BorrowRecords.MemberID = Members.MemberID 
                      INNER JOIN Books ON BorrowRecords.BookID = Books.BookID", conn);

                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex) { MessageBox.Show("Error loading records: " + ex.Message); }
            finally { conn.Close(); }
        }

        // BORROW BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null || comboBox2.SelectedValue == null)
            {
                MessageBox.Show("Please select both a Member and a Book.");
                return;
            }

            try
            {
                conn.Open();

                // 1. Check available copies
                SqlCommand check = new SqlCommand("SELECT AvailableCopies FROM Books WHERE BookID=@BookID", conn);
                check.Parameters.AddWithValue("@BookID", comboBox2.SelectedValue);
                int copies = Convert.ToInt32(check.ExecuteScalar());

                if (copies <= 0)
                {
                    MessageBox.Show("Warning: This book is currently out of stock!", "Not Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. Insert into BorrowRecords
                SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO BorrowRecords (MemberID, BookID, BorrowDate, DueDate, ReturnStatus) 
                      VALUES (@MemberID, @BookID, @BorrowDate, @DueDate, 'Not Returned')", conn);

                cmd.Parameters.AddWithValue("@MemberID", comboBox1.SelectedValue);
                cmd.Parameters.AddWithValue("@BookID", comboBox2.SelectedValue);
                cmd.Parameters.AddWithValue("@BorrowDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@DueDate", DateTime.Now.AddDays(14));
                cmd.ExecuteNonQuery();

                // 3. Decrease AvailableCopies by 1
                // FIXED BUG: You originally had comboBox1.SelectedValue here (MemberID) instead of comboBox2 (BookID)
                SqlCommand update = new SqlCommand("UPDATE Books SET AvailableCopies = AvailableCopies - 1 WHERE BookID=@BookID", conn);
                update.Parameters.AddWithValue("@BookID", comboBox2.SelectedValue);
                update.ExecuteNonQuery();

                MessageBox.Show("Book Borrowed Successfully.");
            }
            catch (Exception ex) { MessageBox.Show("Error borrowing book: " + ex.Message); }
            finally { conn.Close(); LoadBorrowRecords(); }
        }

        // RETURN BUTTON
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Select a borrow record from the table.");
                return;
            }

            // FIXED BUG: Prevent returning a book that is already returned
            string currentStatus = dataGridView1.CurrentRow.Cells["ReturnStatus"].Value.ToString();
            if (currentStatus == "Returned")
            {
                MessageBox.Show("This book has already been returned!");
                return;
            }

            int borrowID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["BorrowID"].Value);

            try
            {
                conn.Open();

                // 1. Update ReturnDate and ReturnStatus
                SqlCommand cmd = new SqlCommand(
                    @"UPDATE BorrowRecords 
                      SET ReturnDate=@ReturnDate, ReturnStatus='Returned' 
                      WHERE BorrowID=@BorrowID", conn);

                cmd.Parameters.AddWithValue("@BorrowID", borrowID);
                cmd.Parameters.AddWithValue("@ReturnDate", DateTime.Now);
                cmd.ExecuteNonQuery();

                // 2. Increase AvailableCopies by 1
                SqlCommand update = new SqlCommand(
                    @"UPDATE Books 
                      SET AvailableCopies = AvailableCopies + 1 
                      WHERE BookID = (SELECT BookID FROM BorrowRecords WHERE BorrowID=@BorrowID)", conn);

                update.Parameters.AddWithValue("@BorrowID", borrowID);
                update.ExecuteNonQuery();

                MessageBox.Show("Book Returned Successfully.");
            }
            catch (Exception ex) { MessageBox.Show("Error returning book: " + ex.Message); }
            finally { conn.Close(); LoadBorrowRecords(); }
        }

        // FILTER / SEARCH BUTTON
        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox3.Text))
            {
                LoadBorrowRecords(); // If filter is empty, load everything
                return;
            }

            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(
                    @"SELECT BorrowID, Members.MemberName, Books.BookTitle, BorrowDate, DueDate, ReturnDate, ReturnStatus 
                      FROM BorrowRecords 
                      INNER JOIN Members ON BorrowRecords.MemberID = Members.MemberID 
                      INNER JOIN Books ON BorrowRecords.BookID = Books.BookID 
                      WHERE ReturnStatus=@Status", conn);

                // Make sure the text in comboBox3 exactly matches 'Returned' or 'Not Returned'
                da.SelectCommand.Parameters.AddWithValue("@Status", comboBox3.Text);

                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex) { MessageBox.Show("Error filtering records: " + ex.Message); }
            finally { conn.Close(); }
        }
    }
}