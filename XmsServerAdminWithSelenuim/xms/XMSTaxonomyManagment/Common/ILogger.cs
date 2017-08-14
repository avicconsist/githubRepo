using System;
﻿using System.Collections.Generic; 
using XMSTaxonomyManagment.ViewModels;

namespace XMSTaxonomyManagment.Common
{
    public interface ILogger
    {
        void WriteLine(string value);
        void LogException(Exception e, string message = null);
         
        void LogError(string message);
        void LogWarnnig(string message);
         
    }
}