using System;
#if DEBUG   //debug section start
using System.Diagnostics;
#endif      //debug section end

namespace MineSweeper
{
    /// <summary>
    /// Minesweeper game engine
    /// </summary>
    public class MinesEngine
    {
        #region Enums
        /// <summary>
        /// Possible states of the current game
        /// </summary>
        public enum GameState
        {
            /// <summary>
            /// Game is lost
            /// </summary>
            Loose,
            /// <summary>
            /// Game is won
            /// </summary>
            Win,
            /// <summary>
            /// Game is in progress
            /// </summary>
            InProgress,
            /// <summary>
            /// Game is stopped
            /// </summary>
            Stopped,
            /// <summary>
            /// Ready to start new game
            /// </summary>
            NewGame
        }   //END (GameState)
        #endregion

        #region Structures
        /// <summary>
        /// Information about the minefield cell
        /// </summary>
        public struct CellInfo
        {
            /// <summary>
            /// Cell of the minefield (-2 if the cell is closed, -1 - has mine; 0 - empty; >0 - number of mines in the nearest cells)
            /// </summary>
            public sbyte cell;
            /// <summary>
            /// Marker on the cell (0 - no mark; 1 - flag mark; 2 - question mark)
            /// </summary>
            public byte marker;
            /// <summary>
            /// State of the cell (false - closed; true - opened)
            /// </summary>
            public bool state;
        }   //END (CellInfo)

        /// <summary>
        /// Minefield cell coordinates
        /// </summary>
        public struct CellCoordinates
        {
            /// <summary>
            /// Number of column
            /// </summary>
            public int Column;
            /// <summary>
            /// Number of row
            /// </summary>
            public int Row;
        }   //END (CellCoordinates)

        /// <summary>
        /// Information about the state of the current game
        /// </summary>
        public struct CurrentGameStateInfo
        {
            /// <summary>
            /// The state of the current game
            /// </summary>
            public GameState State;
            /// <summary>
            /// Number of bombed mines
            /// </summary>
            public uint NumMinesBombed;
            /// <summary>
            /// Number of wrongly placed flags
            /// </summary>
            public uint NumWrongFlags;
            /// <summary>
            /// Bombed mines coordinates
            /// </summary>
            public CellCoordinates[] BombedMines;
            /// <summary>
            /// Wrongly placed flags
            /// </summary>
            public CellCoordinates[] WrongFlags;
        }   //END (CurrentGameStateInfo)
        #endregion

        #region Fields
        /// <summary>
        /// Unique instance of the MinesEngine class
        /// </summary>
        private static MinesEngine Instance;
        /// <summary>
        /// Lock in case of use multythreading
        /// </summary>
        private static object multyThreadLock = new Object();
        /// <summary>
        /// Information about the state of the current game
        /// </summary>
        public CurrentGameStateInfo CurrentGameState;
        /// <summary>
        /// The minefield (-1 - has mine; 0 - empty; >0 - number of mines in the nearest cells)
        /// </summary>
        private sbyte[][] field;
        /// <summary>
        /// Markers on the minefield cells (0 - no mark; 1 - flag mark; 2 - question mark)
        /// </summary>
        private byte[][] markers;
        /// <summary>
        /// States of the minefield cells (false - closed; true - opened)
        /// </summary>
        private bool[][] state;
        /// <summary>
        /// Use question marks is true
        /// </summary>
        private bool UseQuestionMarks;
        #endregion

        #region Properties
        /// <summary>
        /// Number of mines on the minefield
        /// </summary>
        public uint NumMines { get; private set; }
        /// <summary>
        /// Number of flags on the minefield
        /// </summary>
        public uint NumFlags { get; private set; }
        /// <summary>
        /// Width of the minefield
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// Height of the minefield
        /// </summary>
        public int Height { get; private set; }
        /// <summary>
        /// 3BV difficulty level of the current minefield
        /// </summary>
        public uint Level3BV { get; private set; }
        #endregion

