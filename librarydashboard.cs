using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class librarydashboard : Form
    {
        public librarydashboard()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Member mb = new Member();
            mb.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Book bk = new Book();
            bk.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Borrow_and_Return br = new Borrow_and_Return();
            br.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            Login lg = new Login();
            lg.Show();
            this.Hide();
            
            
        }
    }
}
