using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

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
            try
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
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
    }
}