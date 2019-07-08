using System;
using System.Collections.Generic;

namespace MineSweeper
{
    /// <summary>
    /// Minesweeper game statistics
    /// </summary>
    [Serializable]  //this class is serializable. it is needed to save game stats when minesweeper is closed
    public class MinesStatistics
    {
        #region Delegates
        /// <summary>
        /// Delegate used in statistics calculation to ask player names
        /// </summary>
        /// <returns>String containing a player name</returns>
        public delegate string AskPlayerName();
        #endregion

        #region Structures
        /// <summary>
        /// Information about one winner
        /// </summary>
        [Serializable]  //thi struct is serializable with MinesStatistics class
        public struct WinnerInfo : IComparer<WinnerInfo>
        {
            #region Fields
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
            #endregion

            #region Methods
            /// <summary>
            /// Comparator by Level3BV field and then by GameTime field
            /// </summary>
            /// <param name="wi1">First struct to compare</param>
            /// <param name="wi2">Second struct to compare</param>
            /// <returns>Zero if equals,
            ///         less than zero if first structure must be to the left of the second,
            ///         more than zero if first structure must be to the right of the second</returns>
            public int CompareByLevel3BV(WinnerInfo wi1, WinnerInfo wi2)
            {
                if (wi1.Level3BV == wi2.Level3BV)               //if level3bv in first argument equals to level3bv in the second one
                    return CompareByGameTime(wi1, wi2);             //compare first and second by GameTime fields and return comparation result
                return (int)wi2.Level3BV - (int)wi1.Level3BV;   //return result of subtraction of level3bv-s in the first and in the second
                                                                //  and if first is less than second we will retun a value less than zero
                                                                //  otherwise we will return a value more than zero
            }   //END (CompareByLevel3BV)

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
                return (int)wi1.GameTime - (int)wi2.GameTime;   //return result of subtraction of gametime-s of the first and the second
                                                                //  if first is less than second we will return a value less than zero
                                                                //  otherwise we will return a value more than zero
            }   //END (CompareByGameTime)

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
                return CompareByGameTime(wi1, wi2); //compare by gametime field and return the result
            }   //END (Compare)
            #endregion
        }   //ENDSTRUCT (WinnerInfo)

        /// <summary>
        /// Information about statistics by Preset
        /// </summary>
        [Serializable]  //thi struct is serializable with MinesStatistics class
        public struct PresetStatsInfo
        {
            #region Fields
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
            #endregion
        }   //ENDSTRUCT (PresetStatsInfo)
        #endregion

        #region Fields
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
        #endregion

        #region Methods
        /// <summary>
        /// MinesStatistics ctor
        /// </summary>
        private MinesStatistics()
        {
            //ctor is private because MinesStatistics class is developped as a Singleton
            //it is needed to prevent creation of more than one instance of game stats
            StatsByPreset = new PresetStatsInfo[3];                         //get memory for stats by preset
            StatsBy3BV = null;                                              //clear stats by 3bv (just in case)
            Array.Clear(StatsByPreset, 0, 3);                               //clear stats by preset array (just in case too)
            //in stats by preset array there will be only 3 records (because game has only three presets)
            //  we will name presets in this array while constructing stats
            StatsByPreset[0].Preset = MinesSettings.Preset.Newbie;          //elementh 0 is for Newbie
            StatsByPreset[1].Preset = MinesSettings.Preset.Advanced;        //..................Advanced
            StatsByPreset[2].Preset = MinesSettings.Preset.Professional;    //..................Professional
        }   //END (ctor)

        /// <summary>
        /// Get unique instance of the MinesStatistics class
        /// </summary>
        /// <returns>Unique instance of MinesStatistics</returns>
        public static MinesStatistics getInstance()
        {
            if (Instance == null)                   //if instance of class is not created
                lock (multyThreadLock)                  //lock static elements of the class
                    if (Instance == null)                   //if instance of class is still not created
                        Instance = 
                            new MinesStatistics();              //create it
            return Instance;                        //return unique instance of the class
        }   //END (getInstance)

        /// <summary>
        /// Calculate stats for the current game
        /// </summary>
        /// <param name="me">Engine of the current game</param>
        /// <param name="ms">Settings of the current game</param>
        /// <param name="askPN">Handler of the method that asks for a player name</param>
        public void CalcStats(MinesEngine me, MinesSettings ms, uint gameTime, AskPlayerName askPN)
        {
            DateTime curTime = DateTime.Now;                                    //current date and time
            int index = -1;                                                     //index of the current preset in the stats array                        
            if (me.CurrentGameState.State != MinesEngine.GameState.Loose &&     //if game is not lost
                me.CurrentGameState.State != MinesEngine.GameState.Win)         //AND game is not won
                return;                                                             //do nothing and just return
            uint level3BV = me.Level3BV;                                        //get difficulty level of the game from the engine
            switch (ms.CurrentPreset)                                           //switch between game presets
            {
                case MinesSettings.Preset.Newbie: index = 0; break;                 //for Newbie index is zero
                case MinesSettings.Preset.Advanced: index = 1; break;               //for Advanced index is 1
                case MinesSettings.Preset.Professional: index = 2; break;           //for Professional index is 2
                case MinesSettings.Preset.Custom:                                   //for Custom we will not add stats by preset
                                                                                    //  but we will try to add a record to the top by difficulty level
                    if(me.CurrentGameState.State== MinesEngine.GameState.Win)           //if game is won
                        AddToTop(3, curTime, gameTime, level3BV, askPN);                    //try to add record to top list
                    return;                                                             //return
                default: return;                                                    //if somehow preset in game settings is invalid do nothing and return
            }   //ENDSWITCH (preset)
            StatsByPreset[index].TotalGames++;                                  //increase total games played for the current preset
            if (me.CurrentGameState.State == MinesEngine.GameState.Win)         //if game is won
            {
                if (gameTime > StatsByPreset[index].LongestGameTime)                //if current game duration is more than the saved in stats one
                    StatsByPreset[index].LongestGameTime = gameTime;                    //save current game duration
                StatsByPreset[index].WinGames++;                                    //increase won games counter
                StatsByPreset[index].WinPercent =
                    StatsByPreset[index].WinGames / 
                    (double)StatsByPreset[index].TotalGames * 
                    100.0;                                                          //calculate the new win-percentage
                StatsByPreset[index].WinStreak++;                                   //increase win streak counter
                if (StatsByPreset[index].LooseStreak > 
                    StatsByPreset[index].MaxLooseStreak)                            //if current loose streak is longer than the saved one
                    StatsByPreset[index].MaxLooseStreak = 
                        StatsByPreset[index].LooseStreak;                               //save the current loose streak as the maximal
                StatsByPreset[index].LooseStreak = 0;                               //reset current loose streak
                StatsByPreset[index].FastestGameTime = 
                    AddToTop(index, curTime, gameTime, level3BV, askPN);            //try to add current game to top
            }   //ENDIF (game is won)
            else                                                                //if game is LOST
            {
                if (StatsByPreset[index].WinStreak > 
                    StatsByPreset[index].MaxWinStreak)                              //if current win streak is longer than the saved one
                    StatsByPreset[index].MaxWinStreak = 
                        StatsByPreset[index].WinStreak;                                 //save current win streak as the maximal
                StatsByPreset[index].WinStreak = 0;                                 //reset current win streak
                StatsByPreset[index].LooseStreak++;                                 //increase the loose streak counter
            }   //ENDELSE (game is lost)
        }   //END (CalcStats)

        /// <summary>
        /// Add game data to Top if needed (by preset - max 5 player records and by 3BV - max 10 player records)
        /// </summary>
        /// <param name="index">Index of the current preset</param>
        /// <param name="now">CUrrent date and time</param>
        /// <param name="gameTime">Current game time</param>
        /// <param name="level3BV">Difficulty level by 3BV</param>
        /// <param name="askPN">Handler to the method that asks for a player name</param>
        /// <returns>Fastest game time from the Top table</returns>
        private uint AddToTop(int index, DateTime now, uint gameTime, uint level3BV, AskPlayerName askPN)
        {
            //first of all we'll check whether we have to add to StatsBy3BV
            int topLength = (StatsBy3BV == null) ?                                          //if statsby3bv is not created by now
                            0 :                                                                 //set toplenght to zero
                            StatsBy3BV.Length;                                                  //otherwise set toplength to the length of statsby3bv
            bool HaveToAdd = false;                                                         //flag that is true when we have to add to top
            string playerName = null;                                                       //playerName is not specified yet
            if (topLength >= 10)                                                            //if topLength is 10 records or more (we will limit top by 10 records)
            {
                if (StatsBy3BV[topLength - 1].Level3BV < level3BV)                              //if current difficulty level is more than the minimal one in top
                    HaveToAdd = true;                                                               //we have to add it
                else if (StatsBy3BV[topLength - 1].Level3BV > level3BV)                         //if current difficulty level is less than the minimal one in top
                    HaveToAdd = false;                                                              //we mustn't add it
                else if (StatsBy3BV[topLength - 1].GameTime > gameTime)                         //if they are equal and current game time is less than added to the top
                    HaveToAdd = true;                                                               //we have to add it
            }   //ENDIF (top is 10 or more records)
            else                                                                            //if top has less than 10 records
                HaveToAdd = true;                                                               //we have to add current game to the top
            if (HaveToAdd)                                                                  //if we have to add to the top
            {
                playerName = askPN();                                                           //ask for player name
                Array.Resize(ref StatsBy3BV, ++topLength);                                      //get memory for the new record in the top
                //add a new record to the top
                StatsBy3BV[topLength - 1].GameTime = gameTime;                                  //game duration
                StatsBy3BV[topLength - 1].Name = playerName;                                    //player name
                StatsBy3BV[topLength - 1].WinTime = now;                                        //win date and time
                StatsBy3BV[topLength - 1].Level3BV = level3BV;                                  //difficulty level
                switch (index)                                                                  //switch between presets using the specified index
                {
                    case 0:                                                                         //Newbie
                        StatsBy3BV[topLength - 1].Preset = 
                            MinesSettings.Preset.Newbie;                                                //records preset is Newbie
                        break;
                    case 1:                                                                         //Advanced
                        StatsBy3BV[topLength - 1].Preset = 
                            MinesSettings.Preset.Advanced;                                              //records preset is Advanced
                        break;
                    case 2:                                                                         //Professional
                        StatsBy3BV[topLength - 1].Preset = 
                            MinesSettings.Preset.Professional;                                          //records preset is Professional
                        break;
                    default:                                                                        //any other undex value will become a Custom
                        StatsBy3BV[topLength - 1].Preset = 
                            MinesSettings.Preset.Custom;                                                //record preset is Custom
                        break;
                }   //ENDSWITCH (preset)
                Array.Sort(StatsBy3BV, new WinnerInfo().CompareByLevel3BV);                     //sort top descending by 3bv and then ascending by game duration
                Array.Resize(ref StatsBy3BV, (topLength > 10 ? 10 : topLength));                //if top length is more than 10 records cut its length to 10
                                                                                                //  this will cut records that are last in the top
            }   //ENDIF (have to add to top)
            if (index < 0 || index > 2)                                                     //if preset is not newbie, advanced or professional
                return StatsBy3BV[0].GameTime;                                                  //return game duration from the first record of the top
            //now we'll check whether we have to add to StatsByPreset
            topLength = (StatsByPreset[index].Top == null) ?                                //if stats by preset top has records
                        0 :                                                                     //set topLength to zero
                        StatsByPreset[index].Top.Length;                                        //otherwise set it to length of the top
            HaveToAdd = false;                                                              //set have-to-add flag to false
            if (topLength >= 5)                                                             //if top length is 5 or more (we'll limit its length by 5)
            {
                if (StatsByPreset[index].Top[topLength - 1].GameTime > gameTime)                //if current game duration is less than the last record in the top
                    HaveToAdd = true;                                                               //we have to add a new record
            }   //ENDIF (top length is 5 or more)
            else                                                                            //if top length is less than 5
                HaveToAdd = true;                                                               //we have to add a new record
            if (HaveToAdd)                                                                  //if we have to add a new record to top
            {
                Array.Resize(ref StatsByPreset[index].Top, ++topLength);                        //get memory for the new record
                StatsByPreset[index].Top[topLength - 1].GameTime = gameTime;                    //save game duration
                StatsByPreset[index].Top[topLength - 1].Name = (playerName == null) ?           //if player name is not specified yet
                    askPN() :                                                                       //ask for player name
                    playerName;                                                                     //otherwise use already specified name
                StatsByPreset[index].Top[topLength - 1].WinTime = now;                          //save date and time of win
                StatsByPreset[index].Top[topLength - 1].Level3BV = level3BV;                    //save difficulty level
                switch (index)                                                                  //switch between presets
                {
                    case 0:                                                                         //Newbie
                        StatsByPreset[0].Top[topLength - 1].Preset = 
                            MinesSettings.Preset.Newbie;                                                //save preset as Newbie
                        break;
                    case 1:                                                                         //Advanced
                        StatsByPreset[1].Top[topLength - 1].Preset = 
                            MinesSettings.Preset.Advanced;                                              //save preset as Advanced
                        break;
                    case 2:                                                                         //Professional
                        StatsByPreset[2].Top[topLength - 1].Preset = 
                            MinesSettings.Preset.Professional;                                          //save preset as Professional
                        break;
                }   //ENDSWITCH (preset)
                Array.Sort(StatsByPreset[index].Top, new WinnerInfo().CompareByGameTime);       //sort top list acsending by game duration
                Array.Resize(ref StatsByPreset[index].Top, (topLength > 5 ? 5 : topLength));    //if top length is more than 5 records cut its length to 5 records
                                                                                                //  this will cut records that are last in the top
            }   //ENDIF (have to add to top)
            return StatsByPreset[index].Top[0].GameTime;                                    //return game duration from the first record of the top
        }   //END (AddToTop)
        
        /// <summary>
        /// Clear statistics
        /// </summary>
        public void Clear()
        {
            Array.Resize(ref StatsBy3BV, 0);                                //clear stats by difficulty and cut it to zero length
            Array.Clear(StatsByPreset, 0, 3);                               //clear stats by preset
            //set presets for records in stats by preset list
            StatsByPreset[0].Preset = MinesSettings.Preset.Newbie;          //Newbie is for index 0
            StatsByPreset[1].Preset = MinesSettings.Preset.Advanced;        //Advanced is for index 1
            StatsByPreset[2].Preset = MinesSettings.Preset.Professional;    //Professional is for index 2
        }   //END (Clear)
        #endregion
    }   //ENDCLASS (MinesStatictics)
}   //ENDNAMESPACE (MineSweeper)
