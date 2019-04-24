using Gucci.DataAccessLayer.Context;
using Gucci.DataAccessLayer.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gucci.ServiceLayer.Services
{
    public class RatingService
    {
        public int GetRating(int userID)
        {
            GreetNGroupContext context = new GreetNGroupContext();
            var listOfRatingsForUser = from r in context.UserRatings
                                       where r.RatedId1 == userID
                                       select r;
            int averageRating = 0;
            foreach (UserRating rating in listOfRatingsForUser)
            {
                averageRating = averageRating + rating.Rating;
            }
            return averageRating;
        }

        public bool CreateRating(UserRating rating)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    ctx.UserRatings.Add(rating);
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch 
            {
                // log
                return false;
            }
        }
    }
}
