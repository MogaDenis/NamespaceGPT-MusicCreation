using Microsoft.Data.SqlClient;
using MusicCreator.Repository.Interfaces;
using System.Data;

namespace MusicCreator.Repository
{
    public class SqlConnectionFactory : IConnectionFactory
    {
        private readonly string _connectionString = "Data Source=192.168.100.54,2002;Initial Catalog=MusicDB;" +
                "User Id=user;Password=root;Encrypt=False;Integrated Security=false;TrustServerCertificate=true";

        public SqlConnectionFactory() { }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
