using System.ComponentModel.DataAnnotations;

namespace VolgaIT.Models
{
    public class Application
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter the application name.")]
        public string Name { get; set; }


        public DateTime Date { get; set; }

        public string UserId { get; set; }
    }
}