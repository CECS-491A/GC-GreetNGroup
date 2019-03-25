using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Tables
{
    [Table("Tag")]
    public class Tag
    {
        public Tag() {}

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TagId { get; set; }
        public string TagName { get; set; }
    }
}