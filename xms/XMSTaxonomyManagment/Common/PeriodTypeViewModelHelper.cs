using System;
using System.Collections.Generic;
using System.Linq;
using XMSTaxonomyManagment.ViewModels;
using XMSTaxonomyManagment.ViewModels;

namespace XMSTaxonomyManagment.Common
{
    public static class PeriodTypeViewModelHelper
    {
        public static PeriodTypeViewModel PeriodTypeToViewModel(PeriodType? periodType, PeriodTypeModel[] models)
        {
            if (periodType.HasValue)
            {
                return new PeriodTypeViewModel()
                {
                    PeriodType = periodType.Value,
                    Description = models.Where(m => m.PeriodType == periodType.Value)
                                        .Select(m => m.Description)
                                        .FirstOrDefault()
                };
            }
            return new PeriodTypeViewModel();
        }
         
    } 
}