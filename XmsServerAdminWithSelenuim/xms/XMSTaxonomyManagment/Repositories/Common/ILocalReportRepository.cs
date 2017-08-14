using System;
using System.Collections.Generic;
using System.Linq;
using XMSTaxonomyManagment.ViewModels;

namespace XMSTaxonomyManagment.Repositories.Common
{
    public interface ILocalReportRepository
    {
        void Add(LocalReportModel entity);
        void Remove(LocalReportModel entity);
        void Update(string oldSourceId, string oldId, LocalReportModel entity);
        LocalReportModel[] GetLocalReportsByTaxonomyId(string taxonomyId);   
        bool IsUniqueLocalReportId(string taxonomyId, string reportIds);
    }
}