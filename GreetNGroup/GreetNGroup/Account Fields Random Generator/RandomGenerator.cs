using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace GreetNGroup.Account_Fields_Random_Generator
{
    public static class RandomFieldGenerator
    {
        /// <summary>
        /// Method that generates a random 12 character password from a-z, A-Z, and 0-9
        /// </summary>
        /// <returns></returns>
        public static string generatePassword()
        {
            String range = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char[] characterHolder = new char[12];

            Random rd = new Random();

            for(int i = 0; i < 12; i++)
            {
                characterHolder[i] = range[rd.Next(0, range.Length)];
            }

            string password = new string(characterHolder);

            return password;
        }

        //public static string generateSecurityQuestion()
        //{
            

        //}
    }
}