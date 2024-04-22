using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rincon.Models
{
    [Table("ProductStock")]
    public class ProductStock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        [NotMapped]
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public string DateTime { get; set; }
        public int Available { get; set; }
        public int Reserved { get; set; }
        public int Process { get; set; }
    }
}
