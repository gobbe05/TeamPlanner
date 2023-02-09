using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamPlanner.Models
{
    public class GroupUser
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("GroupId")]
        public int GroupId { get; set; }
        [ForeignKey("WorkerId")]
        public string WorkerId { get; set; }
    }
}
