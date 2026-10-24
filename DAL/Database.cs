using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace QuanLySuKienVaBanVe.DAL
{
    /// <summary>
    /// Lớp trợ giúp truy xuất cơ sở dữ liệu dùng chung (ADO.NET),
    /// hỗ trợ thực thi truy vấn độc lập và truy vấn trong SqlTransaction.
    /// </summary>
    public static class Database
    {
        private static string _connStr;

        private static string ConnStr
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_connStr))
                {
                    _connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
                    if (string.IsNullOrWhiteSpace(_connStr))
                    {
                        try
                        {
                            var exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                            var config = ConfigurationManager.OpenExeConfiguration(exePath);
                            _connStr = config.ConnectionStrings?.ConnectionStrings["ConnStr"]?.ConnectionString;
                        }
                        catch
                        {
                            // Bỏ qua lỗi đọc exe config
                        }
                    }
                }
                return _connStr;
            }
        }

        /// <summary>
        /// Tạo và trả về đối tượng SqlConnection mới (chưa mở kết nối).
        /// Bên gọi có trách nhiệm Open và Dispose kết nối này.
        /// </summary>
        public static SqlConnection GetConnection()
        {
            if (string.IsNullOrWhiteSpace(ConnStr))
            {
                throw new InvalidOperationException("Chuỗi kết nối 'ConnStr' trong App.config không tồn tại hoặc bị rỗng.");
            }
            return new SqlConnection(ConnStr);
        }

        #region Helper Methods

        private static void ValidateQuery(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentException("Câu lệnh truy vấn (hoặc tên Stored Procedure) không được để trống.", nameof(query));
            }
        }

        private static void AttachParameters(SqlCommand cmd, SqlParameter[] parameters)
        {
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    if (p != null)
                    {
                        if (p.Value == null)
                        {
                            p.Value = DBNull.Value;
                        }
                        cmd.Parameters.Add(p);
                    }
                }
            }
        }

        #endregion

        #region Truy vấn KHÔNG Transaction (Tự động quản lý mở/đóng Connection)

        /// <summary>
        /// Thực thi truy vấn SELECT và trả về kết quả dưới dạng DataTable.
        /// </summary>
        public static DataTable ExecuteQuery(string query, SqlParameter[] parameters = null, CommandType commandType = CommandType.Text)
        {
            ValidateQuery(query);
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = commandType;
                    AttachParameters(cmd, parameters);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        /// <summary>
        /// Thực thi lệnh INSERT, UPDATE, DELETE và trả về số dòng bị ảnh hưởng.
        /// </summary>
        public static int ExecuteNonQuery(string query, SqlParameter[] parameters = null, CommandType commandType = CommandType.Text)
        {
            ValidateQuery(query);
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = commandType;
                    AttachParameters(cmd, parameters);

                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Thực thi truy vấn trả về ô đầu tiên của dòng đầu tiên (hoặc COUNT/SCOPE_IDENTITY).
        /// </summary>
        public static object ExecuteScalar(string query, SqlParameter[] parameters = null, CommandType commandType = CommandType.Text)
        {
            ValidateQuery(query);
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = commandType;
                    AttachParameters(cmd, parameters);

                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        #endregion

        #region Truy vấn CÓ Transaction (Dùng chung Connection + Transaction được truyền từ bên ngoài)

        /// <summary>
        /// Thực thi truy vấn SELECT trong một SqlTransaction hiện có.
        /// Chú ý: Không đóng hoặc giải phóng conn và tran.
        /// </summary>
        public static DataTable ExecuteQuery(string query, SqlConnection conn, SqlTransaction tran,
            SqlParameter[] parameters = null, CommandType commandType = CommandType.Text)
        {
            ValidateQuery(query);
            ValidateTransaction(conn, tran);

            using (SqlCommand cmd = new SqlCommand(query, conn, tran))
            {
                cmd.CommandType = commandType;
                AttachParameters(cmd, parameters);

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        /// <summary>
        /// Thực thi lệnh INSERT, UPDATE, DELETE trong một SqlTransaction hiện có.
        /// Chú ý: Không đóng hoặc giải phóng conn và tran.
        /// </summary>
        public static int ExecuteNonQuery(string query, SqlConnection conn, SqlTransaction tran,
            SqlParameter[] parameters = null, CommandType commandType = CommandType.Text)
        {
            ValidateQuery(query);
            ValidateTransaction(conn, tran);

            using (SqlCommand cmd = new SqlCommand(query, conn, tran))
            {
                cmd.CommandType = commandType;
                AttachParameters(cmd, parameters);

                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Thực thi truy vấn trả về giá trị đơn lẻ trong một SqlTransaction hiện có.
        /// Chú ý: Không đóng hoặc giải phóng conn và tran.
        /// </summary>
        public static object ExecuteScalar(string query, SqlConnection conn, SqlTransaction tran,
            SqlParameter[] parameters = null, CommandType commandType = CommandType.Text)
        {
            ValidateQuery(query);
            ValidateTransaction(conn, tran);

            using (SqlCommand cmd = new SqlCommand(query, conn, tran))
            {
                cmd.CommandType = commandType;
                AttachParameters(cmd, parameters);

                return cmd.ExecuteScalar();
            }
        }

        private static void ValidateTransaction(SqlConnection conn, SqlTransaction tran)
        {
            if (conn == null)
            {
                throw new ArgumentNullException(nameof(conn), "SqlConnection không được null khi thực thi với Transaction.");
            }
            if (conn.State != ConnectionState.Open)
            {
                throw new InvalidOperationException("SqlConnection phải đang ở trạng thái Open khi thực thi với Transaction.");
            }
            if (tran == null)
            {
                throw new ArgumentNullException(nameof(tran), "SqlTransaction không được null khi gọi hàm hỗ trợ Transaction.");
            }
            if (tran.Connection != conn)
            {
                throw new InvalidOperationException("SqlTransaction không thuộc về SqlConnection được truyền vào.");
            }
        }

        #endregion
    }
}
