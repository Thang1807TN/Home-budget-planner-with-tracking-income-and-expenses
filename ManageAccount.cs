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

namespace Budget_Family
{
    public partial class ManageAccount : Form
    {
        public ManageAccount()
        {
            InitializeComponent();
        }

        string connection = @"Data Source=LAPTOP-A08PHTR5\SQLEXPRESS;Initial Catalog=BudgetFamily;Integrated Security=True;Encrypt=False";
        string sql;
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader dr;

        private void ManageAccount_Load(object sender, EventArgs e)
        {
            ShowData();
        }

        public void ShowData()
        {
            listView1.Items.Clear();
            conn = new SqlConnection(connection);
            conn.Open();
            sql = @"SELECT AccountID, Name,Age,Password, Available FROM Account WHERE Authoriza<>'Admin'";
            cmd = new SqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            int i = 0;
            while (dr.Read())
            {
                listView1.Items.Add(dr[0].ToString());
                listView1.Items[i].SubItems.Add(dr[1].ToString());
                listView1.Items[i].SubItems.Add(dr[2].ToString());
                listView1.Items[i].SubItems.Add(dr[3].ToString());
                listView1.Items[i].SubItems.Add(dr[4].ToString());
                i++;
            }
            conn.Close();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnBlock_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string selectedAccountID = listView1.SelectedItems[0].SubItems[0].Text;

                using (SqlConnection conn = new SqlConnection(connection))
                {
                    conn.Open();
                    string sql = "UPDATE Account SET Available = 'NotActive' WHERE AccountID = @AccountID";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@AccountID", selectedAccountID);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Account has been blocked!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowData();
            }
            else
            {
                MessageBox.Show("Please select an account!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnUnblock_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string selectedAccountID = listView1.SelectedItems[0].SubItems[0].Text;
                string currentStatus = listView1.SelectedItems[0].SubItems[4].Text;
                string currentName = listView1.SelectedItems[0].SubItems[1].Text;

                if (currentStatus == "NotActive")
                {
                    using (SqlConnection conn = new SqlConnection(connection))
                    {
                        conn.Open();
                        string sql = "UPDATE Account SET Available = 'Active' WHERE AccountID = @AccountID";
                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@AccountID", selectedAccountID);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Account has been unblocked!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ShowData();
                }
                else
                {
                    MessageBox.Show("This account is already active!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please select an account!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string selectedAccountID = listView1.SelectedItems[0].SubItems[0].Text;

                using (SqlConnection conn = new SqlConnection(connection))
                {
                    conn.Open();
                    string sql = "DELETE FROM Account WHERE AccountID = @AccountID";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@AccountID", selectedAccountID);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Account has been delete!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowData();
            }
            else
            {
                MessageBox.Show("Please select an account!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            App app = new App();
            app.Show();
        }
    }
}
