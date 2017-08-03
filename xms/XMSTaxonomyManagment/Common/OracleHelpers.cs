using System;
using System.Collections.Generic;
using System.Linq;
using XMSTaxonomyManagment.ViewModels;

namespace XMSTaxonomyManagment.Common
{
    public static class OracleHelpers 
    {
        public static PeriodType? StringToPeriodType(string periodType)
        {
            if (string.IsNullOrWhiteSpace(periodType))
                return null;

            switch (periodType)
            {
                case "MONTHLY": return PeriodType.Monthly;
                case "HALF-YEARLY": return PeriodType.HalfYearly;
                case "IMMEDIATE": return PeriodType.Immediate;
                case "WEEKLY": return PeriodType.Weekly;
                case "YEARLY": return PeriodType.Yearly;
                case "QUARTERLY": return PeriodType.Quarterly;
                default:
                    throw new ArgumentOutOfRangeException(nameof(periodType), periodType, null);
            }
        }

        public static string PeriodTypeToString(PeriodType? periodType)
        {
            if (!periodType.HasValue) return null;

            switch (periodType)
            {
                case PeriodType.Monthly:
                    return "MONTHLY";
                case PeriodType.Quarterly:
                    return "QUARTERLY";
                case PeriodType.Immediate:
                    return "IMMEDIATE";
                case PeriodType.Weekly:
                    return "WEEKLY";
                case PeriodType.Yearly:
                    return "YEARLY";
                case PeriodType.HalfYearly:
                    return "HALF-YEARLY";
                default:
                    throw new ArgumentOutOfRangeException(nameof(periodType), periodType, null);
            }
        }
         
    }

}