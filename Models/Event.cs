using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VolgaIT.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter the event name.")]
        public string Name { get; set; }

        public string? Information { get; set; }

        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Enter the app id.")]
        [ForeignKey("Application")]
        public int AppId { get; set; }
        public virtual Application Application { get; set; }
    }
}