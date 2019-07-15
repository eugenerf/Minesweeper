using System;

namespace MineSweeper
{
    /// <summary>
    /// Minesweeper game settings
    /// </summary>
    [Serializable]  //class is SOAP-serializable (needed to save game settings to the file)
    public class MinesSettings
    {
        #region Enums
        /// <summary>
        /// Settings presets enumeration
        /// </summary>
        public enum Preset
        {
            /// <summary>
            /// Newbie - the easiest game preset
            /// </summary>
            Newbie,
            /// <summary>
            /// Advanced - game will take some time
            /// </summary>
            Advanced,
            /// <summary>
            /// Professional - you'll need several attempts to complete this game
            /// </summary>
            Professional,
            /// <summary>
            /// Custom - player himself sets game settings
            /// </summary>
            Custom
        }   //ENDENUM (Preset)
        #endregion

        #region Fields
        /// <summary>
        /// Lock in case of use multythreading
        /// </summary>
        private static object multyThreadLock = new Object();
        /// <summary>
        /// Unique instance of MinesSettings class
        /// </summary>
        private static MinesSettings Instance;
        /// <summary>
        /// Minimal field width
        /// </summary>
        public const int MinWidth = 9;
        /// <summary>
        /// Maximal field width
        /// </summary>
        public const int MaxWidth = 30;
        /// <summary>
        /// Minimal field height
        /// </summary>
        public const int MinHeight = 9;
        /// <summary>
        /// Maximal field height
        /// </summary>
        public const int MaxHeight = 24;
        /// <summary>
        /// Current settings preset
        /// </summary>
        public Preset CurrentPreset;
        /// <summary>
        /// Width of the minefield
        /// </summary>
        public int FieldWidth;
        /// <summary>
        /// Height of the minefield
        /// </summary>
        public int FieldHeight;
        /// <summary>
        /// Number of mines on the minefield
        /// </summary>
        public uint NumMines;
        /// <summary>
        /// Use question marks if true
        /// </summary>
        public bool UseQuestionMarks;
        #endregion

        #region Methods
        /// <summary>
        /// Empty MineSettings ctor (creates the default game settings)
        /// </summary>
        private MinesSettings()
        {
            //ctor is private. MineSettings is developped as a Singleton.
            //it is needed to prevent creation of more than one settings instance for the game.
            ChangeSettings(Preset.Newbie, true);    //set new settings to Newbie
        }   //END (ctor - default settings)

        /// <summary>
        /// MineSettings ctor by Preset
        /// </summary>
        /// <param name="preset">Preset name (Custom here equals to Newbie)</param>
        private MinesSettings (Preset preset, bool useQuestionMarks)
        {
            //ctor is private. MineSettings is developped as a Singleton.
            //it is needed to prevent creation of more than one settings instance for the game.
            ChangeSettings(preset, useQuestionMarks);   //set settings to the specified preset
        }   //END (ctor - by preset)

        /// <summary>
        /// MineSettings ctor by numeric values
        /// </summary>
        /// <param name="width">Field Width (min: 9, max: 30)</param>
        /// <param name="height">Field Height (min: 9, max: 24)</param>
        /// <param name="mines">Number of mines on the field (min: 1, max: not more than (width-1)*(height-1))</param>
        private MinesSettings(int width, int height, uint mines, bool useQuestionMarks)
        {
            //ctor is private. MineSettings is developped as a Singleton.
            //it is needed to prevent creation of more than one settings instance for the game.
            ChangeSettings(width, height, mines, useQuestionMarks); //change settings according to specified values
        }   //END (ctor - by numeric values)

        /// <summary>
        /// Set default settings
        /// </summary>
        /// <returns>Unique instance of MineSettings class</returns>
        public static MinesSettings setSettings()
        {
            if (Instance == null)                           //if class instance is not created yet
                lock (multyThreadLock)                          //lock static members of class
                {
                    if (Instance == null)                           //if class instance is still not created
                        Instance = new MinesSettings();                 //create unique instance of class with default settings
                }   //ENDLOCK (multy thread lock)
            else                                            //if class instance is already created
                Instance.ChangeSettings(Preset.Newbie, true);   //change settings to default
            return Instance;                                //return unique instance
        }   //END (setSettings - default)

        /// <summary>
        /// Set default settings by preset
        /// </summary>
        /// <returns>Unique instance of MineSettings class</returns>
        public static MinesSettings setSettings(Preset preset, bool useQuestionMarks)
        {
            if (Instance == null)                               //if class instance is not created
                lock (multyThreadLock)                              //lock static members of class
                {
                    if (Instance == null)                               //if class instance is still not created
                        Instance =
                            new MinesSettings(preset,
                                            useQuestionMarks);              //create instance with specified preset
                }   //ENDLOCK (multy thread lock)
            else                                                //if class instance is already created
                Instance.ChangeSettings(preset,
                                        useQuestionMarks);          //change its settings according the specified preset
            return Instance;                                    //return unique instance of class
        }   //END (setSettings - by preset)

