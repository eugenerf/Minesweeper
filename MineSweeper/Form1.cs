using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization;
using System.Threading;

namespace MineSweeper
{
    /// <summary>
    /// Main game form. Holds minefield and game controls plus controls to call other forms
    /// </summary>
    public partial class FormMineSweeper : Form
    {
        #region Fields
        #region Visual settings of the mine field
        /// <summary>
        /// Size of the buttons on the field (they are square)
        /// </summary>
        readonly static int CellSize = 30;
        /// <summary>
        /// Color of the cell with a mistake
        /// </summary>
        readonly static Color MistakeColor = Color.Red;
        /// <summary>
        /// Color of not opened cells of the minefield
        /// </summary>
        readonly static Color ClosedColor = Color.DodgerBlue;
        /// <summary>
        /// Color of opened cells of the mine field
        /// </summary>
        readonly static Color OpenedColor = Color.PowderBlue;
        /// <summary>
        /// Colors of the numbers on the minefield (index in this array corresponds to the number itself)
        /// </summary>
        readonly static Color[] NumberColors = new Color[9]
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
        #endregion

        #region Minesweeper game related
        /// <summary>
        /// Minesweeper settings
        /// </summary>
        public MinesSettings MS = null;
        /// <summary>
        /// Minesweeper statistics
        /// </summary>
        public MinesStatistics MStats = null;
        /// <summary>
        /// Minesweeper engine
        /// </summary>
        MinesEngine ME;
        #endregion

        #region Minefield
        /// <summary>
        /// Prototype for creating the MineField labels
        /// </summary>
        MineFieldLabel flPrototype = null;
        /// <summary>
        /// Labels array for the minefield
        /// </summary>
        MineFieldLabel[][] FL = null;
        #endregion

        #region Child-forms
        /// <summary>
        /// Instance of the About form
        /// </summary>
        FormAbout formAbout = null;
        /// <summary>
        /// Instance of the Settings form
        /// </summary>
        FormSettings formSettings = null;
        /// <summary>
        /// Instance of the Ask for player name form
        /// </summary>
        FormAskName formAskName = null;
        /// <summary>
        /// Instance for the Stats form
        /// </summary>
        FormStatistics formStats = null;
#if DEBUG   //debug section start
        /// <summary>
        /// Instance of the Debug form
        /// </summary>
        Form formDebug = null;
#endif      //debug section end
        #endregion

        #region Other fields
        /// <summary>
        /// True when game started
        /// </summary>
        bool GameStart = false;
        /// <summary>
        /// Duration of the current game in seconds
        /// </summary>
        uint GameSeconds = 0;
        /// <summary>
        /// Left mouse button is now pushed down
        /// </summary>
        MouseEventArgs LBDown = null;
        /// <summary>
        /// Right mouse button is now pushed down
        /// </summary>
        MouseEventArgs RBDown = null;
        #endregion
        #endregion

        #region Methods
        #region Form
        /// <summary>
        /// Main form ctor
        /// </summary>
        public FormMineSweeper()
        {
            InitializeComponent();
        }   //END (ctor)

        /// <summary>
        /// Main form load handler
        /// </summary>
        /// <param name="sender">Sender of the form load event</param>
        /// <param name="e">Form load event args</param>
        private void FormMineSweeper_Load(object sender, EventArgs e)
        {
            //loading game settings
            try                                     //trying to load settings from the file
            {
                FileStream fs = new FileStream("minesettings.soap", FileMode.Open);
                //using SOAP-serialization
                SoapFormatter formatter = new SoapFormatter();
                MS = (MinesSettings)formatter.Deserialize(fs);
                //disposing the file-stream
                fs.Dispose();
                fs = null;
            }   //ENDTRY
            catch (FileNotFoundException)           //if game-settings file wasn't found
            {
                MS = MinesSettings.setSettings();   //loading default settings
            }   //ENDCATCH (File not found)
            catch (SerializationException)          //if something went wrong with serialization
            {
                MS = MinesSettings.setSettings();   //loading default settings
            }   //ENDCATCH (Serialization)

            //loading game stats
            try                                         //trying to load stats from the file
            {
                FileStream fs = new FileStream("minestats.soap", FileMode.Open);
                //using SOAP-serialization
                SoapFormatter formatter = new SoapFormatter();
                MStats = (MinesStatistics)formatter.Deserialize(fs);
                //disposing the file-stream
                fs.Dispose();
                fs = null;
            }   //ENDTRY
            catch (FileNotFoundException)               //if game-stats file wasn't found
            {
                MStats = MinesStatistics.getInstance(); //create empty stats
            }   //ENDCATCH (File not found
            catch (SerializationException)              //if something went wrong with serialization
            {
                MStats = MinesStatistics.getInstance(); //create empty stats
            }   //ENDCATCH (Serialization)
            
            //setting styles for the form controls
            SetStyle(ControlStyles.UserPaint, true);                //controls must be drawn by themselves (not by OS)
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);     //controls must ignore WM_ERASEBKGND message (reduces flicker)
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);    //controls must be pre-drawn in buffer (reduces flicker)
            SetStyle(ControlStyles.ResizeRedraw, true);             //controls must be re-drawn when their size changes
            UpdateStyles();                                         //force apply the new styles

