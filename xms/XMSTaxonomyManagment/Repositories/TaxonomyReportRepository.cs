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
    public class TaxonomyReportRepository : OracleRepository, ITaxonomyReportRepository
    {
        public TaxonomyReportRepository(IDbConnection connection, ILogger logger = null) : base(connection, logger)
        {
        }

        public TaxonomyReportModel[] GetTaxonomyReportsByTaxonomyId(string taxonomyId)
        {

            string query = @"SELECT *  
                FROM XMS_TAXONOMY_REPORTS  
                WHERE TAXONOMY_ID  = :TaxonomyId 
                ORDER BY ID";


            return Connection.Query(query, new
            {
                TaxonomyId = taxonomyId
            }).Select(dbRow => new TaxonomyReportModel()
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

            }).ToArray();

        }
        public void Add(TaxonomyReportModel entity)
        {
            LogLine(string.Format("adding {0} - {1} to database.", entity.Id, entity.Description));

            int rowsAffected = Connection.Execute(@"INSERT INTO XMS_TAXONOMY_REPORTS (DESCRIPTION,
                                                                              PERIOD_TYPE,
                                                                              ENTRY_URI, 
                                                                              FILE_NAME, 
                                                                              ENTITY_IDENTIFIER,
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
                                                                              TAXONOMY_ID,
                                                                              ID)
                                                                       VALUES (:Description,
                                                                                :PeriodType,
                                                                                :EntryUri,
                                                                                :FileName,
                                                                                :EntityIdentifire,
                                                                                :Currency, 
                                                                                :Decimals,
                                                                                :EntitySchema,
                                                                                :DecimalDecimals,
                                                                                :IntegerDecimals,
                                                                                :MonetaryDecimals,
                                                                                :PureDecimals,
                                                                                :SharesDecimals,
                                                                                :TnProcessorId,
                                                                                :TnRevisonId,
                                                                                :TaxonomyId,
                                                                                :Id) ",
                        new
                        {
                            Description = entity.Description,
                            PeriodType = OracleHelpers.PeriodTypeToString(entity.PeriodType),
                            TaxonomyId = entity.TaxonomyId,
                            Id = entity.Id,
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
                            TnRevisonId = entity.TnRevisionId,

                        });

            if (rowsAffected == 0)
            {
                throw new NoRowsAffectedException("Add TaxonomyReport failed   rowsAffected :" + rowsAffected); 
            }

        }

        public void Update(TaxonomyReportModel entity)
        {

            LogLine(string.Format("update {0} - {1} in database.", entity.Id, entity.Description));

            int rowsAffected = Connection.Execute(@"UPDATE  XMS_TAXONOMY_REPORTS  
                            SET DESCRIPTION =: Description,
                            PERIOD_TYPE = :PeriodType,  
                            ENTRY_URI = :EntryUri, 
                            FILE_NAME = :FileName,  
                            ENTITY_IDENTIFIER = :EntityIdentifire,  
                            CURRENCY = :Currency, 
                            DECIMALS = :Decimals, 
                            ENTITY_SCHEMA = :EntitySchema, 
                            DECIMAL_DECIMALS = :DecimalDecimals, 
                            INTEGER_DECIMALS = :IntegerDecimals, 
                            MONETARY_DECIMALS = :MonetaryDecimals, 
                            PURE_DECIMALS = :PureDecimals, 
                            SHARES_DECIMALS = :SharesDecimals, 
                            TN_PROCESSOR_ID = :TnProcessorId, 
                            TN_REVISION_ID = :TnRevisonId
                            WHERE TAXONOMY_ID = :TaxonomyId  and ID = :Id",
         new
         {
             Description = entity.Description,
             PeriodType = OracleHelpers.PeriodTypeToString(entity.PeriodType),
             TaxonomyId = entity.TaxonomyId,
             Id = entity.Id,
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
             TnRevisonId = entity.TnRevisionId,
         }
       );

            if (rowsAffected == 0)
            {
                throw new NoRowsAffectedException("Update TaxonomyReport failed rowsAffected :" + rowsAffected);

                      }

        }

        public void Remove(TaxonomyReportModel entity)
        {

            LogLine(string.Format("remove {0} - {1} from database.", entity.Id, entity.Description));
            int rowsAffected = 0;

            rowsAffected = Connection.Execute(@"DELETE FROM XMS_TAXONOMY_REPORTS  
                                                        WHERE TAXONOMY_ID = :TaxonomyId AND ID = :Id",
                                                    new
                                                    {
                                                        Id = entity.Id,
                                                        TaxonomyId = entity.TaxonomyId
                                                    });
            if (rowsAffected == 0)
            {
                throw new NoRowsAffectedException("Remove TaxonomyReport failed rowsAffected :" + rowsAffected);
            }
        }

        public bool IsUniqueTaxonomyReportId(string taxonomyId, string reportId)
        {

            string query = @"SELECT COUNT(*) 
                                    FROM XMS_TAXONOMY_REPORTS  
                                    WHERE TAXONOMY_ID  = :TaxonomyId AND ID  = :Id";

            var queryresults = Connection.ExecuteScalar<int>(query, new
            {
                TaxonomyId = taxonomyId,
                Id = reportId
            });


            return queryresults == 0;
        }

        public TaxonomyReportIdAndDescription[] GetShortTaxonomyReportsByTaxonomyId(string taxonomyId)
        {
            return Connection.Query<TaxonomyReportIdAndDescription>(@"SELECT DESCRIPTION ,ID 
                                                                  FROM XMS_TAXONOMY_REPORTS 
                                                                  WHERE TAXONOMY_ID  = :taxonomyId 
                                                                  ORDER BY ID",
            new
            {
                taxonomyId = taxonomyId,
            }).ToArray();
        }
    }

}