        /// <summary>
        /// Set default settings by numeric parameters
        /// </summary>
        /// <returns>Unique instance of MineSettings class</returns>
        public static MinesSettings setSettings(int width, int height, uint mines, bool useQuestionMarks)
        {
            if (Instance == null)                                       //if class instance is not created
                lock (multyThreadLock)                                      //lock static members of the class
                {
                    if (Instance == null)                                       //if class instance is still not created
                        Instance = new MinesSettings(width,
                                                    height,
                                                    mines,
                                                    useQuestionMarks);              //create new instance using specified values
                }   //ENDLOCK (multy thread lock)
            else                                                        //if class instance is already created
                Instance.ChangeSettings(width,
                                        height,
                                        mines,
                                        useQuestionMarks);                  //change its settings according to specified values
            return Instance;                                            //return unique instance of the class
        }   //END (setSettings - by numeric values)

        /// <summary>
        /// Change current settings by preset
        /// </summary>
        /// <param name="preset">Preset name (Custom here equals to Newbie)</param>
        private void ChangeSettings(Preset preset, bool useQuestionMarks)
        {
            CurrentPreset =                         //set current preset to:
                (preset == Preset.Custom) ?             //if specified preset is Custom
                Preset.Newbie :                             //set to Newbie
                preset;                                     //otherwise use specified preset
            UseQuestionMarks = useQuestionMarks;    //set flag of using questions
            switch (preset)                         //switch between presets
            {
                case Preset.Newbie:                     //Newbie
                    FieldWidth = 9;                         //set field width
                    FieldHeight = 9;                        //set field height
                    NumMines = 10;                          //set mines number
                    break;
                case Preset.Advanced:                   //Advanced
                    FieldWidth = 16;                        //set field width
                    FieldHeight = 16;                       //set field height
                    NumMines = 40;                          //set mines number
                    break;
                case Preset.Professional:               //Professional
                    FieldWidth = 30;                        //set field width
                    FieldHeight = 16;                       //set field height
                    NumMines = 99;                          //set mines number
                    break;
            }   //ENDSWITCH (preset)
        }   //END (ChangeSettings - by preset)

        /// <summary>
        /// Change current settings by numeric parameters
        /// </summary>
        /// <param name="width">Field Width (min: 9, max: 30)</param>
        /// <param name="height">Field Height (min: 9, max: 24)</param>
        /// <param name="mines">Number of mines on the field (min: 1, max: not more than (width-1)*(height-1))</param>
        private void ChangeSettings(int width, int height, uint mines, bool useQuestionMarks)
        {
            if (width < MinWidth || width > MaxWidth)                               //if field width value is invalid
                throw new ArgumentOutOfRangeException(
                    "width", 
                    "Width must be between " + MinWidth + " and " + MaxWidth);          //throw exception
            if (height < MinHeight || height > MaxHeight)                           //if field height value is invalid
                throw new ArgumentOutOfRangeException(
                    "height", 
                    "Height must be between " + MinHeight + " and " + MaxHeight);       //throw exception
            if (mines > GetMaxMines(width, height))                                 //if number of mines is more than maximal allowed
                throw new ArgumentOutOfRangeException(
                    "numMines",
                    "Too many mines for the current field dimensions");                 //throw exception
            if (mines < 1)                                                          //if number of mines is less than 1
                throw new ArgumentOutOfRangeException(
                    "numMines", 
                    "Number of mines must be 1 or more");                               //throw exception
            CurrentPreset = Preset.Custom;                                          //set preset to custom
            if (width == 9 && height == 9 && mines == 10)                           //if numeric values are equal to Newbie preset
                CurrentPreset = Preset.Newbie;                                          //set preset to Newbie
            if (width == 16 && height == 16 && mines == 40)                         //if numeric values are equal to Advanced preset
                CurrentPreset = Preset.Advanced;                                        //set preset to Advanced
            if (width == 30 && height == 16 && mines == 99)                         //if numeric values are equal to Professional preset
                CurrentPreset = Preset.Professional;                                    //set preset to Professional
            FieldWidth = width;                                                     //set field width
            FieldHeight = height;                                                   //set field height
            NumMines = mines;                                                       //set number of mines
            UseQuestionMarks = useQuestionMarks;                                    //set using of questions flag
        }   //END (ChangeSettings - by numeric values)

        /// <summary>
        /// Get the maximum number of mines for the specified field dimensions
        /// </summary>
        /// <param name="width">Width of the field</param>
        /// <param name="height">Height of the field</param>
        /// <returns>Max number of mines value or zero if field dimensions are invalid</returns>
        public static uint GetMaxMines(int width, int height)
        {
            if (width < MinWidth || width > MaxWidth)       //if width is invalid
                return 0;                                       //return zero
            if (height < MinHeight || height > MaxHeight)   //if height is invalid
                return 0;                                       //return zero
            return (uint)((width - 1) * (height - 1));      //return max mines value
        }   //END (GetMaxMines)
        #endregion
    }   //ENDCLASS (MinesSettings)
}   //ENDNAMESPACE (MineSweeper)