            //creating prototype mine-field label (will be used in cloning all the minefield cells)
            flPrototype = new MineFieldLabel(
                new Size(CellSize, CellSize),   //label size
                ilIconsField,                   //image list for label
                ClosedColor,                    //back color of the label
                Cell_Down,                      //mouse-down event handler
                Cell_Up,                        //mouse-up event handler
                MineFieldLabel_DoubleClick);    //double-click event handler

            //generating all the cells for the maximal-field-dimensions by using prototype label
            Array.Resize(ref FL, MinesSettings.MaxWidth);       //get memory for the field
            for(int i = 0; i < MinesSettings.MaxWidth; i++)     //moving through all the columns
            {
                Array.Resize(ref FL[i], MinesSettings.MaxHeight);   //get memory for the current field column
                for (int j = 0; j < MinesSettings.MaxHeight; j++)   //moving through all the cells in current column
                    FL[i][j] = flPrototype.GetNew(CellSize, i, j);      //create new label for the current cell
                gbMineField.Controls.AddRange(FL[i]);               //add current column of cells into the field
            }   //ENDFOR (columns)

            //initialising the mine field
            InitialiseField();
        }   //END (FormMineSweeper_Load)

        /// <summary>
        /// Main form closing event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void FormMineSweeper_FormClosing(object sender, FormClosingEventArgs e)
        {
            try                                                                     //try to save game stats
            {
                SoapFormatter formatter = new SoapFormatter();                          //using SOAP-serialization
                FileStream fs = new FileStream("minestats.soap", FileMode.Create);      //open stats file
                formatter.Serialize(fs, MStats);                                        //serialize stats to the file-stream
                fs.Flush();                                                             //write stream to the file
                fs.Close();                                                             //close file
                fs = null;                                                              //release the file
            }   //ENDTRY (stats save)
            catch (UnauthorizedAccessException) { }                                 //catch Unauthorised access and do nothing (we just couldn't save stats)
        }   //END (FormMineSweeper_FormClosing)

        /// <summary>
        /// Main form deactivated event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void FormMineSweeper_Deactivate(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)   //if main form is minimized
                tTime.Stop();                                   //stop game timer
        }   //END (FormMineSweeper_Deactivate)

        /// <summary>
        /// Main form activated event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void FormMineSweeper_Activated(object sender, EventArgs e)
        {
            if (ME.CurrentGameState.State == 
                MinesEngine.GameState.InProgress)   //if current game is in progress
                tTime.Start();                          //start game timer
        }   //END (FormMineSweeper_Activated)

