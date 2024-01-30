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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();

        }
        SqlConnection Con = new SqlConnection(@"Data Source=MIKI\SQLEXPRESS01;Initial Catalog=Cafeterias;Integrated Security=True");

        private string lastSearchValue = "";
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=MIKI\SQLEXPRESS01;Initial Catalog=Cafeterias;Integrated Security=True"))
                {
                    con.Open();

                    // Check if all required fields are filled
                    if (!string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox3.Text) &&
                        !string.IsNullOrWhiteSpace(textBox4.Text))
                    {
                        // Validate the username format
                        if (textBox3.Text.StartsWith("@"))
                        {
                            // Check if the admin ID already exists
                            string checkAdminQuery = "SELECT COUNT(*) FROM Adminn WHERE Admin_ID = @AdminID";
                            using (SqlCommand checkCmd = new SqlCommand(checkAdminQuery, con))
                            {
                                checkCmd.Parameters.AddWithValue("@AdminID", textBox1.Text);
                                int existingAdminCount = (int)checkCmd.ExecuteScalar();

                                if (existingAdminCount > 0)
                                {
                                    MessageBox.Show("Admin ID already exists. Please use a different Admin ID.");
                                }
                                else
                                {
                                    // Admin ID is unique and username format is valid, proceed with insertion
                                    string insertQuery = "INSERT INTO Adminn (Admin_ID, User_name, Password) VALUES (@AdminID, @UserName, @Password)";
                                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, con))
                                    {
                                        insertCmd.Parameters.AddWithValue("@AdminID", textBox1.Text);
                                        insertCmd.Parameters.AddWithValue("@UserName", textBox3.Text);
                                        insertCmd.Parameters.AddWithValue("@Password", textBox4.Text);

                                        insertCmd.ExecuteNonQuery();

                                        MessageBox.Show("Admin Successfully Added");
                                        populate();
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Username must start with '@'. Please enter a valid username.");
                        }
                    }
                    else
                    {
                        // Show an error message if any required text box is empty
                        MessageBox.Show("All fields are required. Please fill in all the details.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            populate();
        }
        void populate()
        {
            try
            {
                Con.Open();
                String MyQuery = " select * from Adminn";
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox4.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=MIKI\SQLEXPRESS01;Initial Catalog=Cafeterias;Integrated Security=True"))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("update Adminn set  User_name = @UserName, Password = @Password where Admin_ID = @AdminID", con))
                    {
                        cmd.Parameters.AddWithValue("@UserName", textBox3.Text);
                        cmd.Parameters.AddWithValue("@Password", textBox4.Text);
                        cmd.Parameters.AddWithValue("@AdminID", textBox1.Text);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Admin Successfully Updated");
                        populate();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Enter the admin id");
            }
            else
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(@"Data Source=MIKI\SQLEXPRESS01;Initial Catalog=Cafeterias;Integrated Security=True"))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("delete from Adminn where Admin_ID = @AdminID", con))
                        {
                            cmd.Parameters.AddWithValue("@AdminID", textBox1.Text);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Admin Successfully deleted");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 home = new Form2();
            home.Show();
            this.Hide();

        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(textBox2.Text)) // Check if the textbox is not empty or contains only whitespace
                {
                    Con.Open();
                    string searchText = textBox2.Text.Trim(); // Trim to remove leading/trailing whitespace
                    if (searchText != lastSearchValue) // Check if the search value has changed
                    {
                        lastSearchValue = searchText; // Update the last searched value
                        string MyQuery = "SELECT * FROM Adminn WHERE Admin_ID = @AdminID"; // Use parameterized query to prevent SQL injection
                        SqlDataAdapter da = new SqlDataAdapter(MyQuery, Con);
                        da.SelectCommand.Parameters.AddWithValue("@AdminID", searchText);
                        SqlCommandBuilder builder = new SqlCommandBuilder(da);
                        var ds = new DataSet();
                        da.Fill(ds);
                        dataGridView1.DataSource = ds.Tables[0];
                    }
                    Con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
    }
}
