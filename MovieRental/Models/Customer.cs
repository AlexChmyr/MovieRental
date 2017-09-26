using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MovieRental.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public bool IsSubscribedToNotification { get; set; }

        public MembershipType MembershipType { get; set; }

        [Display(Name = "Membership type")]
        public int MembershipTypeId { get; set; }

        [Min18Years]
        [Display(Name="Date of birth")]
        public DateTime? Birthdate { get; set; }
    }
}