#if DEBUG   //debug section start
        /// <summary>
        /// Shows additional form with fully opened mine field
        /// </summary>
        private void DebugMineField()
        {
            if (formDebug == null)                                                              //if Debug form is not created yet
                formDebug = new Form();                                                             //create it

            //setting the Debug form properties
            formDebug.AutoSizeMode = AutoSizeMode.GrowAndShrink;                                //form grows and shrinks according to its contents
                                                                                                //  (no manual resize)
            formDebug.BackColor = SystemColors.Control;                                         //back color as set in OS for controls
            formDebug.FormBorderStyle = FormBorderStyle.FixedSingle;                            //border is fixed single line
            formDebug.MaximizeBox = false;                                                      //no maximize box
            formDebug.MinimizeBox = true;                                                       //no minimize box
            formDebug.ShowIcon = false;                                                         //do not show icon in the form's header
            formDebug.ShowInTaskbar = false;                                                    //do not show in taskbar
            formDebug.Size = new Size(Width, Height);                                           //size same as of the main form
            formDebug.StartPosition = FormStartPosition.CenterScreen;                           //show form in the center of the screen
            formDebug.Text = "Minesweeper DEBUG";                                               //header text
            formDebug.FormClosing += (object s, FormClosingEventArgs e) => formDebug = null;    //form closing event handler (set form's instance to null)
            formDebug.Controls.Clear();                                                         //clear controls

            GroupBox gbField = new GroupBox();                                                  //create new group box for Debug form
            //setting GroupBox properties
            gbField.Anchor = AnchorStyles.None;                                                 //no anchoring
            gbField.AutoSizeMode = AutoSizeMode.GrowAndShrink;                                  //grows and shrinks according to its contents
            gbField.BackColor = SystemColors.Control;                                           //back color as set in OS for controls
            gbField.Location = new Point(gbMineField.Location.X, gbMineField.Location.Y);       //location is same as of the mine field in the main form
            gbField.Size = new Size(gbMineField.Size.Width, gbMineField.Size.Height);           //size is same as of the mine field in the main form

            //generating cells of the debug mine field
            MineFieldLabel[][] labels = null;                                                   //debug-mine-field's labels (cells)
            Array.Resize(ref labels, MS.FieldWidth);                                            //get memory for the debug mine field
            for (int i = 0; i < MS.FieldWidth; i++)                                             //moving through all the columns of the mine field
            {
                Array.Resize(ref labels[i], MS.FieldHeight);                                        //get memory for the current debug-mine-field's column
                for (int j = 0; j < MS.FieldHeight; j++)                                            //moving through the cells of the current column
                {
                    labels[i][j] = flPrototype.GetNew(CellSize, i, j);                                  //generate the new cell for current column
                    labels[i][j].BackColor = OpenedColor;                                               //set cell's back color to the opened-cell-color
                    if (ME[i, j].cell == -1)                                                            //if current cell has mine
                        labels[i][j].ImageIndex = 0;                                                        //show mine in it
                    else                                                                                //if current cell has NO mine
                    {
                        labels[i][j].ForeColor = NumberColors[ME[i, j].cell];                               //set its fore color (text color) 
                                                                                                            //  according to the number in it
                        labels[i][j].Text = ME[i, j].cell.ToString();                                       //write its number
                    }   //ENDELSE (NO mine)
                }   //ENDFOR (cells in column)
                gbField.Controls.AddRange(labels[i]);                                               //add current column to the debug mine field
            }   //ENDFOR (columns)
            formDebug.Controls.Add(gbField);                                                    //add debug mine field to the Debug form
            formDebug.Show();                                                                   //show Debug mine field
        }   //END (DebugMineField)
