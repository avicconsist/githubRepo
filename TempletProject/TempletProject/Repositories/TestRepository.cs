using Dapper;
using System;
using System.Data;
using System.Linq;
using TempletProject.ViewModels;
using TempletProject.Repositories.Common;
using TempletProject.Common;

namespace TempletProject.Repositories
{
    public class TestRepository : OracleRepository, ITestRepository
    {
        public TestRepository(IDbConnection connection, ILogger logger = null) : base(connection, logger)
        {
        }
         
        public TestModel[] GetTestEntities()
        {
            return Connection.Query("SELECT * FROM XMS_LOCAL_ENTITIES").Select(
            q => new TestModel()
            {
                Id = q.ID,
                Description = q.DESCRIPTION
            }
        ).ToArray();
        }

        public TestModel  GetTestById(string id)
        { 
            string query = @"SELECT * 
                             FROM XMS_LOCAL_ENTITIES  
                             WHERE  ID = :Id";

            return Connection.QuerySingle<TestModel>(query, new
            {
                Id = id
            });

        }
        public void Add(TestModel entity)
        {
            LogLine(string.Format("adding {0} - {1} to database.", entity.Id, entity.Description));

            int rowsAffected = Connection.Execute(@"INSERT INTO XMS_LOCAL_ENTITIES ( ID,DESCRIPTION)
                                                                       VALUES (:Id, :Description) ",
                        new
                        {
                            Description = entity.Description,
                            Id = entity.Id
                        });


            if (rowsAffected == 0)
            {
                throw new NoRowsAffectedException("Add LocalEntity failed rowsAffected :" + rowsAffected);
            }

        }
        public void Update(string oldId, TestModel entity)
        {

            LogLine(string.Format("update {0} - {1} in database.", entity.Id, entity.Description));

            int rowsAffected = Connection.Execute(@"UPDATE XMS_LOCAL_ENTITIES  
                                                       SET ID = :Id,
                                                           DESCRIPTION = :Description   
                                                     WHERE ID = :OldId ",
              new
              {
                  Description = entity.Description,
                  Id = entity.Id,
                  OldId = oldId
              });

            if (rowsAffected == 0)
            {
                throw new NoRowsAffectedException("Update LocalEntity failed rowsAffected :" + rowsAffected);
            }

        }
        public void Remove(TestModel entity)
        {

            LogLine(string.Format("remove {0} - {1} from database.", entity.Id, entity.Description));

            int rowsAffected = Connection.Execute(@"DELETE FROM XMS_LOCAL_ENTITIES  
                                                      WHERE ID = :Id ",
                                                    new
                                                    {
                                                        Id = entity.Id
                                                    });
            if (rowsAffected == 0)
            {
                throw new NoRowsAffectedException("Remove Taxonomy failed rowsAffected :" + rowsAffected);
            }
        }
        public bool IsUniqueId(string id)
        {

            string query = @"SELECT COUNT(*) 
                                    FROM XMS_LOCAL_ENTITIES  
                                    WHERE ID = :Id ";

            var queryresults = Connection.ExecuteScalar<int>(query, new
            {
                Id = id
            });

            return queryresults == 0;

        }

    }
}