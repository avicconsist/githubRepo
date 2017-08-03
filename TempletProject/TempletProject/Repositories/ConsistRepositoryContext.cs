 
using System;
using System.Configuration;
using TempletProject.Common;
using TempletProject.Repositories.Common;  


namespace TempletProject.Repositories
{

    public class ConsistRepositoryContext : IDisposable, IConsistRepositoryContext
    {
        private readonly TransactedOracleConnection _connection; 

        
        public ITestRepository TestRepository { get; }
         

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

            TestRepository = new TestRepository(_connection, logger);

        }  
        
        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}