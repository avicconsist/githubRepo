using System;
using System.Collections.Generic;
using TempletProject.Common;
using TempletProject.ViewModels;

namespace TempletProject.Repositories.Common
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