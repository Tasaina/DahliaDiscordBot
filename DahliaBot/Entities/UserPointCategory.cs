using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DahliaBot.Entities
{
    public partial class UserPointCategory
    {
        [Key]
        public Guid Id { get; set; }
        public int Points { get; set; }
        public virtual PointCategory Category { get; set; }
    }
}