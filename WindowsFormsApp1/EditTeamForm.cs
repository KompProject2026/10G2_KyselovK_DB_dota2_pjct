using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace WindowsFormsApp1
{
    public partial class EditTeamForm : Form
    {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Database1.accdb";
        int? currentId = null;

        public EditTeamForm()
        {
            InitializeComponent();
        }

        public EditTeamForm(DataGridViewRow row)
        {
            InitializeComponent();
            currentId = Convert.ToInt32(row.Cells["team_id"].Value);
            textBox1.Text = row.Cells["team_name"].Value.ToString();
            textBox2.Text = row.Cells["region"].Value.ToString();
            textBox3.Text = row.Cells["tier_level"].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                string query;
                OleDbCommand cmd = new OleDbCommand();

                if (currentId == null)
                {
                    query = "INSERT INTO Teams (team_name, region, tier_level) VALUES (?, ?, ?)";
                }
                else
                {
                    query = "UPDATE Teams SET team_name = ?, region = ?, tier_level = ? WHERE team_id = ?";
                }

                cmd.CommandText = query;
                cmd.Connection = connection;

                cmd.Parameters.AddWithValue("?", textBox1.Text);
                cmd.Parameters.AddWithValue("?", textBox2.Text);
                cmd.Parameters.AddWithValue("?", textBox3.Text);

                if (currentId != null)
                {
                    cmd.Parameters.AddWithValue("?", currentId);
                }

                cmd.ExecuteNonQuery();
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}