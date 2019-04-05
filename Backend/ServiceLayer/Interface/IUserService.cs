using DataAccessLayer.Tables;

namespace ServiceLayer.Interface
{
    public interface IUserService
    {
        bool CreateUser(User user);
        //Jonalyn
        bool IsExistingGNGUser(string username);
        //Winn
        int GetNextUserID();
    }
}
