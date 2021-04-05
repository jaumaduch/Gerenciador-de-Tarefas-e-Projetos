using System.ComponentModel.DataAnnotations;

namespace MyTasks.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        [Display(Name = "Título")]
        public string Title { get; set; }
        [Display(Name = "Data Conclusão")]
        public string DueDate { get; set; }
        [Display(Name = "Importância")]
        public string Importance { get; set; }
        public string Status { get; set; }
        public int ProjectId { get; set; }
    }
}