#endif
        #endregion

        #region Handlers for events of the form controls
        /// <summary>
        /// Game timer tick event handler
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void tTime_Tick(object sender, EventArgs e)
        {
            GameSeconds++;                          //increase duration of the game by one second
            lTime.Text = GameSeconds.ToString();    //show the new duration in the textfield
        }   //END (tTime_Tick)

        /// <summary>
        /// New game button click event handler
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void butNewGame_Click(object sender, EventArgs e)
        {
            InitialiseField();  //initialise the mine field
        }   //END (butNewGame_Click)

        /// <summary>
        /// Mouse button goes down upon the minefield cell (event handler)
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void Cell_Down(object sender, MouseEventArgs e)
        {
            if (ME.CurrentGameState.State != MinesEngine.GameState.NewGame &&   //if current game state NOT equals NewGame
                ME.CurrentGameState.State != MinesEngine.GameState.InProgress)  //AND current game state NOT equals InProgress
                                                                                //which means that current game is not in progress and is not starting
                return;                                                             //then just return (mouse events are handled only when game is not stopped)

            //if we're here then game is not stopped
            MineFieldLabel Cell = (MineFieldLabel)sender;   //cell-sender of the current event (clicked cell of the mine field)

            //save upon which cell acted current mouse button
            if (e.Button == MouseButtons.Left)  //if it was left button
                LBDown = new MouseEventArgs(        //we'll save arguments for the left button (will be used later)
                    MouseButtons.Left,                  //left button constant
                    1,                                  //number of clicks isn't used
                    Cell.Column,                        //as an X coordinate we will save clicked cell's column number
                    Cell.Row,                           //as an Y coordinate we will save clicked cell's row number
                    0);                                 //delta isn't used
            if (e.Button == MouseButtons.Right) //if it was right button
                RBDown = new MouseEventArgs(        //we'll save arguments for the right button (will be used later)
                    MouseButtons.Right,                 //right button constant
                    1,                                  //number of clicks isn't used
                    Cell.Column,                        //as an X coordinate we will save clicked cell's column number
                    Cell.Row,                           //as an Y coordinate we will save clicked cell's row number
                    0);                                 //delta isn't used

            //check whether both left and right buttons acted upon the same cell
            if (SameButton(LBDown, RBDown))                                     //if left and right buttons acted upon the same cell
            {
                                                                                    //we will change the current cell and all cells around the current one
                for (int dI = -1; dI <= 1; dI++)                                    //moving through the previous, current and next columns
                {
                    if (Cell.Column + dI < 0 ||
                        Cell.Column + dI >= ME.Width)                                   //if current dI gets us outside the mine field borders
                        continue;                                                           //we will skip current dI
                    for (int dJ = -1; dJ <= 1; dJ++)                                //moving thouugh the previous, current and next cells in the current column
                    {
                        if (Cell.Row + dJ < 0 ||
                            Cell.Row + dJ >= ME.Height)                                 //if current dJ gets us outside the mine field borders
                            continue;                                                       //we will skip current dJ
                        FL[Cell.Column + dI][Cell.Row + dJ].BackColor = OpenedColor;    //set current cell's back color to the opened-cell-color
                    }   //ENDFOR (cells in the current column)
                }   //ENDFOR (columns)
            }   //ENDIF (same cell)
            else if (e.Button == MouseButtons.Left)                             //if not the same cell and left button acted
                FL[Cell.Column][Cell.Row].BackColor = OpenedColor;                  //set back color of the current cell to the opened-cell-color
        }   //END (Cell_Down)

        /// <summary>
        /// Mouse button goes up upon the minefield cell (event handler)
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void Cell_Up(object sender, MouseEventArgs e)
        {
            if (ME.CurrentGameState.State != MinesEngine.GameState.NewGame &&   //if current game state is NOT NewGame
                ME.CurrentGameState.State != MinesEngine.GameState.InProgress)  //AND current game state is NOT InProgress
                                                                                //which means that game is stopped
                return;                                                             //just return (mine field doesn't work while game is stopped)

            //if we're here then game is NOT stopped
            MineFieldLabel Cell = (MineFieldLabel)sender;                       //get the cell upon mouse button acted
            MouseEventArgs curButton =                                          //create arguments for the current mouse button
                new MouseEventArgs(e.Button,                                        //which button acted (left or right)
                                    e.Clicks,                                       //number of clicks (not used)
                                    Cell.Column,                                    //current cell's column number
                                    Cell.Row,                                       //current cell's row number
                                    e.Delta);                                       //delta (not used)

            //check whether it is a new game
            if (!GameStart)                                                     //if game wasn't started by now (it was the first click on the mine field)
            {
                if (SameButton(LBDown, curButton) || 
                    SameButton(curButton, RBDown))                              //if button goes up on the same cell where it went down
                {
                    GameStart = true;                                               //start game
                    GameSeconds = 0;                                                //clear game time
                    tTime.Start();                                                  //start game timer
                }   //ENDIF (same cell)
            }   //ENDIF (GameStart)

            //check the combinations of the currently acted buttons
            if (SameButton(LBDown, RBDown))                                     //Left+Right buttons acted together
            {
                                                                                    //we will work with the current cell and all the cells around it
                for (int dI = -1; dI <= 1; dI++)                                    //moving through the previous, current and next columns
                {
                    if (Cell.Column + dI < 0 || 
                        Cell.Column + dI >= ME.Width)                                   //if current dI gets us outside the mine field borders
                        continue;                                                           //just skip current dI
                    for (int dJ = -1; dJ <= 1; dJ++)                                    //moving through the previous, current and next cells in current column
                    {
                        if (Cell.Row + dJ < 0 || 
                            Cell.Row + dJ >= ME.Height)                                     //if current dJ gets us outside the mine field borders
                            continue;                                                           //just skip current dJ
                        FL[Cell.Column + dI][Cell.Row + dJ].BackColor = ClosedColor;        //set current cell's color to the closed-cell-color
                    }   //ENDFOR (cells in column)
                }   //ENDFOR (columns)

                MinesEngine.CurrentGameStateInfo GS = 
                    ME.OpenMany(Cell.Column, Cell.Row);                             //open the clicked cell and cells around it

                switch (GS.State)                                                   //switch between the game states
                {
                    case MinesEngine.GameState.InProgress:                          //game's in progress
                        RedrawOpened();                                                 //redraw already opened cells
                        break;
                    case MinesEngine.GameState.Loose:                               //game's lost
                        tTime.Stop();                                                   //stop the game timer
                        RedrawOpened();                                                 //redraw already opened cells
                        OpenLoose(GS);                                                  //open field as for the lost game                        
                        butNewGame.ImageIndex = 3;                                      //set the NewGame button's image to the LOOSE
                        MStats.CalcStats(ME, MS, GameSeconds, AskForUserName);          //calculate stats
                        break;
                    case MinesEngine.GameState.Win:                                 //game's won
                        tTime.Stop();                                                   //stop the game timer
                        RedrawOpened();                                                 //redraw already opened cells
                        butNewGame.ImageIndex = 4;                                      //set the NewGame button's image to the WIN
                        MStats.CalcStats(ME, MS, GameSeconds, AskForUserName);          //calculate stats
                        break;
                    case MinesEngine.GameState.Stopped:                             //game's stopped
                        break;                                                          //do nothing
                }   //ENDSWITCH (game state)
            }   //ENDIF (left+right buttons)
            else if (SameButton(LBDown, curButton))                             //same left button upped as was down (left-click on the cell)
            {
                FL[Cell.Column][Cell.Row].BackColor = ClosedColor;                  //set current cell's back color to the closed-cell-color

                MinesEngine.CurrentGameStateInfo GS = 
                    ME.OpenCell(Cell.Column, Cell.Row);                             //open the clicked cell

                switch (GS.State)                                                   //switch between the game states
                {
                    case MinesEngine.GameState.InProgress:                              //game's in progress
                        RedrawOpened();                                                     //redraw already opened cells
                        break;
                    case MinesEngine.GameState.Loose:                                   //game's lost
                        tTime.Stop();                                                       //stop the game timer
                        RedrawOpened();                                                     //redraw already opened cells
                        OpenLoose(GS);                                                      //open field as for the lost game
                        butNewGame.ImageIndex = 3;                                          //set the NewGame button's image to the LOOSE
                        MStats.CalcStats(ME, MS, GameSeconds, AskForUserName);              //calculate stats
                        break;
                    case MinesEngine.GameState.Win:                                     //game's won
                        tTime.Stop();                                                       //stop the game timer
                        RedrawOpened();                                                     //redraw already opened cells
                        butNewGame.ImageIndex = 4;                                          //set the NewGame button's image to the WIN
                        MStats.CalcStats(ME, MS, GameSeconds, AskForUserName);              //calculate stats
                        break;
                    case MinesEngine.GameState.Stopped:                                 //game's stopped
                        break;                                                              //do nothing
                }   //ENDSWITCH (game state)
            }   //ENDELSEIF (left click)
            else if (SameButton(curButton, RBDown))                             //same right button upped as was down (right-click on the cell)
            {
                ME.ChangeMarker(Cell.Column, Cell.Row);                             //change marker on the current cell
                if (ME[Cell.Column, Cell.Row].marker > 0)                           //if there is marker set on the current cell
                    FL[Cell.Column][Cell.Row].ImageIndex = 
                        ME[Cell.Column, Cell.Row].marker;                               //draw marker on the current cell
                else                                                                //otherwise (marker not set)
                    FL[Cell.Column][Cell.Row].ImageIndex = -1;                          //hide marker on the current cell
                lMines.Text = ((int)ME.NumMines - (int)ME.NumFlags).ToString();     //change number of mines in the mines-textfield
            }   //ENDELSEIF (right click)
            LBDown = null;                                                      //clear the information about the pushed left button of the mouse
            RBDown = null;                                                      //clear the information about the pushed right button of the mouse
        }   //END (Cell_Up)

        /// <summary>
        /// Double click the mine field cell event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void MineFieldLabel_DoubleClick(object sender, EventArgs e)
        {
            if (ME.CurrentGameState.State != MinesEngine.GameState.NewGame &&   //if current game state is NOT NewGame
                ME.CurrentGameState.State != MinesEngine.GameState.InProgress)  //AND current game state is NOT InProgress
                return;                                                             //just do nothing and return

            MineFieldLabel Cell = (MineFieldLabel)sender;                       //get the cell upon which mouse acted

            MinesEngine.CurrentGameStateInfo GS = 
                ME.OpenMany(Cell.Column, Cell.Row);                             //open the current cell and cells around it

            switch (GS.State)                                                   //switch between the current game states
            {
                case MinesEngine.GameState.InProgress:                              //game's in progress
                    RedrawOpened();                                                     //redraw already opened cells
                    break;
                case MinesEngine.GameState.Loose:                                   //game's lost
                    tTime.Stop();                                                       //stop game timer
                    RedrawOpened();                                                     //redraw already opened cells
                    OpenLoose(GS);                                                      //open field as for the lost game
                    butNewGame.ImageIndex = 3;                                          //set the NewGame button's image to LOST
                    MStats.CalcStats(ME, MS, GameSeconds, AskForUserName);              //calculate game stats
                    break;
                case MinesEngine.GameState.Win:                                     //game's won
                    tTime.Stop();                                                       //stop game timer
                    RedrawOpened();                                                     //redraw already opened cells
                    butNewGame.ImageIndex = 4;                                          //set the NewGame button's image to WON
                    MStats.CalcStats(ME, MS, GameSeconds, AskForUserName);              //calculate game stats
                    break;
                case MinesEngine.GameState.Stopped:                                 //game's stopped
                    break;                                                              //do nothing
            }   //ENDSWITCH (game states)
        }   //END (MineFieldLabel_DoubleClick)

        /// <summary>
        /// About button in the menu was clicked event handler
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            if (formAbout == null)                                              //if About form is not created
                formAbout = new FormAbout(this);                                    //create it
            Enabled = false;                                                    //disable the main form
            formAbout.Location =
                new Point(Location.X + Width / 2 - formAbout.Size.Width / 2,
                        Location.Y + Height / 2 - formAbout.Size.Height / 2);   //set About form location to the center of the main form
            formAbout.ShowDialog();                                             //show About form as dialog
        }   //END (tsmiAbout_Click)

        /// <summary>
        /// NewGame button in the menu was clicked event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void tsmiNewGame_Click(object sender, EventArgs e)
        {
            InitialiseField();  //initialise mine field for the new game
        }   //END (tsmiNewGame_Click)

        /// <summary>
        /// Exit button in the menu was clicked event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void tsmiExit_Click(object sender, EventArgs e)
        {
            Close();    //just close the main form
        }   //END (tsmiExit_Click)

        /// <summary>
        /// Parameters button in the menu was clicked event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void tsmiParameters_Click(object sender, EventArgs e)
        {
            if (formSettings == null)                                       //if Settings form is not created
                formSettings = new FormSettings(this);                          //create it
            Enabled = false;                                                //disable the main form
            formSettings.Location =                                     
                new Point(Location.X + Width / 2 - formSettings.Width / 2,
                        Location.Y + Height / 2 - formSettings.Height / 2); //set Settings form location to the center of the main form
            formSettings.ShowDialog();                                      //show Settings form as a dialog
        }   //END (tsmiParameters_Click)

        /// <summary>
        /// Stats button in the menu was clicked event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void tsmiStats_Click(object sender, EventArgs e)
        {
            if (formStats == null)                                              //if Stats form is not created yet
                formStats = new FormStatistics(this);                               //create it
            Enabled = false;                                                    //disable the main form
            formStats.Location = 
                new Point(Location.X + Width / 2 - formStats.Size.Width / 2,
                        Location.Y + Height / 2 - formStats.Size.Height / 2);   //set Stats form's location to the center of the main form
            formStats.ShowDialog();                                             //show Stats form as a dialog
        }   //END (tsmiStats_Click)
        #endregion

        #region Additional
        /// <summary>
        /// Initialises the new minefield and redraws the form for it
        /// </summary>
        public void InitialiseField()
        {

#if DEBUG   //debug section start
            if (formDebug != null) formDebug.Hide();    //hide the debug form if it is created
#endif      //debug section end

            ME = MinesEngine.getEngine(MS); //getting the game engine

            //setting game related fields
            GameStart = false;  //game not started yet
            GameSeconds = 0;    //game time equals zero by now

            //setting form controls to the initial state
            tTime.Stop();                                       //game timer stopped
            butNewGame.ImageIndex = 2;                          //set in-game image for the NewGame button
            lMines.Text = MS.NumMines.ToString();               //show number of mines
            lTime.Text = "";                                    //clear timer textfield

            //redrawing mine field cells
            gbMineField.SuspendLayout();                        //suspend layout of the mine field
            for (int i = 0; i < MS.FieldWidth; i++)             //moving through all the mine field columns
                for (int j = 0; j < MS.FieldHeight; j++)        //moving through all the cells in the current column
                    FL[i][j].ChangeLabel(ClosedColor, -1, "");  //setting current mine field cell to its initial state
            gbMineField.ResumeLayout();                         //resume layout of the mine field

            //changing size of the main form
            Size = new Size(
                MS.FieldWidth * CellSize + 30,                  //width
                MS.FieldHeight * CellSize + 115);               //height

            //changing size of the mine field
            gbMineField.Size = new Size(
                MS.FieldWidth * CellSize + 2,                   //width
                MS.FieldHeight * CellSize + 8);                 //height

            //setting easter egg 2 controls to the initial state
            pbHeartBig.Visible = false;                         //hide the big heart
            pbHeartBig.Size = gbMineField.Size;                 //set size of the big heart equal to size of the mine field
            pbHeartBigger.Visible = false;                      //hide even bigge5r heart
            pbHeartBigger.Size = gbMineField.Size;              //set its size equal to size of the mine field

#if DEBUG   //debug section start
            DebugMineField();   //show debug mine field
#endif      //debug section end
        }   //END (InitialiseField)

        /// <summary>
        /// Redraws opened cells of the minefield
        /// </summary>
        private void RedrawOpened()
        {
            for (int i = 0; i < ME.Width; i++)                  //moving through all the columns of the mine field
            {
                for (int j = 0; j < ME.Height; j++)                 //moving through all cells in the current columnn
                {
                    if (ME[i, j].state && 
                        FL[i][j].BackColor == ClosedColor)              //if cell must be opened and is not
                    {
                        FL[i][j].BackColor = OpenedColor;                   //set its back color to the opened one
                        if (ME[i, j].cell == -1)                            //if cell has mine
                        {
                            FL[i][j].Text = "";                                 //clear its text
                            FL[i][j].ImageIndex = 0;                            //and show mine
                        }
                        else                                                //if cell has no mine
                        {
                            FL[i][j].ImageIndex = -1;                           //hide images in it
                            FL[i][j].ForeColor = NumberColors[ME[i, j].cell];   //set its fore color (number color)
                            FL[i][j].Text = (ME[i, j].cell > 0) ?               //if its number is bigger than zero
                                ME[i, j].cell.ToString() :                      //show its number
                                "";                                             //otherwise it will be empty
                        }   //ENDIF (has mine)
                    }   //ENDIF (must be opened)
                }   //ENDFOR (cells in current column)
            }   //ENDFOR (columns)
        }   //END (RedrawOpened)

        /// <summary>
        /// Open field when game is lost (shows only mines location and mistakes)
        /// </summary>
        /// <param name="GS">Current game state</param>
        private void OpenLoose(MinesEngine.CurrentGameStateInfo GS)
        {
            //show mines
            for (int i = 0; i < ME.Width; i++)      //moving through all the columns
                for (int j = 0; j < ME.Height; j++)     //moving through all the cells in current column
                    if (ME[i, j].cell == -1)                //if cell contains mine
                        FL[i][j].ImageIndex = 0;                //show mine
            
            //highlight mistakes
            for (int k = 0; k < GS.NumMinesBombed; k++)                                         //moving through all the mines that bombed
                FL[GS.BombedMines[k].Column][GS.BombedMines[k].Row].BackColor = MistakeColor;       //and highlighting them with mistake color
            for (int k = 0; k < GS.NumWrongFlags; k++)                                          //moving through all the wrongly placed flags
            {
                FL[GS.WrongFlags[k].Column][GS.WrongFlags[k].Row].BackColor = MistakeColor;         //highlight flag with mistake color
                FL[GS.WrongFlags[k].Column][GS.WrongFlags[k].Row].ImageIndex = 1;                   //show flag on the cell
            }   //ENDFOR (wrong flags)
        }   //END (OpenLoose)

        /// <summary>
        /// Checks whether specified mouse events correspond to the same minefield-cell-button
        /// </summary>
        /// <param name="left">Left mouse button event</param>
        /// <param name="right">Right mouse button event</param>
        /// <returns>True if button is the same</returns>
        private bool SameButton(MouseEventArgs left, MouseEventArgs right)
        {
            if (left != null &&         //if left button exists
                right != null &&        //AND right button exists
                left?.X == right?.X &&  //AND cell columns of left and right buttons equals
                left?.Y == right?.Y)    //AND cell rows of left and right buttons equals
                                        //which means that left and right both acted upon the same mine-field-cell
                return true;                //then we will return true
            return false;               //otherwise we will return false
        }   //END (SameButton)

        /// <summary>
        /// Ask player name using the AskName form
        /// </summary>
        /// <returns>String containing player-name</returns>
        public string AskForUserName()
        {
            if (formAskName == null)                                        //if AskName form is not created
                formAskName = new FormAskName(this);                            //create it
            Enabled = false;                                                //disable the main form
            formAskName.Location = 
                new Point(Location.X + Width / 2 - formAskName.Width / 2,
                        Location.Y + Height / 2 - formAskName.Height / 2);  //set the location of the AskName form to the center of the main form
            formAskName.ShowDialog();                                       //show AskName form as a dialog

            //time for the second easter egg for Zaza (beating heart)
            if (formAskName.PlayerName.ToLower() == "zaza")                 //if player entered "zaza" as a name to the AskName form (not case sensitive)
            {
                Random rnd = new Random();                                      //create random
                int rndMax = (int)Math.Floor(300 / (decimal)ME.NumMines);       //count the max value for the random (300 / number of mines in the won game)
                                                                                //so the more mines - the more probability that easter egg will be shown
                if (rnd.Next(rndMax) == 0)                                      //if random with the calculated max gave us zero
                    DrawHeart();                                                    //show the second easter egg
            }   //ENDIF (player == "zaza")

            return formAskName.PlayerName;                                  //return the player name entered to the AskName form
        }   //END (AskForUserName)

        /// <summary>
        /// Draws the beating heart on the field and then hides it (Second easter egg for Zaza)
        /// </summary>
        private void DrawHeart()
        {
            gbMineField.Hide();                 //hide mine field
            pbHeartBigger.Visible = false;      //hide bigger heart
            pbHeartBig.Visible = true;          //show big heart
            Refresh();                          //redraw all controls in the main form
            for (int i = 0; i < 3; i++)         //heart will beat three times
            {
                //we will change images of the big and the same bigger heart several times
                //and it will imitate the heart beat
                Thread.Sleep(500);                  //wait for 0.5 seconds
                pbHeartBig.Visible = false;         //hide big heart
                pbHeartBigger.Visible = true;       //show bigger heart
                Refresh();                          //redraw the main form
                Thread.Sleep(700);                  //wait for 0.7 seconds
                pbHeartBigger.Visible = false;      //hide bigger heart
                pbHeartBig.Visible = true;          //show big heart
                Refresh();                          //redraw the main form
            }   //ENDFOR (heart beating)
            Thread.Sleep(1000);                 //wait for 1 second
            pbHeartBig.Visible = false;         //hide big heart
            pbHeartBigger.Visible = false;      //hide bigger heart
            gbMineField.Show();                 //show mine field
        }   //END (DrawHeart)
        #endregion
        #endregion
    }   //ENDCLASS (FormMineSweeper)
}   //ENDNAMESPACE (MineSweeper)