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
    public partial class Form1 : Form
    {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Database1.accdb";

        public Form1()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData(string filter = "")
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                string query = "SELECT * FROM Teams";
                if (!string.IsNullOrEmpty(filter))
                {
                    query += $" WHERE team_name LIKE '%{filter}%' OR region LIKE '%{filter}%'";
                }
                query += " ORDER BY tier_level DESC, team_name ASC";

                OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection);
                DataTable dt = new DataTable();

                adapter.Fill(dt);
                dataGridView1.DataSource = dt;

                if (dataGridView1.Columns.Contains("team_id"))
                {
                    dataGridView1.Columns["team_id"].Visible = false;
                }

                if (dataGridView1.Columns.Contains("logo_path"))
                {
                    dataGridView1.Columns["logo_path"].Visible = false;
                }

                if (dataGridView1.Columns.Contains("team_name"))
                    dataGridView1.Columns["team_name"].HeaderText = "Назва команди";

                if (dataGridView1.Columns.Contains("region"))
                    dataGridView1.Columns["region"].HeaderText = "Регіон";

                if (dataGridView1.Columns.Contains("tier_level"))
                    dataGridView1.Columns["tier_level"].HeaderText = "Рівень (Tier)";

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }


        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dataGridView1.Rows[e.RowIndex];
                DetailsForm details = new DetailsForm(row);
                details.ShowDialog();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            EditTeamForm editForm = new EditTeamForm();
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                EditTeamForm editForm = new EditTeamForm(dataGridView1.CurrentRow);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                var result = MessageBox.Show("Видалити команду та ВСІХ її гравців?", "Увага", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["team_id"].Value);
                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {
                        connection.Open();
                        string deletePlayersQuery = "DELETE FROM Players WHERE team_id = ?";
                        OleDbCommand cmdPlayers = new OleDbCommand(deletePlayersQuery, connection);
                        cmdPlayers.Parameters.AddWithValue("?", id);
                        cmdPlayers.ExecuteNonQuery();

                        string deleteTeamQuery = "DELETE FROM Teams WHERE team_id = ?";
                        OleDbCommand cmdTeam = new OleDbCommand(deleteTeamQuery, connection);
                        cmdTeam.Parameters.AddWithValue("?", id);
                        cmdTeam.ExecuteNonQuery();
                    }
                    LoadData();
                }
            }
        }
    }
}
