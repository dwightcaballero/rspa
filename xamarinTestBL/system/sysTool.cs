namespace xamarinTestBL
{
    public partial class system
    {
        public class sysTool
        {
            public static string CleanString(string text)
            {
                if (string.IsNullOrWhiteSpace(text)) return string.Empty;
                text = text.Replace("\"\"", "\"\"\"\"");
                text = text.Replace("';", " ");
                text = text.Replace("'", "''");
                return text;
            }
        }
    }
}
