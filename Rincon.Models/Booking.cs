using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rincon.Models
{
    [Table("Booking")]
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid BookingId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }

        [NotMapped]
        public Product Product { get; set; }
    }
}
