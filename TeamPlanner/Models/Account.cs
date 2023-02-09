using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamPlanner.Models
{
    public class Account : IdentityUser
    {
        public string? Work { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set;}
        public int Salary { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool Accepted { get; set; }
    }
}
