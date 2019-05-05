using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gucci.DataAccessLayer.Tables;

namespace DataAccessLayer.Tables
{
    [Table("JWTToken")]
    public class JWTToken
    {
        public JWTToken(int ID, string jwtToken, int userID)
        {
            Id = ID;
            Token = jwtToken;
            UserId = userID;
        }

        [Required, Key]
        public int Id { get; set; }

        [Required]
        public string Token { get; set; }

        [Required, ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
    
    }
}
