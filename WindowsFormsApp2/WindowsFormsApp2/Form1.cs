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

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        DataSet ds;
        SqlDataAdapter da;
        string connectionString = @"Data Source=localhost\ELSBRD;Initial Catalog=BankDataBase;User ID=sa;Password=12345";
        
        int scr_val;
        public Form1()
        {
            InitializeComponent();
            scr_val = 0;
        }
        
        private void LoadData_Click(object sender, EventArgs e)
        {
             var select = "SELECT CustId, FirstName, SecondName, Address, City, Phone FROM Customer";
             var c = new SqlConnection(connectionString); // Your Connection String here
             var dataAdapter = new SqlDataAdapter(select, c);

             var commandBuilder = new SqlCommandBuilder(dataAdapter);
             var ds = new DataSet();
             dataAdapter.Fill(ds);


            PopulateDataGridView();
        }
    
        void PopulateDataGridView()
        {
            string sql = "SELECT * FROM v_customers_all";
            SqlConnection con = new SqlConnection(connectionString);
            da = new SqlDataAdapter(sql, con);
            ds = new DataSet();
            con.Open();
            da.Fill(ds, "Customer");
            con.Close();
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "Customer";
            label1.Text = dataGridView1.Rows.Count.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            //PopulateDataGridView();
            
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
           
        }


        private void Insert_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    if (sqlCon.State == ConnectionState.Closed)
                        sqlCon.Open();
                    if (Insert.Text == "Save")
                    {
                        SqlCommand sqlCmd = new SqlCommand("CustAddOrEdit", sqlCon);
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@mode", "Add");
                        sqlCmd.Parameters.AddWithValue("@CustId", txtCustId.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@SecondName", txtSecondName.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@City", txtCity.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                        sqlCmd.ExecuteNonQuery();
                        MessageBox.Show("Saved saccessfully");
                    }
                    else
                    {
                        SqlCommand sqlCmd = new SqlCommand("CustAddOrEdit", sqlCon);
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@mode", "Edit");
                        sqlCmd.Parameters.AddWithValue("@CustId", txtCustId.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@SecondName", txtSecondName.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@City", txtCity.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                        sqlCmd.ExecuteNonQuery();
                        MessageBox.Show("Updated saccessfully");
                    }
                    

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error Message");
            }
            finally
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Close();
                }
            }
        }

        void Reset()
        {
            txtCustId.Text = txtFirstName.Text = txtSecondName.Text = txtAddress.Text = txtCity.Text = txtPhone.Text = "";
            Insert.Text = "Save";
            Delete.Enabled = false;
        }
        //working

        private void Update_Click(object sender, EventArgs e)
        {
            Reset();
            
        }
                

        private void Delete_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.SqlClient.SqlConnection sqlConnection1 = new System.Data.SqlClient.SqlConnection(connectionString);
                if (dataGridView1.CurrentRow.Cells["CustId"].Value != DBNull.Value)
                {
                    if (MessageBox.Show("Are you sure to Delete this Record ?", "DataGridView", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        using (SqlConnection sqlCon = new SqlConnection(connectionString))
                        {
                            sqlCon.Open();
                            SqlCommand sqlCmd = new SqlCommand("CustomerDeleteByID", sqlCon);
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            sqlCmd.Parameters.AddWithValue("@CustId", Convert.ToInt32(dataGridView1.CurrentRow.Cells["CustId"].Value));
                            sqlCmd.ExecuteNonQuery();
                            MessageBox.Show("Delete saccessfully");
                        }
                    }
                    //else
                    //e.Cancel = true;
                }
                sqlConnection1.Close();
                MessageBox.Show("Row is Deleted");
                //e.Cancel = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error Message");
            }

            
        }

        private void Next_Click(object sender, EventArgs e)
        {
            int num;
            bool isNum = int.TryParse(numRows.Text.Trim(), out num);
            if (String.IsNullOrEmpty(numRows.Text) || !isNum && numRows.Text != String.Empty)
            {
                MessageBox.Show("Insert the number of rows", "Invalid parameters", MessageBoxButtons.OK, MessageBoxIcon.Error);
                numRows.Clear();
            }
            else
            {

                string nRows = numRows.Text;
                scr_val = scr_val + Convert.ToInt32(nRows);
                if (scr_val > int.Parse(label1.Text))
                {
                    scr_val = 10;
                }
                ds.Clear();
                da.Fill(ds, scr_val, Convert.ToInt32(nRows), "Customer");
                label2.Text = "Record " + (scr_val + 1) + " of ";
            }


        }

        private void Previous_Click(object sender, EventArgs e)
        {
            int num;
            bool isNum = int.TryParse(numRows.Text.Trim(), out num);
            if (String.IsNullOrEmpty(numRows.Text) || !isNum && numRows.Text != String.Empty)
            {
                MessageBox.Show("Insert the number of rows", "Invalid parameters", MessageBoxButtons.OK, MessageBoxIcon.Error);
                numRows.Clear();
            }
            else
            {
                string nRows = numRows.Text;
                scr_val = scr_val - Convert.ToInt32(nRows);
                if (scr_val <= 0)
                {
                    scr_val = 0;
                }
                ds.Clear();
                da.Fill(ds, scr_val, Convert.ToInt32(nRows), "Customer");
                label2.Text = "Record " + (scr_val + 1) + " of ";
            }
        }

        private void textFirstName_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "CustId") 
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT CustId, FirstName, SecondName, Address, City, Phone FROM Customer WHERE CustId LIKE'" + textFirstName.Text + "%'", connectionString);
                DataTable data = new DataTable();
                sda.Fill(data);
                dataGridView1.DataSource = data;
            }

            else if (comboBox1.Text == "FirstName")
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT CustId, FirstName, SecondName, Address, City, Phone FROM Customer WHERE FirstName LIKE '" + textFirstName.Text + "%'", connectionString);
                DataTable data = new DataTable();
                sda.Fill(data);
                dataGridView1.DataSource = data;
            }

            else if (comboBox1.Text == "SecondName")
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT CustId, FirstName, SecondName, Address, City, Phone FROM Customer WHERE SecondName LIKE '" + textFirstName.Text + "%'", connectionString);
                DataTable data = new DataTable();
                sda.Fill(data);
                dataGridView1.DataSource = data;
            }

            else if (comboBox1.Text == "Address")
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT CustId, FirstName, SecondName, Address, City, Phone FROM Customer WHERE Address LIKE '" + textFirstName.Text + "%'", connectionString);
                DataTable data = new DataTable();
                sda.Fill(data);
                dataGridView1.DataSource = data;
            }

            else if (comboBox1.Text == "City")
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT CustId, FirstName, SecondName, Address, City, Phone FROM Customer WHERE City LIKE '" + textFirstName.Text + "%'", connectionString);
                DataTable data = new DataTable();
                sda.Fill(data);
                dataGridView1.DataSource = data;
            }

            else if (comboBox1.Text == "Phone")
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT CustId, FirstName, SecondName, Address, City, Phone FROM Customer WHERE Phone LIKE '" + textFirstName.Text + "%'", connectionString);
                DataTable data = new DataTable();
                sda.Fill(data);
                dataGridView1.DataSource = data;
            }


        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Index != -1)
            {
                txtCustId.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                txtFirstName.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                txtSecondName.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                txtAddress.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                txtCity.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                txtPhone.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                Insert.Text = "Update";
                Delete.Enabled = true;
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }

}
