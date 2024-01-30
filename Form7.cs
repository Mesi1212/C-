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
using System.IO;

namespace sadprroject
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=MIKI\SQLEXPRESS01;Initial Catalog=Cafeterias;Integrated Security=True");
        private object openFileDialog1;

        void populate()
        {
            try
            {
                using (SqlConnection Con = new SqlConnection(@"Data Source=MIKI\SQLEXPRESS01;Initial Catalog=Cafeterias;Integrated Security=True"))
                {
                    Con.Open();
                    String MyQuery = "SELECT FoodId, Food_Name, Image FROM Food"; // Select specific columns without CafeID
                    SqlDataAdapter da = new SqlDataAdapter(MyQuery, Con);
                    SqlCommandBuilder builder = new SqlCommandBuilder(da);
                    var ds = new DataSet();
                    da.Fill(ds);
                    dataGridView1.DataSource = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

    

    private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
                {
                    // Set properties for the OpenFileDialog
                    openFileDialog1.Title = "Select Image File";
                    openFileDialog1.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.gif)|*.jpg;*.jpeg;*.png;*.gif";

                    // Show the dialog and get the result
                    DialogResult result = openFileDialog1.ShowDialog();

                    // Check if the user selected a file
                    if (result == DialogResult.OK)
                    {
                        {
                            // Get the selected image file path
                            string imagePath = openFileDialog1.FileName;

                            // Resize the image to fit into a fixed space
                            Image selectedImage = Image.FromFile(imagePath);
                            int maxWidth = 100; // Set the maximum width
                            int maxHeight = 100; // Set the maximum height
                            Image resizedImage = ResizeImage(selectedImage, maxWidth, maxHeight);

                            // Convert the resized image to a byte array
                            byte[] imageBytes;
                            using (MemoryStream ms = new MemoryStream())
                            {
                                resizedImage.Save(ms, selectedImage.RawFormat);
                                imageBytes = ms.ToArray();
                            }

                            // Open the database connection
                            Con.Open();

                            // Create a parameterized SQL command to insert values into the Food table
                            string insertQuery = "INSERT INTO Food (FoodId, Food_Name, Image) " +
                                                 "VALUES (@FoodId, @FoodName, @Image)";
                            SqlCommand cmd = new SqlCommand(insertQuery, Con);

                            // Set parameter values from TextBoxes
                            cmd.Parameters.AddWithValue("@FoodId", textBox1.Text);
                            cmd.Parameters.AddWithValue("@FoodName", textBox2.Text);

                            // Add the image as a parameter
                            SqlParameter imageParameter = new SqlParameter("@Image", SqlDbType.VarBinary);
                            imageParameter.Value = imageBytes;
                            cmd.Parameters.Add(imageParameter);

                            // Execute the query
                            cmd.ExecuteNonQuery();

                            // Close the connection
                            Con.Close();

                            // Show a message indicating that the food item was successfully added
                            MessageBox.Show("Food Successfully Added");

                            // Clear TextBoxes
                            textBox1.Clear();
                            textBox2.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                if (Con.State == ConnectionState.Open)
                    Con.Close();
            }
        }

            // Method to resize an image to fit into a fixed space
            private Image ResizeImage(Image image, int maxWidth, int maxHeight)
            {
                int width = image.Width;
                int height = image.Height;

                // Calculate the new dimensions while maintaining the aspect ratio
                if (width > maxWidth || height > maxHeight)
                {
                    double ratioX = (double)maxWidth / width;
                    double ratioY = (double)maxHeight / height;
                    double ratio = Math.Min(ratioX, ratioY);

                    width = (int)(image.Width * ratio);
                    height = (int)(image.Height * ratio);
                }

                // Create a new image with the new dimensions
                Bitmap newImage = new Bitmap(width, height);
                using (Graphics g = Graphics.FromImage(newImage))
                {
                    g.DrawImage(image, 0, 0, width, height);
                }
                return newImage;
            }

        



        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            populate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Enter the Food id");
            }
            else
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection Con = new SqlConnection(@"Data Source=MIKI\SQLEXPRESS01;Initial Catalog=Cafeterias;Integrated Security=True"))
                        {
                            Con.Open();
                            string deleteQuery = "DELETE FROM Food WHERE FoodId = @FoodId";
                            SqlCommand cmd = new SqlCommand(deleteQuery, Con);
                            cmd.Parameters.AddWithValue("@FoodId", textBox1.Text);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Food Successfully deleted");
                                populate(); // Refresh the DataGridView
                            }
                            else
                            {
                                MessageBox.Show("No records found with the specified FoodId");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Deletion canceled");
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();

                // Create a parameterized SQL command to update values in the Food table
                string updateQuery = "UPDATE Food SET Food_Name = @FoodName WHERE FoodId = @FoodId";
                SqlCommand cmd = new SqlCommand(updateQuery, Con);

                // Set parameter values from TextBoxes
                cmd.Parameters.AddWithValue("@FoodName", textBox2.Text);
              
                cmd.Parameters.AddWithValue("@FoodId", textBox1.Text);

                // Execute the update query
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // If rows were affected (i.e., the update was successful), show a success message
                    MessageBox.Show("Food Successfully Updated");
                    populate(); // Refresh the data in your application
                }
                else
                {
                    // If no rows were affected, show a message indicating that the Food ID was not found
                    MessageBox.Show("Food ID not found. No changes were made.");
                }

                // Close the connection
                Con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                if (Con.State == ConnectionState.Open)
                    Con.Close();
            }

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
           try
{
    Con.Open();

    // Create a parameterized SQL command to select a record from the Food table
    string selectQuery = "SELECT * FROM Food WHERE FoodId = @FoodId";
    SqlCommand cmd = new SqlCommand(selectQuery, Con);

    // Set the parameter value for FoodId
    cmd.Parameters.AddWithValue("@FoodId", textBox1.Text);

    // Create a new instance of the SqlDataAdapter
    SqlDataAdapter da = new SqlDataAdapter(cmd);

    // Automatically generate SQL commands for updating the database
    SqlCommandBuilder builder = new SqlCommandBuilder(da);

    // Create a new DataSet to store the retrieved data
    var ds = new DataSet();

    // Fill the DataSet with data from the database
    da.Fill(ds);

    // Set the DataSource of the DataGridView to the first table in the DataSet
    dataGridView1.DataSource = ds.Tables[0];

    // Close the connection
    Con.Close();
}
catch (Exception ex)
{
    MessageBox.Show("Error: " + ex.Message);
    if (Con.State == ConnectionState.Open)
        Con.Close();
}

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form3 home = new Form3();
            home.Show();
            this.Hide();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
          
        }
    }
}
