using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rincon.Models
{
    [Table("Task")]
    public class TaskItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool Priority { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ProductSourceId { get; set; }
        public string ProductSource { get; set; }
        public string Quantity { get; set; }
        public string ProductDestinationId { get; set; }
        public string ProductDestination { get; set; }
        public TaskStatus TaskStatus { get; set; }
        public string OperatorId { get; set; }
        public string OperatorName { get; set; }
        public DateTime? StartDate { get; set; }
        public string OperatorComment { get; set; }
        public string ProcessErrorQuantity { get; set; }
        [NotMapped]
        public Product ProductSourceDetail { get; set; }
        [NotMapped]
        public Product ProductDestinationDetail { get; set; }
    }

    public enum TaskStatus 
    {
        Pendiente,
        Iniciada,
        Finalizada
    }
}
