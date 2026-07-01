    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rincon.Models
{
    public class Movement : BindableItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string ProductName { get; set; }
        [NotMapped]
        public Product Product { get; set; }
        [NotMapped]
        public string ProductDescription => Product?.Description ?? ProductName;
        public double Quantity { get; set; }
        public string MovementType { get; set; }
        public string UserName { get; set; }
    }

    public class MovementTypes 
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
