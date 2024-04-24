// <copyright file="SqlConnectionFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MusicCreator.Repository
{
    using Microsoft.Data.SqlClient;
    using MusicCreator.Repository.Interfaces;

    /// <summary>
    ///     Class which returns a new SqlConnection, uses the factory pattern.
    /// </summary>
    public class SqlConnectionFactory : IConnectionFactory
    {
        private readonly string connectionString = "Data Source=localhost,2002;Initial Catalog=MusicDB;" +
                "User Id=user;Password=root;Encrypt=False;Integrated Security=false;TrustServerCertificate=true";

        /// <summary>
        ///     Returns a new SqlConnection to the database.
        /// </summary>
        /// <returns>A new SqlConnection.</returns>
        public SqlConnection GetConnection()
        {
            return new SqlConnection(this.connectionString);
        }
    }
}
