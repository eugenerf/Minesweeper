using System;

namespace MineSweeper
{
    /// <summary>
    /// Minesweeper game settings
    /// </summary>
    [Serializable]
    public class MinesSettings
    {
        /// <summary>
        /// Lock in case of using multythreading
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
        /// Settings presets enumeration
        /// </summary>
        public enum Preset
        {
            Newbie,
            Advanced,
            Professional,
            Custom
        }

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

        /// <summary>
        /// Empty ctor (creates the default game settings)
        /// </summary>
        private MinesSettings()
        {
            ChangeSettings(Preset.Newbie, true);
        }

        /// <summary>
        /// Construct by preset
        /// </summary>
        /// <param name="preset">Preset name (Custom here equals to Newbie)</param>
        private MinesSettings (Preset preset, bool useQuestionMarks)
        {
            ChangeSettings(preset, useQuestionMarks);
        }

        /// <summary>
        /// Construct by numeric parameters
        /// </summary>
        /// <param name="width">Field Width (min: 9, max: 30)</param>
        /// <param name="height">Field Height (min: 9, max: 24)</param>
        /// <param name="mines">Number of mines on the field (min: 1, max: 668, not more than the number of fields)</param>
        private MinesSettings(int width, int height, uint mines, bool useQuestionMarks)
        {
            ChangeSettings(width, height, mines, useQuestionMarks);
        }

        /// <summary>
        /// Set default settings
        /// </summary>
        /// <returns>Unique instance of MineSettings class</returns>
        public static MinesSettings setSettings()
        {
            if (Instance == null)
            {
                lock (multyThreadLock)
                {
                    if (Instance == null) Instance = new MinesSettings();
                }
            }
            else
            {
                Instance.ChangeSettings(Preset.Newbie, true);
            }
            return Instance;
        }

        /// <summary>
        /// Set default settings by preset
        /// </summary>
        /// <returns>Unique instance of MineSettings class</returns>
        public static MinesSettings setSettings(Preset preset, bool useQuestionMarks)
        {
            if (Instance == null)
            {
                lock (multyThreadLock)
                {
                    if (Instance == null) Instance = new MinesSettings(preset, useQuestionMarks);
                }
            }
            else
            {
                Instance.ChangeSettings(preset, useQuestionMarks);
            }
            return Instance;
        }

        /// <summary>
        /// Set default settings by numeric parameters
        /// </summary>
        /// <returns>Unique instance of MineSettings class</returns>
        public static MinesSettings setSettings(int width, int height, uint mines, bool useQuestionMarks)
        {
            if (Instance == null)
            {
                lock (multyThreadLock)
                {
                    if (Instance == null) Instance = new MinesSettings(width, height, mines, useQuestionMarks);
                }
            }
            else
            {
                Instance.ChangeSettings(width, height, mines, useQuestionMarks);
            }
            return Instance;
        }

        /// <summary>
        /// Changes current settings by preset
        /// </summary>
        /// <param name="preset">Preset name (Custom here equals to Newbie)</param>
        private void ChangeSettings(Preset preset, bool useQuestionMarks)
        {
            CurrentPreset = preset;
            UseQuestionMarks = useQuestionMarks;
            switch (preset)
            {
                case Preset.Newbie:
                case Preset.Custom:
                    FieldWidth = 9;
                    FieldHeight = 9;
                    NumMines = 10;
                    break;
                case Preset.Advanced:
                    FieldWidth = 16;
                    FieldHeight = 16;
                    NumMines = 40;
                    break;
                case Preset.Professional:
                    FieldWidth = 30;
                    FieldHeight = 16;
                    NumMines = 99;
                    break;
            }
        }
        
        /// <summary>
        /// Changes current settings by numeric parameters
        /// </summary>
        /// <param name="width">Field Width (min: 9, max: 30)</param>
        /// <param name="height">Field Height (min: 9, max: 24)</param>
        /// <param name="mines">Number of mines on the field (min: 1, max: 668, not more than the number of fields)</param>
        private void ChangeSettings(int width, int height, uint mines, bool useQuestionMarks)
        {
            if (width < MinWidth || width > MaxWidth)
                throw new ArgumentOutOfRangeException("width", "Width must be between " + MinWidth + " and " + MaxWidth);
            if (height < MinHeight || height > MaxHeight)
                throw new ArgumentOutOfRangeException("height", "Height must be between " + MinHeight + " and " + MaxHeight);
            if (mines > GetMaxMines(width, height))
                throw new ArgumentOutOfRangeException(
                    "numMines",
                    "Too many mines for the current field dimensions");
            if (mines < 1)
                throw new ArgumentOutOfRangeException("numMines", "Number of mines must be more than 1");

            CurrentPreset = Preset.Custom;
            if (width == 9 && height == 9 && mines == 10) CurrentPreset = Preset.Newbie;
            if (width == 16 && height == 16 && mines == 40) CurrentPreset = Preset.Advanced;
            if (width == 30 && height == 16 && mines == 99) CurrentPreset = Preset.Professional;

            FieldWidth = width;
            FieldHeight = height;
            NumMines = mines;
            UseQuestionMarks = useQuestionMarks;
        }

        /// <summary>
        /// Get the maximum number of mines for the specified field dimensions
        /// </summary>
        /// <param name="width">Width of the field</param>
        /// <param name="height">Height of the field</param>
        /// <returns></returns>
        public static uint GetMaxMines(int width, int height)
        {
            if (width < MinWidth || width > MaxWidth) return 0;
            if (height < MinHeight || height > MaxHeight) return 0;
            return (uint)((width - 1) * (height - 1));
        }
    }
}
