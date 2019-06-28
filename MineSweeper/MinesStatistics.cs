using System;
using System.Collections.Generic;

namespace MineSweeper
{
    /// <summary>
    /// Minesweeper game statistics
    /// </summary>
    [Serializable]
    public class MinesStatistics
    {
        /// <summary>
        /// Information about one winner
        /// </summary>
        [Serializable]
        public struct WinnerInfo : IComparer<WinnerInfo>
        {
            /// <summary>
            /// Game time in seconds
            /// </summary>
            public uint GameTime;

            /// <summary>
            /// 3BV difficulty level
            /// </summary>
            public uint Level3BV;

            /// <summary>
            /// Winner name
            /// </summary>
            public string Name;

            /// <summary>
            /// Played on preset
            /// </summary>
            public MinesSettings.Preset Preset;

            /// <summary>
            /// Date and time when win took place
            /// </summary>
            public DateTime WinTime;

            /// <summary>
            /// Comparator by Level3BV field and then by GameTime
            /// </summary>
            /// <param name="wi1">First struct to compare</param>
            /// <param name="wi2">Second struct to compare</param>
            /// <returns>Zero if equals,
            ///         less than zero if first structure must be to the left of the second,
            ///         more than zero if first structure must be to the right of the second</returns>
            public int CompareByLevel3BV(WinnerInfo wi1, WinnerInfo wi2)
            {
                if (wi1.Level3BV == wi2.Level3BV) return CompareByGameTime(wi1, wi2);
                return (int)wi2.Level3BV - (int)wi1.Level3BV;
            }

            /// <summary>
            /// Comparator by GameTime field
            /// </summary>
            /// <param name="wi1">First struct to compare</param>
            /// <param name="wi2">Second struct to compare</param>
            /// <returns>Zero if equals,
            ///         less than zero if first structure must be to the left of the second,
            ///         more than zero if first structure must be to the right of the second</returns>
            public int CompareByGameTime(WinnerInfo wi1, WinnerInfo wi2)
            {
                return (int)wi1.GameTime - (int)wi2.GameTime;
            }

            /// <summary>
            /// Comparator by GameTime field (same as CompareByGameTime)
            /// </summary>
            /// <param name="wi1">First struct to compare</param>
            /// <param name="wi2">Second struct to compare</param>
            /// <returns>Zero if equals,
            ///         less than zero if first structure must be to the left of the second,
            ///         more than zero if first structure must be to the right of the second</returns>
            public int Compare(WinnerInfo wi1,WinnerInfo wi2)
            {
                return CompareByGameTime(wi1, wi2);
            }
        }

        /// <summary>
        /// Information about statistics by Preset
        /// </summary>
        [Serializable]
        public struct PresetStatsInfo
        {
            /// <summary>
            /// Preset name
            /// </summary>
            public MinesSettings.Preset Preset;

            /// <summary>
            /// Top winners
            /// </summary>
            public WinnerInfo[] Top;

            /// <summary>
            /// Total games played
            /// </summary>
            public uint TotalGames;

            /// <summary>
            /// Games won
            /// </summary>
            public uint WinGames;

            /// <summary>
            /// Win percent
            /// </summary>
            public double WinPercent;

            /// <summary>
            /// Games won current streak
            /// </summary>
            public uint WinStreak;

            /// <summary>
            /// Games lost current streak
            /// </summary>
            public uint LooseStreak;

            /// <summary>
            /// Longest win game time in seconds
            /// </summary>
            public uint LongestGameTime;

            /// <summary>
            /// Fastest win game time in seconds (e.g. time of the first place in Top 5)
            /// </summary>
            public uint FastestGameTime;

            /// <summary>
            /// Games won maximum streak
            /// </summary>
            public uint MaxWinStreak;

            /// <summary>
            /// Games lost maximum streak
            /// </summary>
            public uint MaxLooseStreak;
        }

        /// <summary>
        /// Unique instance of MinesStatistics class
        /// </summary>
        private static MinesStatistics Instance;

        /// <summary>
        /// Lock in case of using multythreading
        /// </summary>
        private static object multyThreadLock = new Object();

        /// <summary>
        /// Game statistics grouped  by preset
        /// </summary>
        public PresetStatsInfo[] StatsByPreset;

        /// <summary>
        /// Game statistics grouped by 3BV
        /// </summary>
        public WinnerInfo[] StatsBy3BV;

