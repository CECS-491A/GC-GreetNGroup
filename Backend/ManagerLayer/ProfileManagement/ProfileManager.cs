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
        public DateTime DoB { get; set; }
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
                    up.DoB = retrievedUser.DoB;
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

        public int GetUserToUpdateController(string jwtToken)
        {
            if (_jwtServce.IsJWTSignatureTampered(jwtToken))
            {
                if (_userService.GetUserById(_jwtServce.GetUserIDFromToken(jwtToken)) != null)
                {
                    return 1; //OK
                }
                return -1; //BadRequest, invalid user
            }
            return -2; //Unauthorized, invalid token
        }

        public UpdateProfileRequest GetUserToUpdate(string jwtToken)
        {
            int userID = _jwtServce.GetUserIDFromToken(jwtToken);
            if(userID != -1)
            {
                User retrievedUser = _userService.GetUserById(userID);
                UpdateProfileRequest request = new UpdateProfileRequest();
                request.FirstName = retrievedUser.FirstName;
                request.LastName = retrievedUser.LastName;
                request.DoB = retrievedUser.DoB;
                request.City = retrievedUser.City;
                request.State = retrievedUser.State;
                request.Country = retrievedUser.Country;
                return request;
            }
            return null;
        }

        public int UpdateUserProfile(UpdateProfileRequest request)
        {
            if (!_jwtServce.IsJWTSignatureTampered(request.JwtToken)){
                int userID = _jwtServce.GetUserIDFromToken(request.JwtToken);
                User retrievedUser = _userService.GetUserById(userID);
                retrievedUser.FirstName = request.FirstName;
                retrievedUser.LastName = request.LastName;
                retrievedUser.DoB = request.DoB;
                retrievedUser.City = request.City;
                retrievedUser.State = request.State;
                retrievedUser.Country = request.Country;
                if (_userService.UpdateUser(retrievedUser))
                {
                    return 1; //OK. succesful update
                }
                return -2; //Service unavailable
            }
            return -1; //Unauthorized, invalid token
        }

        public string GetUserRating(int userID)
        {
            return _ratingService.GetRating(userID).ToString();
        }

        public bool CheckProfileActivated(string jwtToken)
        {
            int userID = _jwtServce.GetUserIDFromToken(jwtToken);
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
                int raterID = _jwtServce.GetUserIDFromToken(request.jwtToken);
                UserRating ur = new UserRating();
                ur.RatedId1 = Convert.ToInt32(rateeID);
                ur.RaterId1 = Convert.ToInt32(raterID);
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
