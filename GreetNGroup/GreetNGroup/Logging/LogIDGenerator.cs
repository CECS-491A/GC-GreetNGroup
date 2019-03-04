using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreetNGroup.Logging
{
    public class LogIDGenerator
    {

        /// <summary>
        /// Method IDGenerator creates a HashSet of int values with randomly generated
        /// values to create an ID for each event to log.
        /// </summary>
        /// <returns>HashSet<int> listOfIDs which holds the randomly generated int IDs</returns>
        public Dictionary<string, int> GetLogIDs()
        {
            Dictionary<string, int> logIDMap = new Dictionary<string, int>();
            logIDMap.Add("ClickEvent", 1001);
            logIDMap.Add("ErrorEncountered", 1002);
            logIDMap.Add("EventCreated", 1003);
            logIDMap.Add("EntryToWebsite", 1004);
            logIDMap.Add("ExitFromWebsite", 1005);
            logIDMap.Add("AccountDeletion", 1006);

            return logIDMap;
        }
    }
}