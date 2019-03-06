using Newtonsoft.Json;

namespace BrainVR.Logger
{
    public class ExperimentInfoLog : MonoLog
    {
        private Log _log;

        //default instantiates without the player ID
        protected string LogName = "ExperimentInfo";
        public void Instantiate(string timeStamp, string id)
        {
            _log = new Log(id, LogName, timeStamp);
            WriteExperimentData();
        }
        private void WriteExperimentData()
        {
            _log.WriteLine("***EXPERIMENT INFO***");
            var info = ExperimentInfo.Instance;
            info.PopulateInfo();
            var s = JsonConvert.SerializeObject(info, Formatting.Indented);
            _log.WriteLine(s);
            _log.WriteLine("---EXPERIMENT INFO---");
        }
    }
}