        #region Indexer
        /// <summary>
        /// External access to the specified minefield cell (if cell isn't opened, cell field equals -2)
        /// </summary>
        /// <param name="i">Column of the cell</param>
        /// <param name="j">Row of the cell</param>
        /// <returns>CellInfo struct containing information about the specified cell</returns>
        public CellInfo this[int i, int j]
        {
            get                                 //get data from engine
            {
                CellInfo res = new CellInfo();      //create new instance of CellInfo struct
#if DEBUG   //debug section start
                res.cell = field[i][j];             //save specified cell's value
#else       //if no DEBUG
                if (state[i][j])                    //if cell is opened
                    res.cell = field[i][j];             //save its value
                else                                //if cell is closed
                    res.cell = -2;                      //we'll return -2
#endif      //debug section end
                res.marker = markers[i][j];         //save cell's marker
                res.state = state[i][j];            //save cell's state (opened/closed)
                return res;                         //return cell info
            }   //ENDGET (get data)
            private set { }                     //setting of data is prohibited
        }   //ENDTHIS (external access)
        #endregion

        #region Methods
        /// <summary>
        /// Minesweeper engine ctor
        /// </summary>
        /// <param name="MS">Minesweeper settings class</param>
        private MinesEngine(MinesSettings MS)
        {
            //ctor is private. MinesEngine is developped as a Singleton.
            //It is needed to prevent creation of several instances of game engine.
            //Because engine can be only one.
            NewGame(MS);    //just start the new game with specified settings
        }   //END (ctor)

        /// <summary>
        /// Gets unique instance of the MinesEngine with specified settings
        /// </summary>
        /// <param name="MS">Settings for the engine</param>
        /// <returns>Unique instance of the MinesEngine class</returns>
        public static MinesEngine getEngine(MinesSettings MS)
        {
            if (Instance == null)                       //if class instance is not created
                lock (multyThreadLock)                      //begin lock section (to nobody could create instance at the same time)
                {
                    if (Instance == null)                       //again: if class instance is not created
                        Instance = new MinesEngine(MS);             //create unique class instance
                }   //ENDLOCK (lock section)
            else                                        //if class instance is already created
                Instance.NewGame(MS);                       //just begin the new game for it
            return Instance;                            //return unique class instance
        }   //END (getEngine)

        /// <summary>
        /// Start the new game
        /// </summary>
        /// <param name="MS">Minesweeper settings class</param>
        private void NewGame(MinesSettings MS)
        {
            NumMines = MS.NumMines;                     //set mines number
            NumFlags = 0;                               //clear flags number
            Width = MS.FieldWidth;                      //set minefield width
            Height = MS.FieldHeight;                    //set minefield height
            UseQuestionMarks = MS.UseQuestionMarks;     //to use questions or not to?
            Level3BV = 0;                               //clear difficulty level
            CurrentGameState = new CurrentGameStateInfo //set current game state
            {
                State = GameState.NewGame,                  //ready to begin the new game
                BombedMines = null,                         //clear bombed mines array
                WrongFlags = null,                          //clear wrong flags array
                NumMinesBombed = 0,                         //clear number of bombed mines
                NumWrongFlags = 0                           //clear number of wrong flags
            };  //ENDCURRENTGAMESTATE (game state)
            Array.Resize(ref field, Width);             //get memory for the minefield
            Array.Resize(ref markers, Width);           //get memory for markers
            Array.Resize(ref state, Width);             //get memory for cell states
            for (int i = 0; i < Width; i++)             //moving through all the minefield columns
            {
                Array.Resize(ref field[i], Height);         //get memory for the current column
                Array.Resize(ref markers[i], Height);       //get memory for the current column's markers
                Array.Resize(ref state[i], Height);         //get memory for the current column's cell's states
            }   //ENDFOR (columns)
            GenerateField();                            //generate the new minefield
        }   //END (NewGame)

