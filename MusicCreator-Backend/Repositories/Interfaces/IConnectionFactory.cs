using Microsoft.Data.SqlClient;
using System.Data;

namespace MusicCreator.Repository.Interfaces
{
    public interface IConnectionFactory
    {
        SqlConnection GetConnection();
    }
}
