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
namespace sadprroject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=MIKI\SQLEXPRESS01;Initial Catalog=Cafeterias;Integrated Security=True");

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            try
            {
                Con.Open();

                // Check if the user is an admin
                string checkAdminQuery = "SELECT COUNT(*) FROM Adminn WHERE User_name = @UserName";
                SqlCommand adminCommand = new SqlCommand(checkAdminQuery, Con);
                adminCommand.Parameters.AddWithValue("@UserName", textBox1.Text);
                int adminCount = Convert.ToInt32(adminCommand.ExecuteScalar());

                if (adminCount == 1)
                {
                    // User is an admin, check the password
                    string getPasswordQuery = "SELECT Password FROM Adminn WHERE User_name = @UserName";
                    SqlCommand getPasswordCommand = new SqlCommand(getPasswordQuery, Con);
                    getPasswordCommand.Parameters.AddWithValue("@UserName", textBox1.Text);
                    string adminPassword = getPasswordCommand.ExecuteScalar()?.ToString(); // Use ?.ToString() to handle null values

                    if (adminPassword == textBox2.Text)
                    {
                        // Password is correct, show admin form
                        Form2 adminForm = new Form2();
                        adminForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        // Incorrect password
                        MessageBox.Show("Wrong username or password");
                    }
                }
                else
                {
                    // User is not an admin, check if they are a cafeteria or student
                    string checkCafeteriaQuery = "SELECT COUNT(*) FROM Cafeteria WHERE Name = @CafeName";
                    SqlCommand cafeteriaCommand = new SqlCommand(checkCafeteriaQuery, Con);
                    cafeteriaCommand.Parameters.AddWithValue("@CafeName", textBox1.Text);
                    int cafeteriaCount = Convert.ToInt32(cafeteriaCommand.ExecuteScalar());

                    if (cafeteriaCount == 1)
                    {
                        // User is a cafeteria, check the password
                        string getCafeteriaPasswordQuery = "SELECT Password FROM Cafeteria WHERE Name = @CafeName";
                        SqlCommand getCafeteriaPasswordCommand = new SqlCommand(getCafeteriaPasswordQuery, Con);
                        getCafeteriaPasswordCommand.Parameters.AddWithValue("@CafeName", textBox1.Text);
                        string cafeteriaPassword = getCafeteriaPasswordCommand.ExecuteScalar()?.ToString(); // Use ?.ToString() to handle null values

                        if (cafeteriaPassword == textBox2.Text)
                        {
                            // Password is correct, show cafeteria form
                            Form3 cafeteriaForm = new Form3();
                            cafeteriaForm.Show();
                            this.Hide();
                        }
                        else
                        {
                            // Incorrect password
                            MessageBox.Show("Wrong username or password");
                        }
                    }
                    else
                    {
                        // User is not a cafeteria, check if they are a student
                        string checkStudentQuery = "SELECT COUNT(*) FROM Students WHERE StudentID = @StudentID";
                        SqlCommand studentCommand = new SqlCommand(checkStudentQuery, Con);
                        studentCommand.Parameters.AddWithValue("@StudentID", textBox1.Text);
                        int studentCount = Convert.ToInt32(studentCommand.ExecuteScalar());

                        if (studentCount == 1)
                        {
                            // User is a student, check the password
                            string getStudentPasswordQuery = "SELECT Password FROM Students WHERE StudentID = @StudentID";
                            SqlCommand getStudentPasswordCommand = new SqlCommand(getStudentPasswordQuery, Con);
                            getStudentPasswordCommand.Parameters.AddWithValue("@StudentID", textBox1.Text);
                            string studentPassword = getStudentPasswordCommand.ExecuteScalar()?.ToString(); // Use ?.ToString() to handle null values

                            if (studentPassword == textBox2.Text)
                            {
                                // Password is correct, show student form
                                Form9 studentForm = new Form9();
                                studentForm.Show();
                                this.Hide();
                            }
                            else
                            {
                                // Incorrect password
                                MessageBox.Show("Wrong username or password");
                            }
                        }
                        else
                        {
                            // User is not a student, show error message
                            MessageBox.Show("User not found");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                // Close the connection
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
            }

        }
    }
}


           