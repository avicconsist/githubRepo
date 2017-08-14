using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using XMSTaxonomyManagment.ViewModels;

namespace XMSTaxonomyManagment.ViewModels
{
    public class TaxonomyMainViewModel
    {
        
        public TaxonomyReportMainViewModel TaxonomyReport { get; set; }
        public LocalReportMainViewModel LocalReport { get; set; }

    }
}