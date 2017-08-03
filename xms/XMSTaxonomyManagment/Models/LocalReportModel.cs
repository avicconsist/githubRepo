using System;

namespace XMSTaxonomyManagment.ViewModels
{
  
    public class LocalReportModel
    {
        public string Id { get; set; }
        public string TaxonomyId { get; set; }
        public string Description { get; set; }
        public string SourceId { get; set; }
        public PeriodType? PeriodType { get; set; }
        public string EntryUri { get; set; }
        public string FileName { get; set; }
        public string EntityIdentifier { get; set; }
        public string Currency { get; set; }
        public string Decimals { get; set; }
        public string EntitySchema { get; set; }
        public string DecimalDecimals { get; set; }
        public string IntegerDecimals { get; set; }
        public string MonetaryDecimals { get; set; }
        public string PureDecimals { get; set; }
        public string SharesDecimals { get; set; }
        public string TnProcessorId { get; set; }
        public string TnRevisionId { get; set; }
        public string UpdateUser { get; set; }
        public string UpdateUserDsc { get; set; }
        public DateTime? UpdateDate { get; set; } 
    }
}