        private MinesStatistics()
        {
            StatsByPreset = new PresetStatsInfo[3];
            StatsBy3BV = null;
            
            Array.Clear(StatsByPreset, 0, 3);

            StatsByPreset[0].Preset = MinesSettings.Preset.Newbie;
            StatsByPreset[1].Preset = MinesSettings.Preset.Advanced;
            StatsByPreset[2].Preset = MinesSettings.Preset.Professional;
        }

        /// <summary>
        /// Get unique instance of the MinesStatistics class
        /// </summary>
        /// <returns>Unique instance of MinesStatistics</returns>
        public static MinesStatistics getInstance()
        {
            if (Instance == null)
            {
                lock (multyThreadLock)
                {
                    if (Instance == null) Instance = new MineSweeper.MinesStatistics();
                }
            }
            return Instance;
        }

        /// <summary>
        /// Calculate stats for current game
        /// </summary>
        /// <param name="me">Engine of the current game</param>
        /// <param name="ms">Settings of the current game</param>
        /// <param name="fms">Form that called this method</param>
        public void CalcStats(MinesEngine me, MinesSettings ms, uint gameTime, FormMineSweeper fms = null)
        {
            DateTime curTime = DateTime.Now;
            int index = -1;     //index of the current preset in the stats array
                        
            if (me.CurrentGameState.State != MinesEngine.GameState.Loose &&
                me.CurrentGameState.State != MinesEngine.GameState.Win)
            {
                return;
            }

            uint level3BV = me.Level3BV;

            switch (ms.CurrentPreset)
            {
                case MinesSettings.Preset.Newbie: index = 0; break;
                case MinesSettings.Preset.Advanced: index = 1; break;
                case MinesSettings.Preset.Professional: index = 2; break;
                case MinesSettings.Preset.Custom:
                    if(me.CurrentGameState.State== MinesEngine.GameState.Win)
                        AddToTop(3, curTime, gameTime, level3BV, fms);
                    return;
                default: return;
            }

            StatsByPreset[index].TotalGames++;
            if (me.CurrentGameState.State == MinesEngine.GameState.Win)     //game WON
            {
                if (gameTime > StatsByPreset[index].LongestGameTime) StatsByPreset[index].LongestGameTime = gameTime;
                StatsByPreset[index].WinGames++;
                StatsByPreset[index].WinPercent = StatsByPreset[index].WinGames / (double)StatsByPreset[index].TotalGames * 100.0;
                StatsByPreset[index].WinStreak++;
                if (StatsByPreset[index].LooseStreak > StatsByPreset[index].MaxLooseStreak)
                    StatsByPreset[index].MaxLooseStreak = StatsByPreset[index].LooseStreak;
                StatsByPreset[index].LooseStreak = 0;

                StatsByPreset[index].FastestGameTime = AddToTop(index, curTime, gameTime, level3BV, fms);
            }
            else                                                            //game LOST
            {
                if (StatsByPreset[index].WinStreak > StatsByPreset[index].MaxWinStreak)
                    StatsByPreset[index].MaxWinStreak = StatsByPreset[index].WinStreak;
                StatsByPreset[index].WinStreak = 0;
                StatsByPreset[index].LooseStreak++;
            }
        }

