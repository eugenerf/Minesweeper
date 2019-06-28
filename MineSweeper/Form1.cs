﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization;
using System.Threading;

namespace MineSweeper
{
    public partial class FormMineSweeper : Form
    {
        const int FBSize = 30;          //size of the buttons on the field

        public MinesSettings MS;        //minesweeper settings
        public MinesStatistics MStats;  //minesweeper statistics
        MinesEngine ME;                 //minesweeper engine
        Button[][] FB;                  //buttons array for the minefield
        Label[][] FL;                   //labels array for the minefield
        bool GameStart = false;         //true when game started
        uint GameSeconds = 0;           //duration of the current game in seconds
        MouseEventArgs LBDown = null,   //left mouse button is now pushed down
                        RBDown = null;  //right mouse button is now pushed down
        FormAbout formAbout = null;
        FormSettings formSettings = null;
        FormAskName formAskName = null;
        FormStatistics formStats = null;
#if DEBUG
        Form formDebug = null;
#endif

        public FormMineSweeper()
        {
            InitializeComponent();
        }

        private void FormMineSweeper_Load(object sender, EventArgs e)
        {
            try
            {
                FileStream fs = new FileStream("minesettings.soap", FileMode.Open);
                SoapFormatter formatter = new SoapFormatter();
                MS = (MinesSettings)formatter.Deserialize(fs);
                fs.Dispose();
                fs = null;
            }
            catch (FileNotFoundException)
            {
                MS = MinesSettings.setSettings();
            }
            catch (SerializationException)
            {
                MS = MinesSettings.setSettings();
            }

            try
            {
                FileStream fs = new FileStream("minestats.soap", FileMode.Open);
                SoapFormatter formatter = new SoapFormatter();
                MStats = (MinesStatistics)formatter.Deserialize(fs);
                fs.Dispose();
                fs = null;
            }
            catch (FileNotFoundException)
            {
                MStats = MinesStatistics.getInstance();
            }
            catch (SerializationException)
            {
                MStats = MinesStatistics.getInstance();
            }

            InitialiseField();
        }

