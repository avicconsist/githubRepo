using System;
using System.Collections.Generic;
using System.Linq;
using XMSTaxonomyManagment.ViewModels;

namespace XMSTaxonomyManagment.Repositories.Common
{
    public interface IPeriodTypeRepository
    {
        PeriodTypeModel[] GetPeriodTypes(); 
    }
}
