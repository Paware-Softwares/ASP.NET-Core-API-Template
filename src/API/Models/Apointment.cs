using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Apointment : Entity
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Value { get; set; }

        public virtual Customer? Customer { get; set; }

        [Required]
        public Guid CustomerId { get; set; }
    }
}