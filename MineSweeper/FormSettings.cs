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
                FileStream fs = new FileStream(Path.GetTempPath() + "minesettings.soap", FileMode.Create);
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
            Close();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            switch (formParent.MS.CurrentPreset)
            {
                case MinesSettings.Preset.Newbie:
                    rbNewbie.Checked = true;
                    break;
                case MinesSettings.Preset.Amateur:
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
            nudMines.Maximum = (nudHeight.Value * nudWidth.Value > 668) ? 668 : nudHeight.Value * nudWidth.Value;
        }

        private void rbNewbie_CheckedChanged(object sender, EventArgs e)
        {
            if (rbNewbie.Checked)
            {
                formParent.MS = new MinesSettings(MinesSettings.Preset.Newbie);
            }
        }

        private void rbAmateur_CheckedChanged(object sender, EventArgs e)
        {
            if(rbAmateur.Checked)
            {
                formParent.MS = new MinesSettings(MinesSettings.Preset.Amateur);
            }
        }

        private void rbProfessional_CheckedChanged(object sender, EventArgs e)
        {
            if (rbProfessional.Checked)
            {
                formParent.MS = new MinesSettings(MinesSettings.Preset.Professional);
            }
        }

        private void rbCustom_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCustom.Checked)
            {
                formParent.MS = new MinesSettings((int)nudWidth.Value, (int)nudHeight.Value, (uint)nudMines.Value);
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
            nudMines.Maximum = (nudHeight.Value * nudWidth.Value > 668) ? 668 : nudHeight.Value * nudWidth.Value;
            formParent.MS = new MinesSettings((int)nudWidth.Value, (int)nudHeight.Value, (uint)nudMines.Value);
        }

        private void nudWidth_ValueChanged(object sender, EventArgs e)
        {
            nudMines.Maximum = (nudHeight.Value * nudWidth.Value > 668) ? 668 : nudHeight.Value * nudWidth.Value;
            formParent.MS = new MinesSettings((int)nudWidth.Value, (int)nudHeight.Value, (uint)nudMines.Value);
        }

        private void nudMines_ValueChanged(object sender, EventArgs e)
        {
            formParent.MS = new MinesSettings((int)nudWidth.Value, (int)nudHeight.Value, (uint)nudMines.Value);
        }
    }
}
