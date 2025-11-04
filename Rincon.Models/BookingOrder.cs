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
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Comments { get; set; }
        [NotMapped]
        public bool IsCancellable 
        { 
            get 
            { 
                if (Status == OrderStatus.Cancelado) return false;
                
                if (IsBooking) 
                {
                    // Para reservas: solo se puede cancelar si es reserva y no está cancelado
                    return Status == OrderStatus.Reserva;
                }
                else if (IsOrder) 
                {
                    // Para órdenes: se puede cancelar si no está cancelado
                    return true;
                }
                
                return false;
            } 
        }
    }

    public enum OrderStatus
    {
        Despachado,
        Enviado,
        Cancelado,
        Reserva
    }
}
