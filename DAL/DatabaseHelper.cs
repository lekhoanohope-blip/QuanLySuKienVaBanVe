using System.Configuration;
using System.Data.SqlClient;

namespace QuanLySuKienVaBanVe.DAL
{
    public static class DatabaseHelper
    {
        public static string ConnectionString
        {
            get
            {
                var connSetting = ConfigurationManager.ConnectionStrings["ConnStr"];
                if (connSetting != null && !string.IsNullOrWhiteSpace(connSetting.ConnectionString))
                {
                    return connSetting.ConnectionString;
                }
                return @"Data Source=.;Initial Catalog=QuanLySuKienVaBanVe;Integrated Security=True;Encrypt=False";
            }
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
