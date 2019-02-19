using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreetNGroup.DataAccess
{
    [Table("Tag")]
    public class Tag
    {
        public Tag() {}

        [Key]
        public string TagId { get; set; }
        public string TagName { get; set; }
    }
}