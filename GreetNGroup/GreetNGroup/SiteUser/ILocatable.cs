namespace GreetNGroup.Account
{
    public interface ILocatable
    {
        /// Returns the account City Location or Sets a new City
        string Cityloc { get; set; }

        /// Returns the account State Location or sets a new State
        string Stateloc { get; set; }

        /// Returns the account Country Location
        string Countryloc { get; set; }
    }
}