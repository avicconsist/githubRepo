using System;
using System.Collections.Generic;
using XMSTaxonomyManagment.Common;
using XMSTaxonomyManagment.ViewModels;

namespace XMSTaxonomyManagment.Repositories.Common
{
    public class BaseRepository
    {
        public ILogger Logger { get; }


        protected void LogLine(string value)
        {
            if (Logger != null)
                Logger.WriteLine(value);
        }

        public BaseRepository(ILogger logger = null)
        {
            Logger = logger;
        }
    }
}