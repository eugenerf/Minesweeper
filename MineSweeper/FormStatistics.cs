using System;
using System.Windows.Forms;

namespace MineSweeper
{
    public partial class FormStatistics : Form
    {
        private FormMineSweeper formParent = null;

        public FormStatistics()
        {
            InitializeComponent();
        }

        public FormStatistics(FormMineSweeper parent)
        {
            InitializeComponent();

            formParent = parent;
        }

        private void FormStatistics_Load(object sender, EventArgs e)
        {
            cbStatsType.Checked = false;
            lbPreset.SelectedIndex = 0;

            dgvTop10.Visible = false;
            dgvTop5.Visible = true;
            ShowStats(0);
        }

        private void FormStatistics_FormClosing(object sender, FormClosingEventArgs e)
        {
            formParent.Enabled = true;
        }

        private void cbStatsType_CheckedChanged(object sender, EventArgs e)
        {
            if (cbStatsType.Checked) ShowStats();
            else ShowStats(0);
        }

        private void lbPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowStats(lbPreset.SelectedIndex);            
        }

        /// <summary>
        /// Show statistics by preset
        /// </summary>
        /// <param name="index">Preset index</param>
        private void ShowStats(int index)
        {
            if (index < 0 || index > 2) return;

            dgvTop10.Visible = false;
            dgvTop5.Visible = true;
            lbPreset.Enabled = true;
            lbPreset.SelectedIndex = index;
            dgvTop5.Rows.Clear();
            MinesStatistics.PresetStatsInfo psi = formParent.MStats.StatsByPreset[index];
            if (psi.Top != null)
            {
                foreach (MinesStatistics.WinnerInfo wi in formParent.MStats.StatsByPreset[index].Top)
                {
                    //first easter egg for Zaza
                    string name =
                        (wi.Name.ToLower() == "zaza") ?
                        char.ConvertFromUtf32(0x1f498) + wi.Name + char.ConvertFromUtf32(0x1f498) :
                        wi.Name;

                    object[] row =
                        {
                        wi.GameTime.ToString(),
                        name,
                        wi.WinTime.ToString(),
                        wi.Level3BV.ToString() };

                    dgvTop5.Rows.Add(row);
                }
            }
            lTotalGames.Text = psi.TotalGames.ToString();
            lWinGames.Text = psi.WinGames.ToString();
            lWinPercent.Text = string.Format("{0:F2}", psi.WinPercent);
            lWinStreak.Text = psi.WinStreak.ToString();
            lLooseStreak.Text = psi.LooseStreak.ToString();
            lMaxGameTime.Text = psi.LongestGameTime.ToString();
            lMinGameTime.Text = psi.FastestGameTime.ToString();
            lMaxWinStreak.Text = psi.MaxWinStreak.ToString();
            lMaxLooseStreak.Text = psi.MaxLooseStreak.ToString();
        }

        /// <summary>
        /// Show statistics by 3BV
        /// </summary>
        private void ShowStats()
        {
            lbPreset.Enabled = false;
            lbPreset.SelectedIndex = -1;
            dgvTop5.Visible = false;
            dgvTop10.Visible = true;
            dgvTop10.Rows.Clear();
            if (formParent.MStats.StatsBy3BV != null)
            {
                foreach (MinesStatistics.WinnerInfo wi in formParent.MStats.StatsBy3BV)
                {
                    //first easter egg for Zaza
                    string name =
                        (wi.Name.ToLower() == "zaza") ?
                        char.ConvertFromUtf32(0x1f498) + wi.Name + char.ConvertFromUtf32(0x1f498) :
                        wi.Name;

                    object[] row =
                        {
                            wi.Level3BV.ToString(),
                            wi.GameTime.ToString(),
                            name,
                            wi.WinTime.ToString(),
                            wi.Preset.ToString() };

                    dgvTop10.Rows.Add(row);
                }
            }
            lTotalGames.Text = "";
            lWinGames.Text = "";
            lWinPercent.Text = "";
            lWinStreak.Text = "";
            lLooseStreak.Text = "";
            lMaxGameTime.Text = "";
            lMinGameTime.Text = "";
            lMaxWinStreak.Text = "";
            lMaxLooseStreak.Text = "";
        }

        private void butClearStats_Click(object sender, EventArgs e)
        {
            formParent.MStats.Clear();
            cbStatsType.Checked = false;
            ShowStats(0);
        }

        private void butClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
