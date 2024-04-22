using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Music.MusicDomain;
using MusicCreator.Repository;
using MusicCreator.Repository.Interfaces;

namespace MusicCreator_Tests.Repositories
{
    [TestClass]
    public class MusicTagRepositoryTests
    {
        private SqlConnection _connection = null!;
        private IMusicTagRepository _musicTagRepository = null!;

        [TestInitialize]
        public void Initialize()
        {
            _connection = new SqlConnection("Data Source=172.30.242.145,2002;Initial Catalog=MusicDB;" +
                "User Id=user;Password=root;Encrypt=False;Integrated Security=false;TrustServerCertificate=true");
            _connection.Open();

            var truncateCommand = new SqlCommand("TRUNCATE TABLE MUSICTAG", _connection);
            truncateCommand.ExecuteNonQuery();

            SqlConnectionFactory connectionFactory = new();
            _musicTagRepository = new MusigTagRepository(connectionFactory);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _connection.Close();
        }

        [TestMethod]
        public void TestAdd_SuccessfulAddition_ReturnsVoid()
        {
            // Arrange
            var musicTag = new MusicTag(0, "TestTag");

            // Act
            _musicTagRepository.Add(musicTag);

            // Assert
            var retrievedTag = _musicTagRepository.GetAll().FirstOrDefault();
            Assert.IsNotNull(retrievedTag);
            Assert.AreEqual(musicTag.Id, retrievedTag.Id);
            Assert.AreEqual(musicTag.Title, retrievedTag.Title);
        }

        [TestMethod]
        public void TestSearch_ValidId_ReturnsMusicTag()
        {
            // Arrange
            var musicTag = new MusicTag(0, "TestTag");
            _musicTagRepository.Add(musicTag);

            // Act
            var retrievedTag = _musicTagRepository.Search(musicTag.Id);

            // Assert
            Assert.IsNotNull(retrievedTag);
            Assert.AreEqual(musicTag.Id, retrievedTag.Id);
            Assert.AreEqual(musicTag.Title, retrievedTag.Title);
        }

        [TestMethod]
        public void TestSearch_InvalidId_ReturnsNull()
        {
            // Act
            var retrievedTag = _musicTagRepository.Search(-1);

            // Assert
            Assert.IsNull(retrievedTag);
        }

        [TestMethod]
        public void TestGetAll_ReturnsAllTagsInRepository()
        {
            // Arrange
            var musicTags = new List<MusicTag>
            {
                new MusicTag(0, "Tag1"),
                new MusicTag(0, "Tag2"),
                new MusicTag(0, "Tag3")
            };

            foreach (var tag in musicTags)
            {
                _musicTagRepository.Add(tag);
            }

            // Act
            var retrievedTags = _musicTagRepository.GetAll();

            // Assert
            Assert.AreEqual(musicTags.Count, retrievedTags.Count);
            for (int i = 0; i < musicTags.Count; i++)
            {
                Assert.AreEqual(musicTags[i].Id, retrievedTags[i].Id);
                Assert.AreEqual(musicTags[i].Title, retrievedTags[i].Title);
            }
        }
    }
}