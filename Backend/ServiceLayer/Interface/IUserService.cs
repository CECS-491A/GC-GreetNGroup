using DataAccessLayer.Tables;

namespace ServiceLayer.Interface
{
    public interface IUserService
    {
        bool InsertUser(User user);

        bool IsUsernameFound(string username);
        bool IsUsernameFoundById(int uId);

        bool DeleteUser(User userToDelete);

        int GetNextUserID();
        User GetUserById(int userID);
        User GetUserByUsername(string username);

        bool UpdateUser(User updatedUser);
        int GetUserUid(string username);
    }
}
