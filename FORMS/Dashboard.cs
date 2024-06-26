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

namespace TrainerInterface
{
    public partial class Dashboard : Form
    {
        private string name;
        private string username;
        private string password;
        private string email;
        private string experience;
        private string id;
        private string clients;
        private string rating;
        public Dashboard(string name, string username, string password, string email, string experience, string id, string clients, string rating)
        {
            InitializeComponent();

            this.name = name;
            this.username = username;
            this.password = password;
            this.email = email;
            this.experience = experience;
            this.id = id;
            this.clients = clients;
            this.rating = rating;

            appointmentPanel.Hide();
            feedbackPanel.Hide();
            workoutPanel.Hide();
            dietPlanPanel.Hide();
            createPanel.Hide();
            wcPanel.Hide();
            dcPanel.Hide();

            initializeInfo();
            PopulateAppointment();
            PopulateFeedback();
            PopulateWorkouts();
            PopulateDietPlans();
            PopulateExercies();
            PopulateMeals();
            PopulateMachines();
        }

        private void PopulateFeedback()
        {
            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";

            try
            {
                // Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to select usernames and passwords from the INFO table
                    string query = "select res1.name, member.name as [MemberName], rating, feedback from member join (\tSELECT name, trainer_review.gym_id as [member_id], trainer_review.rating, feedback \tFROM TRAINER_REVIEW \tjoin gym \ton gym.gym_id = trainer_review.member_id WHERE trainer_id = 'T1') res1 on member.member_id = res1.member_id";

                    // Create a data adapter to fetch data from the database
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        // Create a DataTable to hold the fetched data (optional, can build the list directly)
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Clear the existing items in the feedbackTable
                        feedbackTable.Items.Clear();

                        // Variable to store total rating
                        double totalRating = 0.0;

                        // Loop through each row in the DataTable (or directly iterate over the data)
                        foreach (DataRow row in dataTable.Rows)
                        {
                            // Build the string with separation
                            string feedbackItem = $"{row["name"]}          {row["MemberName"]}          {row["rating"]}          {row["feedback"]}";

                            // Add the formatted feedback item to the ListBox
                            feedbackTable.Items.Add(feedbackItem);

                            // Extract rating as double and add to total
                            double rating = Convert.ToDouble(row["rating"]);
                            totalRating += rating;
                        }

                        // Calculate average rating (handle division by zero)
                        double averageRating = totalRating > 0 ? totalRating / dataTable.Rows.Count : 0.0;

                        // Display average rating (replace with your desired location)
                        feedbackText.Text = averageRating.ToString("F2"); // Format to two decimal places
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors
                MessageBox.Show("Error: " + ex.Message);
            }
                
            try
            {
                // Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Modified query to calculate average rating per gym
                    string query = "SELECT res1.name, AVG(rating) AS AverageRating " +
                                    "FROM member " +
                                    "JOIN ( " +
                                        "SELECT name, trainer_review.gym_id AS member_id, trainer_review.rating, feedback " +
                                        "FROM TRAINER_REVIEW " +
                                        "JOIN gym ON gym.gym_id = trainer_review.member_id " +
                                        "WHERE trainer_id = 'T1' " +
                                    ") AS res1 ON member.member_id = res1.member_id " +
                                    "GROUP BY res1.name;";

                    // Create a data adapter to fetch data from the database
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        // Create a DataTable to hold the fetched data
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Clear the existing items in the feedbackTable (optional)
                        gymRatings.Items.Clear();

                        // Loop through each row in the DataTable
                        foreach (DataRow row in dataTable.Rows)
                        {
                            // Get gym name and average rating
                            string gymName = row["name"] as string;
                            double averageRating = Convert.ToDouble(row["AverageRating"]);

                            // Build the string for the registrationsListBox
                            string listBoxItem = $"{gymName}: {averageRating:F2}"; // Format to two decimal places

                            // Add the item to the registrationsListBox
                            gymRatings.Items.Add(listBoxItem);
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
        private void PopulateAppointment()
        {
            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";

            try
            {
                // Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to select usernames and passwords from the INFO table
                    string query = "\r\nSELECT res1.[GymName], name, date from member join (\tSELECT gym.[name] as [GymName], member_id, date\tFROM TRAINING_SESSION \tjoin gym\ton GYM.gym_id = TRAINING_SESSION.gym_id\tWHERE trainer_id = '" + this.id + "') as RES1 on member.member_id = res1.member_id";

                    // Create a data adapter to fetch data from the database
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        // Create a DataTable to hold the fetched data (optional, can build the list directly)
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Clear the existing items in the appointmentTable
                        appointmentTable.Items.Clear();

                        // Loop through each row in the DataTable (or directly iterate over the data)
                        foreach (DataRow row in dataTable.Rows)
                        {
                            // Build the string with separation
                            string appointmentItem = $"{row["GymName"]}          {row["name"]}          {row["date"]}";

                            // Add the formatted feedback item to the ListBox
                            appointmentTable.Items.Add(appointmentItem);
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
        private void PopulateWorkouts()
        {
            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";

            try
            {
                // Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to select usernames and passwords from the INFO table
                    string query = "select * from Workout_Plan";

                    // Create a data adapter to fetch data from the database
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        // Create a DataTable to hold the fetched data (optional, can build the list directly)
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Clear the existing items in the feedbackTable
                        wpListBox.Items.Clear();

                        // Loop through each row in the DataTable (or directly iterate over the data)
                        foreach (DataRow row in dataTable.Rows)
                        {
                            // Build the string with separation
                            string feedbackItem = $"{row["workoutPlan_id"]}      {row["objective"]}                      {row["guidelines"]}                          {row["difficulty_level"]}";

                            // Add the formatted feedback item to the ListBox
                            wpListBox.Items.Add(feedbackItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors
                MessageBox.Show("Error: " + ex.Message);
            }

            try
            {
                // Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to select usernames and passwords from the INFO table
                    string query = "SELECT RES1.workoutPlan_id, dayofweek, equipment_name, sets, reps, restIntervals FROM EXERCISE JOIN (\tSELECT EXERCISE_DAY.workoutPlan_id, dayofweek, exercise_id\tFROM EXERCISE_DAY\tJOIN DAY\tON DAY.day_id = EXERCISE_DAY.day_id) AS RES1 on EXERCISE.exercise_id = RES1.exercise_id ORDER BY workoutPlan_id";

                    // Create a data adapter to fetch data from the database
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        // Create a DataTable to hold the fetched data (optional, can build the list directly)
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Clear the existing items in the feedbackTable
                        exerciseListBox.Items.Clear();

                        // Loop through each row in the DataTable (or directly iterate over the data)
                        foreach (DataRow row in dataTable.Rows)
                        {
                            // Build the string with separation
                            string feedbackItem = $"{row["workoutPlan_id"]}      {row["dayofweek"]}       {row["equipment_name"]}                                           {row["sets"]}   {row["reps"]}   {row["restintervals"]}";

                            // Add the formatted feedback item to the ListBox
                            exerciseListBox.Items.Add(feedbackItem);
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
        private void PopulateDietPlans()
        {
            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";

            try
            {
                // Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to select usernames and passwords from the INFO table
                    string query = "select dietPlan_id, type, objective, guidelines, difficulty_level from DIET_Plan";

                    // Create a data adapter to fetch data from the database
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        // Create a DataTable to hold the fetched data (optional, can build the list directly)
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Clear the existing items in the feedbackTable
                        dpListBox.Items.Clear();

                        // Loop through each row in the DataTable (or directly iterate over the data)
                        foreach (DataRow row in dataTable.Rows)
                        {
                            // Build the string with separation
                            string feedbackItem = $"{row["dietPlan_id"]}      {row["type"]}      {row["objective"]}                      {row["guidelines"]}                          {row["difficulty_level"]}";

                            // Add the formatted feedback item to the ListBox
                            dpListBox.Items.Add(feedbackItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors
                MessageBox.Show("Error: " + ex.Message);
            }

            try
            {
                // Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to select usernames and passwords from the INFO table
                    string query = "SELECT dietPlan_id, meal_id, RES2.name AS [NutritionName], ALLERGEN.NAME as [AllergenName] FROM ALLERGEN JOIN ( \tSELECT dietPlan_id, meal_id, NUTRITION.name, allergen_id \tFROM NUTRITION \tJOIN ( \t\tSELECT dietPlan_id, meal.meal_id, nutrition_id \t\tFROM DIET_PLAN \t\tJOIN MEAL \t\tON MEAL.meal_id = DIET_PLAN.member_id \t) AS RES1 \tON RES1.nutrition_id = NUTRITION.nutrition_id ) AS RES2 ON ALLERGEN.ALLERGEN_ID = RES2.ALLERGEN_ID";

                    // Create a data adapter to fetch data from the database
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        // Create a DataTable to hold the fetched data (optional, can build the list directly)
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Clear the existing items in the feedbackTable
                        mealListBox1.Items.Clear();

                        // Loop through each row in the DataTable (or directly iterate over the data)
                        foreach (DataRow row in dataTable.Rows)
                        {
                            // Build the string with separation
                            string feedbackItem = $"{row["dietPlan_id"]}           {row["meal_id"]}           {row["NutritionName"]}           {row["AllergenName"]}";

                            // Add the formatted feedback item to the ListBox
                            mealListBox1.Items.Add(feedbackItem);
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
        private void initializeInfo()
        {
            textBox1.Text = this.name;
            textBox8.Text = this.password;
            textBox9.Text = this.email;
            textBox10.Text = this.username;

            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";

            try
            {
                // Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to select usernames and passwords from the INFO table
                    string query = "select name from TRAINER_GYM join GYM on gym.gym_id = TRAINER_GYM.gym_id where trainer_id = 'T1'";

                    // Create a data adapter to fetch data from the database
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        // Create a DataTable to hold the fetched data (optional, can build the list directly)
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Clear the existing items in the registrations
                        registrations.Items.Clear();

                        // Loop through each row in the DataTable (or directly iterate over the data)
                        foreach (DataRow row in dataTable.Rows)
                        {
                            // Build the string with separation
                            string regItem = $"{row["name"]}";

                            // Add the formatted feedback item to the ListBox
                            registrations.Items.Add(regItem);
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
        private void PopulateExercies()
        {
            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";

            try
            {
                // Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to select usernames and passwords from the INFO table
                    string query = "select name, sets, reps, restIntervals from EXERCISE";

                    // Create a data adapter to fetch data from the database
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        // Create a DataTable to hold the fetched data (optional, can build the list directly)
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Clear the existing items in the mondayEx
                        mondayEx.Items.Clear();
                        tuesdayEx.Items.Clear();
                        wednesdayEx.Items.Clear();
                        thursdayEx.Items.Clear();
                        fridayEx.Items.Clear();
                        saturdayEx.Items.Clear();
                        sundayEx.Items.Clear();

                        // Loop through each row in the DataTable (or directly iterate over the data)
                        foreach (DataRow row in dataTable.Rows)
                        {
                            // Build the string with separation
                            string regItem = $"{row["name"]} {row["sets"]} {row["reps"]} {row["restIntervals"]}";

                            // Add the formatted feedback item to the ListBox
                            mondayEx.Items.Add(regItem);
                            tuesdayEx.Items.Add(regItem);
                            wednesdayEx.Items.Add(regItem);
                            thursdayEx.Items.Add(regItem);
                            fridayEx.Items.Add(regItem);
                            saturdayEx.Items.Add(regItem);
                            sundayEx.Items.Add(regItem);
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
        private void PopulateMeals()
        {
            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";

            try
            {
                // Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to select usernames and passwords from the INFO table
                    string query = "select nutrition_id, name, unit, quantity, calories from NUTRITION";

                    // Create a data adapter to fetch data from the database
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        // Create a DataTable to hold the fetched data (optional, can build the list directly)
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Clear the existing items in the mondayEx
                        breakfastListBox5.Items.Clear();
                        lunchListBox2.Items.Clear();
                        dinnerListBox3.Items.Clear();
                        snackListBox4.Items.Clear();

                        // Loop through each row in the DataTable (or directly iterate over the data)
                        foreach (DataRow row in dataTable.Rows)
                        {
                            // Build the string with separation
                            string regItem = $"{row["nutrition_id"]} {row["name"]} {row["unit"]} {row["quantity"]} {row["calories"]}";

                            // Add the formatted feedback item to the ListBox
                            breakfastListBox5.Items.Add(regItem);
                            lunchListBox2.Items.Add(regItem);
                            dinnerListBox3.Items.Add(regItem);
                            snackListBox4.Items.Add(regItem);
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
        private void PopulateMachines()
        {
            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";

            try
            {
                // Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to select usernames and passwords from the INFO table
                    string query = "select distinct equipment_name from exercise";

                    // Create a data adapter to fetch data from the database
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        // Create a DataTable to hold the fetched data (optional, can build the list directly)
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Clear the existing items in the mondayEx
                        machinesCheckedListBox.Items.Clear();

                        // Loop through each row in the DataTable (or directly iterate over the data)
                        foreach (DataRow row in dataTable.Rows)
                        {
                            // Build the string with separation
                            string regItem = $"{row["equipment_name"]}";

                            // Add the formatted feedback item to the ListBox
                            machinesCheckedListBox.Items.Add(regItem);
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // New Selection
            button1.BackColor = Color.Red;
            pictureBox2.BackColor = Color.Red;

            // Potential Previous Selection
            button2.BackColor = Color.Black;
            pictureBox3.BackColor = Color.Black;

            button3.BackColor = Color.Black;
            pictureBox4.BackColor = Color.Black;
            
            button4.BackColor = Color.Black;
            pictureBox5.BackColor = Color.Black;
            
            button5.BackColor = Color.Black;
            pictureBox6.BackColor = Color.Black;

            // Show Screen
            appointmentPanel.Show();
            feedbackPanel.Hide();
            workoutPanel.Hide();
            dietPlanPanel.Hide();
            createPanel.Hide();
            wcPanel.Hide();
            dcPanel.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // New Selection
            button2.BackColor = Color.Red;
            pictureBox3.BackColor = Color.Red;

            // Potential Previous Selection
            button1.BackColor = Color.Black;
            pictureBox2.BackColor = Color.Black;

            button3.BackColor = Color.Black;
            pictureBox4.BackColor = Color.Black;

            button4.BackColor = Color.Black;
            pictureBox5.BackColor = Color.Black;

            button5.BackColor = Color.Black;
            pictureBox6.BackColor = Color.Black;

            // Show Screens
            appointmentPanel.Hide();
            feedbackPanel.Hide();
            workoutPanel.Hide();
            panelWP.Hide();
            dietPlanPanel.Hide();
            createPanel.Show();
            wcPanel.Show();
            dcPanel.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // New Selection
            button3.BackColor = Color.Red;
            pictureBox4.BackColor = Color.Red;

            // Potential Previous Selection
            button1.BackColor = Color.Black;
            pictureBox2.BackColor = Color.Black;

            button2.BackColor = Color.Black;
            pictureBox3.BackColor = Color.Black;

            button4.BackColor = Color.Black;
            pictureBox5.BackColor = Color.Black;

            button5.BackColor = Color.Black;
            pictureBox6.BackColor = Color.Black;

            // Show Screens
            appointmentPanel.Hide();
            feedbackPanel.Hide();
            workoutPanel.Show();
            panelWP.Show();
            dietPlanPanel.Hide();
            createPanel.Hide();
            wcPanel.Hide();
            dcPanel.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // New Selection
            button4.BackColor = Color.Red;
            pictureBox5.BackColor = Color.Red;

            // Potential Previous Selection
            button2.BackColor = Color.Black;
            pictureBox3.BackColor = Color.Black;

            button3.BackColor = Color.Black;
            pictureBox4.BackColor = Color.Black;

            button1.BackColor = Color.Black;
            pictureBox2.BackColor = Color.Black;

            button5.BackColor = Color.Black;
            pictureBox6.BackColor = Color.Black;

            // Show Screens
            appointmentPanel.Hide();
            feedbackPanel.Hide();
            workoutPanel.Hide();
            panelWP.Hide();
            dietPlanPanel.Show();
            dpPanel.Show();
            createPanel.Hide();
            wcPanel.Hide();
            dcPanel.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // New Selection
            button5.BackColor = Color.Red;
            pictureBox6.BackColor = Color.Red;

            // Potential Previous Selection
            button2.BackColor = Color.Black;
            pictureBox3.BackColor = Color.Black;

            button3.BackColor = Color.Black;
            pictureBox4.BackColor = Color.Black;

            button4.BackColor = Color.Black;
            pictureBox5.BackColor = Color.Black;

            button1.BackColor = Color.Black;
            pictureBox2.BackColor = Color.Black;

            // Show Screen
            appointmentPanel.Hide();
            feedbackPanel.Show();
            workoutPanel.Hide();
            dietPlanPanel.Hide();
            createPanel.Hide();
            wcPanel.Hide();
            dcPanel.Hide();
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void wcTB2_TextChanged(object sender, EventArgs e)
        {
            //wcTB2.ForeColor = Color.Red;
            //panel9.BackColor = Color.Red;
        }

        private void wcTB3_TextChanged(object sender, EventArgs e)
        {
            //wcTB3.ForeColor = Color.Red;
            //panel7.BackColor = Color.Red;
        }

        private void wcTB1_TextChanged(object sender, EventArgs e)
        {
            //wcTB1.ForeColor = Color.Red;
            //panel6.BackColor = Color.Red;
        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {
            //textBox16.ForeColor = Color.Red;
            //panel10.BackColor = Color.Red;
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            //textBox15.ForeColor = Color.Red;
            //panel8.BackColor = Color.Red;
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            //textBox14.ForeColor = Color.Red;
            //panel5.BackColor = Color.Red;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void feedbackCol1_TextChanged(object sender, EventArgs e)
        {

        }

        private void appointmentPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        public string GetNewWorkoutPlanId()
        {
            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";
            string newId = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT CONCAT('W', CAST(COUNT(workoutplan_id) + 2 AS varchar(10))) AS new_id " +
                                 "FROM WORKOUT_PLAN";

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

        public string GetNewDietPlanId()
        {
            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";
            string newId = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT CONCAT('D', CAST(COUNT(dietplan_id) + 2 AS varchar(10))) AS new_id " +
                                 "FROM DIET_PLAN";

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

        public string GetNewMealId()
        {
            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";
            string newId = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT CONCAT('ML', CAST(COUNT(meal_id) + 2 AS varchar(7))) AS new_id " +
                                 "FROM MEAL";

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

        public string GetNewDayId()
        {
            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";
            string newId = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT CONCAT('Y', CAST(COUNT(day_id) + 2 AS varchar(10))) AS new_id " +
                                 "FROM DAY";

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

        public string GetNewExerciseId()
        {
            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";
            string newId = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT CONCAT('E', CAST(COUNT(exercise_id) + 2 AS varchar(10))) AS new_id " +
                                 "FROM EXERCISE";

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


        private void wcButton_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";

            // Insert into WORKOUT_PLAN
            string id = GetNewWorkoutPlanId();
            string sql = "INSERT INTO workout_PLAN VALUES ('" + id + "','" + wcTB1.Text + "','" + wcTB3.Text + "','" + wcTB2.Text + "')";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Execute the INSERT statement
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            
            string temp = GetNewDayId();

            // Insert into DAY
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (string item in daysListBox.CheckedItems)
                    {
                        sql = "INSERT INTO DAY VALUES ('" + GetNewDayId() + "','" + id + "','" + item + "')";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                // Handle errors during database connection or execution
                MessageBox.Show("Error: " + ex.Message);
            }


            // MONDAY
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (string item in mondayEx.CheckedItems)
                    {
                        string[] x = item.Split(' ');
                        sql = "INSERT INTO EXERCISE VALUES ('" + GetNewExerciseId() + "','" + temp + "','" + id + "','" + "Dumbbell" + "','" + x[0] + "','" + x[1] + "','" + x[2] + "','" + x[3] + "')";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors during database connection or execution
                MessageBox.Show("Error: " + ex.Message);
            }

            int numberPart = int.Parse(temp.Substring(1));  // Extract the numeric part (assuming it starts at index 1)
            temp = $"Y{numberPart + 1}";

            // TUESDAY
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (string item in tuesdayEx.CheckedItems)
                    {
                        string[] x = item.Split(' ');
                        sql = "INSERT INTO EXERCISE VALUES ('" + GetNewExerciseId() + "','" + temp + "','" + id + "','" + "Dumbbell" + "','" + x[0] + "','" + x[1] + "','" + x[2] + "','" + x[3] + "')";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors during database connection or execution
                Console.WriteLine("Error: " + ex.Message);
            }

            numberPart = int.Parse(temp.Substring(1));  // Extract the numeric part (assuming it starts at index 1)
            temp = $"Y{numberPart + 1}";

            // WEDNESDAY
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (string item in wednesdayEx.CheckedItems)
                    {
                        string[] x = item.Split(' ');
                        sql = "INSERT INTO EXERCISE VALUES ('" + GetNewExerciseId() + "','" + temp + "','" + id + "','" + "Dumbbell" + "','" + x[0] + "','" + x[1] + "','" + x[2] + "','" + x[3] + "')";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors during database connection or execution
                Console.WriteLine("Error: " + ex.Message);
            }

            numberPart = int.Parse(temp.Substring(1));  // Extract the numeric part (assuming it starts at index 1)
            temp = $"Y{numberPart + 1}";

            // THURSDAY
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (string item in thursdayEx.CheckedItems)
                    {
                        string[] x = item.Split(' ');
                        sql = "INSERT INTO EXERCISE VALUES ('" + GetNewExerciseId() + "','" + temp + "','" + id + "','" + "Dumbbell" + "','" + x[0] + "','" + x[1] + "','" + x[2] + "','" + x[3] + "')";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors during database connection or execution
                Console.WriteLine("Error: " + ex.Message);
            }

            numberPart = int.Parse(temp.Substring(1));  // Extract the numeric part (assuming it starts at index 1)
            temp = $"Y{numberPart + 1}";

            // FRIDAY
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (string item in fridayEx.CheckedItems)
                    {
                        string[] x = item.Split(' ');
                        sql = "INSERT INTO EXERCISE VALUES ('" + GetNewExerciseId() + "','" + temp + "','" + id + "','" + "Dumbbell" + "','" + x[0] + "','" + x[1] + "','" + x[2] + "','" + x[3] + "')";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors during database connection or execution
                Console.WriteLine("Error: " + ex.Message);
            }

            numberPart = int.Parse(temp.Substring(1));  // Extract the numeric part (assuming it starts at index 1)
            temp = $"Y{numberPart + 1}";

            // SATURDAY
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (string item in saturdayEx.CheckedItems)
                    {
                        string[] x = item.Split(' ');
                        sql = "INSERT INTO EXERCISE VALUES ('" + GetNewExerciseId() + "','" + temp + "','" + id + "','" + "Dumbbell" + "','" + x[0] + "','" + x[1] + "','" + x[2] + "','" + x[3] + "')";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors during database connection or execution
                Console.WriteLine("Error: " + ex.Message);
            }

            numberPart = int.Parse(temp.Substring(1));  // Extract the numeric part (assuming it starts at index 1)
            temp = $"Y{numberPart + 1}";

            // SUNDAY
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (string item in sundayEx.CheckedItems)
                    {
                        string[] x = item.Split(' ');
                        sql = "INSERT INTO EXERCISE VALUES ('" + GetNewExerciseId() + "','" + temp + "','" + id + "','" + "Dumbbell" + "','" + x[0] + "','" + x[1] + "','" + x[2] + "','" + x[3] + "')";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors during database connection or execution
                Console.WriteLine("Error: " + ex.Message);
            }

            MessageBox.Show("Workout plan added successfully!");
        }

        private void dcPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";
            string sql;
            
            // BREAKFAST
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string mealID = GetNewMealId();
                    mealID = GetNewMealId();
                    
                    foreach (string item in breakfastListBox5.CheckedItems)
                    {
                        string[] x = item.Split(' ');
                        sql = "INSERT INTO MEAL_Nutrient VALUES ('" + x[0] + "','" + mealID + "','" + x[4] + "')";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                    

                    sql = "INSERT INTO diet_PLAN VALUES ('" + GetNewDietPlanId() + "','" + mealID + "','" + "Breakfast" + "','" + textBox14.Text + "','" + textBox15.Text + "','" + textBox16.Text + "','" + "M1" + "')";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    sql = "INSERT INTO MEAL VALUES ('" + mealID + "','" + "N1" + "')";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors during database connection or execution
               
            }


            // LUNCH
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string mealID = GetNewMealId();

                    foreach (string item in lunchListBox2.CheckedItems)
                    {
                        string[] x = item.Split(' ');
                        sql = "INSERT INTO MEAL_Nutrient VALUES ('" + x[0] + "','" + mealID + "','" + x[4] + "')";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }

                    sql = "INSERT INTO diet_PLAN VALUES ('" + GetNewDietPlanId() + "','" + mealID + "','" + "Lunch" + "','" + textBox14.Text + "','" + textBox15.Text + "','" + textBox16.Text + "','" + "M1" + "')";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    sql = "INSERT INTO MEAL VALUES ('" + mealID + "','" + "N1" + "')";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors during database connection or execution
                Console.WriteLine("Error: " + ex.Message);
            }


            // DINNER
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string mealID = GetNewMealId();

                    foreach (string item in dinnerListBox3.CheckedItems)
                    {
                        string[] x = item.Split(' ');
                        sql = "INSERT INTO MEAL_Nutrient VALUES ('" + x[0] + "','" + mealID + "','" + x[4] + "')";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }

                    sql = "INSERT INTO diet_PLAN VALUES ('" + GetNewDietPlanId() + "','" + mealID + "','" + "Dinner" + "','" + textBox14.Text + "','" + textBox15.Text + "','" + textBox16.Text + "','" + "M1" + "')";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    sql = "INSERT INTO MEAL VALUES ('" + mealID + "','" + "N1" + "')";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors during database connection or execution
                Console.WriteLine("Error: " + ex.Message);
            }


            // SNACK
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string mealID = GetNewMealId();

                    foreach (string item in snackListBox4.CheckedItems)
                    {
                        string[] x = item.Split(' ');
                        sql = "INSERT INTO MEAL_Nutrient VALUES ('" + x[0] + "','" + mealID + "','" + x[4] + "')";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }

                    sql = "INSERT INTO diet_PLAN VALUES ('" + GetNewDietPlanId() + "','" + mealID + "','" + "Snack" + "','" + textBox14.Text + "','" + textBox15.Text + "','" + textBox16.Text + "','" + "M1" + "')";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    sql = "INSERT INTO MEAL VALUES ('" + mealID + "','" + "N1" + "')";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors during database connection or execution
                Console.WriteLine("Error: " + ex.Message);
            }

            

            MessageBox.Show("Diet plan added successfully!");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";
            string sql = "select dietPlan_id, type, objective, guidelines, difficulty_level from DIET_Plan";

            try
            {
                if (difficultyBox1.CheckedItems[0].ToString() == "Easy")
                    sql = "select dietPlan_id, type, objective, guidelines, difficulty_level from DIET_Plan WHERE difficulty_level = 'Easy'";

                if (difficultyBox1.CheckedItems[0].ToString() == "Medium")
                    sql = "select dietPlan_id, type, objective, guidelines, difficulty_level from DIET_Plan WHERE difficulty_level = 'Medium'";

                if (difficultyBox1.CheckedItems[0].ToString() == "Hard")
                    sql = "select dietPlan_id, type, objective, guidelines, difficulty_level from DIET_Plan WHERE difficulty_level = 'Hard'";

                // Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a data adapter to fetch data from the database
                    using (SqlDataAdapter adapter = new SqlDataAdapter(sql, connection))
                    {
                        // Create a DataTable to hold the fetched data (optional, can build the list directly)
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Clear the existing items in the feedbackTable
                        dpListBox.Items.Clear();

                        // Loop through each row in the DataTable (or directly iterate over the data)
                        foreach (DataRow row in dataTable.Rows)
                        {
                            // Build the string with separation
                            string feedbackItem = $"{row["dietPlan_id"]}      {row["type"]}      {row["objective"]}                      {row["guidelines"]}                          {row["difficulty_level"]}";

                            // Add the formatted feedback item to the ListBox
                            dpListBox.Items.Add(feedbackItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors during database connection or execution
                MessageBox.Show(ex.Message);
            }


        }

        private void button7_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";
            string sql = "select * from WORKOUT_PLAN";

            try
            {
                if (difficultyBox2.CheckedItems[0].ToString() == "Easy")
                    sql = "select * from Workout_Plan where difficulty_level = 'Easy'";

                if (difficultyBox2.CheckedItems[0].ToString() == "Medium")
                    sql = "select * from Workout_Plan where difficulty_level = 'Medium'";

                if (difficultyBox2.CheckedItems[0].ToString() == "Hard")
                    sql = "select * from Workout_Plan where difficulty_level = 'Hard'";


                // Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    // Create a data adapter to fetch data from the database
                    using (SqlDataAdapter adapter = new SqlDataAdapter(sql, connection))
                    {
                        // Create a DataTable to hold the fetched data (optional, can build the list directly)
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Clear the existing items in the feedbackTable
                        wpListBox.Items.Clear();

                        // Loop through each row in the DataTable (or directly iterate over the data)
                        foreach (DataRow row in dataTable.Rows)
                        {
                            // Build the string with separation
                            string feedbackItem = $"{row["workoutPlan_id"]}      {row["objective"]}                      {row["guidelines"]}                          {row["difficulty_level"]}";

                            // Add the formatted feedback item to the ListBox
                            wpListBox.Items.Add(feedbackItem);
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

        private void button8_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";
            string sql = "select res1.name, member.name as [MemberName], rating, feedback from member join ( SELECT name, trainer_review.gym_id as [member_id], trainer_review.rating, feedback FROM TRAINER_REVIEW join gym on gym.gym_id = trainer_review.member_id WHERE trainer_id = 'T1' ) as res1 on member.member_id = res1.member_id";
            
            if (timedCheckedListBox1.CheckedItems[0].ToString() == "High to Low")
                sql = "select res1.name, member.name as [MemberName], rating, feedback from member join ( SELECT name, trainer_review.gym_id as [member_id], trainer_review.rating, feedback FROM TRAINER_REVIEW join gym on gym.gym_id = trainer_review.member_id WHERE trainer_id = 'T1' ) as res1 on member.member_id = res1.member_id order by rating desc;";

            if (timedCheckedListBox1.CheckedItems[0].ToString() == "Low to High")
                sql = "select res1.name, member.name as [MemberName], rating, feedback from member join ( SELECT name, trainer_review.gym_id as [member_id], trainer_review.rating, feedback FROM TRAINER_REVIEW join gym on gym.gym_id = trainer_review.member_id WHERE trainer_id = 'T1' ) as res1 on member.member_id = res1.member_id order by rating asc;";

            try
            {
                // Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a data adapter to fetch data from the database
                    using (SqlDataAdapter adapter = new SqlDataAdapter(sql, connection))
                    {
                        // Create a DataTable to hold the fetched data (optional, can build the list directly)
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Clear the existing items in the feedbackTable
                        feedbackTable.Items.Clear();

                        // Variable to store total rating
                        double totalRating = 0.0;

                        // Loop through each row in the DataTable (or directly iterate over the data)
                        foreach (DataRow row in dataTable.Rows)
                        {
                            // Build the string with separation
                            string feedbackItem = $"{row["name"]}          {row["MemberName"]}          {row["rating"]}          {row["feedback"]}";

                            // Add the formatted feedback item to the ListBox
                            feedbackTable.Items.Add(feedbackItem);

                            // Extract rating as double and add to total
                            double rating = Convert.ToDouble(row["rating"]);
                            totalRating += rating;
                        }

                        // Calculate average rating (handle division by zero)
                        double averageRating = totalRating > 0 ? totalRating / dataTable.Rows.Count : 0.0;

                        // Display average rating (replace with your desired location)
                        feedbackText.Text = averageRating.ToString("F2"); // Format to two decimal places
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (timCheckedListBox2.CheckedItems.Count == 0)
                return;

            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";
            string sql = "SELECT res1.GymName, name, date from member join ( SELECT gym.name AS GymName, member_id, date FROM TRAINING_SESSION join gym on gym.gym_id = TRAINING_SESSION.gym_id WHERE trainer_id = '" + this.id + "' ) AS res1 on member.member_id = res1.member_id";

            if (timCheckedListBox2.CheckedItems[0].ToString() == "Oldest to Newest")
                sql = "SELECT res1.GymName, name, date from member join ( SELECT gym.name AS GymName, member_id, date FROM TRAINING_SESSION join gym on gym.gym_id = TRAINING_SESSION.gym_id WHERE trainer_id = '" + this.id + "' ) AS res1 on member.member_id = res1.member_id ORDER BY date DESC;";

            if (timCheckedListBox2.CheckedItems[0].ToString() == "Newest to Oldest")
                sql = "SELECT res1.GymName, name, date from member join ( SELECT gym.name AS GymName, member_id, date FROM TRAINING_SESSION join gym on gym.gym_id = TRAINING_SESSION.gym_id WHERE trainer_id = '" + this.id + "' ) AS res1 on member.member_id = res1.member_id ORDER BY date ASC;";

            try
            {
                // Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();


                    // Create a data adapter to fetch data from the database
                    using (SqlDataAdapter adapter = new SqlDataAdapter(sql, connection))
                    {
                        // Create a DataTable to hold the fetched data (optional, can build the list directly)
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Clear the existing items in the appointmentTable
                        appointmentTable.Items.Clear();

                        // Loop through each row in the DataTable (or directly iterate over the data)
                        foreach (DataRow row in dataTable.Rows)
                        {
                            // Build the string with separation
                            string appointmentItem = $"{row["GymName"]}          {row["name"]}          {row["date"]}";

                            // Add the formatted feedback item to the ListBox
                            appointmentTable.Items.Add(appointmentItem);
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

        private void button11_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";
            string sql = "select res1.name, member.name as [MemberName], rating, feedback from member join ( SELECT name, trainer_review.gym_id as [member_id], trainer_review.rating, feedback FROM TRAINER_REVIEW join gym on gym.gym_id = trainer_review.member_id WHERE trainer_id = 'T1' ) as res1 on member.member_id = res1.member_id";
            
            if (ratingCheckedListBox1.CheckedItems[0].ToString() == "> 4")
                sql = "select res1.name, member.name as [MemberName], rating, feedback from member join ( SELECT name, trainer_review.gym_id as [member_id], trainer_review.rating, feedback FROM TRAINER_REVIEW join gym on gym.gym_id = trainer_review.member_id WHERE trainer_id = 'T1' ) as res1 on member.member_id = res1.member_id where rating > 4;";

            if (ratingCheckedListBox1.CheckedItems[0].ToString() == "> 3 & < 4")
                sql = "select res1.name, member.name as [MemberName], rating, feedback from member join ( SELECT name, trainer_review.gym_id as [member_id], trainer_review.rating, feedback FROM TRAINER_REVIEW join gym on gym.gym_id = trainer_review.member_id WHERE trainer_id = 'T1' ) as res1 on member.member_id = res1.member_id where rating > 3 AND rating < 4;";

            if (ratingCheckedListBox1.CheckedItems[0].ToString() == "> 2 & < 3")
                sql = "select res1.name, member.name as [MemberName], rating, feedback from member join ( SELECT name, trainer_review.gym_id as [member_id], trainer_review.rating, feedback FROM TRAINER_REVIEW join gym on gym.gym_id = trainer_review.member_id WHERE trainer_id = 'T1' ) as res1 on member.member_id = res1.member_id  where rating > 2 AND rating < 3";

            if (ratingCheckedListBox1.CheckedItems[0].ToString() == "> 1 & < 2")
                sql = "select res1.name, member.name as [MemberName], rating, feedback from member join ( SELECT name, trainer_review.gym_id as [member_id], trainer_review.rating, feedback FROM TRAINER_REVIEW join gym on gym.gym_id = trainer_review.member_id WHERE trainer_id = 'T1' ) as res1 on member.member_id = res1.member_id  where rating > 1 AND rating < 2";

            if (ratingCheckedListBox1.CheckedItems[0].ToString() == "> 0 & < 1")
                sql = "select res1.name, member.name as [MemberName], rating, feedback from member join ( SELECT name, trainer_review.gym_id as [member_id], trainer_review.rating, feedback FROM TRAINER_REVIEW join gym on gym.gym_id = trainer_review.member_id WHERE trainer_id = 'T1' ) as res1 on member.member_id = res1.member_id  where rating > 0 AND rating < 1";
            try
            {
                // Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a data adapter to fetch data from the database
                    using (SqlDataAdapter adapter = new SqlDataAdapter(sql, connection))
                    {
                        // Create a DataTable to hold the fetched data (optional, can build the list directly)
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Clear the existing items in the feedbackTable
                        feedbackTable.Items.Clear();

                        // Variable to store total rating
                        double totalRating = 0.0;

                        // Loop through each row in the DataTable (or directly iterate over the data)
                        foreach (DataRow row in dataTable.Rows)
                        {
                            // Build the string with separation
                            string feedbackItem = $"{row["name"]}          {row["MemberName"]}          {row["rating"]}          {row["feedback"]}";

                            // Add the formatted feedback item to the ListBox
                            feedbackTable.Items.Add(feedbackItem);

                            // Extract rating as double and add to total
                            double rating = Convert.ToDouble(row["rating"]);
                            totalRating += rating;
                        }

                        // Calculate average rating (handle division by zero)
                        double averageRating = totalRating > 0 ? totalRating / dataTable.Rows.Count : 0.0;

                        // Display average rating (replace with your desired location)
                        feedbackText.Text = averageRating.ToString("F2"); // Format to two decimal places
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";
            string equipmentName = machinesCheckedListBox.CheckedItems[0].ToString();

            try
            {
                // Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to select usernames and passwords from the INFO table
                    string query = "SELECT* FROM Workout_Plan W JOIN Exercise E ON W.workoutPlan_id = E.workoutPlan_id WHERE E.equipment_name != '" + machinesCheckedListBox.CheckedItems[0].ToString() + "'";

                    // Create a data adapter to fetch data from the database
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        // Create a DataTable to hold the fetched data (optional, can build the list directly)
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Clear the existing items in the feedbackTable
                        wpListBox.Items.Clear();

                        // Loop through each row in the DataTable (or directly iterate over the data)
                        foreach (DataRow row in dataTable.Rows)
                        {
                            // Build the string with separation
                            string feedbackItem = $"{row["workoutPlan_id"]}      {row["objective"]}                      {row["guidelines"]}                          {row["difficulty_level"]}";

                            // Add the formatted feedback item to the ListBox
                            wpListBox.Items.Add(feedbackItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors
                MessageBox.Show("Error: " + ex.Message);
            }

            try
            {
                // Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to select usernames and passwords from the INFO table
                    string query = "SELECT RES1.workoutPlan_id, dayofweek, equipment_name, sets, reps, restIntervals FROM EXERCISE JOIN(SELECT EXERCISE_DAY.workoutPlan_id, dayofweek, exercise_id FROM EXERCISE_DAY JOIN DAY ON DAY.day_id = EXERCISE_DAY.day_id) AS RES1 on EXERCISE.exercise_id = RES1.exercise_id where equipment_name != '" + machinesCheckedListBox.CheckedItems[0].ToString() + "' ORDER BY workoutPlan_id";

                    // Create a data adapter to fetch data from the database
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        // Create a DataTable to hold the fetched data (optional, can build the list directly)
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Clear the existing items in the feedbackTable
                        exerciseListBox.Items.Clear();

                        // Loop through each row in the DataTable (or directly iterate over the data)
                        foreach (DataRow row in dataTable.Rows)
                        {
                            // Build the string with separation
                            string feedbackItem = $"{row["workoutPlan_id"]}      {row["dayofweek"]}       {row["equipment_name"]}                                           {row["sets"]}   {row["reps"]}   {row["restintervals"]}";

                            // Add the formatted feedback item to the ListBox
                            exerciseListBox.Items.Add(feedbackItem);
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

        private void button13_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";

            try
            {
                // Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to select usernames and passwords from the INFO table
                    string query = "SELECT * FROM Diet_Plan D JOIN Meal_Nutrient M ON D.member_id = M.meal_id JOIN Nutrition N ON N.Nutrition_id = M.Nutrient_id JOIN Allergen A ON A.allergen_id = N.allergen_id WHERE A.[name] != 'Peanuts'";

                    // Create a data adapter to fetch data from the database
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        // Create a DataTable to hold the fetched data (optional, can build the list directly)
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Clear the existing items in the feedbackTable
                        dpListBox.Items.Clear();

                        // Loop through each row in the DataTable (or directly iterate over the data)
                        foreach (DataRow row in dataTable.Rows)
                        {
                            // Build the string with separation
                            string feedbackItem = $"{row["dietPlan_id"]}      {row["type"]}      {row["objective"]}                      {row["guidelines"]}                          {row["difficulty_level"]}";

                            // Add the formatted feedback item to the ListBox
                            dpListBox.Items.Add(feedbackItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors
                MessageBox.Show("Error: " + ex.Message);
            }

            try
            {
                // Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to select usernames and passwords from the INFO table
                    string query = "SELECT dietPlan_id, meal_id, RES2.name AS [NutritionName], ALLERGEN.NAME as [AllergenName] FROM ALLERGEN JOIN ( \tSELECT dietPlan_id, meal_id, NUTRITION.name, allergen_id \tFROM NUTRITION \tJOIN ( \t\tSELECT dietPlan_id, meal.meal_id, nutrition_id \t\tFROM DIET_PLAN \t\tJOIN MEAL \t\tON MEAL.meal_id = DIET_PLAN.member_id \t) AS RES1 \tON RES1.nutrition_id = NUTRITION.nutrition_id ) AS RES2 ON ALLERGEN.ALLERGEN_ID = RES2.ALLERGEN_ID where ALLERGEN.NAME != 'Peanuts'";

                    // Create a data adapter to fetch data from the database
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        // Create a DataTable to hold the fetched data (optional, can build the list directly)
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Clear the existing items in the feedbackTable
                        mealListBox1.Items.Clear();

                        // Loop through each row in the DataTable (or directly iterate over the data)
                        foreach (DataRow row in dataTable.Rows)
                        {
                            // Build the string with separation
                            string feedbackItem = $"{row["dietPlan_id"]}           {row["meal_id"]}           {row["NutritionName"]}           {row["AllergenName"]}";

                            // Add the formatted feedback item to the ListBox
                            mealListBox1.Items.Add(feedbackItem);
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

        private void button14_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";

            try
            {
                // Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to select usernames and passwords from the INFO table
                    string query = "SELECT * FROM Diet_Plan D JOIN Meal_Nutrient M ON D.member_id = M.meal_id JOIN Nutrition N ON N.Nutrition_id = M.Nutrient_id WHERE N.[name] = 'Carbohydrates' AND N.[unit] = 'grams' AND N.[quantity] < 300";

                    // Create a data adapter to fetch data from the database
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        // Create a DataTable to hold the fetched data (optional, can build the list directly)
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Clear the existing items in the feedbackTable
                        dpListBox.Items.Clear();

                        // Loop through each row in the DataTable (or directly iterate over the data)
                        foreach (DataRow row in dataTable.Rows)
                        {
                            // Build the string with separation
                            string feedbackItem = $"{row["dietPlan_id"]}      {row["type"]}      {row["objective"]}                      {row["guidelines"]}                          {row["difficulty_level"]}";

                            // Add the formatted feedback item to the ListBox
                            dpListBox.Items.Add(feedbackItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors
                MessageBox.Show("Error: " + ex.Message);
            }

            try
            {
                // Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to select usernames and passwords from the INFO table
                    string query = "SELECT dietPlan_id, meal_id, RES2.name AS [NutritionName], ALLERGEN.NAME as [AllergenName] FROM ALLERGEN JOIN ( \tSELECT dietPlan_id, meal_id, NUTRITION.name, allergen_id \tFROM NUTRITION \tJOIN ( \t\tSELECT dietPlan_id, meal.meal_id, nutrition_id \t\tFROM DIET_PLAN \t\tJOIN MEAL \t\tON MEAL.meal_id = DIET_PLAN.member_id \t) AS RES1 \tON RES1.nutrition_id = NUTRITION.nutrition_id ) AS RES2 ON ALLERGEN.ALLERGEN_ID = RES2.ALLERGEN_ID where RES2.NAME = 'Carbohydrates'";

                    // Create a data adapter to fetch data from the database
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        // Create a DataTable to hold the fetched data (optional, can build the list directly)
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Clear the existing items in the feedbackTable
                        mealListBox1.Items.Clear();

                        // Loop through each row in the DataTable (or directly iterate over the data)
                        foreach (DataRow row in dataTable.Rows)
                        {
                            // Build the string with separation
                            string feedbackItem = $"{row["dietPlan_id"]}           {row["meal_id"]}           {row["NutritionName"]}           {row["AllergenName"]}";

                            // Add the formatted feedback item to the ListBox
                            mealListBox1.Items.Add(feedbackItem);
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

        private void button12_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=ALI-DELL\\SQLEXPRESS;Initial Catalog=SYNtrainer;Integrated Security=True";

            try
            {
                // Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to select usernames and passwords from the INFO table
                    string query = "select dietPlan_id, type, objective, guidelines, difficulty_level from DIET_Plan where type = 'Breakfast'";

                    // Create a data adapter to fetch data from the database
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        // Create a DataTable to hold the fetched data (optional, can build the list directly)
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Clear the existing items in the feedbackTable
                        dpListBox.Items.Clear();

                        // Loop through each row in the DataTable (or directly iterate over the data)
                        foreach (DataRow row in dataTable.Rows)
                        {
                            // Build the string with separation
                            string feedbackItem = $"{row["dietPlan_id"]}      {row["type"]}      {row["objective"]}                      {row["guidelines"]}                          {row["difficulty_level"]}";

                            // Add the formatted feedback item to the ListBox
                            dpListBox.Items.Add(feedbackItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors
                MessageBox.Show("Error: " + ex.Message);
            }

            try
            {
                // Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to select usernames and passwords from the INFO table
                    string query = "SELECT dietPlan_id, meal_id, RES2.name AS [NutritionName], ALLERGEN.NAME as [AllergenName], calories FROM ALLERGEN JOIN ( \tSELECT dietPlan_id, meal_id, NUTRITION.name, allergen_id, calories \tFROM NUTRITION \tJOIN ( \t\tSELECT dietPlan_id, meal.meal_id, nutrition_id \t\tFROM DIET_PLAN \t\tJOIN MEAL \t\tON MEAL.meal_id = DIET_PLAN.member_id where type = 'Breakfast' \t) AS RES1 \tON RES1.nutrition_id = NUTRITION.nutrition_id where calories < 500 ) AS RES2 ON ALLERGEN.ALLERGEN_ID = RES2.ALLERGEN_ID";

                    // Create a data adapter to fetch data from the database
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        // Create a DataTable to hold the fetched data (optional, can build the list directly)
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Clear the existing items in the feedbackTable
                        mealListBox1.Items.Clear();

                        // Loop through each row in the DataTable (or directly iterate over the data)
                        foreach (DataRow row in dataTable.Rows)
                        {
                            // Build the string with separation
                            string feedbackItem = $"{row["dietPlan_id"]}           {row["meal_id"]}           {row["NutritionName"]}           {row["AllergenName"]}           {row["calories"]}";

                            // Add the formatted feedback item to the ListBox
                            mealListBox1.Items.Add(feedbackItem);
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
    }
}