        /// <summary>
        /// Generates the new minefield according to the MineSweeper rules
        /// </summary>
        private void GenerateField()
        {
            uint minesPlaced = 0;                   //number of mines already placed to the minefield
            Random rnd = new Random();              //create random (will be used to randomly select cells for mine-placing)
            for (int i = 0; i < Width; i++)         //moving through all the minefield columns
            {
                Array.Clear(field[i], 0, Height);       //clear column
                Array.Clear(markers[i], 0, Height);     //clear markers
                Array.Clear(state[i], 0, Height);       //clear states
            }   //ENDFOR (columns)
            //placing mines randomly
            while (minesPlaced != NumMines)         //repeating while there are mines left unplaced
            {
                int i = rnd.Next(Width);                //randomly generate column number
                int j = rnd.Next(Height);               //randomly generate row number
                if (field[i][j] != -1)                  //if there is no mine in the randomly taken cell
                {
                    field[i][j] = -1;                       //place mine
                    minesPlaced++;                          //increase number of placed mines
                }   //ENDIF (no mine)
            }   //ENDWHILE (left unplaced mines)
            //filling up cells of the minefield with the information about the surrounding mines
            for (int i = 0; i < Width; i++)         //moving through all the minefield columns
            {
                for (int j = 0; j < Height; j++)        //moving through all the cells in current column
                {
                    if (field[i][j] == -1)                  //if current cell has mine
                        continue;                               //skip it
                    field[i][j] = CountMines(i, j);         //fill number of mines aroud the current cell to the current cell
                }   //ENDFOR (cells in column)
            }   //ENDFOR (columns)
            Calculate3BVLevel();                    //calculate difficulty level of the new minefield
        }   //END (GenerateField)

        /// <summary>
        /// Check whether all the cells (except those with mines) of the minefield have been opened
        /// </summary>
        /// <returns>True if all the cells except those with mines are opened, False otherwise</returns>
        private bool AllOpened()
        {
            //we'll simply move through all cells and foreach cell check whether it is opened or has a mine.
            //if every cell in the minefield is opened or has a mine, then minefield is fully opened.
            bool res = true;                        //result of the current minefield check
            for (int i = 0; i < Width; i++)         //moving through all the columns
                for (int j = 0; j < Height; j++)        //moving through all the cells in the current column
                    res &=                                  //logically multiply result by
                        (state[i][j] ||                         //state of the current cell (which is TRUE if cell is opened) OR
                        field[i][j] == -1);                     //current cell contains a mine
            return res;                             //return check result
        }   //END (AllOpened)

        /// <summary>
        /// Cyclically changes marker on the specified cell of the minefield
        /// </summary>
        /// <param name="i">Column of the specified cell</param>
        /// <param name="j">Row of the specified cell</param>
        /// <returns>Returns the value of the new cell marker</returns>
        public byte ChangeMarker(int i, int j)
        {
            if (state[i][j])                                                //if cell is opened
                return markers[i][j];                                           //do nothing and return its marker
#if DEBUG   //debug section start
            Debug.WriteLine($"ChangeMarker: \t" +
                            $"WAS: markers[{i}][{j}] = " + 
                            $"{markers[i][j]}, NumFlags = {NumFlags}");     //write debug message
            Debug.WriteLineIf(UseQuestionMarks, "\tUsing Question marks");  //write debug message
#endif      //debug section end
            if (markers[i][j] == 1)                                         //if current marker is flag
                NumFlags--;                                                     //decrease number of flags placed
                                                                                //  (we're gonna change this flag with something else)
            if (UseQuestionMarks)                                           //if we're using question marks
                markers[i][j] =                                                 //set new marker
                    (markers[i][j] == 2) ?                                          //if current marker is a question
                    (byte)0 :                                                           //set new marker to empty (move to the first marker)
                    (byte)(markers[i][j] + 1);                                          //otherwise move to the next marker
            else                                                            //if we're not using questions
                markers[i][j] =                                                 //set new marker
                    (markers[i][j] == 1) ?                                          //if current marker is a flag
                    (byte)0 :                                                           //set new marker to empty (move to the first marker)
                    (byte)(markers[i][j] + 1);                                          //otherwise move to the next marker
            if (markers[i][j] == 1)                                         //if new marker is flag
                NumFlags++;                                                     //increase number of flags placed
#if DEBUG   //debug section start
            Debug.WriteLine($"\tIS: markers[{i}][{j}] = " + 
                            $"{markers[i][j]}, NumFlags = {NumFlags}");     //write debug message
#endif      //debug section end
            return markers[i][j];                                           //return the new marker
        }   //END (ChangeMarker)

