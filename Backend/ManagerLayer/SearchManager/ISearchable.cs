namespace Gucci.ManagerLayer.SearchManager
{
    // Interface for searchable objects, accepts generic param with covariance
    public interface ISearchable<out T> where T : class
    {
        T SearchByName(string name);
        T SearchById(int id);
    }
}
