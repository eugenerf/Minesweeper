using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MineSweeper
{
    public partial class FormAskName : Form
    {
        FormMineSweeper parentForm = null;
        public string PlayerName = "";

        public FormAskName()
        {
            InitializeComponent();
        }

        public FormAskName(FormMineSweeper parent)
        {
            InitializeComponent();

            parentForm = parent;
        }

        private void FormAskName_FormClosing(object sender, FormClosingEventArgs e)
        {
            parentForm.Enabled = true;
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            PlayerName = "";
            Close();
        }

        private void FormAskName_Load(object sender, EventArgs e)
        {
            cbName.Items.Clear();
            foreach(MinesStatistics.PresetStatsInfo psi in parentForm.MStats.StatsByPreset)
            {
                if (psi.Top != null)
                {
                    foreach (MinesStatistics.WinnerInfo wi in psi.Top)
                    {
                        if (wi.Name != null && wi.Name != "")
                            if (!cbName.Items.Contains(wi.Name))
                                cbName.Items.Add(wi.Name);
                    }
                }
            }
        }

        private void cbName_SelectedIndexChanged(object sender, EventArgs e)
        {
            PlayerName = cbName.Text;
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbName_TextUpdate(object sender, EventArgs e)
        {
            PlayerName = cbName.Text;
        }
    }
}