        /// <summary>
        /// Automatically opens empty cells of the minefield starting from the specified cell
        /// </summary>
        /// <param name="i">Column of the specified cell</param>
        /// <param name="j">Row of the specified cell</param>
        private void AutoOpenCells(int i, int j)
        {
            //works recursively
            //returns when:
            if (i < 0 || i >= Width) return;    //hits vertical borders of the minefield
            if (j < 0 || j >= Height) return;   //hits horizontal borders of the minefield
            if (state[i][j]) return;            //gets to already opened cell
            if (markers[i][j] == 1) return;     //gets to the cell marked with flag
            if (field[i][j] == -1) return;      //gets to the mine

            //finished returning
            //beginning of the method's job section
            if (field[i][j] > 0)                //when got to the number
            {
                state[i][j] = true;                 //open it
                return;                             //and return
            }   //ENDIF (number)
            //if we're here, the cell should be opened
            state[i][j] = true;                 //open current cell

            //finished job section
            //moving to the next cells:
            AutoOpenCells(i - 1, j);            //to the left
            AutoOpenCells(i - 1, j - 1);        //to the top-left
            AutoOpenCells(i, j - 1);            //to the top
            AutoOpenCells(i + 1, j - 1);        //to the top-right
            AutoOpenCells(i + 1, j);            //to the right
            AutoOpenCells(i + 1, j + 1);        //to the bottom-right
            AutoOpenCells(i, j + 1);            //to the bottom
            AutoOpenCells(i - 1, j + 1);        //to the bottom-left
        }   //END (AutoOpenCells)

        /// <summary>
        /// Get location of the empty cell (with no mine and with the least number (0 if there is any zero-cell))
        /// </summary>
        /// <param name="i">Found column number</param>
        /// <param name="j">Found row number</param>
        /// <returns>True if empty cell was found</returns>
        private bool GetEmptyCell(out int i, out int j)
        {
            //we will look for the first cell with a number and without a mine.
            //if we will find a zero-cell (e.g. empty cell) we will immediately return its location.
            //if we somehow will not find any cell with a number we will assume the search was unsuccessfull
            int minNumber = 9;                                      //minimal cell-number found (except mines)
                                                                    //  (we'll use 9 because the maximal cell number is 8.
                                                                    //  so if we will find no number-cells this variable will remain equals 9)
            i = -1;                                                 //set returned column number to -1 (we'll return this value in case of no success)
            j = -1;                                                 //set returned row number to -1 (we'll return this value in case of no success)
            for (int x = 0; x < Width; x++)                         //moving through all the columns in the mine field
            {
                for (int y = 0; y < Height; y++)                        //moving through all the cells in current column
                {
                    if (field[x][y] == -1)                                  //if current cell contains a mine
                        continue;                                               //skip it
                    if (field[x][y] == 0)                                   //if current cell is a zero-one
                    {
                        i = x;                                                  //return its column number
                        j = y;                                                  //return its row number
                        return true;                                            //return true (search is successfull)
                    }   //ENDIF (zero-cell)
                    if (field[x][y] > 0 && field[x][y] < minNumber)         //if current cell contains a number which is less than minNumber
                    {
                        i = x;                                                  //save its column number
                        j = y;                                                  //save its row number
                        minNumber = field[x][y];                                //save the new numNumber value
                    }   //ENDIF (cell with min number)
                }   //ENDFOR (cells in column)
            }   //ENDFOR (columns)
            if (minNumber == 9)                                     //if minNumber equals 9
                return false;                                           //then search is not successfull
                                                                        //  return false (column and row are already returned equal -1)
            else                                                    //if minNumber is not equal 9
                return true;                                            //then search is successfull
                                                                        //  return true (column and row are already returned)
        }   //END (GetEmptyCell)

