using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XMSTaxonomyManagment.ViewModels
{
   
    public class TaxonomyReportViewModel
    { 
        public string Id { get; set; }
        public string TaxonomyId { get; set; }
        public string Description { get; set; } 
        public PeriodTypeViewModel PeriodType { get; set; } 
        public string EntryUri { get; set; } 
        public string FileName { get; set; } 
        public string EntityIdentifire { get; set; }
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

    }
}