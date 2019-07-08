using System;
using System.Windows.Forms;

namespace MineSweeper
{
    /// <summary>
    /// Ask player name form
    /// </summary>
    public partial class FormAskName : Form
    {
        #region Fields
        /// <summary>
        /// Parent form instance
        /// </summary>
        FormMineSweeper parentForm = null;
        /// <summary>
        /// Player name entered in this form
        /// </summary>
        public string PlayerName = "";
        #endregion

        #region Methods
        #region Form
        /// <summary>
        /// AskName form ctor
        /// </summary>
        /// <param name="parent">Instance of the parent form</param>
        public FormAskName(FormMineSweeper parent)
        {
            InitializeComponent();
            parentForm = parent;    //save parent form's instance
        }   //END (ctor)

        /// <summary>
        /// AskName form closing event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void FormAskName_FormClosing(object sender, FormClosingEventArgs e)
        {
            parentForm.Enabled = true;  //enable parent form
        }   //END(FormAskName_FormClosing)

        /// <summary>
        /// AskName form load event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void FormAskName_Load(object sender, EventArgs e)
        {
            //when form is loading we will make a menu of player-names entered before
            //names for this menu will be taken from game stats
            cbName.Items.Clear();                                   //clear menu of player names

            //get names from stats by preset
            foreach (MinesStatistics.PresetStatsInfo psi 
                in parentForm.MStats.StatsByPreset)                 //moving through all the records in stats by preset array
            {
                if (psi.Top != null)                                    //if top list in stats by preset is not empty
                {
                    foreach (MinesStatistics.WinnerInfo wi 
                        in psi.Top)                                     //moving through all the records in top list
                    {
                        if (wi.Name != null && wi.Name != "")               //if player name exists for the current record
                            if (!cbName.Items.Contains(wi.Name))                //if player name is not already added to the menu
                                cbName.Items.Add(wi.Name);                          //add it to the menu
                    }   //ENDFOREACH (records in the top list)
                }   //ENDIF (top list isn't empty)
            }   //ENDFOREACH (records in stats by preset)

            //get names from stats by difficulty level
            if (parentForm.MStats.StatsBy3BV != null)               //if stats by difficulty level is not empty
            {
                foreach (MinesStatistics.WinnerInfo wi 
                    in parentForm.MStats.StatsBy3BV)                    //moving through all the records in stats by difficulty level list
                {
                    if (wi.Name != null && wi.Name != "")                   //if player name exists for the current record
                        if (!cbName.Items.Contains(wi.Name))                    //if player name is not already added to the menu
                            cbName.Items.Add(wi.Name);                              //add it to the menu
                }   //ENDFOREACH (records in stats by difficulty)
            }   //ENDIF (stats by difficulty is not empty)
        }   //END (FormAskName_Load)
        #endregion

        #region Form controls events handlers
        /// <summary>
        /// Cancel button click event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void butCancel_Click(object sender, EventArgs e)
        {
            PlayerName = "";    //clear player name
            Close();            //close AskName form
        }   //END (butCancel_Click)

        /// <summary>
        /// Player name menu index changed event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void cbName_SelectedIndexChanged(object sender, EventArgs e)
        {
            PlayerName = cbName.Text;   //save player name
        }   //END (cbName_SelectedIndexChanged)

        /// <summary>
        /// OK button click event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void butOK_Click(object sender, EventArgs e)
        {
            Close();    //close AskName form
        }   //END (butOK_Click)

        /// <summary>
        /// Player name updated event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void cbName_TextUpdate(object sender, EventArgs e)
        {
            PlayerName = cbName.Text;   //save player name
        }   //END (cbName_TextUpdate)
        #endregion
        #endregion
    }   //ENDCLASS (FormAskName)
}   //ENDNAMESPACE (MineSweeper)
