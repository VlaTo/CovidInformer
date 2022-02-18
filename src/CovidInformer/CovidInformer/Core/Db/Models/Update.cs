using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CovidInformer.Core.Db.Models
{
    [Table("Update")]
    public class Update
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public long Id
        {
            get; 
            set;
        }

        [Column("Updated")]
        [DataType(DataType.Date)]
        public DateTime Updated
        {
            get;
            set;
        }
        
        public ICollection<Counter> Counters
        {
            get;
            internal set;
        }
    }
}