        /// <summary>
        /// Count number of mines around the specified cell
        /// </summary>
        /// <param name="i">Column of the specified cell</param>
        /// <param name="j">Row of the specified cell</param>
        /// <returns>Number of mines around the specified cell</returns>
        private sbyte CountMines(int i,int j)
        {
            sbyte num = 0;                              //number of mines around specified cell will be stored in this variable
            for (int dI = -1; dI <= 1; dI++)            //moving through previous, current and next columns
            {
                if (i + dI < 0 || i + dI >= Width)          //if dI got us outside the minefield borders
                    continue;                                   //skip current dI value
                for (int dJ = -1; dJ <= 1; dJ++)            //moving through previous, current and next cell inside the current column
                {
                    if (j + dJ < 0 || j + dJ >= Height)         //if dJ got us outside the minefield borders
                        continue;                                   //skip current dJ value
                    if (dI == 0 && dJ == 0)                     //if dI and dJ got us to the specified cell (both equals zero)
                        continue;                                   //we will skip the specified cell
                    if (field[i + dI][j + dJ] == -1)            //if cell contains a mine
                        num++;                                      //count it
                }   //ENDFOR (cells in column)
            }   //ENDFOR (columns)
            return num;                                 //return number of mines around the specified cell
        }   //END (CountMines)

        /// <summary>
        /// Open the specified cell
        /// </summary>
        /// <param name="i">Column of the specified cell</param>
        /// <param name="j">Row of the specified cell</param>
        /// <returns>Structure representing the current game state</returns>
        public CurrentGameStateInfo OpenCell(int i, int j)
        {
            if (CurrentGameState.State == GameState.Stopped)    //if game is stopped
                return CurrentGameState;                            //do nothing and just return current game state
            if (CurrentGameState.State == GameState.Win ||      //if game is already won
                CurrentGameState.State == GameState.Loose)      //OR game is already lost
            {
                CurrentGameState.State = GameState.Stopped;         //set game state to stopped
                return CurrentGameState;                            //and return the new game state
            }   //ENDIF (game won or lost)
            if (CurrentGameState.State == GameState.NewGame)    //if it is the beginning of the new game
            {
                //for the new game (first click on the minefield) we will make minefield interactive.
                //it means that if player's first click hits a mine, we will move this mine somewhere else.
                //and that guarantees that the first click will never blow a mine
                CurrentGameState.State = GameState.InProgress;      //set game state to InProgress
                if (field[i][j] == -1)                              //if current cell has a mine
                {
                    //we will look for the new location for the found mine
                    int newI = 0, newJ = 0;                             //new mine's location
                    if (GetEmptyCell(out newI, out newJ))               //if location search was successfull
                    {
                        field[i][j] = 0;                                    //remove the mine from the current cell
                        field[newI][newJ] = -1;                             //set it to the new location
                        //and now we have to change number of mines in cells around the current cell and the new mine's location
                        for (int dI = -1; dI <= 1; dI++)                    //moving through the previous, current and next columns
                        {
                            for (int dJ = -1; dJ <= 1; dJ++)                    //moving through the previous, current and next cells in the current columns
                            {
                                if (i + dI >= 0 && i + dI < Width)                  //if dI got us from current cell INSIDE the minefield borders
                                    if (j + dJ >= 0 && j + dJ < Height)                 //if dJ got us from current cell INSIDE the minefield borders
                                        if (field[i + dI][j + dJ] != -1)                    //if cell doesn't contain a mine
                                            field[i + dI][j + dJ] = 
                                                CountMines(i + dI, j + dJ);                     //count mines around it
                                if (newI + dI >= 0 && newI + dI < Width)            //if dI got us from new mine's location INSIDE the minefield borders
                                    if (newJ + dJ >= 0 && newJ + dJ < Height)           //if dJ got us from new mine's location INSIDE the minefield borders
                                        if (field[newI + dI][newJ + dJ] != -1)              //if cell doesn't contain a mine
                                            field[newI + dI][newJ + dJ] = 
                                                CountMines(newI + dI, newJ + dJ);               //count mines around it
                            }   //ENDFOR (cells in column)
                        }   //ENDFOR (columns)
                    }   //ENDIF (search is successfull)
                }   //ENDIF (relocation the mine)
            }   //ENDIF (new game)
            if (markers[i][j] == 1)                             //cell is marked with flag - not to open
            {
                CurrentGameState.State = GameState.InProgress;      //set game state to InProgress
                return CurrentGameState;                            //return the new game state
            }   //ENDIF (flag)
            if (field[i][j] == -1)                              //mine found
            {
                CurrentGameState.State = GameState.Loose;           //set game state to lost
                CurrentGameState.NumMinesBombed++;                  //increase the number of bombed mines
                Array.Resize(
                    ref CurrentGameState.BombedMines, 
                    (int)CurrentGameState.NumMinesBombed);          //get memory for the list of bombed mines
                CurrentGameState.BombedMines
                    [CurrentGameState.NumMinesBombed - 1] = 
                    new CellCoordinates { Column = i, Row = j };    //write currently bombed mine to the list of bombed mines
            }   //ENDIF (mine)
            AutoOpenCells(i, j);                                //auto open all cells starting from the current one
            if (AllOpened())                                    //if whole field is opened
                CurrentGameState.State = GameState.Win;             //we can say that the game is won
            OpenField();                                        //open field if needed
            return CurrentGameState;                            //return the current game state
        }   //END (OpenCell)

