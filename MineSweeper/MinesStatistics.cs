using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public struct WinnerInfo : IComparable
        {
            /// <summary>
            /// Game time in seconds
            /// </summary>
            public uint GameTime;

            /// <summary>
            /// Winner name
            /// </summary>
            public string Name;

            /// <summary>
            /// Date and time when win took place
            /// </summary>
            public DateTime WinTime;

            public int CompareTo(object obj)
            {
                WinnerInfo wi = (WinnerInfo)obj;
                return (int)GameTime - (int)wi.GameTime;
            }
        }

        /// <summary>
        /// Information about statistics for one game setting (preset)
        /// </summary>
        [Serializable]
        public struct PresetStatsInfo
        {
            /// <summary>
            /// Preset name
            /// </summary>
            public MinesSettings.Preset Preset;

            /// <summary>
            /// Top five winners for this preset
            /// </summary>
            public WinnerInfo[] TopFive;

            /// <summary>
            /// Total games played on this preset
            /// </summary>
            public uint TotalGames;

            /// <summary>
            /// Games won on this preset
            /// </summary>
            public uint WinGames;

            /// <summary>
            /// Win percent on this preset
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
        /// Game statistics
        /// </summary>
        public PresetStatsInfo[] Stats;

        public MinesStatistics()
        {
            Stats = new PresetStatsInfo[3];
            
            Array.Clear(Stats, 0, 3);

            Stats[0].Preset = MinesSettings.Preset.Newbie;
            Stats[1].Preset = MinesSettings.Preset.Amateur;
            Stats[2].Preset = MinesSettings.Preset.Professional;
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
            for (int i = 0; i < 3; i++)
            {
                if (Stats[i].Preset == ms.CurrentPreset)
                {
                    index = i;
                    break;
                }
            }
            if (index < 0 || index > 2) return;

            if (me.CurrentGameState.State != MinesEngine.GameState.Loose &&
                me.CurrentGameState.State != MinesEngine.GameState.Win)
            {
                return;
            }

            Stats[index].TotalGames++;
            if (me.CurrentGameState.State == MinesEngine.GameState.Win)     //game WON
            {
                if (gameTime > Stats[index].LongestGameTime) Stats[index].LongestGameTime = gameTime;
                Stats[index].WinGames++;
                Stats[index].WinPercent = Stats[index].WinGames / (double)Stats[index].TotalGames * 100.0;
                Stats[index].WinStreak++;
                if (Stats[index].LooseStreak > Stats[index].MaxLooseStreak)
                    Stats[index].MaxLooseStreak = Stats[index].LooseStreak;
                Stats[index].LooseStreak = 0;

                Stats[index].FastestGameTime = AddToTopFive(index, curTime, gameTime, fms);
            }
            else                                                            //game LOST
            {
                if (Stats[index].WinStreak > Stats[index].MaxWinStreak)
                    Stats[index].MaxWinStreak = Stats[index].WinStreak;
                Stats[index].WinStreak = 0;
                Stats[index].LooseStreak++;
            }
        }

        /// <summary>
        /// Add game data to TopFive if needed
        /// </summary>
        /// <param name="index">Index of the current preset</param>
        /// <param name="now">CUrrent date and time</param>
        /// <param name="gameTime">Current game time</param>
        /// <param name="fms">Form that called this method</param>
        /// <returns>Fastest game time from the TopFive table</returns>
        private uint AddToTopFive(int index, DateTime now, uint gameTime, FormMineSweeper fms)
        {
            int topLength = (Stats[index].TopFive == null) ? 0 : Stats[index].TopFive.Length;

            if (topLength >= 5)
            {
                if (Stats[index].TopFive[topLength - 1].GameTime > gameTime)
                {
                    Array.Resize(ref Stats[index].TopFive, ++topLength);
                    Stats[index].TopFive[topLength - 1].GameTime = gameTime;
                    Stats[index].TopFive[topLength - 1].Name = fms.AskForUserName();
                    Stats[index].TopFive[topLength - 1].WinTime = now;

                    Array.Sort(Stats[index].TopFive);
                    Array.Resize(ref Stats[index].TopFive, (topLength > 5 ? 5 : topLength));
                }
            }
            else
            {
                Array.Resize(ref Stats[index].TopFive, ++topLength);
                Stats[index].TopFive[topLength - 1].GameTime = gameTime;
                Stats[index].TopFive[topLength - 1].Name = fms.AskForUserName();
                Stats[index].TopFive[topLength - 1].WinTime = now;

                Array.Sort(Stats[index].TopFive);
                Array.Resize(ref Stats[index].TopFive, (topLength > 5 ? 5 : topLength));
            }

            
            return Stats[index].TopFive[0].GameTime;
        }
    }
}
