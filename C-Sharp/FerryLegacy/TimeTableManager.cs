using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace FerryLegacy
{
    public class TimeTableManager
    {
        // Private Attributes
        private List<TimeTableEntry> _timeTableEntries;

        // Constructor - Initialize the time table entry list
        public TimeTableManager()
        {
            var reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\data\\timetable.txt");
            string json = reader.ReadToEnd();
            _timeTableEntries = JsonConvert.DeserializeObject<List<TimeTableEntry>>(json);
        }

        // Return the full list of time table entries
        public List<TimeTableEntry> GetFullTimeTable()
        {
            return _timeTableEntries;
        }
    }
}