using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XMSTaxonomyManagment.ViewModels
{
    public class ResultsViewModel
    {
        public bool EntityUpdated { get; set; }
        public List<string> Errors { get; set; } 
        public string EntityId { get; set; }
    } 
}