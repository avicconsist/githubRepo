using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using XMSTaxonomyManagment.Common;
using XMSTaxonomyManagment.Repositories.Common;


namespace XMSTaxonomyManagment.Repositories
{

    public class ConsistRepositoryContext : IDisposable, IConsistRepositoryContext
    {
        private readonly TransactedOracleConnection _connection; 

        public ITaxonomyReportRepository TaxonomyReportRepository { get; }

        public ILocalReportRepository LocalReportRepository { get; }

        public IPeriodTypeRepository PeriodTypeRepository { get; }

        public ILocalEntityRepository LocalEntityRepository { get; }

        public ITaxonomyRepository TaxonomyRepository { get; }

        

        public void Commit()
        {
            _connection.Transaction.Commit();
        }

        public void Rollback()
        {
            _connection.Transaction.Rollback();
        }
         
        public ConsistRepositoryContext(ILogger logger = null)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            var conection = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString; 
            _connection = new TransactedOracleConnection(conection); 

            TaxonomyReportRepository = new TaxonomyReportRepository(_connection, logger);
            LocalReportRepository = new LocalReportRepository(_connection, logger);
            PeriodTypeRepository = new PeriodTypeRepository(_connection, logger);
            LocalEntityRepository = new LocalEntityRepository(_connection, logger);
            TaxonomyRepository = new TaxonomyRepository(_connection, logger);
        }  
        
        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}