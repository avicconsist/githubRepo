using Dapper;
using System;
using System.Data;
using System.Linq;
using XMSTaxonomyManagment.Common;
using XMSTaxonomyManagment.ViewModels;
using XMSTaxonomyManagment.Repositories.Common;



namespace XMSTaxonomyManagment.Repositories
{
    public class PeriodTypeRepository : OracleRepository, IPeriodTypeRepository
    {
        public PeriodTypeRepository(IDbConnection connection, ILogger logger = null) : base(connection, logger)
        {
        }  
        public PeriodTypeModel[] GetPeriodTypes()
        {
           
            return Connection.Query("SELECT ID, DESCRIPTION FROM XMS_PERIOD_TYPES").Select(
              q => new PeriodTypeModel()
              {
                  PeriodType = OracleHelpers.StringToPeriodType(q.ID),
                  Description = q.DESCRIPTION
              }
          ).ToArray();
        } 
       
    }
}