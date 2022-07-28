using System.Data;
using System.Data.SqlClient;

namespace WebApiPenu2.Utiles
{
    public class DataBaseOperations
    {
        private string _connectionString = String.Empty;
        public static DataBaseOperations Methods => new DataBaseOperations();

        public static bool  IsServerOnline { get; set; }
        
        public DataBaseOperations()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path);
            var root = configurationBuilder.Build();
            _connectionString = root.GetSection("ConnectionStrings").GetSection("DevConnections").Value;
        }
        private bool ValidateConnection()
        {
            try
            {
                using(SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    conn.Close();
                }
                IsServerOnline = true;
            }catch(Exception ex)
            {
                IsServerOnline = false;
            }
            return IsServerOnline;
        }
        public DataSet ExecuteCommand(SqlCommand command)
        {
            if (!IsServerOnline)
            {
                throw new Exception("Se perdio la conexion al servidor");
            }
            DataSet dataset = new DataSet("Resultado");
            using(SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlComm = command;
                sqlComm.Connection = conn;
                sqlComm.CommandTimeout = 15000;
                SqlDataAdapter da = new SqlDataAdapter { SelectCommand = sqlComm };
                da.Fill(dataset);
            }
            return dataset;
        }

        public string ConnectionString
        {
            get => _connectionString;
        }
    }
}
