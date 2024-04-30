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
    public class Product : BindableItem

    {
        private string location;
        private string supplier;
        private string comment;

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
        public string Location {
            get => this.location;
            set
            {
                this.location = value;
                OnPropertyChanged(nameof(Location));
            }
        }
        public string Supplier {
            get => this.supplier;
            set
            {
                this.supplier = value;
                OnPropertyChanged(nameof(Supplier));
            }
        }
        public string Comment {
            get => this.comment;
            set
            {
                this.comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }


        public override string ToString()
        {
            return $"{Id} {Description} {WoodState}";
        }

    }
}
