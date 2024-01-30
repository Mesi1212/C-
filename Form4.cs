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
using System.Text.RegularExpressions;


namespace sadprroject
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=MIKI\SQLEXPRESS01;Initial Catalog=Cafeterias;Integrated Security=True");



        private void button1_Click(object sender, EventArgs e)
        {



            try
            {
                Con.Open();

                // Check if all required text boxes are filled
                if (!string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox2.Text) &&
                    !string.IsNullOrWhiteSpace(textBox3.Text) && !string.IsNullOrWhiteSpace(textBox4.Text) &&
                    !string.IsNullOrWhiteSpace(textBox5.Text))
                {
                    // Regular expression pattern for validating StudentID format
                    string studentIdPattern = @"^UGR\/\d{4}\/\d{2}$";

                    // Validate StudentID format using regular expression
                    if (Regex.IsMatch(textBox1.Text, studentIdPattern))
                    {
                        // Create a parameterized SQL command to insert values into the Students table
                        string insertQuery = "INSERT INTO Students (StudentID, Name, Year, Department, Password) " +
                                             "VALUES (@StudentID, @Name, @Year, @Department, @Password)";
                        SqlCommand cmd = new SqlCommand(insertQuery, Con);

                        // Set parameter values from TextBoxes
                        cmd.Parameters.AddWithValue("@StudentID", textBox1.Text);
                        cmd.Parameters.AddWithValue("@Name", textBox2.Text);

                        // Parse and format the Year (date) input
                        if (DateTime.TryParse(textBox3.Text, out DateTime dateValue))
                        {
                            string formattedDate = dateValue.ToString("yyyy-MM-dd");
                            cmd.Parameters.AddWithValue("@Year", formattedDate);
                        }
                        else
                        {
                            // Handle invalid date input from the user
                            MessageBox.Show("Invalid date format. Please enter a valid date in the format YYYY-MM-DD.");
                            return; // Exit the method if the date format is invalid
                        }

                        cmd.Parameters.AddWithValue("@Department", textBox4.Text);
                        cmd.Parameters.AddWithValue("@Password", textBox5.Text);

                        // Execute the query
                        cmd.ExecuteNonQuery();

                        // Close the connection
                        Con.Close();

                        // Show a message indicating that the student was successfully added
                        MessageBox.Show("Student Successfully Added");

                        // Clear TextBoxes
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        textBox4.Clear();
                        textBox5.Clear();
                    }
                    else
                    {
                        // Show an error message if the StudentID format is invalid
                        MessageBox.Show("Invalid StudentID format. StudentID must start with 'UGR/' followed by 4 numbers, a '/', and 2 additional numbers.");
                    }
                }
                else
                {
                    // Show an error message if any required text box is empty
                    MessageBox.Show("All fields are required. Please fill in all the details.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (Con.State == System.Data.ConnectionState.Open)
                    Con.Close();
            }


        }




        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox4.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox5.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();

        }
        void populate()
        {
            try
            {
                Con.Open();
                String MyQuery = " select * from Students";
                SqlDataAdapter da = new SqlDataAdapter(MyQuery, Con);
                SqlCommandBuilder builder = new SqlCommandBuilder(da);
                var ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

                Con.Close();
            }
            catch
            {

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {

                MessageBox.Show("Enter the Student id");

            }

            else
            {
                if (Con.State == ConnectionState.Closed)
                {
                    Con.Open();
                }


                String myquery = "delete from Students where StudentID ='" + textBox1.Text + "' ;";
                SqlCommand cmd = new SqlCommand(myquery, Con);
                cmd.ExecuteNonQuery();

                Con.Close();

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();

                // Check if all required text boxes are filled
                if (!string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox2.Text) &&
                    !string.IsNullOrWhiteSpace(textBox3.Text) && !string.IsNullOrWhiteSpace(textBox4.Text) &&
                    !string.IsNullOrWhiteSpace(textBox5.Text))
                {
                    // Create a parameterized SQL command to update a student's information in the Students table
                    string updateQuery = "UPDATE Students SET Name = @Name, Department = @Department, " +
                                         "Password = @Password, Year = @Year WHERE StudentID = @StudentID";
                    SqlCommand cmd = new SqlCommand(updateQuery, Con);

                    // Set parameter values from TextBoxes
                    cmd.Parameters.AddWithValue("@Name", textBox2.Text);
                    cmd.Parameters.AddWithValue("@Department", textBox4.Text);
                    cmd.Parameters.AddWithValue("@Password", textBox5.Text);
                    cmd.Parameters.AddWithValue("@Year", textBox3.Text);
                    cmd.Parameters.AddWithValue("@StudentID", textBox1.Text);

                    // Execute the update query
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // If rows were affected (i.e., the update was successful), show a success message
                        MessageBox.Show("Student Successfully Updated");
                        populate(); // Refresh the data in your application
                    }
                    else
                    {
                        // If no rows were affected, show a message indicating that the student ID was not found
                        MessageBox.Show("Student ID not found. No changes were made.");
                    }
                }
                else
                {
                    // Show an error message if any required text box is empty
                    MessageBox.Show("All fields are required. Please fill in all the details.");
                }

                // Close the connection
                Con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                Con.Close();
            }


        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            populate();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Con.Open();
                String MyQuery = " select * from Students where StudentID = '" + textBox6.Text + "'";
                SqlDataAdapter da = new SqlDataAdapter(MyQuery, Con);
                SqlCommandBuilder builder = new SqlCommandBuilder(da);
                var ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

                Con.Close();
            }
            catch
            {

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 home = new Form2();
            home.Show();
            this.Hide();

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}