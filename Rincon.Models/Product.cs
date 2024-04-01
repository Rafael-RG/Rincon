using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rincon.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string Description { get; set; }
        public bool? Machimbre { get; set; }
        public double? Thickness { get; set; }
        public double? Width { get; set; }
        public double? Length { get; set; }
        public double? Diameter { get; set; }
        public  ProductType  ProductType { get; set; }
        public WoodState WoodState { get; set; }
        public Machimbre? MachimbreSate { get; set; }
        public string? Location { get; set; }
        public string? Supplier { get; set; }
        public string? Comment { get; set; }


        public override string ToString()
        {
            return $"{Id} {Description} {WoodState}";
        }

    }
}
