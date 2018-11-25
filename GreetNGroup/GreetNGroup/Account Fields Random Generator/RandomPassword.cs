using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace GreetNGroup.Account_Fields_Random_Generator
{
    public static class RandomPassword
    {
        /// <summary>
        /// Method that generates a random password
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
    }
}