using System.Collections.Generic;

namespace XMSTaxonomyManagment.ViewModels
{
    public class IsUniqIdViewModel
    {
        public bool IsUniqId { get; set; }
        public List<string> Errors { get; set; }  
        public string ReportId { get; set; }
        public string ReportSourceId { get; set; }
    } 
}