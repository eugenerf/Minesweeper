using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization;
using System.Threading;
using System.Linq;

namespace MineSweeper
{
    public partial class FormMineSweeper : Form
    {
        readonly static int CellSize = 30;                      //size of the buttons on the field
        readonly static Color MistakeColor = Color.Red;         //color of the cell with a mistake
        readonly static Color ClosedColor = Color.DodgerBlue;   //color of not opened cells of the minefield
        readonly static Color OpenedColor = Color.PowderBlue;   //color of opened cells of the mine field
        readonly static Color[] NumberColors = new Color[9]     //colors of the numbers on the minefield
        {
            OpenedColor,            //0 (same as back color)
            Color.Blue,             //1
            Color.Green,            //2
            Color.Red,              //3
            Color.DarkBlue,         //4
            Color.DarkRed,          //5
            Color.CornflowerBlue,   //6
            Color.Black,            //7
            Color.SlateGray         //8
        };

        public MinesSettings MS = null;        //minesweeper settings
        public MinesStatistics MStats = null;  //minesweeper statistics
        MinesEngine ME;                 //minesweeper engine
        MineFieldLabel flPrototype = null;     //Prototype for creating the MineField labels
        MineFieldLabel[][] FL = null;          //labels array for the minefield
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
            
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            UpdateStyles();
            
            flPrototype = new MineFieldLabel(new Size(CellSize, CellSize),
                                            ilIconsField,
                                            ClosedColor,
                                            Cell_Down,
                                            Cell_Up,
                                            MineFieldLabel_DoubleClick);

            Array.Resize(ref FL, MinesSettings.MaxWidth);

