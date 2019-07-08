using System;
using System.Windows.Forms;

namespace MineSweeper
{
    /// <summary>
    /// Statistics form. Shows game stats
    /// </summary>
    public partial class FormStatistics : Form
    {
        #region Fields
        /// <summary>
        /// Parent form instance
        /// </summary>
        private FormMineSweeper formParent = null;
        #endregion

        #region Methods
        #region Form
        /// <summary>
        /// Stats form ctor
        /// </summary>
        /// <param name="parent">Parent form instance</param>
        public FormStatistics(FormMineSweeper parent)
        {
            InitializeComponent();
            formParent = parent;    //save parents form instance
        }   //END (ctor)

        /// <summary>
        /// Stats form load event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void FormStatistics_Load(object sender, EventArgs e)
        {
            cbStatsType.Checked = false;    //uncheck StatsType checkbox
            lbPreset.SelectedIndex = 0;     //select first record in Preset listbox
            dgvTop10.Visible = false;       //hide Top10 datagridview (stats by difficulty)
            dgvTop5.Visible = true;         //show Top5 datagridview (stats by preset)
            ShowStats(0);                   //show stats by preset
        }   //END (FormStatistics_Load)

        /// <summary>
        /// Stats form closing event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void FormStatistics_FormClosing(object sender, FormClosingEventArgs e)
        {
            formParent.Enabled = true;  //enable parent form
        }   //END (FormStatistics_FormClosing)
        #endregion

        #region Form controls events handlers
        /// <summary>
        /// StatsType checkbox checked changed event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void cbStatsType_CheckedChanged(object sender, EventArgs e)
        {
            if (cbStatsType.Checked)    //if StatsType checked
                ShowStats();                //show stats by difficulty
            else                        //if StatsType NOT checked
                ShowStats(0);               //show stats by preset
        }   //END (cbStatsType_CheckedChanged)

        /// <summary>
        /// Preset selected index changed event hendler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void lbPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowStats(lbPreset.SelectedIndex);  //show stats for currently selected preset
        }   //END (lbPreset_SelectedIndexChanged)

        /// <summary>
        /// ClearStats button click event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void butClearStats_Click(object sender, EventArgs e)
        {
            formParent.MStats.Clear();      //clear game stats
            cbStatsType.Checked = false;    //uncheck StatsType checkbox
            ShowStats(0);                   //show stats by preset
        }   //END (butClearStats_Click)

        /// <summary>
        /// Close button click event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event argument</param>
        private void butClose_Click(object sender, EventArgs e)
        {
            Close();    //close Stats form
        }   //END (butClose_Click)
        #endregion

        #region Additional
        /// <summary>
        /// Show statistics by 3BV difficulty level
        /// </summary>
        private void ShowStats()
        {
            lbPreset.Enabled = false;                               //disable Preset listbox
            lbPreset.SelectedIndex = -1;                            //select no records in Preset listbox
            dgvTop5.Visible = false;                                //hide Top5 datagridview (stats by preset)
            dgvTop10.Visible = true;                                //show Top10 datagridview (stats by difficulty)
            dgvTop10.Rows.Clear();                                  //clear Top10 datagridview
            if (formParent.MStats.StatsBy3BV != null)               //if stats by difficulty exists
            {
                foreach (MinesStatistics.WinnerInfo wi 
                    in formParent.MStats.StatsBy3BV)                    //moving through all the records in stats by difficulty list
                {
                    //show first easter egg for Zaza
                    string name =                                           //generate player name
                        (wi.Name.ToLower() == "zaza") ?                         //if player name is zaza (case insensitive)
                        char.ConvertFromUtf32(0x1f498) + 
                        wi.Name + 
                        char.ConvertFromUtf32(0x1f498) :                            //add Unicode heart-with-arrow characters to it
                        wi.Name;                                                    //otherwise make no changes to player name
                    //generate row object for the current record
                    object[] row =
                        {
                            wi.Level3BV.ToString(),                             //difficulty level
                            wi.GameTime.ToString(),                             //game duration
                            name,                                               //player name
                            wi.WinTime.ToString(),                              //date and time of win
                            wi.Preset.ToString() };                             //game preset
                    dgvTop10.Rows.Add(row);                                 //add row object to Top10 datagridview
                }   //ENDFOREACH (records in stats)
            }   //ENDIF (stats by difficulty exists)

            //clear boxes that are NOT used in the current stats type
            lTotalGames.Text = "";                                  //total games played
            lWinGames.Text = "";                                    //total games won
            lWinPercent.Text = "";                                  //win percent
            lWinStreak.Text = "";                                   //current win streak
            lLooseStreak.Text = "";                                 //current loose streak
            lMaxGameTime.Text = "";                                 //max game duration
            lMinGameTime.Text = "";                                 //min game duration
            lMaxWinStreak.Text = "";                                //max win streak
            lMaxLooseStreak.Text = "";                              //max loose streak
        }   //END (ShowStats - by difficulty)