        /// <summary>
        /// Add game data to Top if needed (by preset - max 5 player records and by 3BV - max 10 player records)
        /// </summary>
        /// <param name="index">Index of the current preset</param>
        /// <param name="now">CUrrent date and time</param>
        /// <param name="gameTime">Current game time</param>
        /// <param name="fms">Form that called this method</param>
        /// <returns>Fastest game time from the Top table</returns>
        private uint AddToTop(int index, DateTime now, uint gameTime, uint level3BV, FormMineSweeper fms)
        {
            //first of all we'll check whether we have to add to StatsBy3BV
            int topLength = (StatsBy3BV == null) ? 0 : StatsBy3BV.Length;
            bool HaveToAdd = false;
            string playerName = null;

            if (topLength >= 10)
            {
                if (StatsBy3BV[topLength - 1].Level3BV < level3BV) HaveToAdd = true;
                else if (StatsBy3BV[topLength - 1].Level3BV > level3BV) HaveToAdd = false;
                else if (StatsBy3BV[topLength - 1].GameTime > gameTime) HaveToAdd = true;
            }
            else HaveToAdd = true;

            if (HaveToAdd)  //add to top
            {
                playerName = fms.AskForUserName();
                Array.Resize(ref StatsBy3BV, ++topLength);
                StatsBy3BV[topLength - 1].GameTime = gameTime;
                StatsBy3BV[topLength - 1].Name = playerName;
                StatsBy3BV[topLength - 1].WinTime = now;
                StatsBy3BV[topLength - 1].Level3BV = level3BV;
                switch (index)
                {
                    case 0: StatsBy3BV[topLength - 1].Preset = MinesSettings.Preset.Newbie; break;
                    case 1: StatsBy3BV[topLength - 1].Preset = MinesSettings.Preset.Advanced; break;
                    case 2: StatsBy3BV[topLength - 1].Preset = MinesSettings.Preset.Professional; break;
                    default: StatsBy3BV[topLength - 1].Preset = MinesSettings.Preset.Custom; break;
                }

                Array.Sort(StatsBy3BV, new WinnerInfo().CompareByLevel3BV);
                Array.Resize(ref StatsBy3BV, (topLength > 10 ? 10 : topLength));
            }

            if (index < 0 || index > 2) return StatsBy3BV[0].GameTime;  //Preset is not Newbie, Advanced or Professional

            //now we'll check whether we have to add to StatsByPreset
            topLength = (StatsByPreset[index].Top == null) ? 0 : StatsByPreset[index].Top.Length;
            HaveToAdd = false;

            if (topLength >= 5)
            {
                if (StatsByPreset[index].Top[topLength - 1].GameTime > gameTime) HaveToAdd = true;
            }
            else HaveToAdd = true;

            if (HaveToAdd)  //add to top
            {
                Array.Resize(ref StatsByPreset[index].Top, ++topLength);
                StatsByPreset[index].Top[topLength - 1].GameTime = gameTime;
                StatsByPreset[index].Top[topLength - 1].Name = (playerName == null) ? fms.AskForUserName() : playerName;
                StatsByPreset[index].Top[topLength - 1].WinTime = now;
                StatsByPreset[index].Top[topLength - 1].Level3BV = level3BV;
                switch (index)
                {
                    case 0: StatsByPreset[0].Top[topLength - 1].Preset = MinesSettings.Preset.Newbie; break;
                    case 1: StatsByPreset[1].Top[topLength - 1].Preset = MinesSettings.Preset.Advanced; break;
                    case 2: StatsByPreset[2].Top[topLength - 1].Preset = MinesSettings.Preset.Professional; break;
                }

                Array.Sort(StatsByPreset[index].Top, new WinnerInfo().CompareByGameTime);
                Array.Resize(ref StatsByPreset[index].Top, (topLength > 5 ? 5 : topLength));
            }

            return StatsByPreset[index].Top[0].GameTime;
        }

        /// <summary>
        /// Get full statistics according to 3BV difficulty level
        /// </summary>
        /// <returns>String containing statistics</returns>
        public string GetStats()
        {
            string res = "";

            for(int i = 0; i < StatsBy3BV?.Length; i++)
            {
                res += StatsBy3BV[i].Level3BV.ToString() + "\t";
                res += StatsBy3BV[i].GameTime.ToString() + "\t";
                res += StatsBy3BV[i].Name + "\t";
                res += StatsBy3BV[i].WinTime.ToShortTimeString() + "\t";
                res += StatsBy3BV[i].Preset.ToString() + "\r\n";
            }

            return res;
        }

        /// <summary>
        /// Get full statistics for specified preset
        /// </summary>
        /// <param name="preset">Preset to get statistics for</param>
        /// <returns>String containing statistics or null if preset is not Newbie, Advanced or Professional</returns>
        public string GetStats(MinesSettings.Preset preset)
        {
            string res = "";
            int index = 0;

            switch (preset)
            {
                case MinesSettings.Preset.Newbie: index = 0; break;
                case MinesSettings.Preset.Advanced: index = 1; break;
                case MinesSettings.Preset.Professional: index = 2; break;
                default: return null;
            }

            for(int i = 0; i < StatsByPreset[index].Top?.Length; i++)
            {
                res += StatsByPreset[index].Top[i].GameTime.ToString() + "\t";
                res += StatsByPreset[index].Top[i].Name + "\t";
                res += StatsByPreset[index].Top[i].WinTime.ToString() + "\t";
                res += StatsByPreset[index].Top[i].Level3BV.ToString() + "\r\n";
            }

            return res;
        }

        /// <summary>
        /// Clear statistics
        /// </summary>
        public void Clear()
        {
            Array.Resize(ref StatsBy3BV, 0);
            Array.Clear(StatsByPreset, 0, 3);

            StatsByPreset[0].Preset = MinesSettings.Preset.Newbie;
            StatsByPreset[1].Preset = MinesSettings.Preset.Advanced;
            StatsByPreset[2].Preset = MinesSettings.Preset.Professional;
        }
    }
}
