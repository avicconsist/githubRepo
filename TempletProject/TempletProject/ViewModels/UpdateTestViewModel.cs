using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TempletProject.ViewModels
{ 
    public class UpdateTestViewModel :TestViewModel
    { 
        public string OldId { get; set; } 
    }
}