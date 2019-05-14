using Gucci.DataAccessLayer.Tables;
using Gucci.ManagerLayer.ProfileManagement;
using Gucci.ServiceLayer.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ManagerLayer.UserManagement;

namespace UnitTest
{
    [TestClass]
    public class RateUserUT
    {
        TestingUtils tu;
        UserManager userMan;
        UserService _userService;
        RatingService ratingService;
        UserProfileManager userProfileManager;
        public RateUserUT()
        {
            tu = new TestingUtils();
            userMan = new UserManager();
            _userService = new UserService();
            ratingService = new RatingService();
            userProfileManager = new UserProfileManager();
        }
        [TestMethod]
        public void CreateRating_Pass()
        {
            UserRating ur = new UserRating
            {
                RatedId1 = 1,
                RaterId1 = 2,
                Rating = 1
            };
            var rateExist = ratingService.CreateRating(ur, "");
            Assert.IsTrue(rateExist);
        }

        [TestMethod]
        public void UpdateRating_Pass()
        {
            UserRating ur = new UserRating
            {
                RatedId1 = 1,
                RaterId1 = 2,
                Rating = 1
            };
            var rateExist = ratingService.UpdateRating(ur, "");
            Assert.IsTrue(rateExist);
        }

        [TestMethod]
        public void GetRating_Pass()
        {
            var userRating = ratingService.GetRating(2, 1);
            Assert.IsTrue(userRating);
        }

        [TestMethod]
        public void GetRating_Fail_Doesntexist()
        {
            var userRating = ratingService.GetRating(2, 3);
            Assert.IsFalse(userRating);
        }
    }
}
