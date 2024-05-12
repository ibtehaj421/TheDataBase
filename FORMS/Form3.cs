using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace TrainerInterface
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox1.ForeColor = Color.Red;
            panel1.BackColor = Color.Red;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            textBox4.Clear();
            textBox4.ForeColor = Color.Red;
            panel5.BackColor = Color.Red;
        }

        private void Login_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string email = textBox1.Text;
                string password = textBox4.Text;

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || email == "Email" || password == "Password")
                {
                    MessageBox.Show("Please provide complete information.");
                    return;
                }

                textBox1.Clear();
                textBox4.Clear();

                string query = "SELECT COUNT(*) FROM ADMIN WHERE Email = @Email AND Password = @Password";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);

                    int count = (int)cmd.ExecuteScalar();

                    if (count > 0)
                    {
                        // LOGIN ADMIN

                        // Create an instance of the Dashboard
                        //Dashboard dashboard = new Dashboard();

                        //this.Hide();

                        // Show the Dashboard
                        //dashboard.Show();
                    }

                    else
                    {
                        MessageBox.Show("Invalid email or password.");
                        textBox1.Clear();
                        textBox4.Clear();
                    }
                }
            }
        }
    }
}
