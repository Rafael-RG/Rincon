using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rincon.Models
{
    [Table("BookingOrder")]
    public class BookingOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid BookingOrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Client { get; set; }
        public string Address { get; set; }
        public bool Shipment { get; set; }
        public string Phone { get; set; }
        public bool IsOrder { get; set; }
        public bool IsBooking { get; set; }
    }
}
