using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamPlanner.Models
{
    public class Time
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("WorkerId")]
        public string WorkerId { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Week { get; set; }
        public string Weekday { get; set; }
        [ForeignKey("GroupId")]
        public int GroupId { get; set; }
        public int Salary { get; set; }
    }
}
