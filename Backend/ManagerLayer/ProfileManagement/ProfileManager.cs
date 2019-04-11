using DataAccessLayer.Context;
using DataAccessLayer.Tables;
using ServiceLayer.Interface;
using ServiceLayer.Requests;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Script.Serialization;

namespace ManagerLayer.ProfileManagement
{
    class UserProfile
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public int EventCreationCount { get; set; }
        [Required]
        public string Rating { get; set; }
    }

    public class ProfileManager
    {
        private ICryptoService _cryptoService;
        private IUserService _userService;
        private IJWTService _jwtServce;
        private RatingService _ratingService;

        public ProfileManager()
        {
            _userService = new UserService();
            _cryptoService = new CryptoService();
            _jwtServce = new JWTService();
            _ratingService = new RatingService();
        }

        public int GetUserController(string userID)
        {
            try
            {
                int userIDConverted = Convert.ToInt32(userID);
                if (CheckUserExists(userIDConverted))
                {
                    return 1; //OK
                }
                return -1;
            }
            catch
            {
                //log
                return -2;
            }
        }

        public bool CheckUserExists(int userID)
        {
            return _userService.IsUsernameFoundById(userID);
        }

        public string GetUserProfile(string userID)
        {
            try
            {
                int convertedUserID = Convert.ToInt32(userID);
                User retrievedUser = _userService.GetUserById(convertedUserID);
                if (retrievedUser != null)
                {
                    UserProfile up = new UserProfile();
                    up.FirstName = retrievedUser.FirstName;
                    up.LastName = retrievedUser.LastName;
                    up.UserName = retrievedUser.UserName;
                    up.City = retrievedUser.City;
                    up.State = retrievedUser.State;
                    up.Country = retrievedUser.Country;
                    up.EventCreationCount = retrievedUser.EventCreationCount;
                    up.Rating = GetUserRating(convertedUserID);
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    return js.Serialize(up);
                }
                return "";
            }
            catch (FormatException)
            {
                //log
                return "Unable to get user";
            }
        }

        public string GetUserRating(int userID)
        {
            return _ratingService.GetRating(userID).ToString();
        }

        public bool CheckProfileActivated(string jwtToken)
        {
            string hashedUID = _jwtServce.GetUserIDFromToken(jwtToken);
            //need a function to undero hashedUID to regular uid
            int userID = 0;
            if (_userService.IsUsernameFoundById(userID))
            {
                User retrievedUser = _userService.GetUserById(userID);
                return retrievedUser.IsActivated;
            }
            return false;
        }

        public int RateUser(RateRequest request, string rateeID)
        {
            try
            {
                UserRating ur = new UserRating();
                ur.RatedId1 = Convert.ToInt32(rateeID);
                ur.RaterId1 = Convert.ToInt32(request.RaterID);
                ur.Rating = Convert.ToInt32(request.rating);
                _ratingService.CreateRating(ur);
                return 1;
            }
            catch (FormatException)
            {
                //log
                return -1;
            }
        }
    }
}
