namespace Pokemon.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime DateAdd { get; set; }

        public virtual User User { get; set; }
    }
}
