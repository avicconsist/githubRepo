using System.Collections.Generic;

namespace TempletProject.ViewModels
{
    public class ColumnValidationResult
    {
        public string EntityId { get; set; }
        public string ColumnId { get; set; }
        public bool IsValid { get; set; }
        public List<string> ValidationMessages { get; private set; }

        public ColumnValidationResult()
        {
            ValidationMessages = new List<string>();
        }
    }
}