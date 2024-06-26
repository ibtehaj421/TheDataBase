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
using System.Collections;

namespace TrainerInterface
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            PopulateGymNames();
        }

        private void PopulateGymNames()
        {
            // Clear existing items in the CheckListBox
            checkedListBox1.Items.Clear();
            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";

            try
            {
                // Open a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to retrieve gym names
                    string query = "SELECT NAME FROM GYM";

                    // Create a command to execute the query
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Execute the query and read the results
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Iterate through the results and add gym names to the CheckListBox
                            while (reader.Read())
                            {
                                checkedListBox1.Items.Add(reader["NAME"].ToString());
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                // Handle any errors
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox1.ForeColor = Color.Red;
            panel1.BackColor = Color.Red;
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            textBox3.Clear();
            textBox3.ForeColor = Color.Red;
            panel3.BackColor = Color.Red;
        }

        private void textBox4_Click(object sender, EventArgs e)
        {
            textBox4.Clear();
            textBox4.ForeColor = Color.Red;
            panel5.BackColor = Color.Red;
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            textBox2.ForeColor = Color.Red;
            panel2.BackColor = Color.Red;
        }

        private void textBox5_Click(object sender, EventArgs e)
        {
            textBox5.Clear();
            textBox5.ForeColor = Color.Red;
            panel6.BackColor = Color.Red;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Create an instance of the login form
            Form1 loginForm = new Form1();

            this.Hide();

            // Show the login form
            loginForm.Show();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        public string GetNewTrainerId()
        {
            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";
            string newId = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT CONCAT('T', CAST(COUNT(trainer_id) + 2 AS varchar(10))) AS new_id " +
                                 "FROM TRAINER";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())  // Check if there's a result
                            {
                                newId = reader["new_id"].ToString();  // Get the value from the "new_id" column
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that might occur during database connection or execution
                Console.WriteLine("Error: " + ex.Message);
            }

            return newId;
        }


        private void Login_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cm;

                string name = textBox5.Text;
                string username = textBox1.Text;
                string email = textBox3.Text;
                string password = textBox4.Text;
                string experience = textBox2.Text;
                string id = GetNewTrainerId();
                string clients = "0";
                string rating = "0";

                if (string.IsNullOrEmpty(name) || checkedListBox1.CheckedItems.Count == 0 || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(experience) || string.IsNullOrEmpty(password) || name == "Name" || username == "Username" || email == "Email" || experience == "Experience" || password == "Password")
                {
                    MessageBox.Show("Please provide complete information.");
                    return;
                }

                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();

                string query1 = "Insert into TRAINER (trainer_id, name, username, password, email, experience, clients, rating) values ('" + id + "','" + name + "','" + username + "','" + password + "','" + email + "','" + experience + "','" + clients + "','" + rating + "')";
                string query2 = "SELECT COUNT(*) FROM TRAINER WHERE Email = @Email OR Username = @Username";
                string query3 = "";

                using (SqlCommand cmd = new SqlCommand(query2, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Username", username);

                    int count = (int)cmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Email or Username already exists.");
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        textBox4.Clear();
                        textBox5.Clear();
                    }

                    else
                    {
                        cm = new SqlCommand(query1, conn);
                        cm.ExecuteNonQuery();
                        cm.Dispose();

                        foreach (string item in checkedListBox1.CheckedItems)
                        {
                            query3 = "INSERT INTO TRAINER_GYM VALUES ('" + id + "', (SELECT gym_id FROM GYM WHERE NAME = '" + item.ToString() + "'))";
                            cm = new SqlCommand(query3, conn);
                            cm.ExecuteNonQuery();
                            cm.Dispose();
                        }

                        query3 = "INSERT INTO verification_request VALUES ((SELECT CONCAT('R', COUNT(request_id) + 1) FROM verification_request), '" + id + "')";
                        cm = new SqlCommand(query3, conn);
                        cm.ExecuteNonQuery();
                        cm.Dispose();

                        conn.Close();

                        // Create an instance of the Dashboard
                        Dashboard dashboard = new Dashboard(name, username, email, password, experience, id, clients, rating);

                        this.Hide();

                        // Show the login form
                        dashboard.Show();
                    }
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
