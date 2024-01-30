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
    public partial class Form9 : Form
    {
        public Form9()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=MIKI\SQLEXPRESS01;Initial Catalog=Cafeterias;Integrated Security=True");
        void populate()
        {
            try
            {
                Con.Open();
                String MyQuery = "SELECT FoodId, Food_Name, Image FROM Food"; // Exclude CafeID from the query
                SqlDataAdapter da = new SqlDataAdapter(MyQuery, Con);
                SqlCommandBuilder builder = new SqlCommandBuilder(da);
                var ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

                Con.Close();
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

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 home = new Form1();
            home.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection Con = new SqlConnection(@"Data Source=MIKI\SQLEXPRESS01;Initial Catalog=Cafeterias;Integrated Security=True"))
                {
                    Con.Open();

                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Comment (FoodId, StudentID, Comment) VALUES (@FoodId, @StudentID, @Comment)", Con))
                    {
                        cmd.Parameters.AddWithValue("@FoodId", textBox4.Text);
                        cmd.Parameters.AddWithValue("@StudentID", textBox1.Text);
                        cmd.Parameters.AddWithValue("@Comment", textBox5.Text);
                        cmd.ExecuteNonQuery();
                    }

                    using (SqlCommand com = new SqlCommand("SELECT COUNT(*) FROM FoodChoice WHERE Food_Name = @FoodName", Con))
                    {
                        com.Parameters.AddWithValue("@FoodName", textBox3.Text);
                        int count = Convert.ToInt32(com.ExecuteScalar());

                        if (count > 0)
                        {
                            using (SqlCommand updateCmd = new SqlCommand("UPDATE FoodChoice SET Count = Count + 1 WHERE Food_Name = @FoodName AND StudentID = @StudentID", Con))
                            {
                                updateCmd.Parameters.AddWithValue("@FoodName", textBox3.Text);
                                updateCmd.Parameters.AddWithValue("@StudentID", textBox1.Text); // Use StudentID instead of Choice_ID
                                updateCmd.ExecuteNonQuery();
                            }

                        }
                        else
                        {
                            using (SqlCommand insertCmd = new SqlCommand("INSERT INTO FoodChoice (Food_Name, Count, CafeID, Time, StudentID) VALUES (@FoodName, 1, @CafeID, GETDATE(), @StudentID)", Con))
                            {
                                insertCmd.Parameters.AddWithValue("@FoodName", textBox3.Text);
                                insertCmd.Parameters.AddWithValue("@CafeID", textBox6.Text);
                                insertCmd.Parameters.AddWithValue("@StudentID", textBox1.Text); // Use StudentID instead of Choice_ID
                                insertCmd.ExecuteNonQuery();
                            }


                        }
                    }

                    MessageBox.Show("Operation successful");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            textBox4.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox6.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            
        }
    }
}