            for(int i = 0; i < MinesSettings.MaxWidth; i++)
            {
                Array.Resize(ref FL[i], MinesSettings.MaxHeight);
                for(int j = 0; j < MinesSettings.MaxHeight; j++)
                {
                    FL[i][j] = flPrototype.GetNew(new Point(CellSize * i + 2, CellSize * j + 8));
                }
                gbMineField.Controls.AddRange(FL[i]);
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
            ME = MinesEngine.getEngine(MS);

            GameStart = false;
            GameSeconds = 0;
            tTime.Stop();

            butNewGame.ImageIndex = 2;

            gbMineField.SuspendLayout();
            for (int i = 0; i < MS.FieldWidth; i++)
                for (int j = 0; j < MS.FieldHeight; j++)
                    FL[i][j].ChangeLabel(ClosedColor, -1, "");
            gbMineField.ResumeLayout();

            Size = new Size(MS.FieldWidth * CellSize + 30, MS.FieldHeight * CellSize + 115);
            gbMineField.Size = new Size(MS.FieldWidth * CellSize + 2, MS.FieldHeight * CellSize + 8);
            lMines.Text = MS.NumMines.ToString();
            lTime.Text = "";

            pbHeartBig.Visible = false;
            pbHeartBig.Size = gbMineField.Size;
            pbHeartBigger.Visible = false;
            pbHeartBigger.Size = gbMineField.Size;

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
                    if (ME[i, j].state && FL[i][j].BackColor == ClosedColor)
                    {
                        FL[i][j].BackColor = OpenedColor;
                        if (ME[i, j].cell == -1)
                        {
                            FL[i][j].Text = "";
                            FL[i][j].ImageIndex = 0;
                        }
                        else
                        {
                            FL[i][j].ImageIndex = -1;
                            FL[i][j].ForeColor = NumberColors[ME[i, j].cell];
                            FL[i][j].Text = (ME[i, j].cell > 0) ? ME[i, j].cell.ToString() : "";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Open field when game is lost (shows only mines location and mistakes)
        /// </summary>
        /// <param name="GS">Current game state</param>
        private void OpenLoose(MinesEngine.CurrentGameStateInfo GS)
        {
            //show mines
            for (int i = 0; i < ME.Width; i++)
                for (int j = 0; j < ME.Height; j++)
                    if (ME[i, j].cell == -1) FL[i][j].ImageIndex = 0;


            //highlight mistakes
            for (int k = 0; k < GS.NumMinesBombed; k++)
                FL[GS.BombedMines[k].Column][GS.BombedMines[k].Row].BackColor = MistakeColor;
            for (int k = 0; k < GS.NumWrongFlags; k++)
            {
                FL[GS.WrongFlags[k].Column][GS.WrongFlags[k].Row].BackColor = MistakeColor;
                FL[GS.WrongFlags[k].Column][GS.WrongFlags[k].Row].ImageIndex = 1;
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
            if (ME.CurrentGameState.State != MinesEngine.GameState.NewGame &&
                ME.CurrentGameState.State != MinesEngine.GameState.InProgress)
                return;

            int i = 0, j = 0;
            if (!GetCellIndex((Label)sender, out i, out j)) return;

            if (e.Button == MouseButtons.Left)
                LBDown = new MouseEventArgs(MouseButtons.Left, 1, i, j, 0);
            if (e.Button == MouseButtons.Right)
                RBDown = new MouseEventArgs(MouseButtons.Right, 1, i, j, 0);

            if (SameButton(LBDown, RBDown))
            {
                for (int dI = -1; dI <= 1; dI++)
                {
                    if (i + dI < 0 || i + dI >= ME.Width) continue;
                    for (int dJ = -1; dJ <= 1; dJ++)
                    {
                        if (j + dJ < 0 || j + dJ >= ME.Height) continue;
                        FL[i + dI][j + dJ].BackColor = OpenedColor;
                    }
                }
            }
            else if (e.Button == MouseButtons.Left) FL[i][j].BackColor = OpenedColor;
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
        /// Mouse button goes up on the minefield cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cell_Up(object sender, MouseEventArgs e)
        {
            if (ME.CurrentGameState.State != MinesEngine.GameState.NewGame &&
                ME.CurrentGameState.State != MinesEngine.GameState.InProgress)
                return;

            int i = 0, j = 0;
            if (!GetCellIndex((Label)sender, out i, out j)) return;

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
                    if (i + dI < 0 || i + dI >= ME.Width) continue;
                    for (int dJ = -1; dJ <= 1; dJ++)
                    {
                        if (j + dJ < 0 || j + dJ >= ME.Height) continue;
                        FL[i + dI][j + dJ].BackColor = ClosedColor;
                    }
                }

                MinesEngine.CurrentGameStateInfo GS = ME.OpenMany(i, j);

                switch (GS.State)
                {
                    case MinesEngine.GameState.InProgress:
                        RedrawOpened();
                        break;
                    case MinesEngine.GameState.Loose:
                        RedrawOpened();
                        OpenLoose(GS);
                        tTime.Stop();
                        butNewGame.ImageIndex = 3;
                        MStats.CalcStats(ME, MS, GameSeconds);
                        break;
                    case MinesEngine.GameState.Win:
                        RedrawOpened();
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
                FL[i][j].BackColor = ClosedColor;

                MinesEngine.CurrentGameStateInfo GS = ME.OpenCell(i, j);

                switch (GS.State)
                {
                    case MinesEngine.GameState.InProgress:
                        RedrawOpened();
                        break;
                    case MinesEngine.GameState.Loose:
                        RedrawOpened();
                        OpenLoose(GS);
                        tTime.Stop();
                        butNewGame.ImageIndex = 3;
                        MStats.CalcStats(ME, MS, GameSeconds);
                        break;
                    case MinesEngine.GameState.Win:
                        RedrawOpened();
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
                ME.ChangeMarker(i, j);
                if (ME[i, j].marker > 0) FL[i][j].ImageIndex = ME[i, j].marker;
                else FL[i][j].ImageIndex = -1;
                lMines.Text = ((int)ME.NumMines - (int)ME.NumFlags).ToString();
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
            if (ME.CurrentGameState.State == MinesEngine.GameState.InProgress) tTime.Start();
        }

        /// <summary>
        /// Easter egg for Zaza (draws the beating heart on the field and then hides it)
        /// </summary>
        private void DrawHeart()
        {
            gbMineField.Visible = false;

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

            gbMineField.Visible = true;
        }

        private void MineFieldLabel_DoubleClick(object sender, EventArgs e)
        {
            if (ME.CurrentGameState.State != MinesEngine.GameState.NewGame &&
                ME.CurrentGameState.State != MinesEngine.GameState.InProgress)
                return;

            int i = 0, j = 0;
                if (!GetCellIndex((Label)sender, out i, out j)) return;

            MinesEngine.CurrentGameStateInfo GS = ME.OpenMany(i, j);

            switch (GS.State)
            {
                case MinesEngine.GameState.InProgress:
                    RedrawOpened();
                    break;
                case MinesEngine.GameState.Loose:
                    RedrawOpened();
                    OpenLoose(GS);
                    tTime.Stop();
                    butNewGame.ImageIndex = 3;
                    MStats.CalcStats(ME, MS, GameSeconds);
                    break;
                case MinesEngine.GameState.Win:
                    RedrawOpened();
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

            MineFieldLabel[][] labels = null;
            Array.Resize(ref labels, MS.FieldWidth);
            for(int i = 0; i < MS.FieldWidth; i++)
            {
                Array.Resize(ref labels[i], MS.FieldHeight);

                for (int j = 0; j < MS.FieldHeight; j++)
                {
                    labels[i][j] = flPrototype.GetNew(new Point(CellSize * i + 2, CellSize * j + 8));
                    labels[i][j].BackColor = OpenedColor;

                    if (ME[i, j].cell == -1)
                    {
                        labels[i][j].ImageIndex = 0;
                    }
                    else
                    {
                        labels[i][j].ForeColor = NumberColors[ME[i, j].cell];
                        labels[i][j].Text = ME[i, j].cell.ToString();
                    }
                }
                gbField.Controls.AddRange(labels[i]);
            }

            formDebug.Controls.Add(gbField);

            formDebug.Show();
        }
#endif
    }
}