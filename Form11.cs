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
    public partial class Form11 : Form
    {
        public Form11()
        {
            InitializeComponent();
            PopulateCafeterias();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=MIKI\SQLEXPRESS01;Initial Catalog=Cafeterias;Integrated Security=True");
        private void PopulateCafeterias()
        {
            try
            {
                Con.Open();
                string query = "SELECT CafeID, Name, Phone_No, Info, Password FROM Cafeteria"; // Include Password column in the query
                SqlDataAdapter adapter = new SqlDataAdapter(query, Con);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Set the Password column's visibility to false in the DataGridView
                dataGridViewCafeterias.DataSource = dataTable;
                dataGridViewCafeterias.Columns["Password"].Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=MIKI\SQLEXPRESS01;Initial Catalog=Cafeterias;Integrated Security=True"))
                {
                    con.Open();

                    // Check if all required fields are filled
                    if (!string.IsNullOrWhiteSpace(txtCafeID.Text) && !string.IsNullOrWhiteSpace(txtName.Text) &&
                        !string.IsNullOrWhiteSpace(txtPhone.Text) && !string.IsNullOrWhiteSpace(txtInfo.Text) &&
                        !string.IsNullOrWhiteSpace(txtPassword.Text))
                    {
                        string insertQuery = "INSERT INTO Cafeteria (CafeID, Name, Phone_No, Info, Password) " +
                                             "VALUES (@CafeID, @Name, @Phone_No, @Info, @Password)";
                        using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@CafeID", txtCafeID.Text);
                            cmd.Parameters.AddWithValue("@Name", txtName.Text);
                            cmd.Parameters.AddWithValue("@Phone_No", txtPhone.Text);
                            cmd.Parameters.AddWithValue("@Info", txtInfo.Text);

                            string password = txtPassword.Text;
                            if (password.Length > 3)
                            {
                                cmd.Parameters.AddWithValue("@Password", password);
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Cafeteria added successfully");
                                PopulateCafeterias(); // Refresh DataGridView
                            }
                            else
                            {
                                MessageBox.Show("Password must be at least 4 characters long.");
                            }
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


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection Con = new SqlConnection(@"Data Source=MIKI\SQLEXPRESS01;Initial Catalog=Cafeterias;Integrated Security=True"))
                {
                    Con.Open();
                    string updateQuery = "UPDATE Cafeteria SET Name = @Name, Phone_No = @Phone_No, Info = @Info WHERE CafeID = @CafeID";
                    using (SqlCommand cmd = new SqlCommand(updateQuery, Con))
                    {
                        cmd.Parameters.AddWithValue("@Name", txtName.Text);
                        cmd.Parameters.AddWithValue("@Phone_No", txtPhone.Text);
                        cmd.Parameters.AddWithValue("@Info", txtInfo.Text);
                        cmd.Parameters.AddWithValue("@CafeID", txtCafeID.Text);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cafeteria updated successfully");
                            PopulateCafeterias(); // Refresh DataGridView
                        }
                        else
                        {
                            MessageBox.Show("Cafeteria ID not found. No changes were made.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection Con = new SqlConnection(@"Data Source=MIKI\SQLEXPRESS01;Initial Catalog=Cafeterias;Integrated Security=True"))
                {
                    Con.Open();
                    string deleteQuery = "DELETE FROM Cafeteria WHERE CafeID = @CafeID";
                    SqlCommand cmd = new SqlCommand(deleteQuery, Con);
                    cmd.Parameters.AddWithValue("@CafeID", txtCafeID.Text);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cafeteria deleted successfully");
                        PopulateCafeterias(); // Refresh DataGridView
                    }
                    else
                    {
                        MessageBox.Show("Cafeteria ID not found. No changes were made.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
            }
        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();
                string searchQuery = "SELECT * FROM Cafeteria WHERE CafeID = @CafeID";
                SqlDataAdapter adapter = new SqlDataAdapter(searchQuery, Con);
                adapter.SelectCommand.Parameters.AddWithValue("@CafeID", txtCafeID.Text);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridViewCafeterias.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Form2 home = new Form2();
            home.Show();
            this.Hide();

        }

        private void dataGridViewCafeterias_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
