using System.ComponentModel.DataAnnotations;

namespace MyTasks.Models
{
    public class ChangePasswordModel 
    {
        [Display(Name = "Senha")]
        [Required(ErrorMessage = "A senha anterior deve ser informada")]
        public string Password { get; set; }

        [Display(Name="Nova Senha")]
        [Required(ErrorMessage = "Informe a nova senha")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirmar Senha")]
        [Required(ErrorMessage = "Confirme a senha")]
        [Compare("NewPassword", ErrorMessage = "Senhas não conferem!")]
        public string ConfirmPassword { get; set; }

    }
}