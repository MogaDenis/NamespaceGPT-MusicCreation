// <copyright file="IConnectionFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MusicCreator.Repository.Interfaces
{
    using Microsoft.Data.SqlClient;

    /// <summary>
    ///     Functional interface for getting a SqlConnection.
    /// </summary>
    public interface IConnectionFactory
    {
        /// <summary>
        ///     Returns a new SqlConnection to the database.
        /// </summary>
        /// <returns>A new SqlConnection.</returns>
        SqlConnection GetConnection();
    }
}
