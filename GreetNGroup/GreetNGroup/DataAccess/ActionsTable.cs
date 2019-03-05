using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreetNGroup.DataAccess
{
    [Table("Action")]
    public class ActionsTable
    {
        public ActionsTable() { }

        public ActionsTable(int aId, string aName)
        {
            ActionId = aId;
            ActionName = aName;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ActionId { get; set; }
        public string ActionName { get; set; }
    }
}