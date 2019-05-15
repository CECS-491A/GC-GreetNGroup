using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using Gucci.DataAccessLayer.Tables;
using Gucci.ServiceLayer.Interface;
using Gucci.ServiceLayer.Model;
using Gucci.ServiceLayer.Requests;
using Gucci.ServiceLayer.Services;
using Newtonsoft.Json;

namespace Gucci.ManagerLayer.ProfileManagement
{
    public class UserProfileManager
    {
        private readonly DateTime requiredAgeOfUser = DateTime.Now.AddYears(-18); // Must be 18 years old to use this application
        private IUserService _userService;
        private IJWTService _jwtServce;
        private RatingService _ratingService;
        private readonly List<string> canRateUsers = new List<string> { "CanRate" };

        public UserProfileManager()
        {
            _userService = new UserService();
            _jwtServce = new JWTService();
            _ratingService = new RatingService();
        }

        public string GetUserRating(int userID)
        {
            return _ratingService.GetRating(userID).ToString();
        }

        public HttpResponseMessage GetUser(string userID)
        {
            try
            {
                int convertedUserID = Convert.ToInt32(userID);

                User retrievedUser = _userService.GetUserById(convertedUserID);
                if (retrievedUser == null)
                {
                    var httpResponseFail = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent("User does not exist")
                    };
                    return httpResponseFail;
                }
                UserProfile up = new UserProfile
                {
                    FirstName = retrievedUser.FirstName,
                    LastName = retrievedUser.LastName,
                    UserName = retrievedUser.UserName,
                    DoB = retrievedUser.DoB.ToString("MMM dd, yyyy"),
                    City = retrievedUser.City,
                    State = retrievedUser.State,
                    Country = retrievedUser.Country,
                    EventCreationCount = retrievedUser.EventCreationCount,
                    Rating = GetUserRating(convertedUserID)
                };
                var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(up))
                };
                return httpResponse;
            }
            catch
            {
                //log
                var httpResponse = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent("Unable to get user")
                };
                return httpResponse;
            }
        }

        public HttpResponseMessage UpdateUserProfile(UpdateProfileRequest request)
        {
            try
            {
                var isSignatureTampered = _jwtServce.IsJWTSignatureTampered(request.JwtToken);
                if (isSignatureTampered) // Check if signature is tampered
                {
                    var httpResponseFail = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new StringContent("Session is invalid")
                    };
                    return httpResponseFail;
                }

                if (request.DoB > requiredAgeOfUser) // Check if the user is over 18
                {
                    var httpResponseFail = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new StringContent("This software is intended for persons over 18 years of age.")
                    };
                    return httpResponseFail;
                }

                List<string> userInfo = new List<string>
                {
                request.FirstName,
                request.LastName,
                request.DoB.ToString(),
                request.City,
                request.State,
                request.Country
            };

                foreach (string item in userInfo) // Check if any of the fields are null
                {
                    if (String.IsNullOrWhiteSpace(item))
                    {
                        var httpResponseFail = new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new StringContent("Fields cannot be null")
                        };
                        return httpResponseFail;
                    }
                }

                int userID = _jwtServce.GetUserIDFromToken(request.JwtToken);
                User retrievedUser = _userService.GetUserById(userID);
                retrievedUser.FirstName = request.FirstName;
                retrievedUser.LastName = request.LastName;
                retrievedUser.DoB = request.DoB;
                retrievedUser.City = request.City;
                retrievedUser.State = request.State;
                retrievedUser.Country = request.Country;

                if (!retrievedUser.IsActivated) // Check to see if the profile is activated, if not, activate it
                {
                    retrievedUser.IsActivated = true;
                }

                var isUserUpdated = _userService.UpdateUser(retrievedUser); // Update user with new fields
                if (!isUserUpdated)
                {
                    var httpResponseFail = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                    {
                        Content = new StringContent("Unable to update user")
                    };
                    return httpResponseFail;
                }

                var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("Profile has been updated")
                };
                return httpResponse;
            }
            catch (Exception e)
            {
                var httpResponseFail = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Unable to update user at this time, internal server error.")
                };
                return httpResponseFail;
            }
        }

        public HttpResponseMessage IsProfileActivated(string jwtToken)
        {
            try
            {
                if(jwtToken == null)
                {
                    var failHttpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("false")
                    };
                    return failHttpResponse;
                }
                var userIDFromToken = _jwtServce.GetUserIDFromToken(jwtToken);
                if (!_userService.IsUsernameFoundById(userIDFromToken))
                {
                    var failHttpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("false")
                    };
                    return failHttpResponse;
                }

                User retrievedUser = _userService.GetUserById(userIDFromToken);
                if (!retrievedUser.IsActivated)
                {
                    var failHttpResponse = new HttpResponseMessage(HttpStatusCode.Forbidden)
                    {
                        Content = new StringContent("false")
                    };
                    return failHttpResponse;
                }
                var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("true")
                };
                return httpResponse;
            }
            catch (Exception e)
            {
                var httpResponse = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent(e.ToString())
                };
                return httpResponse;
            }

        }
        /// <summary>
        /// Either updates or adds a rating to the selected User
        /// </summary>
        /// <param name="request">Object that holds jwt and rating</param>
        /// <param name="rateeID">User that is being rated</param>
        /// <returns>an integer</returns>
        public int RateUser(RateRequest request, string rateeID)
        {
            int raterID = _jwtServce.GetUserIDFromToken(request.jwtToken);
            UserRating ur = new UserRating
            {
                RatedId1 = Convert.ToInt32(rateeID),
                RaterId1 = Convert.ToInt32(raterID),
                Rating = Convert.ToInt32(request.rating)
            };

            try
            {
                var hasClaims = _jwtServce.CheckUserClaims(request.jwtToken, canRateUsers);
                if (hasClaims.CompareTo("Authorized") != 0)
                {
                    return -2;
                }
                var userRating = _ratingService.GetRating(Convert.ToInt32(raterID), Convert.ToInt32(rateeID));
                if (userRating == false)
                {
                    _ratingService.CreateRating(ur, "");
                    return 1;
                }
                _ratingService.UpdateRating(ur, "");
                return 2;
            }
            catch (Exception e)
            {
                //log
                return -1;
            }
        }
    }
}
