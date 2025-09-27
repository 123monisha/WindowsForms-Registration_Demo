using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsApp_Registration
{
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection();
        int status = 1;
        public Form1()
        {
            InitializeComponent();
            con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=Banking; Integrated Security=true";
            lblAccNo.Text = AccountNumber().ToString();
            cmdAccounts.DataSource = Accounts();
            cmdAccounts.DisplayMember = "Account";
            cmdAccounts.ValueMember = "Id";
            cmdAccounts.SelectedIndex = 0;
            
        }
        int AccountNumber()
        {
            SqlCommand cmd = new SqlCommand("select max(Accno)+1 from Customers ", con);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        DataTable Accounts()
        {
            SqlDataAdapter da = new SqlDataAdapter("select Id, Account from Accounts ", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            DataRow r = dt.NewRow();
            r[0] = -1;
            r[1] = "choose accounts";
            dt.Rows.InsertAt(r, 0);
            return dt;

        }
        

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!cbStatus.Checked)
            {
                status = 0;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            if(con.State==ConnectionState.Closed)
            {
                con.Open();

            }
            cmd.CommandText="insert into Customers values(@accno, @name, @address, @account, @status)";
            cmd.Parameters.AddWithValue("@accno", Convert.ToInt32(lblAccNo.Text));

            cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
            cmd.Parameters.AddWithValue("@address", txtAddress.Text.Trim());
            cmd.Parameters.AddWithValue("@account", Convert.ToInt32(cmdAccounts.SelectedValue));
            cmd.Parameters.AddWithValue("@status", status);

            int updated = cmd.ExecuteNonQuery();
            if(updated>0)
            {
                lblMsg.Text = "Inseted Suceesfully";
            }
            else
            {
                lblMsg.Text = "sorry we cant insert";
            }
            con.Close();



        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            txtAddress.Clear();
            lblAccNo.Text = AccountNumber().ToString();
            cmdAccounts.SelectedIndex = 0;
            cbStatus.Checked = true;
        }
        

    }
    
}
