using System;
using System.Windows.Forms;

namespace MineSweeper
{
    public partial class FormAbout : Form
    {
        FormMineSweeper formParent;

        public FormAbout()
        {
            InitializeComponent();
        }

        public FormAbout (FormMineSweeper parent)
        {
            InitializeComponent();
            formParent = parent;
        }

        private void FormAbout_FormClosing(object sender, FormClosingEventArgs e)
        {
            formParent.Enabled = true;
        }

        private void butClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
