using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rincon.Models
{
    /// <summary>
    /// User details
    /// </summary>
    public class User : BindableItem
    {
        private string name;


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }

        public string Name 
        {
            get => this.name;
            set
            {
                this.name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Password { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public Role Role { get; set; }
    }
}