        /// <summary>
        /// Show statistics by preset
        /// </summary>
        /// <param name="index">Preset index</param>
        private void ShowStats(int index)
        {
            if (index < 0 || index > 2) return;                             //return if specified index is invalid
            dgvTop10.Visible = false;                                       //hide Top10 datagridview (stats by difficulty)
            dgvTop5.Visible = true;                                         //show Top5 datagridview (stats by preset)
            lbPreset.Enabled = true;                                        //enable Preset listbox
            lbPreset.SelectedIndex = index;                                 //set Preset listbox selection to specified index
            dgvTop5.Rows.Clear();                                           //clear Top5 datagridview
            MinesStatistics.PresetStatsInfo psi = 
                formParent.MStats.StatsByPreset[index];                     //get current game's stats by preset
            if (psi.Top != null)                                            //if Top list exists
            {
                foreach (MinesStatistics.WinnerInfo wi 
                    in formParent.MStats.StatsByPreset[index].Top)              //moving through all the records in Top list
                {
                    //show first easter egg for Zaza
                    string name =                                                   //generate player name
                        (wi.Name.ToLower() == "zaza") ?                                 //if player name is zaza (case insensitive)
                        char.ConvertFromUtf32(0x1f498) + 
                        wi.Name + 
                        char.ConvertFromUtf32(0x1f498) :                                    //add Unicode heart-with-arrow characters to it
                        wi.Name;                                                            //otherwise make no changes to player name
                    //generate row object for Top5 datagridview
                    object[] row =
                        {
                        wi.GameTime.ToString(),                                         //game duration
                        name,                                                           //player name
                        wi.WinTime.ToString(),                                          //date and time of win
                        wi.Level3BV.ToString() };                                       //difficulty level
                    dgvTop5.Rows.Add(row);                                          //add row object to Top5 datagridview
                }   //ENDFOREACH (records in top list)
            }   //ENDIF (top list exists)
            //fill other stats boxes
            lTotalGames.Text = psi.TotalGames.ToString();                   //total games
            lWinGames.Text = psi.WinGames.ToString();                       //games won
            lWinPercent.Text = string.Format("{0:F2}", psi.WinPercent);     //win percent
            lWinStreak.Text = psi.WinStreak.ToString();                     //current win streak
            lLooseStreak.Text = psi.LooseStreak.ToString();                 //current loose streak
            lMaxGameTime.Text = psi.LongestGameTime.ToString();             //max game duration
            lMinGameTime.Text = psi.FastestGameTime.ToString();             //min game duration
            lMaxWinStreak.Text = psi.MaxWinStreak.ToString();               //max win streak
            lMaxLooseStreak.Text = psi.MaxLooseStreak.ToString();           //max loose streak
        }   //END (ShowStats - by preset)
        #endregion
        #endregion
    }   //ENDCLASS (FormStatistics)
}   //ENDNAMESPACE (MineSweeper)