        /// <summary>
        /// Open the specified cell and the cells that surround the specified one
        /// </summary>
        /// <param name="i">Column of the specified cell</param>
        /// <param name="j">Row of the specified cell</param>
        /// <returns>Structure representing the current game state</returns>
        public CurrentGameStateInfo OpenMany(int i, int j)
        {
            if (CurrentGameState.State == GameState.Stopped)                    //if game is stopped
                return CurrentGameState;                                            //do nothing and return current game state
            if (CurrentGameState.State == GameState.Win ||                      //if game is won
                CurrentGameState.State == GameState.Loose)                      //OR game is lost
            {
                CurrentGameState.State = GameState.Stopped;                         //set game state to stopped
                return CurrentGameState;                                            //return new game state
            }   //ENDIF (won or lost)
            if (!state[i][j])                                                   //if specified cell is closed
                return CurrentGameState;                                            //do nothing and return current game state
            int numFlagsAround = field[i][j];                                   //number of flags required in the surrounding cells
#if DEBUG   //debug section start
            Debug.WriteLine($"OpenMany:\n\tnumFlagsAround = " + 
                            $"{numFlagsAround} (i = {i}, j = {j})");            //write debug message
#endif      //debug section end
            for (int di = -1; di <= 1; di++)                                    //moving through the previous, current and next columns
            {
#if DEBUG   //debug section start
                Debug.Write($"\tdi = {di}, i+di = {i+di}");                         //write debug message
                Debug.WriteIf(i + di < 0 || i + di >= Width, " column SKIPPED");    //write debug message
                Debug.WriteLine("");                                                //write debug message
#endif      //debug section end
                if (i + di < 0 || i + di >= Width)                                  //if di got us outside the minefield borders
                    continue;                                                           //skip current di value
                for (int dj = -1; dj <= 1; dj++)                                    //moving through the previous, current and next cells in the current column
                {
#if DEBUG   //debug section start
                    Debug.Write($"\t\tdj = {dj}, j+dj = {j + dj}");                     //write debug message
                    Debug.WriteIf(j + dj < 0 || j + dj >= Height, " row SKIPPED");      //write debug message
                    Debug.WriteLine("");                                                //write debug message
                    Debug.WriteLineIf(di == 0 && dj == 0, "\t\tcenter cell SKIPPED");   //write debug message
#endif      //debug section end
                    if (di == 0 && dj == 0)                                             //if got to the specified cell (both di and dj equal zero)
                        continue;                                                           //skip this cell
                    if (j + dj < 0 || j + dj >= Height)                                 //if dj got us outside the minefield borders
                        continue;                                                           //skip current dj value
#if DEBUG   //debug section start
                    Debug.WriteLine($"\t\tmarkers[{i + di}][{j + dj}] = " + 
                                    $"{markers[i + di][j + dj]}");                      //write debug message
#endif      //debug section end
                    if (markers[i + di][j + dj] == 1)                                   //if got to flag
                    {
                        numFlagsAround--;                                                   //decrease number of required flags
#if DEBUG   //debug section start
                        Debug.WriteLine($"\t\tdecreased numFlagsAround: " + 
                                        $"{numFlagsAround}");                               //write debug message
#endif      //debug section end
                    }   //ENDIF (flag)
                }   //ENDFOR (cells in column)
            }   //ENDFOR (columns)
#if DEBUG   //debug section start
            Debug.WriteLine($"\tRESULT numFlagsAround = {numFlagsAround}");     //write debug message
#endif      //debug section end
            if (numFlagsAround == 0)                                            //if number of flags around equals to the number in the specified cell
                                                                                //  can open the region of cells
            {
                //we'll open all cells in the region and check whether player made a mistake or not
                for (int dI = -1; dI <= 1; dI++)                                    //moving through the previous, current and next columns
                {
                    if (i + dI < 0 || i + dI >= Width)                                  //if di got us outside the minefield borders
                        continue;                                                           //skip current di value
                    for (int dJ = -1; dJ <= 1; dJ++)                                    //moving through the previous, current and next cells in the current column
                    {
                        if (j + dJ < 0 || j + dJ >= Height)                             //if dj got us outside the minefield borders
                            continue;                                                       //skip current dj value
                        if (markers[i + dI][j + dJ] == 1)                               //if cell is marked with flag
                        {
                            if (field[i + dI][j + dJ] != -1)                                //if cell has no mine
                            {
                                CurrentGameState.NumWrongFlags++;                               //increase the number of the wrong flags
                                Array.Resize(
                                    ref CurrentGameState.WrongFlags, 
                                    (int)CurrentGameState.NumWrongFlags);                       //get memory for the wrong flags list
                                CurrentGameState.WrongFlags
                                    [CurrentGameState.NumWrongFlags - 1] = 
                                    new CellCoordinates { Column = i + dI, Row = j + dJ };      //write current cell to the wrong flag list
                            }   //ENDIF (no mine)
                            continue;                                                       //skip current cell
                        }   //ENDIF (flag)
                        if (field[i + dI][j + dJ] == -1)                                //if cell has a mine
                        {
                            CurrentGameState.State = GameState.Loose;                       //set game state to lost
                            CurrentGameState.NumMinesBombed++;                              //increase the number of bombed mines
                            Array.Resize(
                                ref CurrentGameState.BombedMines, 
                                (int)CurrentGameState.NumMinesBombed);                      //get memory for the bombed mines list
                            CurrentGameState.BombedMines
                                [CurrentGameState.NumMinesBombed - 1] = 
                                new CellCoordinates { Column = i + dI, Row = j + dJ };      //write current cell to the bombed mines list
                        }   //ENDIF (mine)
                        AutoOpenCells(i + dI, j + dJ);                                  //auto open cells starting from the current one
                    }   //ENDFOR (cells in column)
                }   //ENDFOR (columns)
                OpenField();                                                        //open whole minefield if needed
                if (CurrentGameState.State == GameState.Win)                        //if game is won
                {
                    CurrentGameState.BombedMines = null;                                //clear the bombed mines list
                    CurrentGameState.NumMinesBombed = 0;                                //clear the number of bombed mines
                    CurrentGameState.WrongFlags = null;                                 //clear the wrong flags list
                    CurrentGameState.NumWrongFlags = 0;                                 //clear the number of wrong flags
                }   //ENDIF (game is won)
            }   //ENDIF (open region of cells)
            return CurrentGameState;                                            //return current game state
        }   //END (OpenMany)

