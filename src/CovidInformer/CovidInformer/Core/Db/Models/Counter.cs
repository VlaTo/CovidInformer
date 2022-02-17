using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CovidInformer.Core.Db.Models
{
    [Table("Counter")]
    public class Counter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public long Id
        {
            get;
            set;
        }

        [Column("Value")]
        public ulong Value
        {
            get;
            set;
        }

        [Column("CountryRef")]
        public long CountryId
        {
            get;
            set;
        }

        [NotMapped]
        [ForeignKey(nameof(CountryId))]
        public Country Country
        {
            get; 
            set;
        }

        [Column("UpdateRef")]
        public long UpdateId
        {
            get;
            set;
        }

        [NotMapped]
        [ForeignKey(nameof(UpdateId))]
        public Update Update
        {
            get; 
            set;
        }
    }
}