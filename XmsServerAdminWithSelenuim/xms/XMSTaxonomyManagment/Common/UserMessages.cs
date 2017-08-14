namespace XMSTaxonomyManagment.Common
{
    public static class UserMessages
    {
        public static string UNKNOWN_ERROR
        {
            get
            {
                return "אירעה שגיאה לא מוכרת";
            }
        }

        public static string TAXONOMYID_ERROR
        {
            get
            {
                return "שדה קוד טקסונומיה הינו שדה חובה";
            }
        }
        public static string SOURCEID_ERROR
        {
            get
            {
                return "שדה קוד דוח מקור הינו שדה חובה";
            }
        }
        public static string ID_ERROR
        {
            get
            {
                return "שדה קוד דוח הינו שדה חובה";
            }
        }
        public static string ROW_NOT_FOUND_ERROR
        {
            get
            {
                return "לא נמצאה שורה למחיקה";
            }
        }
        public static string TAXSONOMY_IS_NOT_UNIQ_ID
        {
            get
            {
                return "קוד טקסונומיה קיים במערכת יש לבחור קוד אחר";
            }
        }
    }
}