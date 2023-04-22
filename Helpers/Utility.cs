using System.Data.SqlClient;

namespace CashOut.Helpers
{
    public static class Utility
    {
        public static string GetSafeString(this SqlDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetString(colIndex);

            return string.Empty;
        }

        public static decimal GetSafeDecimal(this SqlDataReader reader, int colIndex)
        {
            if(!reader.IsDBNull(colIndex))
                return reader.GetDecimal(colIndex);

            return 0;
        }

        public static int GetSafeInt(this SqlDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetInt32(colIndex);

            return 0;
        }

        public static long GetSafeLong(this SqlDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetInt64(colIndex);

            return 0;
        }
    }
}
