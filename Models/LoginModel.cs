using System.ComponentModel.DataAnnotations;

namespace MyTasks.Models
{
    public class LoginModel  : EmailModel 
    {
        [Display(Name = "Senha")]
        [Required(ErrorMessage = "A senha é obrigatória")]
        public string Password { get; set; }
    }
}