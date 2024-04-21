using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Music.MusicDomain;
using MusicCreator.Repository;
using MusicCreator.Repository.Interfaces;
using System.Data;
using System.Data.Common;

namespace MusicCreator.Tests.Repositories
{
    [TestClass]
    public class SongRepositoryTests
    {
        private SqlConnection _connection = null!;

        [TestInitialize]
        public void Initialize()
        {
            _connection = new SqlConnection("Data Source=192.168.100.54,2002;Initial Catalog=MusicDB;" +
                "User Id=user;Password=root;Encrypt=False;Integrated Security=false;TrustServerCertificate=true");
            _connection.Open();

            var truncateCommand = new SqlCommand("TRUNCATE TABLE SONG", _connection);
            truncateCommand.ExecuteNonQuery();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _connection.Close();
        }

        [TestMethod]
        public void TestAdd_SuccessfulAddition()
        {
            // Arrange
            SqlConnectionFactory connectionFactory = new();
            var repository = new SongRepository(connectionFactory);

            var song = new Song(0, "test", 0, [0x10, 0x20, 0x30], "test");

            // Act
            repository.Add(song);

            // Assert
            Assert.AreEqual(1, repository.GetAll().Count);
        }
    }
}