        /// <summary>
        /// Initialises the new minefield and redraws the form for it
        /// </summary>
        public void InitialiseField()
        {
#if DEBUG
            if (formDebug != null) formDebug.Hide();
#endif

            int oldWidth = (FB == null) ? 0 : FB.Length;
            int oldHeight = (oldWidth == 0) ? 0 : ((FB[0] == null) ? 0 : FB[0].Length);
            ME = MinesEngine.getEngine(MS);
            Array.Resize(ref FB, MS.FieldWidth);
            Array.Resize(ref FL, MS.FieldWidth);

            GameStart = false;
            GameSeconds = 0;
            tTime.Stop();

            gbMineField.Controls.Clear();
            butNewGame.ImageIndex = 2;

            for (int i = 0; i < oldWidth && i < MS.FieldWidth; i++)
            {
                Array.Resize(ref FB[i], MS.FieldHeight);
                Array.Resize(ref FL[i], MS.FieldHeight);
                for (int j = 0; j < oldHeight && j < MS.FieldHeight; j++)
                {
                    FB[i][j].BackColor = Color.DodgerBlue;
                    FB[i][j].ImageIndex = -1;
                    FB[i][j].Visible = true;

                    FL[i][j].BackColor = Color.PowderBlue;
                    FL[i][j].ImageIndex = -1;
                    FL[i][j].Text = "";
                    FL[i][j].Visible = false;

                    gbMineField.Controls.Add(FB[i][j]);
                    gbMineField.Controls.Add(FL[i][j]);
                }
                for (int j = oldHeight; j < MS.FieldHeight; j++)
                {
                    FB[i][j] = new Button();
                    FB[i][j].Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    FB[i][j].AutoSize = false;
                    FB[i][j].Enabled = true;
                    FB[i][j].Location = new Point(FBSize * i + 10, FBSize * j + 10);
                    FB[i][j].Size = new Size(FBSize, FBSize);
                    FB[i][j].TabStop = false;
                    FB[i][j].ImageList = ilIconsField;
                    FB[i][j].MouseDown += Cell_Down;
                    FB[i][j].MouseUp += Cell_Up;
                    FB[i][j].FlatStyle = FlatStyle.Flat;
                    FB[i][j].BackColor = Color.DodgerBlue;
                    FB[i][j].ImageIndex = -1;
                    FB[i][j].Visible = true;

                    FL[i][j] = new Label();
                    FL[i][j].Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    FL[i][j].AutoSize = false;
                    FL[i][j].BorderStyle = BorderStyle.FixedSingle;
                    FL[i][j].Enabled = true;
                    FL[i][j].Font = new Font("Candara", 14, FontStyle.Bold);
                    FL[i][j].ImageAlign = ContentAlignment.MiddleCenter;
                    FL[i][j].Location = new Point(FBSize * i + 10, FBSize * j + 10);
                    FL[i][j].Size = new Size(FBSize, FBSize);
                    FL[i][j].TextAlign = ContentAlignment.MiddleCenter;
                    FL[i][j].ImageList = ilIconsField;
                    FL[i][j].MouseDown += Cell_Down;
                    FL[i][j].MouseUp += Cell_Up;
                    FL[i][j].DoubleClick += MineFieldLabel_DoubleClick;
                    FL[i][j].BackColor = Color.PowderBlue;
                    FL[i][j].ImageIndex = -1;
                    FL[i][j].Text = "";
                    FL[i][j].Visible = false;

                    gbMineField.Controls.Add(FB[i][j]);
                    gbMineField.Controls.Add(FL[i][j]);
                }
            }

            for (int i = oldWidth; i < MS.FieldWidth; i++)
            {
                Array.Resize(ref FB[i], MS.FieldHeight);
                Array.Resize(ref FL[i], MS.FieldHeight);
                for (int j = 0; j < MS.FieldHeight; j++)
                {
                    FB[i][j] = new Button();
                    FB[i][j].Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    FB[i][j].AutoSize = false;
                    FB[i][j].Enabled = true;
                    FB[i][j].Location = new Point(FBSize * i + 10, FBSize * j + 10);
                    FB[i][j].Size = new Size(FBSize, FBSize);
                    FB[i][j].TabStop = false;
                    FB[i][j].ImageList = ilIconsField;
                    FB[i][j].MouseDown += Cell_Down;
                    FB[i][j].MouseUp += Cell_Up;
                    FB[i][j].FlatStyle = FlatStyle.Flat;
                    FB[i][j].BackColor = Color.DodgerBlue;
                    FB[i][j].ImageIndex = -1;
                    FB[i][j].Visible = true;

                    FL[i][j] = new Label();
                    FL[i][j].Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    FL[i][j].AutoSize = false;
                    FL[i][j].BorderStyle = BorderStyle.FixedSingle;
                    FL[i][j].Enabled = true;
                    FL[i][j].Font = new Font("Candara", 14, FontStyle.Bold);
                    FL[i][j].ImageAlign = ContentAlignment.MiddleCenter;
                    FL[i][j].Location = new Point(FBSize * i + 10, FBSize * j + 10);
                    FL[i][j].Size = new Size(FBSize, FBSize);
                    FL[i][j].TextAlign = ContentAlignment.MiddleCenter;
                    FL[i][j].ImageList = ilIconsField;
                    FL[i][j].MouseDown += Cell_Down;
                    FL[i][j].MouseUp += Cell_Up;
                    FL[i][j].DoubleClick += MineFieldLabel_DoubleClick;
                    FL[i][j].BackColor = Color.PowderBlue;
                    FL[i][j].ImageIndex = -1;
                    FL[i][j].Text = "";
                    FL[i][j].Visible = false;

                    gbMineField.Controls.Add(FB[i][j]);
                    gbMineField.Controls.Add(FL[i][j]);
                }
            }

            Size = new Size(MS.FieldWidth * FBSize + 20 + 25, MS.FieldHeight * FBSize + 20 + 30 + 30 + 45);

            gbMineField.Location = new Point(5, 30 + 30);
            gbMineField.Size = new Size(MS.FieldWidth * FBSize + 20, MS.FieldHeight * FBSize + 20);

            lMineIco.Location = new Point(5, 30);
            lMines.Location = new Point(40, 30);

            lTime.Location = new Point(MS.FieldWidth * FBSize + 20 + 5 - 40, 30);
            lTimeIco.Location = new Point(MS.FieldWidth * FBSize + 20 + 5 - 40 - 5 - 30, 30);

            butNewGame.Location = new Point(((MS.FieldWidth * FBSize + 20 + 10) - 30) / 2, 30);

            lMines.Text = MS.NumMines.ToString();
            lTime.Text = "";

            pbHeartBig.Visible = false;
            pbHeartBigger.Visible = false;
            gbMineField.Controls.Add(pbHeartBig);
            gbMineField.Controls.Add(pbHeartBigger);

#if DEBUG
            DebugMineField();
#endif
        }

