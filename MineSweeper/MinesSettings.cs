using System;

namespace MineSweeper
{
    [Serializable]
    public class MinesSettings
    {
        /// <summary>
        /// Settings presets enumeration
        /// </summary>
        public enum Preset
        {
            Newbie,
            Amateur,
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
        /// Construct by preset
        /// </summary>
        /// <param name="preset">Preset name (Custom here equals to Newbie)</param>
        public MinesSettings (Preset preset)
        {
            switch (preset)
            {
                case Preset.Newbie:
                case Preset.Custom:
                    FieldWidth = 9;
                    FieldHeight = 9;
                    NumMines = 10;
                    break;
                case Preset.Amateur:
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
        /// Construct by numeric parameters
        /// </summary>
        /// <param name="width">Field Width (min: 9, max: 30)</param>
        /// <param name="height">Field Height (min: 9, max: 24)</param>
        /// <param name="mines">Number of mines on the field (min: 1, max: 668, not more than the number of fields)</param>
        public MinesSettings(int width, int height, uint mines)
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
            if (width == 16 && height == 16 && mines == 40) CurrentPreset = Preset.Amateur;
            if (width == 30 && height == 16 && mines == 99) CurrentPreset = Preset.Professional;

            FieldWidth = width;
            FieldHeight = height;
            NumMines = mines;
        }
    }
}
