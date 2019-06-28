using System;

namespace MineSweeper
{
    /// <summary>
    /// Minesweeper game engine
    /// </summary>
    public class MinesEngine
    {
        /// <summary>
        /// Unique instance of the MineSweeper class
        /// </summary>
        private static MinesEngine Instance;

        /// <summary>
        /// Lock in case of use multythreading
        /// </summary>
        private static object multyThreadLock = new Object();

        /// <summary>
        /// Possible states of the current game
        /// </summary>
        public enum GameState
        {
            Loose,
            Win,
            InProgress,
            Stopped,
            NewGame
        }

        /// <summary>
        /// Information about the minfield cell
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
        }

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
        }

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
        }

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

        /// <summary>
        /// External access to the already opened cells in the minefield
        /// </summary>
        /// <param name="i">Column of the cell</param>
        /// <param name="j">Row of the cell</param>
        /// <returns>CellInfo struct containing information about the specified cell</returns>
        public CellInfo this[int i, int j]
        {
            get
            {
                CellInfo res = new CellInfo();
#if DEBUG
                res.cell = field[i][j];
#else
                if (state[i][j]) res.cell = field[i][j];
                else res.cell = -2;
#endif
                res.marker = markers[i][j];
                res.state = state[i][j];
                return res;
            }
            private set
            {
                field[i][j] = value.cell;
                markers[i][j] = value.marker;
                state[i][j] = value.state;
            }
        }

        /// <summary>
        /// Minesweeper engine constructor (using Settings class)
        /// </summary>
        /// <param name="MS">Minesweeper settings class</param>
        private MinesEngine(MinesSettings MS)
        {
            NumMines = MS.NumMines;
            NumFlags = 0;
            Width = MS.FieldWidth;
            Height = MS.FieldHeight;
            UseQuestionMarks = MS.UseQuestionMarks;
            Level3BV = 0;
            CurrentGameState = new CurrentGameStateInfo { State = GameState.NewGame, BombedMines = null, NumMinesBombed = 0 };

            Array.Resize(ref field, Width);
            Array.Resize(ref markers, Width);
            Array.Resize(ref state, Width);
            for (int i = 0; i < Width; i++)
            {
                Array.Resize(ref field[i], Height);
                Array.Resize(ref markers[i], Height);
                Array.Resize(ref state[i], Height);
            }

            GenerateField();
        }

        /// <summary>
        /// Gets unique instance of the MinesEngine with specified settings
        /// </summary>
        /// <param name="MS">Settings for the engine</param>
        /// <returns>Unique instance of the MinesEngine class</returns>
        public static MinesEngine getEngine(MinesSettings MS)
        {
            if (Instance == null)
            {
                lock (multyThreadLock)
                {
                    if (Instance == null) Instance = new MinesEngine(MS);
                }
            }
            else Instance.NewGame(MS);
            return Instance;
        }

        /// <summary>
        /// Starts the new game
        /// </summary>
        /// <param name="MS">Minesweeper settings class</param>
        public void NewGame(MinesSettings MS)
        {
            NumMines = MS.NumMines;
            NumFlags = 0;
            Width = MS.FieldWidth;
            Height = MS.FieldHeight;
            UseQuestionMarks = MS.UseQuestionMarks;
            CurrentGameState = new CurrentGameStateInfo { State = GameState.NewGame, BombedMines = null, NumMinesBombed = 0 };

            Array.Resize(ref field, Width);
            Array.Resize(ref markers, Width);
            Array.Resize(ref state, Width);
            for (int i = 0; i < Width; i++)
            {
                Array.Resize(ref field[i], Height);
                Array.Resize(ref markers[i], Height);
                Array.Resize(ref state[i], Height);
            }

            GenerateField();
        }

        /// <summary>
        /// Generates the new minefield according to the MineSweeper rules
        /// </summary>
        private void GenerateField()
        {
            uint minesPlaced = 0;
            Random rnd = new Random();

            for (int i = 0; i < Width; i++)
            {
                Array.Clear(field[i], 0, Height);
                Array.Clear(markers[i], 0, Height);
                Array.Clear(state[i], 0, Height);
            }

            //placing mines randomly
            while (minesPlaced != NumMines)
            {
                int i = rnd.Next(Width);
                int j = rnd.Next(Height);
                if (field[i][j] != -1)
                {
                    field[i][j] = -1;
                    minesPlaced++;
                }
            }

            //filling up cells of the minefield with the information about the surrounding mines
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (field[i][j] == -1) continue;
                    field[i][j] = CountMines(i, j);
                }
            }

            Calculate3BVLevel();
        }

        /// <summary>
        /// Check whether all the cells (except those with mines) of the minefield have been opened
        /// </summary>
        /// <returns>True if all the cells except those with mines are opened, False otherwise</returns>
        private bool AllOpened()
        {
            bool res = true;
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    res &= (state[i][j] || field[i][j] == -1);
            return res;
        }

        /// <summary>
        /// Cyclically changes marker on the specified cell of the minefield
        /// </summary>
        /// <param name="i">Column of the specified cell</param>
        /// <param name="j">Row of the specified cell</param>
        /// <returns>Returns the value of the new cell marker</returns>
        public byte ChangeMarker(int i, int j)
        {
            if (markers[i][j] == 1) NumFlags--;
            if(UseQuestionMarks)
                markers[i][j] = (markers[i][j] == 2) ? (byte)0 : (byte)(markers[i][j] + 1);
            else
                markers[i][j] = (markers[i][j] == 1) ? (byte)0 : (byte)(markers[i][j] + 1);
            if (markers[i][j] == 1) NumFlags++;
            return markers[i][j];
        }

        /// <summary>
        /// Automatically opens empty cells of the minefield from the specified cell
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

            if (field[i][j] > 0)        //when we got to the number
            {
                state[i][j] = true;     //we'll open it
                return;                 //and return
            }

            //if we're here, the cell should be opened
            state[i][j] = true;
            //and after that we have to move further:
            AutoOpenCells(i - 1, j);        //to the left
            AutoOpenCells(i - 1, j - 1);    //to the top-left
            AutoOpenCells(i, j - 1);        //to the top
            AutoOpenCells(i + 1, j - 1);    //to the top-right
            AutoOpenCells(i + 1, j);        //to the right
            AutoOpenCells(i + 1, j + 1);    //to the bottom-right
            AutoOpenCells(i, j + 1);        //to the bottom
            AutoOpenCells(i - 1, j + 1);    //to the bottom-left
        }

        /// <summary>
        /// Get location of the empty cell (with no mine and with the least number (0 if there is any zero-cell))
        /// </summary>
        /// <param name="i">Found column number</param>
        /// <param name="j">Found row number</param>
        /// <returns>True if empty cell was found</returns>
        private bool GetEmptyCell(out int i, out int j)
        {
            int minNumber = 9;  //minimal cell-number found (except mines)
            i = -1;
            j = -1;

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (field[x][y] == -1) continue;    //skip mines
                    if (field[x][y] == 0)   //found a zero-cell
                    {
                        i = x;
                        j = y;
                        return true;
                    }
                    if (field[x][y] > 0 && field[x][y] < minNumber)
                    {
                        i = x;
                        j = y;
                        minNumber = field[x][y];
                    }
                }
            }

            if (minNumber == 9) return false;
            else return true;
        }

        /// <summary>
        /// Count number of mines around for specified cell
        /// </summary>
        /// <param name="i">Column of the specified cell</param>
        /// <param name="j">Row of the specified cell</param>
        /// <returns>Number of mines around the specified cell</returns>
        private sbyte CountMines(int i,int j)
        {
            sbyte num = 0;
            for (int dI = -1; dI <= 1; dI++)
            {
                if (i + dI < 0 || i + dI >= Width) continue;
                for (int dJ = -1; dJ <= 1; dJ++)
                {
                    if (j + dJ < 0 || j + dJ >= Height) continue;
                    if (dI == 0 && dJ == 0) continue;       //current cell
                    if (field[i + dI][j + dJ] == -1) num++;
                }
            }
            return num;
        }

        /// <summary>
        /// Open the specified cell
        /// </summary>
        /// <param name="i">Column of the specified cell</param>
        /// <param name="j">Row of the specified cell</param>
        /// <returns>Enumeration representing the current game state</returns>
        public CurrentGameStateInfo OpenCell(int i, int j)
        {
            if (CurrentGameState.State == GameState.Stopped) return CurrentGameState;

            if (CurrentGameState.State == GameState.Win || CurrentGameState.State == GameState.Loose)
            {
                CurrentGameState.State = GameState.Stopped;
                return CurrentGameState;
            }

            if(CurrentGameState.State== GameState.NewGame)  //new game - open the first cell
            {
                CurrentGameState.State = GameState.InProgress;  //and now game is in progress
                //we'll make the field interactive. It means that first click will never BOOM
                if (field[i][j] == -1)  //mine found
                {
                    //let's find the new cell for this mine
                    int newI = 0, newJ = 0; //new mine location
                    if (GetEmptyCell(out newI, out newJ))   //new location found
                    {
                        //let's move mine to the new location                        
                        field[i][j] = 0;        //remove mine
                        field[newI][newJ] = -1; //set mine
                        for (int dI = -1; dI <= 1; dI++)
                        {
                            for (int dJ = -1; dJ <= 1; dJ++)
                            {
                                //recount mine-numbers
                                if (i + dI >= 0 && i + dI < Width)
                                    if (j + dJ >= 0 && j + dJ < Height)
                                        if (field[i + dI][j + dJ] != -1)
                                            field[i + dI][j + dJ] = CountMines(i + dI, j + dJ);

                                if (newI + dI >= 0 && newI + dI < Width)
                                    if (newJ + dJ >= 0 && newJ + dJ < Height)
                                        if (field[newI + dI][newJ + dJ] != -1)
                                            field[newI + dI][newJ + dJ] = CountMines(newI + dI, newJ + dJ);
                            }
                        }
                    }
                }
            }

            if (markers[i][j] == 1)         //cell is marked with flag - not to open
            {
                CurrentGameState.State = GameState.InProgress;
                return CurrentGameState;
            }
            if (field[i][j] == -1)          //BOOM!!!
            {
                OpenField();
                CurrentGameState.State = GameState.Loose;
                CurrentGameState.NumMinesBombed++;
                CurrentGameState.BombedMines = new CellCoordinates[1];
                CurrentGameState.BombedMines[0] = new CellCoordinates { Column = i, Row = j };
                return CurrentGameState;
            }
            AutoOpenCells(i, j);            //auto open current cell and all the empty cells or cells with numbers
            if (AllOpened())                                //when all cells (except mined) are opened
            {
                OpenField();                                //open the whole field
                CurrentGameState.State = GameState.Win;     //and say that user WON the game
            }
            return CurrentGameState;        //game is still in progress
        }

        /// <summary>
        /// Open the specified cell and the cells that surrounds the specified one
        /// </summary>
        /// <param name="i">Column of the specified cell</param>
        /// <param name="j">Row of the specified cell</param>
        /// <returns>Enumeration representing the current game state</returns>
        public CurrentGameStateInfo OpenMany(int i, int j)
        {
            if (CurrentGameState.State == GameState.Stopped) return CurrentGameState;

            if (CurrentGameState.State == GameState.Win || CurrentGameState.State == GameState.Loose)
            {
                CurrentGameState.State = GameState.Stopped;
                return CurrentGameState;
            }

            if (!state[i][j]) return CurrentGameState;  //if not opened cell was clicked do nothing

            int numFlagsAround = field[i][j]; //number of flags required in the surrounding cells
            //going through the surrounding cells
            for (int di = -1; di <= 1; di++)
            {
                if (i + di < 0 || i + di >= Width) continue;    //out of the field's borders
                for (int dj = -1; dj <= 1; dj++)
                {
                    if (di == 0 && dj == 0) continue;   //we'll skip current cell
                    if (j + dj < 0 || j + dj >= Height) continue;    //out of the field's borders
                    if (markers[i + di][j + dj] == 1) numFlagsAround--;  //found flag or question - decrease number of required flags
                }
            }

            if (numFlagsAround == 0)    //number of flags around equals to the number in current cell
            {
                //we'll open all cells in the region
                for (int dI = -1; dI <= 1; dI++)
                {
                    if (i + dI < 0 || i + dI >= Width) continue;
                    for (int dJ = -1; dJ <= 1; dJ++)
                    {
                        if (j + dJ < 0 || j + dJ >= Height) continue;

                        CurrentGameState = OpenCell(i + dI, j + dJ);
                        if (CurrentGameState.State == GameState.Loose ||
                            CurrentGameState.State == GameState.Win)

                            break;
                    }
                    if (CurrentGameState.State == GameState.Loose ||
                            CurrentGameState.State == GameState.Win)

                        break;
                }
            }

            return CurrentGameState;
        }

        /// <summary>
        /// Opens the entire field
        /// </summary>
        private void OpenField()
        {
            for (int i = 0; i < Width; i++)
            {
                Array.Clear(markers[i], 0, Height);                     //clear all markers
                for (int j = 0; j < Height; j++) state[i][j] = true;    //open each cell in the field
            }
        }

        /// <summary>
        /// Calculates 3BV difficulty level for current minefield
        /// </summary>
        private void Calculate3BVLevel()
        {
            Level3BV = 0;

            //we will simply open the whole field and count the minimal number of "clicks" we needed for it
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (field[i][j] == -1)  //it is mine
                    {
                        state[i][j] = true;         //we'll just open this cell
                    }
                    if (field[i][j] == 0)   //empty cell
                    {
                        if (!state[i][j])           //and it is not opened (this opening is not counted yet)
                        {
                            Level3BV++;             //count it
                            AutoOpenCells(i, j);    //and open the whole opening
                        }
                    }
                    if (field[i][j] > 0)    //cell with number
                    {
                        //we'll look around this cell and if we won't find empty cells we'll count this one
                        bool NoEmptyAround = true;
                        for(int dI = -1; dI <= 1; dI++)
                        {
                            if (i + dI < 0 || i + dI >= Width) continue;
                            for(int dJ = -1; dJ <= 1; dJ++)
                            {
                                if (j + dJ < 0 || j + dJ >= Height) continue;
                                if (field[i + dI][j + dJ] == 0) NoEmptyAround = false;
                            }
                        }
                        if (NoEmptyAround) Level3BV++;  //no empty cells - count this one
                        state[i][j] = true;             //open the current cell
                    }
                }
            }

            //and now we will close the field
            for (int i = 0; i < Width; i++) Array.Clear(state[i], 0, Height);
        }
    }
}
