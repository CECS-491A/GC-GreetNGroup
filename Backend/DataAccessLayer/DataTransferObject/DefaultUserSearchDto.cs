using System;

namespace Gucci.DataAccessLayer.DataTransferObject
{
    public class DefaultUserSearchDto
    {
        public string Username { get; set; }
        public int UserId { get; set; }

        public DefaultUserSearchDto()
        {
            Username = "No username found";
            UserId = 0;
        }

        public DefaultUserSearchDto(string username, int userID)
        {
            Username = username;
            UserId = userID;
        }

        public bool CompareDefaultUserDto(DefaultUserSearchDto u)
        {
            if (string.Compare(Username, u.Username, StringComparison.Ordinal) == 0)
            {
                return true;
            }

            return false;
        }
    }
}
