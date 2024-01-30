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
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=MIKI\SQLEXPRESS01;Initial Catalog=Cafeterias;Integrated Security=True");

        void populate()
        {
            try
            {
                Con.Open();
                String MyQuery = " select * from Foodchoice";
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

            private void button1_Click(object sender, EventArgs e)
        {

            if (Con.State == ConnectionState.Closed)
            {
                Con.Open();
            }


            String myquery = "delete from Foodchoice";
            SqlCommand cmd = new SqlCommand(myquery, Con);
            MessageBox.Show("   Successfully deleted");
            cmd.ExecuteNonQuery();

            Con.Close();
        
        
    }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            populate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 home = new Form3();
            home.Show();
            this.Hide();

        }
    }
}
