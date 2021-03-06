﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using System;
//important namespace
//for the write function

namespace BrainVR.Logger
{
    public class Header
    {
        public string Participant;
        public string Timestamp;

        public Header(string participant, string timestamp)
        {
            Participant = participant;
            Timestamp = timestamp;
        }
    }

    public class Log
    {
        readonly StreamWriter _logFile;

        public string FilePath;
        public string DateString;
        private const string RelativePath = "/../logs/";

        public Log(string id, string logName, string timeStamp = null)
        {
            if (timeStamp == null)
            {
                var dateTime = DateTime.Now;
                timeStamp = dateTime.ToString("HH-mm-ss-dd-MM-yyy");
            }
            DateString = timeStamp;
            // path + id/day-month-year
            var folderName = id + "_" + DateTime.Now.ToString("dd-MM-yyy") + "/";
            FilePath = Application.dataPath + RelativePath + folderName;
            //creates a folder for the patient, if it doesnt exist
            Directory.CreateDirectory(FilePath);
            //vytvoři logfile - poslední parametr určuje, zda v případě existejnce souboru bude pokračovat v zápisu (true), či jej smaže (false);
            _logFile = new StreamWriter(FilePath + id + "_" + logName + "_" + DateString + ".txt", true) {AutoFlush = true};
            //because we can restart etc. many times during a day, the file continues but always writes a new header
            WriteHeader(id);
        }
        //simple header file for each new star of the experiment
        private void WriteHeader(string id)
        {
            var header = new Header(id, DateString);
            _logFile.WriteLine("***SESSION HEADER***");
            _logFile.WriteLine(JsonConvert.SerializeObject(header, Formatting.Indented));
            _logFile.WriteLine("---SESSION HEADER---");
        }
        //takes a string and writes it down
        public void WriteLine(string str)
        {
            //this is the header line for analysiss software
            _logFile.WriteLine(str);
        }
        public void Close() 
        {
            _logFile.Close();
        }
        //takes a list of string as an argument and turns them into a one line that is written into the file
        public void WriteList(List<string> data)
        {
            //basically LINQ foreach
            string line = data.Aggregate("", (current, text) => current + (text + ";"));
            _logFile.WriteLine(line);
        }
        #region Helpers
        public static string NewLine { get { return Environment.NewLine; } }
        #endregion
    }
}