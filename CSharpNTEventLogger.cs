using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpWindowsEventLogger
{
    public class DotNetWindowsEventLogger
    {
        private string SourceName;
        private string LogName;


        public string WriteEventLogEntry(  int eventId, 
                                           string eventMsg, 
                                           int eventType,
                                           string sourceName)
        {
            // Error=1, Warning=2, Information=4 
            if ((eventType !=1) && (eventType != 2) && (eventType != 4))
                return string.Format("WriteEventLogEntry: eventType [{0}] invalid, must be in [1,2,4] Error=1, Warning=2, Information=4.",
                                      eventType);
            EventLogEntryType eventLogEntryType = (EventLogEntryType)eventType;

            if (eventId <=0)
                return string.Format("WriteEventLogEntry: eventId [{0}] invalid, must be > 0, should be > 1000.",
                                      eventId);

            if (!EventLog.SourceExists(sourceName))
                return string.Format("WriteEventLogEntry failed: source [{0}] doesn't exist. Call CreateEventLogSource() first.", 
                                      sourceName);

            try
            {
                EventLog.WriteEntry(sourceName, eventMsg, eventLogEntryType, eventId);
            }
            catch (Exception e)
            {
                return string.Format("WriteEventLogEntry failed: during call to EventLog.WriteEntry: \r\n    exception = [{0}]",
                                     e.ToString()); 
            }

            return string.Empty;
        }


        public bool CreateEventLogSource(string logName, string sourceName)
        {
            if (!EventLog.SourceExists(sourceName))
                EventLog.CreateEventSource(sourceName, logName);
            
            LogName = logName;
            SourceName = sourceName;

            return EventLog.SourceExists(sourceName);
        }

    }
}
