using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreetNGroup.Data_Access
{
    public static class DataBaseQueries
    {
        //
        // all of the following is not using entity framework --plain sql server code -- not agnostic
        //
        
        // String to connect to database
        private static string _connectionString = "Server=greetngroupdb.cj74stlentvn.us-west-1.rds.amazonaws.com;" +
                                                  "Database=;User Id=gucci;Password=password123!;";

        /// <summary>
        /// To generate SELECT statements to send to the database and return data
        ///
        /// generic "SELECT x FROM x WHERE x"
        /// </summary>
        /// <param name="selectArg"></param> what to SELECT from
        /// <param name="fromArg"></param> what to FROM from
        /// <param name="whereArg"></param>
        /// <returns></returns>
        public string QuerySelectWhere(string selectArg, string fromArg, string whereArg)
        {
            string q = "SELECT " + selectArg + " FROM " + fromArg + " WHERE " + whereArg ";";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(q), connection)
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return reader[selectArg];
                        }
                    }
                }
            }
        }
    }
}