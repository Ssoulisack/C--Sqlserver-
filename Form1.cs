using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkwithDB
{   
    public partial class info : Form
    {   
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter da;
        SqlDataReader dr;
        public info()
        {
            InitializeComponent();
        }

        private void info_Load(object sender, EventArgs e)
        {// TODO: This line of code loads data into the 'myDBDataSet1.names' table. You can move, or remove it, as needed.
            this.namesTableAdapter.Fill(this.myDBDataSet1.names);
            // 1 Address of sql server and Database
            conn = new SqlConnection("Data Source=DESKTOP-F01AA5A\\SQLEXPRESS;Initial Catalog=MyDB;User ID=Soulisack;Password=77477686");
            /*if(conn.State == ConnectionState.Closed)
            {
                conn.Open();
                MessageBox.Show("Connection successfully");
            } */
            btnshow_Click(null, null);
            cbcolumn.SelectedIndex = 0;
        }
        //INSERT 
        private void btnsave_Click(object sender, EventArgs e)
        {
            conn.Open();
            string query = "INSERT INTO names VALUES('"+txtfname.Text+"','"+txtlname.Text+"')";
            cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Saved");
            btnshow_Click(null, null);
        }
        //Button Update
        private void btUpdate_Click(object sender, EventArgs e)
        {
            if(txtID.Text != string.Empty)
            {
                conn.Open();
                cmd = new SqlCommand("UPDATE names SET Firstname = '" + txtfname.Text + "',Lastname='"+txtlname.Text+"' WHERE ID = '"+int.Parse(txtID.Text)+"'", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Updated");
                btnshow_Click(null, null);
            }
            else
            {
                MessageBox.Show("Please fill ID for Edit ");

            }
        }
        //Showdata on grid
        private void btnshow_Click(object sender, EventArgs e)
        {
            
            conn.Open();
            string query = "SELECT * FROM names";
            cmd = new SqlCommand(query, conn);
            var result = cmd.ExecuteReader();
           /* DataTable dt = new DataTable();  //Method 1
            dt.Load(result);
            dataGridView1.DataSource = dt;*/
            dataGridView1.Rows.Clear();
            while (result.Read())
            {
                dataGridView1.Rows.Add(result["ID"], result["Firstname"], result["Lastname"], "Edit", "Delete");
            }  //Add coloumn 
            conn.Close();

            /*while (result.Read())
            {
            dataGridView1.Rows.Add(result["ID"], result["Firstname"]+" "+ result["Lastname"]);
            }*/
        }
        //Button Fetch
        private void btfetch_Click(object sender, EventArgs e)
        {
            if(txtID.Text != string.Empty)
            {
                conn.Open();
                string NameID = txtID.Text;
            
                cmd = new SqlCommand("SELECT * FROM names WHERE ID = "+NameID, conn);
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtfname.Text = reader["Firstname"].ToString();
                    txtlname.Text = reader["Lastname"].ToString();
                }
                else
                {
                    MessageBox.Show("No Record found");
                }
            }
            else
            {
                MessageBox.Show("You have to fill IDNumber which Textbox ID ");
            }
            conn.Close();

        } 
        //Button Exit 
        private void btnexit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        //button clears
        private void btnclr_Click(object sender, EventArgs e)
        {
            txtID.Text = "";
            txtfname.Text = "";
            txtlname.Text = "";
        }
        //button Delete
        private void btdelete_Click(object sender, EventArgs e)
        {
            if(txtID.Text != "")
            {

            if(MessageBox.Show("Do you want to Delete these?", "Delete Record", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
            conn.Open();
            cmd = new SqlCommand("DELETE FROM names WHERE ID="+txtID.Text, conn);
            cmd.ExecuteNonQuery();
            conn.Close();

            }
            }
            else
            {
                MessageBox.Show("Input ID for Delete");
            }
            btnshow_Click(null, null);

        }
        //gridcontent_click
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                if (selectedRow.Cells[0].Value != null)
                {
                    string GridView = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

                    if (e.ColumnIndex == 3 && e.RowIndex > -1)
                    {
                        txtID.Text = GridView;
                    }
                    else if (e.ColumnIndex == 4 && e.RowIndex > -1)
                    {
                        if (MessageBox.Show("Do you want to Delete these?", "Delete Record", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            conn.Open();
                            cmd = new SqlCommand("DELETE FROM names WHERE ID=" + GridView, conn);
                            cmd.ExecuteNonQuery();
                            conn.Close();
                            // Refresh or update your DataGridView here if needed
                            btnshow_Click(null, null);
                        
                        }
                    }
                }
                else
                {
                    MessageBox.Show("The selected Rows or Cless is null");
                }
                
            }
        }
        //tb search
        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            conn.Open();
            string src = txtsearch.Text;
            string query = "SELECT * FROM names";
            if(cbcolumn.SelectedIndex == 0 )
            {
                query += " WHERE Firstname LIKE '%" + src + "%' OR Lastname LIKE '%" + src + "%' ";
                if (int.TryParse(src, out _))
                {
                    query += " OR ID = " + src;
                }

            }
            else
            {
                if(cbcolumn.SelectedIndex == 1 && src.Length > 0)
                {
                    // Check if the input is a valid integer for ID
                    if (!int.TryParse(src, out _))
                    {
                        MessageBox.Show("Please enter a valid integer for ID.");
                        conn.Close();
                        return;
                    }

                    // Continue with the query for ID
                    query += " WHERE ID = " + src;
                }
                else if(cbcolumn.SelectedIndex == 2)
                {
                    query += " WHERE Firstname LIKE '%" + src +"%' ";
                }else if(cbcolumn.SelectedIndex == 3)
                {
                    query += " WHERE Lastname LIKE '%" + src + "%' ";
                }

            }
            
            cmd = new SqlCommand(query, conn);
            var reader = cmd.ExecuteReader();

            dataGridView1.Rows.Clear();

            while (reader.Read())
            {
                dataGridView1.Rows.Add(reader["ID"], reader["Firstname"], reader["Lastname"], "Edit", "Delete");
            }

            conn.Close();
        }

    }

    }

