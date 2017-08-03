using System;
using XMSTaxonomyManagment.Repositories.Common;


namespace XMSTaxonomyManagment.Repositories
{
    public interface IConsistRepositoryContext : IDisposable 
    {
        ITaxonomyReportRepository TaxonomyReportRepository { get; } 
        ILocalReportRepository LocalReportRepository { get; } 
        IPeriodTypeRepository PeriodTypeRepository { get; }
        ILocalEntityRepository LocalEntityRepository { get; }
        ITaxonomyRepository TaxonomyRepository { get; }

        void Commit();
        void Rollback();
    }
}