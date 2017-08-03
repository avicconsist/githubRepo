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



    public class TaxonomyRepository : OracleRepository, ITaxonomyRepository
    {
        public TaxonomyRepository(IDbConnection connection, ILogger logger = null) : base(connection, logger)
        {
        }


        public TaxonomyModel GetTaxonomyById(string taxonomyId)
        {


            string query = @"SELECT * 
                             FROM XMS_TAXONOMIES  
                             WHERE  TAXONOMY_ID = :TaxonomyId";

            return Connection.QuerySingle<TaxonomyModel>(query, new
            {
                TaxonomyId = taxonomyId
            });

        }
        public TaxonomyModel[] GetAllTaxonomies()
        {
            return Connection.Query<TaxonomyModel>(@"SELECT * 
                                                     FROM XMS_TAXONOMIES 
                                                     ORDER BY TAXONOMY_DATE DESC").ToArray();
        }
        public TaxonomyIdAndDescription[] GetShortTaxonomies()
        {
            return Connection.Query<TaxonomyIdAndDescription>(@"SELECT DESCRIPTION ,TAXONOMY_ID 
                                                                  FROM XMS_TAXONOMIES 
                                                                  ORDER BY TAXONOMY_DATE DESC").ToArray();
        }
        public void Add(TaxonomyModel entity)
        {
            LogLine(string.Format("adding {0} - {1} to database.", entity.TaxonomyId, entity.Description));

            int rowsAffected = 0;

            rowsAffected = Connection.Execute(@"INSERT INTO XMS_TAXONOMIES (  TAXONOMY_ID,
                                                                              DESCRIPTION, 
                                                                              TAXONOMY_DATE, 
                                                                              ENTITY_IDENTIFIER,
                                                                              CURRENCY, 
                                                                              DECIMALS, 
                                                                              ENTITY_SCHEMA, 
                                                                              TAXONOMY_CREATION_DATE,
                                                                              TN_PROCESSOR_ID,
                                                                              DECIMAL_DECIMALS, 
                                                                              INTEGER_DECIMALS, 
                                                                              MONETARY_DECIMALS,
                                                                              PURE_DECIMALS,
                                                                              SHARES_DECIMALS, 
                                                                              TN_REVISION_ID)
                                                                       VALUES (:TaxonomyId,
                                                                                :Description,
                                                                                :TaxonomyDate,
                                                                                :EntityIdentifier, 
                                                                                :Currency, 
                                                                                :Decimals,
                                                                                :EntitySchema,
                                                                                :TaxonomyCreationDate,
                                                                                :TnProcessorId,
                                                                                :DecimalDecimals,
                                                                                :IntegerDecimals,
                                                                                :MonetaryDecimals,
                                                                                :PureDecimals,
                                                                                :SharesDecimals,
                                                                                :TnRevisionId ) ",
                        new
                        {
                            Description = entity.Description,
                            TaxonomyId = entity.TaxonomyId,
                            TaxonomyDate = entity.TaxonomyDate,
                            EntityIdentifier = entity.EntityIdentifier,
                            Currency = entity.Currency,
                            Decimals = entity.Decimals,
                            EntitySchema = entity.EntitySchema,
                            TaxonomyCreationDate = entity.TaxonomyCreationDate,
                            TnProcessorId = entity.TnProcessorId,
                            DecimalDecimals = entity.DecimalDecimals,
                            IntegerDecimals = entity.IntegerDecimals,
                            MonetaryDecimals = entity.MonetaryDecimals,
                            PureDecimals = entity.PureDecimals,
                            SharesDecimals = entity.SharesDecimals,
                            TnRevisionId = entity.TnRevisionId,
                        });


            if (rowsAffected == 0)
            {
                throw new NoRowsAffectedException("Add Taxonomy failed rowsAffected :" + rowsAffected);
            }

        }
        public void Update(string oldTaxonomyId, TaxonomyModel entity)
        {
            int rowsAffected = 0;
            LogLine(string.Format("update {0} - {1} in database.", entity.TaxonomyId, entity.Description));

            rowsAffected = Connection.Execute(@"UPDATE  XMS_TAXONOMIES  
                                                SET   DESCRIPTION =: Description,
                                                      TAXONOMY_ID = :TaxonomyId,
                                                      TAXONOMY_DATE = :TaxonomyDate, 
                                                      ENTITY_IDENTIFIER = :EntityIdentifier,  
                                                      CURRENCY = :Currency,  
                                                      DECIMALS = :Decimals, 
                                                      ENTITY_SCHEMA = :EntitySchema, 
                                                      TAXONOMY_CREATION_DATE = :TaxonomyCreationDate, 
                                                      TN_PROCESSOR_ID = :TnProcessorId, 
                                                      DECIMAL_DECIMALS = :DecimalDecimals, 
                                                      INTEGER_DECIMALS = :IntegerDecimals, 
                                                      MONETARY_DECIMALS = :MonetaryDecimals, 
                                                      PURE_DECIMALS = :PureDecimals, 
                                                      SHARES_DECIMALS = :SharesDecimals, 
                                                      TN_REVISION_ID = :TnRevisionId
                                               WHERE  TAXONOMY_ID = :OldTaxonomyId",
          new
          {
              Description = entity.Description,
              TaxonomyId = entity.TaxonomyId,
              TaxonomyDate = entity.TaxonomyDate,
              EntityIdentifier = entity.EntityIdentifier,
              Currency = entity.Currency,
              Decimals = entity.Decimals,
              EntitySchema = entity.EntitySchema,
              TaxonomyCreationDate = entity.TaxonomyCreationDate,
              TnProcessorId = entity.TnProcessorId,
              DecimalDecimals = entity.DecimalDecimals,
              IntegerDecimals = entity.IntegerDecimals,
              MonetaryDecimals = entity.MonetaryDecimals,
              PureDecimals = entity.PureDecimals,
              SharesDecimals = entity.SharesDecimals,
              TnRevisionId = entity.TnRevisionId,
              OldTaxonomyId = oldTaxonomyId
          });

            if (rowsAffected == 0)
            {
                throw new NoRowsAffectedException("Update Taxonomy failed rowsAffected :" + rowsAffected);
            }

        }
        public void Remove(TaxonomyModel entity)
        {

            LogLine(string.Format("remove {0} - {1} from database.", entity.TaxonomyId, entity.Description));
            int rowsAffected = 0;

            rowsAffected = Connection.Execute(@"DELETE FROM XMS_TAXONOMIES  
                                                        WHERE TAXONOMY_ID = :TaxonomyId ",
                                                    new
                                                    {
                                                        TaxonomyId = entity.TaxonomyId
                                                    });
            if (rowsAffected == 0)
            {
                throw new NoRowsAffectedException("Remove Taxonomy failed rowsAffected :" + rowsAffected);
            }
        }
        public bool IsUniqueId(string taxonomyId)
        {

            string query = @"SELECT COUNT(*) 
                                    FROM XMS_TAXONOMIES  
                                    WHERE TAXONOMY_ID  = :TaxonomyId ";

            var queryresults = Connection.ExecuteScalar<int>(query, new
            {
                TaxonomyId = taxonomyId
            });

            return queryresults == 0;

        }
    }
}