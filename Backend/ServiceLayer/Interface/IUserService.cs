using DataAccessLayer.Tables;

namespace ServiceLayer.Interface
{
    public interface IUserService
    {
        bool CreateUser(User user);
        //Winn
        int GetNextUserID();
        bool IsUsernameFound(string username);
        User GetUser(int userID);
        bool DeleteUser(User userToDelete);
        bool IsUsernameFoundById(int uId);
    }
}
