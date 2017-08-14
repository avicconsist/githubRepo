using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using XMSTaxonomyManagment.Common;
using XMSTaxonomyManagment.ViewModels;
using XMSTaxonomyManagment.Repositories.Common;



namespace XMSTaxonomyManagment.Repositories
{
    public class LocalReportRepository : OracleRepository, ILocalReportRepository
    {
        public LocalReportRepository(IDbConnection connection, ILogger logger = null) : base(connection, logger)
        {
        }

        public LocalReportModel[] GetLocalReportsByTaxonomyId(string taxonomyId)
        {
            string query = @"SELECT *  
                FROM XMS_LOCAL_REPORTS  
                WHERE TAXONOMY_ID  = :TAXONOMY_ID 
                ORDER BY ID";


            return Connection.Query(query, new
            {
                TAXONOMY_ID = taxonomyId
            }).Select(dbRow => new LocalReportModel()
            {
                Currency = dbRow.CURRENCY,
                Decimals = dbRow.DECIMALS,
                DecimalDecimals = dbRow.DECIMAL_DECIMALS,
                Description = dbRow.DESCRIPTION,
                EntityIdentifier = dbRow.ENTITY_IDENTIFIER,
                EntitySchema = dbRow.ENTITY_SCHEMA,
                EntryUri = dbRow.ENTRY_URI,
                FileName = dbRow.FILE_NAME,
                Id = dbRow.ID,
                IntegerDecimals = dbRow.INTEGER_DECIMALS,
                MonetaryDecimals = dbRow.MONETARY_DECIMALS,
                PeriodType = OracleHelpers.StringToPeriodType(dbRow.PERIOD_TYPE),
                PureDecimals = dbRow.PURE_DECIMALS,
                SharesDecimals = dbRow.SHARES_DECIMALS,
                TaxonomyId = dbRow.TAXONOMY_ID,
                TnProcessorId = dbRow.TN_PROCESSOR_ID,
                TnRevisionId = dbRow.TN_REVISION_ID,
                SourceId = dbRow.SOURCE_ID,
                UpdateDate = dbRow.UPDATE_DATE,
                UpdateUser = dbRow.UPDATE_USER,
                UpdateUserDsc = dbRow.UPDATE_USER_DSC

            }).ToArray();
        }

        public void Add(LocalReportModel entity)
        {
            LogLine(string.Format("adding {0} - {1} to database.", entity.Id, entity.Description));


            int rowsAffected = Connection.Execute(@"INSERT INTO XMS_LOCAL_REPORTS (TAXONOMY_ID,
                                                                                        SOURCE_ID,
                                                                                        ID,
                                                                                        ENTRY_URI,
                                                                                        FILE_NAME,
                                                                                        DESCRIPTION,
                                                                                        ENTITY_IDENTIFIER,
                                                                                        PERIOD_TYPE,
                                                                                        CURRENCY,
                                                                                        DECIMALS,
                                                                                        ENTITY_SCHEMA,
                                                                                        DECIMAL_DECIMALS,
                                                                                        INTEGER_DECIMALS,
                                                                                        MONETARY_DECIMALS,
                                                                                        PURE_DECIMALS,
                                                                                        SHARES_DECIMALS,
                                                                                        TN_PROCESSOR_ID,
                                                                                        TN_REVISION_ID,
                                                                                        UPDATE_USER,
                                                                                        UPDATE_USER_DSC,
                                                                                        UPDATE_DATE) 
                                                                                VALUES (:TaxonomyId,
                                                                                        :SourceId,
                                                                                        :Id,
                                                                                        :EntryUri,
                                                                                        :FileName,
                                                                                        :Description,
                                                                                        :EntityIdentifire,
                                                                                        :PeriodType,
                                                                                        :Currency,
                                                                                        :Decimals,
                                                                                        :EntitySchema,
                                                                                        :DecimalDecimals,
                                                                                        :IntegerDecimals,
                                                                                        :MonetaryDecimals,
                                                                                        :PureDecimals,
                                                                                        :SharesDecimals,
                                                                                        :TnProcessorId,
                                                                                        :TnRevisionId,
                                                                                        :UpdateDate,
                                                                                        :UpdateUser,
                                                                                        :UpdateUserDsc)",
                           new
                           {
                               Description = entity.Description,
                               PeriodType = OracleHelpers.PeriodTypeToString(entity.PeriodType),
                               TaxonomyId = entity.TaxonomyId,
                               Id = entity.Id,
                               SourceId = entity.SourceId,
                               EntryUri = entity.EntryUri,
                               FileName = entity.FileName,
                               EntityIdentifire = entity.EntityIdentifier,
                               Currency = entity.Currency,
                               Decimals = entity.Decimals,
                               EntitySchema = entity.EntitySchema,
                               DecimalDecimals = entity.DecimalDecimals,
                               IntegerDecimals = entity.IntegerDecimals,
                               MonetaryDecimals = entity.MonetaryDecimals,
                               PureDecimals = entity.PureDecimals,
                               SharesDecimals = entity.SharesDecimals,
                               TnProcessorId = entity.TnProcessorId,
                               TnRevisionId = entity.TnRevisionId,
                               UpdateDate = entity.UpdateDate,
                               UpdateUser = entity.UpdateUser,
                               UpdateUserDsc = entity.UpdateUserDsc,
                           });

