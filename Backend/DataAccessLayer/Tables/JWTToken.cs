using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gucci.DataAccessLayer.Tables;

namespace DataAccessLayer.Tables
{
    [Table("JWTToken")]
    public class JWTToken
    {
        public JWTToken(int ID, string jwtToken, string username)
        {
            Id = ID;
            Token = jwtToken;
            UserName = username;
        }

        [Required, Key]
        public int Id { get; set; }

        [Required]
        public string Token { get; set; }

        [Required, ForeignKey("User")]
        public string UserName { get; set; }
        public User User { get; set; }
    
    }
}
