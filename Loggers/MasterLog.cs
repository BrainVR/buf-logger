using System;
using UnityEngine;

namespace BrainVR.Logger
{
    public class MasterLog : Singleton<MasterLog>
    {
        public bool ShouldLog = true;

        private ExperimentInfoLog _experimentInfoLog;
        private PlayerLog _playerLog;

        public string CreationTimestamp { get; private set; }

        //if force is set to true, then deletes and reinstantiates logs
        public void Instantiate(bool force = false)
        {
            var participantId = ExperimentInfo.Instance.Participant.Id;
            if (participantId == null)
            {
                Debug.Log("NO participant id provided");
                return;
            }
            if (_experimentInfoLog && !force)
            {
                Debug.Log("Log already exists, close them first with CloseLogs()");
                return;
            }
            InstantiateLoggers(participantId);
        }
        public void StartLogging()
        {
            if(_playerLog) _playerLog.StartLogging();
        }
        public void StopLogging()
        {
            if (_playerLog) _playerLog.StopLogging();
        }
        public void CloseLogs()
        {
            StopLogging();
            if (_experimentInfoLog)
            {
                _experimentInfoLog.Close();
                Destroy(_experimentInfoLog);
                _experimentInfoLog = null; //because of timing issues when closing and opening in one method
            }
            if (!_playerLog) return;
            _playerLog.Close();
            Destroy(_playerLog);
            _playerLog = null;
        }
        #region private flow
        //RETURNS if logs already exist, otherwise reinstantiates them
        private void InstantiateLoggers(string participantId)
        {
            CloseLogs();
            //to get one timestapm in order to synchronize loading of log files
            CreationTimestamp = DateTime.Now.ToString("HH-mm-ss-dd-MM-yyy");

            _playerLog = gameObject.AddComponent<PlayerLog>();
            _experimentInfoLog = gameObject.AddComponent<ExperimentInfoLog>();

            _playerLog.Instantiate(CreationTimestamp, participantId);
            _experimentInfoLog.Instantiate(CreationTimestamp, participantId);
        }
        #endregion
    }
}