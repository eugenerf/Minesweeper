using System;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Soap;
using System.IO;

namespace MineSweeper
{
    /// <summary>
    /// Settings form. Holds controls used for changing game settings
    /// </summary>
    public partial class FormSettings : Form
    {
        #region Fields
        /// <summary>
        /// Parent form instance
        /// </summary>
        FormMineSweeper formParent;
        #endregion

        #region Methods
        #region Form
        /// <summary>
        /// Settings form ctor
        /// </summary>
        /// <param name="parent">Parent form instance</param>
        public FormSettings(FormMineSweeper parent)
        {
            InitializeComponent();
            formParent = parent;    //save parent form instance
        }   //END (ctor)

        /// <summary>
        /// Settings form load event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void FormSettings_Load(object sender, EventArgs e)
        {
            switch (formParent.MS.CurrentPreset)                            //switch between presets
            {
                case MinesSettings.Preset.Newbie:                               //current preset is Newbie
                    rbNewbie.Checked = true;                                        //check Newbie radiobutton
                    break;
                case MinesSettings.Preset.Advanced:                             //current preset is Advanced
                    rbAdvanced.Checked = true;                                       //check Advanced radiobutton
                    break;
                case MinesSettings.Preset.Professional:                         //current preset is Professional
                    rbProfessional.Checked = true;                                  //check Professional radiobutton
                    break;
                case MinesSettings.Preset.Custom:                               //current preset is Custom
                    rbCustom.Checked = true;                                        //check Custom radiobutton
                    break;
            }   //ENDSWITCH (presets)
            nudHeight.Value = formParent.MS.FieldHeight;                    //set Height box to the current mine field height
            nudWidth.Value = formParent.MS.FieldWidth;                      //set Width box to the current mine field width
            nudMines.Value = formParent.MS.NumMines;                        //set Mines box to the current mines number
            nudMines.Maximum =
                MinesSettings.GetMaxMines((int)nudWidth.Value,
                                        (int)nudHeight.Value);              //set maximal value of the Mines box to appropriate for current mine field
            cbUseQuestionMarks.Checked = formParent.MS.UseQuestionMarks;    //set use question marks check-box as in current settings
        }   //END (FormSettings_Load)

        /// <summary>
        /// Settings form closing event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void FormSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            try                                                 //trying to save settings to the file
            {
                SoapFormatter formatter = new SoapFormatter();  //use SOAP-serialization
                FileStream fs = 
                    new FileStream(                             //open filestream
                        "minesettings.soap",                        //filename
                        FileMode.Create);                           //create new file
                formatter.Serialize(fs, formParent.MS);         //serialize settings to the filestream
                fs.Flush();                                     //write data to the file
                fs.Close();                                     //close file
                fs = null;                                      //release filestream
            }   //ENDTRY (save settings)
            catch (UnauthorizedAccessException) { }             //catch unauthorised access exception and do nothing
            catch (IOException) { }                             //catch input/output exception and do nothing
            formParent.Enabled = true;                          //enable parent form
            formParent.InitialiseField();                       //initialise mine field with new game settings
        }   //END (FormSettings_FormClosing)
        #endregion

        #region Form controls events handlers
        /// <summary>
        /// Close button click event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void butClose_Click(object sender, EventArgs e)
        {
            if (rbNewbie.Checked)                               //if Newbie radiobutton is checked
            {
                formParent.MS = MinesSettings.setSettings(          //set new game settings
                    MinesSettings.Preset.Newbie,                        //preset: Newbie
                    cbUseQuestionMarks.Checked);                        //questions: equals checkbox state
            }   //ENDIF (newbie)
            else if (rbAdvanced.Checked)                         //if Advanced radiobutton is checked
            {
                formParent.MS = MinesSettings.setSettings(          //set new game settings
                    MinesSettings.Preset.Advanced,                      //preset: Advanced
                    cbUseQuestionMarks.Checked);                        //questions: equals checkbox state
            }   //END (advanced)
            else if (rbProfessional.Checked)                    //if Professional radiobutton is checked
            {
                formParent.MS = MinesSettings.setSettings(          //set new game settings
                    MinesSettings.Preset.Professional,                  //preset: Professional
                    cbUseQuestionMarks.Checked);                        //questions: equals checkbox state
            }   //END (professional)
            else if (rbCustom.Checked)                          //if Custom radiobutton is checked
            {
                formParent.MS = MinesSettings.setSettings(          //set new game settings
                    (int)nudWidth.Value,                                //width equals Width box
                    (int)nudHeight.Value,                               //height equals Height box
                    (uint)nudMines.Value,                               //mines equals Mines box
                    cbUseQuestionMarks.Checked);                        //questions: equals checkbox state
            }   //END (custom)
            else                                                //if somehow no radiobutton is checked
                formParent.MS = MinesSettings.setSettings();        //set default game settings
            Close();                                            //close Settings form
        }   //END (butClose_Click)

        /// <summary>
        /// Custom radiobutton checked changed event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void rbCustom_CheckedChanged(object sender, EventArgs e)
        {
            nudWidth.Enabled = rbCustom.Checked;    //set Width box's enabled state equal to Custom radiobutton checked state
            nudHeight.Enabled = rbCustom.Checked;   //set Height box's enabled state equal to Custom radiobutton checked state
            nudWidth.Enabled = rbCustom.Checked;    //set Mines box's enabled state equal to Custom radiobutton checked state
        }   //END (rbCustom_CheckedChanged)

        /// <summary>
        /// Height box value changed event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void nudHeight_ValueChanged(object sender, EventArgs e)
        {
            nudMines.Maximum = 
                MinesSettings.GetMaxMines((int)nudWidth.Value, (int)nudHeight.Value);   //set maximal value for the Mines box
        }   //END (nudHeight_ValueChanged)

        /// <summary>
        /// Width box value changed event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void nudWidth_ValueChanged(object sender, EventArgs e)
        {
            nudMines.Maximum = 
                MinesSettings.GetMaxMines((int)nudWidth.Value, (int)nudHeight.Value);   //set maximal value for the Mines box
        }   //END (nudWidth_ValueChanged)
        #endregion
        #endregion
    }   //ENDCLASS (FormSettings)
}   //ENDNAMESPACE (MineSweeper)
