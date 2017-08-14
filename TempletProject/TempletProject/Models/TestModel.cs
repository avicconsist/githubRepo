using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TempletProject.ViewModels
{ 
    public class TestModel
    {
        public string  Id { get; set; } 
        public string Description { get; set; }   
        public DateTime CreatedDate { get; set; } 
    }
}