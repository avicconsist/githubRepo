using System.Collections.Generic;

namespace XMSTaxonomyManagment.ViewModels
{
    public class UpdateLocalReportViewModel : LocalReportViewModel  
    {
        public string OldSourceId { get; set; }
        public string OldId { get; set; }
    }
}