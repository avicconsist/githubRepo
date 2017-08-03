using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using XMSTaxonomyManagment.ViewModels;

namespace XMSTaxonomyManagment.ViewModels
{
    public class LocalReportMainViewModel
    {
        public TaxonomyDescriptionViewModel[] Taxonomies { get; set; }
        public TaxonomyDescriptionViewModel SelectedTaxonomy { get; set; } 
    }
}