// <copyright file="MusigTagRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MusicCreator.Repository
{
    using System.Data;
    using Microsoft.Data.SqlClient;
    using Music.MusicDomain;
    using MusicCreator.Repository.Interfaces;

    /// <summary>
    /// .
    /// </summary>
    public class MusigTagRepository : IMusicTagRepository
    {
        private readonly SqlConnection connection;
        private readonly SqlDataAdapter adapter;
        private readonly DataSet dataset;
        private readonly DataTable? table;

        private readonly IConnectionFactory connectionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MusigTagRepository"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory used to create database connections.</param>
        public MusigTagRepository(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;

            string query = "select * from MUSICTAG";
            this.connection = this.connectionFactory.GetConnection();

            this.adapter = new SqlDataAdapter(query, this.connection);
            this.dataset = new DataSet();

            this.adapter.Fill(this.dataset, "MusicTag");
            this.table = this.dataset.Tables["MusicTag"];

            var commandBuilder = new SqlCommandBuilder(this.adapter);
            this.adapter.InsertCommand = commandBuilder.GetInsertCommand();
        }

        /// <summary>
        /// Adds a new music tag to the repository.
        /// </summary>
        /// <param name="elem">The music tag to add.</param>
        public void Add(MusicTag elem)
        {
            if (this.table == null)
            {
                return;
            }

            DataRow row = this.table.NewRow();

            // row["musictag_id"] = elem.getId();
            row["tag"] = elem.Title;
            this.table.Rows.Add(row);
            this.adapter.Update(this.dataset, "MusicTag");
        }

        /// <summary>
        /// Searches for a music tag with the specified ID in the repository.
        /// </summary>
        /// <param name="id">The ID of the music tag to search for.</param>
        /// <returns>The music tag with the specified ID, or null if not found.</returns>
        public MusicTag? Search(int id)
        {
            if (this.table == null)
            {
                return null;
            }

            var elems = from DataRow row in this.table.Rows
                        where (int)row["musictag_id"] == id
                        select row;

            if (elems == null)
            {
                return null;
            }

            DataRow? elem = elems.FirstOrDefault();
            if (elem == null)
            {
                return null;
            }

            return GenerateMusicTagFromRowObject(elem);
        }

        /// <summary>
        /// Retrieves all music tags from the repository.
        /// </summary>
        /// <returns>A list of all music tags in the repository.</returns>
        public List<MusicTag> GetAll()
        {
            if (this.table == null)
            {
                return new List<MusicTag>();
            }

            var elems = from DataRow row in this.table.Rows
                        select GenerateMusicTagFromRowObject(row);

            return elems.ToList();
        }

        private static MusicTag GenerateMusicTagFromRowObject(DataRow row)
        {
            int id = (int)row["musictag_id"]; // I hope you will do a better job
            string title = (string)row["tag"];
            return new MusicTag(id, title);
        }
    }
}
