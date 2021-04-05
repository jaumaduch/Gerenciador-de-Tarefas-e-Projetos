//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyTasks.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Project
    {
        public Project()
        {
            this.Tasks = new HashSet<Task>();
        }
    
        public int Id { get; set; }
        [Display(Name = "T�tulo")]
        public string Title { get; set; }
         [Display(Name = "Criado em")]
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> UserId { get; set; }
        [Display(Name = "Usu�rio")]
        public virtual User User { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
