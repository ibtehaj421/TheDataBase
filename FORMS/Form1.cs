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
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TrainerInterface
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox1.ForeColor = Color.Red;
            panel1.BackColor = Color.Red;
        }
        private void textBox2_Click(object sender, EventArgs e)
        {
            if (textBox2.ForeColor == Color.White)
                textBox2.Clear();

            textBox2.ForeColor = Color.Red;
            panel2.BackColor = Color.Red;
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            textBox3.Clear();
            textBox3.ForeColor = Color.Red;
            panel3.BackColor = Color.Red;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.ForeColor == Color.White)
                 textBox2.Clear();
          
            textBox2.ForeColor = Color.Red;
            panel2.BackColor = Color.Red;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Create an instance of the login form
            Form2 registrationForm = new Form2();

            this.Hide();
            
            // Show the login form
            registrationForm.Show();
        }

        private void Login_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string username = textBox1.Text;
                string email = textBox2.Text;
                string password = textBox3.Text;

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || username == "Username" || email == "Email" || password == "Password")
                {
                    MessageBox.Show("Please provide complete information.");
                    return;
                }

                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();

                string query = "SELECT COUNT(*) FROM TRAINER WHERE Username = @Username AND Email = @Email AND Password = @Password";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);

                    int count = (int)cmd.ExecuteScalar();

                    if (count > 0)
                    {
                        string name, experience, id, clients, rating;

                        // SQL query to fetch the data
                        query = "SELECT trainer_id, name, experience, clients, rating FROM TRAINER WHERE username = @username";

                        try
                        {
                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                connection.Open();

                                using (SqlCommand command = new SqlCommand(query, connection))
                                {
                                    command.Parameters.AddWithValue("@username", username);

                                    using (SqlDataReader reader = command.ExecuteReader())
                                    {
                                        if (reader.Read())
                                        {
                                            // Retrieve values from the database and store them in variables
                                            name = reader["name"].ToString();
                                            experience = reader["experience"].ToString();
                                            id = reader["trainer_id"].ToString();
                                            clients = reader["clients"].ToString();
                                            rating = reader["rating"].ToString();
                                        }
                                        else
                                        {
                                            // Handle the case where no data was found for the given username
                                            MessageBox.Show("No data found for the provided username.");
                                            return;
                                        }
                                    }
                                }
                            }

                            // Create an instance of the Dashboard with the retrieved values
                            Dashboard dashboard = new Dashboard(name, username, email, password, experience, id, clients, rating);

                            this.Hide();

                            // Show the Dashboard
                            dashboard.Show();
                        }

                        catch (Exception ex)
                        {
                            // Handle any errors
                            MessageBox.Show("Error: " + ex.Message);
                        }
                    }


                    else
                    {
                        MessageBox.Show("Invalid email or password.");
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                    }
                }
            }
        }
    }
}
