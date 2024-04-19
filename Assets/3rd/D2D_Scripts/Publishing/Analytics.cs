using System;
using System.Collections.Generic;
using D2D.Core;
using D2D.Databases;
using D2D.Gameplay;
using D2D;
using D2D.Utilities;
using UnityEngine;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    [DefaultExecutionOrder(199)]
    public class Analytics : GameStateMachineUser
    {
        [SerializeField] private bool _logging;
        
        private const string LevelCountKey = "LevelCount"; 
            
        private int SceneNumber => _level.SceneNumber;
        public bool IsBootScene => !IsLevelScene;

        private bool IsLevelScene => _level != null;
        private int LevelNumber => _db.PassedLevels.Value + 1;

        private DataContainer<int> CompletedLevelsCount
            = new DataContainer<int>("CompletedLevelsCount", 0);

        private Dictionary<string, object> DefaultData => 
            new Dictionary<string, object>
            {
                {"level_number", LevelNumber.ToString()}, 
                {"level_name", LevelNumber.ToString()},
                {"level_count", CompletedLevelsCount.Value.ToString()}, 
                {"level_diff", "normal"},
                {"level_loop", LevelNumber.ToString()}, 
                {"level_random", "1"}, 
                {"level_type", "normal"},
                {"game_mode", "classic"}
            };

        public static float timeSinceAppStart;
        public static float timeOfLevelStart;
        
        public static int knockouts;
        [HideInInspector] public int knockoutsOfPlayerFromStart;
        [HideInInspector] public int knockoutsOfEnemiesFromStart;
        public static int wins;
        
        public float TimeElapsedFromAppStart => Time.time - timeSinceAppStart;
        public float LevelPlaytime => Time.time - timeOfLevelStart;

        public string WinToKnockoutsPercentage => (wins == 0 ? -knockouts : knockouts == 0 ? wins : Math.Round((float) wins / ((float) knockouts), 2)).ToString();
        [HideInInspector] public string WinToKnockoutsName = "wins_too_knockouts";
        
        private Level _level;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnApplicationStart()
        {
            var a = FindObjectOfType<Analytics>();
            a._level = FindObjectOfType<Level>();
            timeSinceAppStart = Time.time;
            
           
        }

        private void Start()
        {
            _level = Find<Level>();
            
            knockoutsOfPlayerFromStart = 0;
            knockoutsOfEnemiesFromStart = 0;
            
            timeOfLevelStart = Time.time;
            
            var d = DefaultData;
            d.Add("time", TimeElapsedFromAppStart.Round().ToString());
            d.Add("loses", knockouts.ToString());
            d.Add(WinToKnockoutsName, WinToKnockoutsPercentage);
            SendEvent(DefaultData, "level_start");
        }

        private void OnAppOpen()
        {
            CompletedLevelsCount.Value = 0;
        }

        protected override void OnGameFinish()
        {
            //SendLevelFinishDataToYandex(isLeave: false);
        }

        private void OnApplicationQuit()
        {
            //if (IsLevelScene)
            //    SendLevelFinishDataToYandex(isLeave: true);
        }

        private void SendLevelFinishDataToYandex(bool isLeave)
        {
            bool isWin = _stateMachine.Was<WinState>();
            var result = isWin ? "win" : "lose";

            if (isWin)
                wins++;

            var progress = isWin ? "100" : "0";
            if (isLeave)
                progress = "leave";

            var data = DefaultData;
            
            if (isWin)
            {
                data["level_name"] = (LevelNumber - 1).ToString();
                data["level_number"] = (LevelNumber - 1).ToString();
                data["level_loop"] = (LevelNumber - 1).ToString();
            }
            
            data.Add("result", result);
            data.Add("time", TimeElapsedFromAppStart.Round().ToString());
            data.Add("level_playtime", LevelPlaytime.Round().ToString());
            data.Add("loses", knockouts.ToString());
            // data.Add(WinToKnockoutsName, WinToKnockoutsPercentage);
            data.Add("progress", progress);

            CompletedLevelsCount.Value += 1;
            CompletedLevelsCount.Save();

            SendEvent(data, "level_finish");
        }

        private void SendEvent(Dictionary<string, object> data, string eventName, bool useBuffer = true)
        {
            
        }

        private void InitCallback()
        {

        }

        private void OnHideUnity(bool isGameShown) => Time.timeScale = isGameShown ? 1 : 0;
    }
}