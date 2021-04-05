using System.ComponentModel.DataAnnotations;

namespace MyTasks.Models
{
    public class RegistrationModel : LoginModel 
    {

        [Display(Name = "Confirmar Senha")]
        [Required(ErrorMessage = "Você deve confirmar a senha")]
        [Compare("Password", ErrorMessage = "As senhas não conferem !")]
        public string ConfirmPassword { get; set; }

    }
}