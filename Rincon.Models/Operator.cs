using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rincon.Models
{
    public class Operator : BindableItem
    {
        private string id;
        private string name;
        private string lastName;
        private string pin;
        private DateTime createDate;

        public string Id
        {
            get => this.id;
            set
            {
                this.id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string Name
        {
            get => this.name;
            set
            {
                this.name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string LastName
        {
            get => this.lastName;
            set
            {
                this.lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public string Pin
        {
            get => this.pin;
            set
            {
                this.pin = value;
                OnPropertyChanged(nameof(Pin));
            }
        }

        public DateTime CreateDate
        {
            get => this.createDate;
            set
            {
                this.createDate = value;
                OnPropertyChanged(nameof(CreateDate));
            }
        }

        public string FullName => $"{Name} {LastName}".Trim();
    }
}
