﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rincon.Models
{
    /// <summary>
    /// User details
    /// </summary>
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }
    }
}
