using System.Data.SqlClient;

namespace QuanLySuKienVaBanVe.DAL
{
    /// <summary>Lớp cầu nối tương thích ngược, ủy quyền kết nối về Database.GetConnection().</summary>
    public static class DatabaseHelper
    {
        public static string ConnectionString => Database.ConnectionString;

        public static SqlConnection GetConnection()
        {
            return Database.GetConnection();
        }
    }
}