        /// <summary>
        /// Game timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tTime_Tick(object sender, EventArgs e)
        {
            GameSeconds++;
            lTime.Text = GameSeconds.ToString();
        }

        /// <summary>
        /// Redraws opened cells of the minefield
        /// </summary>
        private void RedrawOpened()
        {
            for (int i = 0; i < ME.Width; i++)
            {
                for (int j = 0; j < ME.Height; j++)
                {
                    if (ME[i, j].state && FB[i][j].Visible != false)
                    {
                        FB[i][j].Visible = false;
                        if (ME[i, j].cell == -1)
                        {
                            FL[i][j].ImageIndex = 0;
                        }
                        else if (ME[i, j].cell != 0)
                        {
                            switch (ME[i, j].cell)
                            {
                                case 1:
                                    FL[i][j].ForeColor = Color.Blue;
                                    break;
                                case 2:
                                    FL[i][j].ForeColor = Color.Green;
                                    break;
                                case 3:
                                    FL[i][j].ForeColor = Color.Red;
                                    break;
                                case 4:
                                    FL[i][j].ForeColor = Color.DarkBlue;
                                    break;
                                case 5:
                                    FL[i][j].ForeColor = Color.DarkRed;
                                    break;
                                case 6:
                                    FL[i][j].ForeColor = Color.CornflowerBlue;
                                    break;
                                case 7:
                                    FL[i][j].ForeColor = Color.Black;
                                    break;
                                case 8:
                                    FL[i][j].ForeColor = Color.SlateGray;
                                    break;
                            }
                            FL[i][j].Text = ME[i, j].cell.ToString();
                        }
                        FL[i][j].Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// New game button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butNewGame_Click(object sender, EventArgs e)
        {
            InitialiseField();
        }

        /// <summary>
        /// Gets the indices of the cell that corresponds to the clicked button on the minefield
        /// </summary>
        /// <param name="but">Clicked button</param>
        /// <param name="column">Output of the column number</param>
        /// <param name="row">Output of the row number</param>
        /// <returns>True if indices were successfully found</returns>
        private bool GetCellIndex(Button but, out int column, out int row)
        {
            for (int i = 0; i < FB.Length; i++)
            {
                for (int j = 0; j < FB[i].Length; j++)
                {
                    if (but.Equals(FB[i][j]))
                    {
                        column = i;
                        row = j;
                        return true;
                    }
                }
            }
            row = -1;
            column = -1;
            return false;
        }

        /// <summary>
        /// Gets the indices of the cell that corresponds to the clicked label on the minefield
        /// </summary>
        /// <param name="lab">Clicked label</param>
        /// <param name="column">Output of the column number</param>
        /// <param name="row">Output of the row number</param>
        /// <returns>True if indices were successfully found</returns>
        private bool GetCellIndex(Label lab, out int column, out int row)
        {
            for (int i = 0; i < FL.Length; i++)
            {
                for (int j = 0; j < FL[i].Length; j++)
                {
                    if (lab.Equals(FL[i][j]))
                    {
                        column = i;
                        row = j;
                        return true;
                    }
                }
            }
            row = -1;
            column = -1;
            return false;
        }

        /// <summary>
        /// Mouse button goes down on the minefield cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cell_Down(object sender, MouseEventArgs e)
        {
            int i = 0, j = 0;
            if (sender.GetType().ToString().IndexOf("Button") != -1)
            {
                if (!GetCellIndex((Button)sender, out i, out j)) return;
            }
            else if (sender.GetType().ToString().IndexOf("Label") != -1)
            {
                if (!GetCellIndex((Label)sender, out i, out j)) return;
            }
            else return;

            if (e.Button == MouseButtons.Left)
                LBDown = new MouseEventArgs(MouseButtons.Left, 1, i, j, 0);
            if (e.Button == MouseButtons.Right)
                RBDown = new MouseEventArgs(MouseButtons.Right, 1, i, j, 0);

            if (SameButton(LBDown, RBDown))
            {
                for (int dI = -1; dI <= 1; dI++)
                {
                    for (int dJ = -1; dJ <= 1; dJ++)
                    {
                        if (i + dI < 0 || i + dI >= ME.Width) continue;
                        if (j + dJ < 0 || j + dJ >= ME.Height) continue;
                        FB[i + dI][j + dJ].BackColor = Color.PowderBlue;
                    }
                }
            }
        }

        /// <summary>
        /// Checks whether specified mouse events correspond to the same minefield-cell-button
        /// </summary>
        /// <param name="left">Left mouse button event</param>
        /// <param name="right">Right mouse button event</param>
        /// <returns>True if button is the same</returns>
        private bool SameButton(MouseEventArgs left, MouseEventArgs right)
        {
            if (left != null &&
                right != null &&
                left?.X == right?.X &&
                left?.Y == right?.Y)

                return true;
            return false;
        }

        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            formAbout = new FormAbout(this);
            Enabled = false;

            formAbout.Location = new Point(Location.X + Width / 2 - formAbout.Size.Width / 2,
                                            Location.Y + Height / 2 - formAbout.Size.Height / 2);

            formAbout.ShowDialog();
        }

        private void tsmiNewGame_Click(object sender, EventArgs e)
        {
            InitialiseField();
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tsmiParameters_Click(object sender, EventArgs e)
        {
            formSettings = new FormSettings(this);
            Enabled = false;

            formSettings.Location = new Point(Location.X + Width / 2 - formSettings.Width / 2,
                                            Location.Y + Height / 2 - formSettings.Height / 2);
            formSettings.ShowDialog();
        }

        /// <summary>
        /// Mouse button goes down on the minefield cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cell_Up(object sender, MouseEventArgs e)
        {
            int i = 0, j = 0;
            if (sender.GetType().ToString().IndexOf("Button") != -1)
            {
                if (!GetCellIndex((Button)sender, out i, out j)) return;
            }
            else if (sender.GetType().ToString().IndexOf("Label") != -1)
            {
                if (!GetCellIndex((Label)sender, out i, out j)) return;
            }
            else return;

            MouseEventArgs curButton = new MouseEventArgs(e.Button, e.Clicks, i, j, e.Delta);

            if (!GameStart)                                                         //game not started yet
            {
                if (SameButton(LBDown, curButton) || SameButton(curButton, RBDown)) //up on the same button as was down
                {
                    GameStart = true;
                    GameSeconds = 0;
                    tTime.Start();
                }
            }

            if (SameButton(LBDown, RBDown))                                          //left+right button upped
            {
                for (int dI = -1; dI <= 1; dI++)
                {
                    for (int dJ = -1; dJ <= 1; dJ++)
                    {
                        if (i + dI < 0 || i + dI >= ME.Width) continue;
                        if (j + dJ < 0 || j + dJ >= ME.Height) continue;
                        FB[i + dI][j + dJ].BackColor = Color.DodgerBlue;
                    }
                }

                MinesEngine.CurrentGameStateInfo GS = ME.OpenMany(i, j);
                RedrawOpened();

                switch (GS.State)
                {
                    case MinesEngine.GameState.InProgress:
                        break;
                    case MinesEngine.GameState.Loose:
                        tTime.Stop();
                        butNewGame.ImageIndex = 3;
                        MStats.CalcStats(ME, MS, GameSeconds);
                        for (int k = 0; k < GS.NumMinesBombed; k++)
                            FL[GS.BombedMines[k].Column][GS.BombedMines[k].Row].BackColor = Color.Red;
                        for (int k = 0; k < GS.NumWrongFlags; k++)
                        {
                            FL[GS.WrongFlags[k].Column][GS.WrongFlags[k].Row].BackColor = Color.Red;
                            FL[GS.WrongFlags[k].Column][GS.WrongFlags[k].Row].Text = "";
                            FL[GS.WrongFlags[k].Column][GS.WrongFlags[k].Row].ImageIndex = 1;
                        }
                        break;
                    case MinesEngine.GameState.Win:
                        tTime.Stop();
                        butNewGame.ImageIndex = 4;
                        MStats.CalcStats(ME, MS, GameSeconds, this);
                        break;
                    case MinesEngine.GameState.Stopped:
                        break;
                }
            }
            else if (SameButton(LBDown, curButton))                                  //same left button upped as was down
            {
                MinesEngine.CurrentGameStateInfo GS = ME.OpenCell(i, j);
                RedrawOpened();

                switch (GS.State)
                {
                    case MinesEngine.GameState.InProgress:
                        break;
                    case MinesEngine.GameState.Loose:
                        tTime.Stop();
                        butNewGame.ImageIndex = 3;
                        MStats.CalcStats(ME, MS, GameSeconds);
                        for (int k = 0; k < GS.NumMinesBombed; k++)
                            FL[GS.BombedMines[k].Column][GS.BombedMines[k].Row].BackColor = Color.Red;
                        break;
                    case MinesEngine.GameState.Win:
                        tTime.Stop();
                        butNewGame.ImageIndex = 4;
                        MStats.CalcStats(ME, MS, GameSeconds, this);
                        break;
                    case MinesEngine.GameState.Stopped:
                        break;
                }
            }
            else if (SameButton(curButton, RBDown))                                  //same right button upped as was down
            {
                if (sender.GetType().ToString().IndexOf("Button") != -1)
                {
                    ME.ChangeMarker(i, j);
                    if (ME[i, j].marker > 0) FB[i][j].ImageIndex = ME[i, j].marker;
                    else FB[i][j].ImageIndex = -1;
                    lMines.Text = ((int)ME.NumMines - (int)ME.NumFlags).ToString();
                }
            }
            LBDown = null;
            RBDown = null;
        }

        private void FormMineSweeper_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                SoapFormatter formatter = new SoapFormatter();
                FileStream fs = new FileStream("minestats.soap", FileMode.Create);
                formatter.Serialize(fs, MStats);
                fs.Flush();
                fs = null;
            }
            catch (UnauthorizedAccessException)
            {

            }
        }

        public string AskForUserName()
        {
            formAskName = new FormAskName(this);
            Enabled = false;

            formAskName.Location = new Point(Location.X + Width / 2 - formAskName.Width / 2,
                                            Location.Y + Height / 2 - formAskName.Height / 2);
            formAskName.ShowDialog();

            if (formAskName.PlayerName.ToLower() == "zaza") //second easter egg for Zaza
            {
                Random rnd = new Random();
                int rndMax = (int)Math.Floor(300 / (decimal)ME.NumMines);
                if (rnd.Next(rndMax) == 0) DrawHeart();
            }

            return formAskName.PlayerName;
        }

        private void tsmiStats_Click(object sender, EventArgs e)
        {
            formStats = new FormStatistics(this);
            Enabled = false;

            formStats.Location = new Point(Location.X + Width / 2 - formStats.Size.Width / 2,
                                            Location.Y + Height / 2 - formStats.Size.Height / 2);

            formStats.ShowDialog();
        }

        private void FormMineSweeper_Deactivate(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized) tTime.Stop();
        }

        private void FormMineSweeper_Activated(object sender, EventArgs e)
        {
            if (GameStart) tTime.Start();
        }

        /// <summary>
        /// Easter egg for Zaza (draws the beating heart on the field and then hides it)
        /// </summary>
        private void DrawHeart()
        {
            bool[][] fb = null;
            bool[][] fl = null;

            Array.Resize(ref fb, FB.Length);
            Array.Resize(ref fl, FL.Length);
            for (int i = 0; i < FB.Length; i++)
            {
                Array.Resize(ref fb[i], FB[i].Length);
                Array.Resize(ref fl[i], FL[i].Length);
                for (int j = 0; j < FB[i].Length; j++)
                {
                    fb[i][j] = FB[i][j].Visible;
                    fl[i][j] = FL[i][j].Visible;

                    FB[i][j].Visible = false;
                    FL[i][j].Visible = false;
                }
            }

            pbHeartBigger.Visible = false;
            pbHeartBig.Visible = true;
            gbMineField.Refresh();

            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(500);

                pbHeartBig.Visible = false;
                pbHeartBigger.Visible = true;
                gbMineField.Refresh();

                Thread.Sleep(700);

                pbHeartBigger.Visible = false;
                pbHeartBig.Visible = true;
                gbMineField.Refresh();
            }

            Thread.Sleep(1000);

            pbHeartBig.Visible = false;
            pbHeartBigger.Visible = false;

            for(int i = 0; i < FB.Length; i++)
            {
                for(int j = 0; j < FB[i].Length; j++)
                {
                    FB[i][j].Visible = fb[i][j];
                    FL[i][j].Visible = fl[i][j];
                }
            }
        }

