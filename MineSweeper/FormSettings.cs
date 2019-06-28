using System;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Soap;
using System.IO;

namespace MineSweeper
{
    public partial class FormSettings : Form
    {
        FormMineSweeper formParent;

        public FormSettings()
        {
            InitializeComponent();
        }

        public FormSettings(FormMineSweeper parent)
        {
            InitializeComponent();
            formParent = parent;
        }

        private void FormSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                SoapFormatter formatter = new SoapFormatter();
                FileStream fs = new FileStream("minesettings.soap", FileMode.Create);
                formatter.Serialize(fs, formParent.MS);
                fs.Flush();
                fs = null;
            }
            catch(UnauthorizedAccessException)
            {

            }

            formParent.Enabled = true;
            formParent.InitialiseField();
        }

        private void butClose_Click(object sender, EventArgs e)
        {
            if (rbNewbie.Checked)
            {
                formParent.MS = MinesSettings.setSettings(MinesSettings.Preset.Newbie, cbUseQuestionMarks.Checked);
            }
            else if (rbAmateur.Checked)
            {
                formParent.MS = MinesSettings.setSettings(MinesSettings.Preset.Advanced, cbUseQuestionMarks.Checked);
            }
            else if (rbProfessional.Checked)
            {
                formParent.MS = MinesSettings.setSettings(MinesSettings.Preset.Professional, cbUseQuestionMarks.Checked);
            }
            else if (rbCustom.Checked)
            {
                formParent.MS = MinesSettings.setSettings((int)nudWidth.Value,
                                                        (int)nudHeight.Value, 
                                                        (uint)nudMines.Value, 
                                                        cbUseQuestionMarks.Checked);
            }
            else formParent.MS = MinesSettings.setSettings();

            Close();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            switch (formParent.MS.CurrentPreset)
            {
                case MinesSettings.Preset.Newbie:
                    rbNewbie.Checked = true;
                    break;
                case MinesSettings.Preset.Advanced:
                    rbAmateur.Checked = true;
                    break;
                case MinesSettings.Preset.Professional:
                    rbProfessional.Checked = true;
                    break;
                case MinesSettings.Preset.Custom:
                    rbCustom.Checked = true;
                    break;
            }
            nudHeight.Value = formParent.MS.FieldHeight;
            nudWidth.Value = formParent.MS.FieldWidth;
            nudMines.Value = formParent.MS.NumMines;
            nudMines.Maximum = MinesSettings.GetMaxMines((int)nudWidth.Value, (int)nudHeight.Value);
            cbUseQuestionMarks.Checked = formParent.MS.UseQuestionMarks;
        }

        private void rbCustom_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCustom.Checked)
            {
                nudWidth.Enabled = true;
                nudHeight.Enabled = true;
                nudMines.Enabled = true;
            }
            else
            {
                nudWidth.Enabled = false;
                nudHeight.Enabled = false;
                nudMines.Enabled = false;
            }
        }

        private void nudHeight_ValueChanged(object sender, EventArgs e)
        {
            nudMines.Maximum = MinesSettings.GetMaxMines((int)nudWidth.Value, (int)nudHeight.Value);
        }

        private void nudWidth_ValueChanged(object sender, EventArgs e)
        {
            nudMines.Maximum = MinesSettings.GetMaxMines((int)nudWidth.Value, (int)nudHeight.Value);
        }
    }
}
