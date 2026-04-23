using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class DetailsForm : Form
    {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Database1.accdb";

        public DetailsForm(DataGridViewRow teamRow)
        {
            InitializeComponent();
            ShowTeamDetails(teamRow);
        }

        private void ShowTeamDetails(DataGridViewRow row)
        {
            this.Text = "Детали команды: " + row.Cells["team_name"].Value.ToString();
            label1.Text = row.Cells["team_name"].Value.ToString();

            string imageName = row.Cells["logo_path"].Value.ToString();
            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images", imageName);

            if (File.Exists(imagePath))
            {
                pictureBox1.Image = Image.FromFile(imagePath);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }

            LoadPlayers(row.Cells["team_id"].Value.ToString());
        }

        private void LoadPlayers(string teamId)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string query = $"SELECT nickname, role, mmr FROM Players WHERE team_id = {teamId}";
                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}