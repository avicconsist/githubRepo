using System;
using System.Collections.Generic;
using System.Linq;
using XMSTaxonomyManagment.ViewModels;

namespace XMSTaxonomyManagment.Repositories.Common
{
    public interface ITaxonomyReportRepository
    {
        void Add(TaxonomyReportModel entity);
        void Remove(TaxonomyReportModel entity);
        void Update(TaxonomyReportModel entity);
        TaxonomyReportModel[] GetTaxonomyReportsByTaxonomyId(string taxonomyId);  
        bool IsUniqueTaxonomyReportId(string taxonomyId, string reportId); 
        TaxonomyReportIdAndDescription[] GetShortTaxonomyReportsByTaxonomyId(string taxonomyId);
    }
}