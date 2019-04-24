using System;

namespace Gucci.DataAccessLayer.DataTransferObject
{
    public class DefaultUserSearchDto
    {
        public string Username { get; set; }

        public DefaultUserSearchDto()
        {
            Username = "No username found";
        }

        public DefaultUserSearchDto(string username)
        {
            Username = username;
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
