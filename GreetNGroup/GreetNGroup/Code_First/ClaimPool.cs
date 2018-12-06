using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;

namespace GreetNGroup.Code_First
{
    public class ClaimPool
    {
        [Key]
        public string ClaimId { get; set; }
        public string ClaimName { get; set; }
    }

    internal class TypeNameAttribute : Attribute
    {
    }
}