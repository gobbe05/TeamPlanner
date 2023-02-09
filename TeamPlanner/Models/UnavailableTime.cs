using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TeamPlanner.Models
{
    public class UnavailableTime
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
    }
}
