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
        private string description;
        private WoodState woodState;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        public string Description
        {
            get => description;
            set { description = value; OnPropertyChanged(nameof(Description)); }
        }

        public bool? Machimbre { get; set; }
        public bool? Deck { get; set; }
        public double? Thickness { get; set; }
        public double? Width { get; set; }
        public double? Length { get; set; }
        public double? Diameter { get; set; }
        public  ProductType  ProductType { get; set; }

        public WoodState WoodState
        {
            get => woodState;
            set { woodState = value; OnPropertyChanged(nameof(WoodState)); }
        }

        public Machimbre? MachimbreSate { get; set; }
        public DeckType? DeckSate { get; set; }

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

        public string DependOf { get; set; }

        [NotMapped]
        public string MachimbreDeckLabel =>
            Machimbre == true ? "Machimbre" :
            Deck == true ? "Deck" :
            string.Empty;

        [NotMapped]
        public string MachimbreDeckSubLabel =>
            Machimbre == true && MachimbreSate.HasValue ? MachimbreSate.ToString() :
            Deck == true && DeckSate.HasValue ? DeckSate.ToString() :
            string.Empty;

        [NotMapped]
        public bool HasMachimbreOrDeck => Machimbre == true || Deck == true;

        public override string ToString()
        {
            return $"{Id} {Description} {WoodState}";
        }

    }
}
