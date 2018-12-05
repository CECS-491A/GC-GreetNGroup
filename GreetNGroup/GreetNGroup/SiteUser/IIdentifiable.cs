namespace GreetNGroup.SiteUser
{
    public interface IIdentifiable
    {
        /// Returns the account user name or sets a new username
        string Username { get; set; }

        /// Returns the account password or Sets a new Password
        string Password { get; set; }

        /// Returns the first name of user or Sets a new first name
        string Firstname { get; set; }

        /// Returns the last name of user or sets a new last name
        string Lastname { get; set; }
        
        /// Returns the account Date of Birth or sets a new Date of Birth
        string DOB { get; set; }
    }
}