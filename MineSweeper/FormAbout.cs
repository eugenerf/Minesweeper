using System;
using System.Windows.Forms;

namespace MineSweeper
{
    /// <summary>
    /// About form. Shows the about information
    /// </summary>
    public partial class FormAbout : Form
    {
        #region Fields
        /// <summary>
        /// Parent form instance (Main form)
        /// </summary>
        FormMineSweeper formParent;
        #endregion

        #region Methods
        /// <summary>
        /// About form ctor
        /// </summary>
        /// <param name="parent">Instance of the parent form</param>
        public FormAbout (FormMineSweeper parent)
        {
            InitializeComponent();
            formParent = parent;    //save parent form's instance
        }   //END (ctor)

        /// <summary>
        /// Form closing event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void FormAbout_FormClosing(object sender, FormClosingEventArgs e)
        {
            formParent.Enabled = true;  //enable parent form
        }   //END (FormAbout_FormClosing)

        /// <summary>
        /// Close button click event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void butClose_Click(object sender, EventArgs e)
        {
            Close();    //close the About form
        }   //END (butClose_Click)
        #endregion
    }   //ENDCLASS (FormAbout)
}   //ENDNAMESPACE (MineSweeper)
