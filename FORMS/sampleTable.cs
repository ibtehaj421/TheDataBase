private void change_Click(object sender, EventArgs e)
{
    //sql query to be tested here.
    string connectionString = "Data Source=YGGDRASIL\\SQLEXPRESS;Initial Catalog=UserInfo;Integrated Security=True";
    string sqlQuery = "SELECT * FROM USERS"; // Replace with your actual query

    DataTable dataTable = new DataTable();

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();
        using (SqlCommand command = new SqlCommand(sqlQuery, connection))
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                adapter.Fill(dataTable);
            }
        }
    }
    currentWP.Controls.Clear();
    for (int colIndex = 0; colIndex < columnheaders.Length; colIndex++)
    {
        Label headerLabel = new Label();
        headerLabel.Text = columnheaders[colIndex];
        headerLabel.Font = new Font("Cascadia code", 12, FontStyle.Bold);
        headerLabel.ForeColor = Color.Azure;
        currentWP.Controls.Add(headerLabel, colIndex, 0);
    }
    int rowIndex = 1;
    foreach (DataRow row in dataTable.Rows)
    {
        for (int colIndex = 0; colIndex < dataTable.Columns.Count; colIndex++)
        {
            Label label = new Label();
            label.Text = row[colIndex].ToString();
            label.Font = new Font("Cascadia code", 11, FontStyle.Regular);
            label.ForeColor = Color.Azure;
            // Set other control properties as needed (e.g., font size)

            currentWP.Controls.Add(label, colIndex, rowIndex);
        }

        rowIndex++; // Keep track of row index for next iteration
    }
}
