using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreetNGroup.DataAccess
{
    [Table("Action")]
    public class ActionsTable
    {
        public ActionsTable() { }

        public ActionsTable(string aId, string aName)
        {
            ActionId = aId;
            ActionName = aName;
        }

        [Key]
        public string ActionId { get; set; }
        public string ActionName { get; set; }
    }
}