using DataAccessLayer.Tables;

namespace ServiceLayer.Interface
{
    public interface IUserService
    {
        bool CreateUser(User user);

        bool IsUsernameFound(string username);
        //Winn
        int GetNextUserID();
    }
}
