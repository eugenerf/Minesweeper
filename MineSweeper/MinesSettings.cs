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
        /// Use standard double-button click if true
        /// (like in MS Minesweeper:
        ///     double-click region must cover at leat alredy opened cell, 
        ///     number of flags and questions must be equal to the number of mines,
        ///     if flags and questions are placed correctly, then open and OK,
        ///     if there are mistakes in flags and questions placing, BOOM!);
        /// (alternative^ Safe DoubleClick:
        ///     double-click opens region always if it is safe (no unmarked mines),
        ///     but if there are mistakes in mines marking (some flags or questions are not on mines), region opens and BOOM!)
        /// </summary>
        public bool UseStdDoubleClick;

        /// <summary>
        /// Construct by preset
        /// </summary>
        /// <param name="preset">Preset name (Custom here equals to Newbie)</param>
        public MinesSettings (Preset preset, bool useQuestionMarks, bool useStdDoubleClick)
        {
            ChangeSettings(preset, useQuestionMarks, useStdDoubleClick);
        }

        /// <summary>
        /// Construct by numeric parameters
        /// </summary>
        /// <param name="width">Field Width (min: 9, max: 30)</param>
        /// <param name="height">Field Height (min: 9, max: 24)</param>
        /// <param name="mines">Number of mines on the field (min: 1, max: 668, not more than the number of fields)</param>
        public MinesSettings(int width, int height, uint mines, bool useQuestionMarks, bool useStdDoubleClick)
        {
            ChangeSettings(width, height, mines, useQuestionMarks, useStdDoubleClick);
        }

        /// <summary>
        /// Changes current settings by preset
        /// </summary>
        /// <param name="preset">Preset name (Custom here equals to Newbie)</param>
        public void ChangeSettings(Preset preset, bool useQuestionMarks, bool useStdDoubleClick)
        {
            CurrentPreset = preset;
            UseQuestionMarks = useQuestionMarks;
            UseStdDoubleClick = useStdDoubleClick;
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
        public void ChangeSettings(int width, int height, uint mines, bool useQuestionMarks, bool useStdDoubleClick)
        {
            if (width < 9 || width > 30)
                throw new ArgumentOutOfRangeException("width", "Width must be between 9 and 30");
            if (height < 9 || height > 24)
                throw new ArgumentOutOfRangeException("height", "Height must be between 9 and 24");
            if (mines > width * height)
                throw new ArgumentOutOfRangeException(
                    "numMines",
                    "Number of mines cannot exceed number of cells in the minefield");
            if (mines < 1 || mines > 668)
                throw new ArgumentOutOfRangeException("numMines", "Number of mines must be between 1 and 668");

            CurrentPreset = Preset.Custom;
            if (width == 9 && height == 9 && mines == 10) CurrentPreset = Preset.Newbie;
            if (width == 16 && height == 16 && mines == 40) CurrentPreset = Preset.Advanced;
            if (width == 30 && height == 16 && mines == 99) CurrentPreset = Preset.Professional;

            FieldWidth = width;
            FieldHeight = height;
            NumMines = mines;
            UseQuestionMarks = useQuestionMarks;
            UseStdDoubleClick = useStdDoubleClick;
        }
    }
}
