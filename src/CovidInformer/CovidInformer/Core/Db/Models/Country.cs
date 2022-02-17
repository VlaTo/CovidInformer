using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CovidInformer.Core.Db.Models
{
    [Table("Country")]
    public class Country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public long Id
        {
            get;
            set;
        }

        [Column("DisplayName", TypeName = "nvarchar(100)")]
        public string DisplayName
        {
            get;
            set;
        }

        [NotMapped]
        public List<Counter> Counters
        {
            get;
            internal set;
        }
    }
}