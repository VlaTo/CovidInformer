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

        [MaxLength(100)]
        [Column("DisplayName", TypeName = "nvarchar(100)")]
        public string DisplayName
        {
            get;
            set;
        }

        [MaxLength(255)]
        [Column("NativeName", TypeName = "nvarchar(255)")]
        public string NativeName
        {
            get;
            set;
        }

        [MaxLength(2)]
        [Column("RegionName", TypeName = "char(2)")]
        public string TwoLetterISORegionName
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