            if (rowsAffected == 0)
            {
                throw new NoRowsAffectedException("Add LocalReport failed rowsAffected :" + rowsAffected);
            }
        }

        public void Update(string oldSourceId, string oldId, LocalReportModel entity)
        {

            LogLine(string.Format("update {0} - {1} in database.", entity.Id, entity.Description));


            int rowsAffected = Connection.Execute(@"UPDATE XMS_LOCAL_REPORTS   
                            SET  
                                 SOURCE_ID = :SourceId,           
                                 ID = :Id,
                                 ENTRY_URI = :EntryUri,                     
                                 FILE_NAME = :FileName,                    
                                 DESCRIPTION = :Description,                 
                                 ENTITY_IDENTIFIER = :EntityIdentifire,            
                                 PERIOD_TYPE = :PeriodType,               
                                 CURRENCY = :Currency,                      
                                 DECIMALS = :Decimals ,                    
                                 ENTITY_SCHEMA = :EntitySchema ,       
                                 DECIMAL_DECIMALS = :DecimalDecimals ,             
                                 INTEGER_DECIMALS = :IntegerDecimals ,             
                                 MONETARY_DECIMALS = :MonetaryDecimals ,             
                                 PURE_DECIMALS = :PureDecimals ,              
                                 SHARES_DECIMALS = :SharesDecimals ,               
                                 TN_PROCESSOR_ID = :TnProcessorId ,               
                                 TN_REVISION_ID = :TnRevisionId ,               
                                 UPDATE_USER = :UpdateUser ,                  
                                 UPDATE_USER_DSC = :UpdateUserDsc ,               
                                 UPDATE_DATE = :UpdateDate               
                                 WHERE TAXONOMY_ID = :TaxonomyId 
                                   AND ID = :OldId
                                   AND SOURCE_ID = :OldSourceId",
         new
         {
             Description = entity.Description,
             PeriodType = OracleHelpers.PeriodTypeToString(entity.PeriodType),
             TaxonomyId = entity.TaxonomyId,
             Id = entity.Id,
             SourceId = entity.SourceId,
             EntryUri = entity.EntryUri,
             FileName = entity.FileName,
             EntityIdentifire = entity.EntityIdentifier,
             Currency = entity.Currency,
             Decimals = entity.Decimals,
             EntitySchema = entity.EntitySchema,
             DecimalDecimals = entity.DecimalDecimals,
             IntegerDecimals = entity.IntegerDecimals,
             MonetaryDecimals = entity.MonetaryDecimals,
             PureDecimals = entity.PureDecimals,
             SharesDecimals = entity.SharesDecimals,
             TnProcessorId = entity.TnProcessorId,
             TnRevisionId = entity.TnRevisionId,
             UpdateDate = entity.UpdateDate,
             UpdateUser = entity.UpdateUser,
             UpdateUserDsc = entity.UpdateUserDsc,
             OldSourceId = oldSourceId,
             OldId = oldId
         });

            if (rowsAffected == 0)
            {
                throw new NoRowsAffectedException("Update LocalReport failed rowsAffected :" + rowsAffected);
            }
        }

        public void Remove(LocalReportModel entity)
        {

            LogLine(string.Format("remove {0} - {1} from database.", entity.Id, entity.Description));
            int rowsAffected = 0;

            rowsAffected = Connection.Execute(@"DELETE FROM XMS_LOCAL_REPORTS  
                                                        WHERE TAXONOMY_ID = :TaxonomyId
                                                          AND SOURCE_ID = :SourceId
                                                          AND ID = :Id",
                                                   new
                                                   {
                                                       Id = entity.Id,
                                                       SourceId = entity.SourceId,
                                                       TaxonomyId = entity.TaxonomyId
                                                   });

            if (rowsAffected == 0)
            {
                throw new NoRowsAffectedException("Remove LocalReport failed rowsAffected :" + rowsAffected);
            }

        }
        public bool IsUniqueLocalReportId(string taxonomyId, string reportId)
        {
             
            string query = @"SELECT COUNT(*) 
                                    FROM XMS_LOCAL_REPORTS  
                                    WHERE TAXONOMY_ID  = :TaxonomyId  
                                    AND ID  = :Id";

          

           var queryresults = Connection.ExecuteScalar<int>(query, new
            {
                TaxonomyId = taxonomyId,
                Id = reportId,
                
            });

            return queryresults == 0;
        }

    }
}