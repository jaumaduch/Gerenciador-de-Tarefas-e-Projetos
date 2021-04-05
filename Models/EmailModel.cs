using System.ComponentModel.DataAnnotations;

namespace MyTasks.Models
{
    public class EmailModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "O email deve ser informado")]
        public string Email { get; set; }
    }
}