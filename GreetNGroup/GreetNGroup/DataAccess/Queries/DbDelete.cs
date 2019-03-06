using System.Linq;
using GreetNGroup.DataAccess.Tables;

namespace GreetNGroup.DataAccess.Queries
{
    public class DbDelete
    {
        public static void DeleteUserById(int userId)
        {
            using (var ctx = new GreetNGroupContext())
            {
                var user = ctx.Users.FirstOrDefault(c => c.UserId.Equals(userId));

                if(user != null) ctx.Users.Remove(user);

                ctx.SaveChanges();
            }
        }
    }
}