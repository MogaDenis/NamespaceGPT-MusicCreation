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
            _connection = new SqlConnection("Data Source=localhost,2002;Initial Catalog=MusicDB;" +
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
            var truncateCommand = new SqlCommand("TRUNCATE TABLE MUSICTAG", _connection);
            truncateCommand.ExecuteNonQuery();

            _connection.Close();
        }

        [TestMethod]
        public void TestAdd_SuccessfulAddition_ReturnsVoid()
        {
            // Arrange
            var musicTag = new MusicTag(0, "TEST");

            // Act
            int id = _musicTagRepository.Add(musicTag);

            // Assert
            var retrievedTag = _musicTagRepository.GetAll().FirstOrDefault();
            Assert.IsNotNull(retrievedTag);
            Assert.AreEqual(id, retrievedTag.Id);
            Assert.AreEqual(musicTag.Title, retrievedTag.Title);
        }

        [TestMethod]
        public void TestSearch_ValidId_ReturnsMusicTag()
        {
            // Arrange
            var musicTag = new MusicTag(0, "TestTag");
            int id = _musicTagRepository.Add(musicTag);

            // Act
            var retrievedTag = _musicTagRepository.Search(id);

            // Assert
            Assert.IsNotNull(retrievedTag);
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
                new (0, "Tag1"),
                new (0, "Tag2"),
                new (0, "Tag3")
            };

            List<int> ids = [];

            foreach (var tag in musicTags)
            {
                ids.Add(_musicTagRepository.Add(tag));
            }

            // Act
            var retrievedTags = _musicTagRepository.GetAll();

            // Assert
            Assert.AreEqual(musicTags.Count, retrievedTags.Count);
            for (int i = 0; i < musicTags.Count; i++)
            {
                Assert.AreEqual(ids[i], retrievedTags[i].Id);
                Assert.AreEqual(musicTags[i].Title, retrievedTags[i].Title);
            }
        }
    }
}