        /// <summary>
        /// Open the entire field if it is needed
        /// </summary>
        private void OpenField()
        {
            if (CurrentGameState.State != GameState.Win &&              //if game is not won
                CurrentGameState.State != GameState.Loose)              //AND game is not lost
                return;                                                     //do nothing and just return
            for (int i = 0; i < Width; i++)                             //moving through all the minefield columns
            {
                Array.Clear(markers[i], 0, Height);                         //clear all markers of the current column
                for (int j = 0; j < Height; j++)                            //moving through all the cells in the current column
                {
                    if (CurrentGameState.State == GameState.Win)                //if game is won
                        state[i][j] = true;                                         //open each cell of the minefield
                    else if (CurrentGameState.State == GameState.Loose)         //if game is lost
                    {
                        if (field[i][j] == -1)                                      //if current cell has a mine
                            state[i][j] = true;                                         //open it
                    }   //ENDELSEIF (game lost)
                    else                                                        //if somehow we are here with game nor won nor lost
                        return;                                                     //do nothing and just return
                }   //ENDFOR (cells in column)
            }   //ENDFOR (columns)
        }   //END (OpenField)

        /// <summary>
        /// Calculate 3BV difficulty level for the current minefield
        /// </summary>
        private void Calculate3BVLevel()
        {
            //difficulty 3BV level is the amount of clicks needed to fully open mine field.
            //we will simply open entire field and count the number of "clicks" that it took.
            //after that we will close entire field
            Level3BV = 0;                                       //set difficulty level to zero
            for (int i = 0; i < Width; i++)                     //moving through all the columns of the minefield
            {
                for (int j = 0; j < Height; j++)                    //moving through all the cells in the current column
                {
                    if (field[i][j] == -1)                              //if mine is found
                        state[i][j] = true;                                 //open this cell
                    if (field[i][j] == 0)                               //if cell is empty
                        if (!state[i][j])                                   //if current cell is closed
                        {
                            Level3BV++;                                         //count this "click"
                            AutoOpenCells(i, j);                                //auto open cells starting from the current one
                        }   //ENDIF (cell is closed)
                    if (field[i][j] > 0)                                //if cell contains a number
                    {
                        //we'll look around this cell and if we won't find empty cells we'll count just this one
                        bool NoEmptyAround = true;                          //true if there are no empty cells around the current one
                        for(int dI = -1; dI <= 1; dI++)                     //moving through the previous, current and next columns
                        {
                            if (i + dI < 0 || i + dI >= Width)                  //if current di got us outside the minefield borders
                                continue;                                           //skip current di value
                            for(int dJ = -1; dJ <= 1; dJ++)                     //moving through the previous, current and next cells in the current column
                            {
                                if (j + dJ < 0 || j + dJ >= Height)                 //if current dj got us outside the minefield borders
                                    continue;                                           //skip current dj value
                                if (field[i + dI][j + dJ] == 0)                     //if found empty cell
                                    NoEmptyAround = false;                              //set no-empty flag to false
                            }   //ENDFOR (cells in column)
                        }   //ENDFOR (columns)
                        if (NoEmptyAround)                                  //if there are no empty cells around the current one
                            Level3BV++;                                         //count the current cell as another "click"
                        state[i][j] = true;                                 //open the current cell
                    }   //ENDIF (cell with number)
                }   //ENDFOR (cells in column)
            }   //ENDFOR (columns)
            for (int i = 0; i < Width; i++)                     //moving through all the columns in the minefield
                Array.Clear(state[i], 0, Height);                   //close all cells in the current column
        }   //END (Calculate3BVLevel)
        #endregion
    }   //ENDCLASS (MinesEngine)
}   //ENDNAMESPACE (MineSweeper)
