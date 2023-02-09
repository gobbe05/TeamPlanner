using System.ComponentModel.DataAnnotations;

namespace TeamPlanner.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int MoneyUsed { get; set; }
        public int TimeUsed { get; set; }
        public int Budget { get; set; }
    }
}
