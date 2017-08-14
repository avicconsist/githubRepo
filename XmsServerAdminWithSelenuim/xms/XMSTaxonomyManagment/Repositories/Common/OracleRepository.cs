using System.Data;
using XMSTaxonomyManagment.Common;

namespace XMSTaxonomyManagment.Repositories.Common
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