using System.Collections.Generic;
using BrainVR.Logger.Helpers;
using BrainVR.Logger.Interfaces;
using UnityEngine;

//REquires an objsect with a player tag to be present

namespace BrainVR.Logger
{
    public class PlayerLog : MonoLog
    {
        public GameObject Player;

        private IPlayerController _playerController;

        private IInput _input;
        //HOW OFTEN DO YOU WANNA LOG
        //applies only to children that can log continuously (Plyer Log), not to those that log based on certain events (Quest log, BaseExperiment log)
        public float LoggingFrequency = 0.005F;

        float _deltaTime;
        private double _lastTimeWrite;

        //this is for filling in custom number of fields that follow after common fields
        // for example, we need one column for input, but it is not always used, so we need to
        // create empty single column
        private const int NEmpty = 1;

        public void Instantiate(string timeStamp, string id)
        {
            Log = new Log(id, "player", timeStamp);
            SetupLog();
        }
        #region MonoBehaviour
        void Update()
        {
            _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;//calculating FPS
        }
        void FixedUpdate()
        {
            if (!Logging) return;
            if (!(_lastTimeWrite + LoggingFrequency < SystemTimer.TimeSinceMidnight)) return;
            LogPlayerUpdate();
            _lastTimeWrite = SystemTimer.TimeSinceMidnight;
        }
        #endregion
        public void StartLogging()
        {
            if (Logging) return;
            if (!Player)
            {
                Debug.Log("There is no player Game object. Cannot start player log.");
                return;
            }
            SubscribeToInput();
            _lastTimeWrite = SystemTimer.TimeSinceMidnight;
            Logging = true;
        }
        public void StopLogging()
        {
            if (!Logging) return;
            UnsubscribeInput();
            Logging = false;
        }
        public void LogPlayerInput(object sender, ButtonPressedArgs e)
        {
            var strgs = CollectData();
            AddValue(ref strgs, e.ButtonName);
            WriteLine(strgs);
        }
        public void LogPlayerUpdate()
        {
            var strgs = CollectData();
            strgs.AddRange(WriteBlank(NEmpty));
            WriteLine(strgs);
        }
        protected string HeaderLine()
        {
            var line = "Time;";
            line += _playerController.HeaderLine;
            line += "FPS; Input;";
            return line;
        }
        #region Logging
        protected List<string> CollectData()
        {
            var strgs = _playerController.PlayerInformation();
            AddTimestamp(ref strgs);
            AddValue(ref strgs, (1.0f / _deltaTime).ToString("F4")); //adds FPS
            //needs an empty column for possible input information
            return strgs;
        }
        #endregion

        #region private helpers
        private void SetupLog()
        {
            if (!Player) Player = GameObject.FindGameObjectWithTag("Player");
            if (!Player)
            {
                Debug.Log("There is no player Game object. Cannot setup player log.");
                Log.WriteLine("There is no player Game object in the game. Can't log");
                return;
            }
            _playerController = Player.GetComponent<IPlayerController>();
            if (_playerController == null)
            {
                Debug.Log("player GO does not have Player Controller component.");
                Log.WriteLine("There is no valid player Game object in the game. Can't log");
                return;
            }
            _input = _playerController.Input;
            Log.WriteLine(HeaderLine());
        }
        private void SubscribeToInput()
        {
            if (_input != null) _input.ButtonPressed += LogPlayerInput;
        }

        private void UnsubscribeInput()
        {
            if (_input != null) _input.ButtonPressed -= LogPlayerInput;
        }
        #endregion
    }
}
