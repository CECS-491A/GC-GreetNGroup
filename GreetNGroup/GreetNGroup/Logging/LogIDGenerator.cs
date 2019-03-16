using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreetNGroup.Logging
{
    public class LogIDGenerator
    {

        /// <summary>
        /// Method IDGenerator creates a Dictionary of int values representing the specific
        /// event that is being logged and string keys associated with that ID
        /// </summary>
        /// <returns>Dictionary<string, int> which holds the mapped ids</returns>
        public static Dictionary<string, int> GetLogIDs()
        {
            Dictionary<string, int> logIDMap = new Dictionary<string, int>();
            logIDMap.Add("ClickEvent", 1001);
            logIDMap.Add("ErrorEncountered", 1002);
            logIDMap.Add("EventCreated", 1003);
            logIDMap.Add("EntryToWebsite", 1004);
            logIDMap.Add("ExitFromWebsite", 1005);
            logIDMap.Add("AccountDeletion", 1006);
            logIDMap.Add("InternalErrors", 1007);
            logIDMap.Add("MaliciousAttacks", 1008);

            return logIDMap;
        }
    }
}