        private void MineFieldLabel_DoubleClick(object sender, EventArgs e)
        {
            int i = 0, j = 0;
            if (sender.GetType().ToString().IndexOf("Label") != -1)
            {
                if (!GetCellIndex((Label)sender, out i, out j)) return;
            }
            else return;

            for (int dI = -1; dI <= 1; dI++)
            {
                for (int dJ = -1; dJ <= 1; dJ++)
                {
                    if (i + dI < 0 || i + dI >= ME.Width) continue;
                    if (j + dJ < 0 || j + dJ >= ME.Height) continue;
                    FB[i + dI][j + dJ].BackColor = Color.DodgerBlue;
                }
            }

            MinesEngine.CurrentGameStateInfo GS = ME.OpenMany(i, j);
            RedrawOpened();

            switch (GS.State)
            {
                case MinesEngine.GameState.InProgress:
                    break;
                case MinesEngine.GameState.Loose:
                    tTime.Stop();
                    butNewGame.ImageIndex = 3;
                    MStats.CalcStats(ME, MS, GameSeconds);
                    for (int k = 0; k < GS.NumMinesBombed; k++)
                        FL[GS.BombedMines[k].Column][GS.BombedMines[k].Row].BackColor = Color.Red;
                    for (int k = 0; k < GS.NumWrongFlags; k++)
                    {
                        FL[GS.WrongFlags[k].Column][GS.WrongFlags[k].Row].BackColor = Color.Red;
                        FL[GS.WrongFlags[k].Column][GS.WrongFlags[k].Row].Text = "";
                        FL[GS.WrongFlags[k].Column][GS.WrongFlags[k].Row].ImageIndex = 1;
                    }
                    break;
                case MinesEngine.GameState.Win:
                    tTime.Stop();
                    butNewGame.ImageIndex = 4;
                    MStats.CalcStats(ME, MS, GameSeconds, this);
                    break;
                case MinesEngine.GameState.Stopped:
                    break;
            }
        }

#if DEBUG
        private void DebugMineField()
        {
            if (formDebug == null) formDebug = new Form();
            
            formDebug.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            formDebug.BackColor = SystemColors.Control;
            formDebug.FormBorderStyle = FormBorderStyle.FixedSingle;
            formDebug.MaximizeBox = false;
            formDebug.MinimizeBox = true;
            formDebug.ShowIcon = false;
            formDebug.ShowInTaskbar = false;
            formDebug.Size = new Size(Width, Height);
            formDebug.StartPosition = FormStartPosition.CenterScreen;
            formDebug.Text = "Minesweeper DEBUG";
            formDebug.FormClosing += (object s, FormClosingEventArgs e) => formDebug = null;
            formDebug.Controls.Clear();

            GroupBox gbField = new GroupBox();
            gbField.Anchor = AnchorStyles.None;
            gbField.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            gbField.BackColor = SystemColors.Control;
            gbField.Location = new Point(gbMineField.Location.X, gbMineField.Location.Y);
            gbField.Size = new Size(gbMineField.Size.Width, gbMineField.Size.Height);

            Label[][] labels = null;
            Array.Resize(ref labels, FL.Length);
            for(int i = 0; i < FL.Length; i++)
            {
                Array.Resize(ref labels[i], FL[i].Length);

                for (int j = 0; j < labels[i].Length; j++)
                {
                    labels[i][j] = new Label();
                    labels[i][j].Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    labels[i][j].AutoSize = false;
                    labels[i][j].BorderStyle = BorderStyle.FixedSingle;
                    labels[i][j].Enabled = true;
                    labels[i][j].Font = new Font("Candara", 14, FontStyle.Bold);
                    labels[i][j].ImageAlign = ContentAlignment.MiddleCenter;
                    labels[i][j].Location = new Point(FBSize * i + 10, FBSize * j + 10);
                    labels[i][j].Size = new Size(FBSize, FBSize);
                    labels[i][j].TextAlign = ContentAlignment.MiddleCenter;
                    labels[i][j].ImageList = ilIconsField;
                    labels[i][j].MouseDown += Cell_Down;
                    labels[i][j].MouseUp += Cell_Up;
                    labels[i][j].BackColor = Color.PowderBlue;
                    labels[i][j].ImageIndex = -1;
                    labels[i][j].Text = "";
                    labels[i][j].Visible = true;

                    if (ME[i, j].cell == -1)
                    {
                        labels[i][j].ImageIndex = 0;
                    }
                    else if (ME[i, j].cell != 0)
                    {
                        switch (ME[i, j].cell)
                        {
                            case 1:
                                labels[i][j].ForeColor = Color.Blue;
                                break;
                            case 2:
                                labels[i][j].ForeColor = Color.Green;
                                break;
                            case 3:
                                labels[i][j].ForeColor = Color.Red;
                                break;
                            case 4:
                                labels[i][j].ForeColor = Color.DarkBlue;
                                break;
                            case 5:
                                labels[i][j].ForeColor = Color.DarkRed;
                                break;
                            case 6:
                                labels[i][j].ForeColor = Color.CornflowerBlue;
                                break;
                            case 7:
                                labels[i][j].ForeColor = Color.Black;
                                break;
                            case 8:
                                labels[i][j].ForeColor = Color.SlateGray;
                                break;
                        }
                        labels[i][j].Text = ME[i, j].cell.ToString();
                    }
                }
                gbField.Controls.AddRange(labels[i]);
            }

            formDebug.Controls.Add(gbField);

            formDebug.Show();
        }

        private void FormDebug_FormClosing1(object sender, FormClosingEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void FormDebug_FormClosing(object sender, FormClosingEventArgs e)
        {
            throw new NotImplementedException();
        }
#endif
    }
}