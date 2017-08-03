using System.Data;
using TempletProject.Common;

namespace TempletProject.Repositories.Common
{
    public class OracleRepository : BaseRepository
    {
        public IDbConnection Connection { get; }

        public OracleRepository(IDbConnection connection, ILogger logger = null) : base(logger)
        {
            Connection = connection;
        }
    }
}