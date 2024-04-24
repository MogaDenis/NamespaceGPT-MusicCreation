using Microsoft.Data.SqlClient;
using Music.MusicDomain;
using MusicCreator.Repository;
using MusicCreator.Repository.Interfaces;

namespace MusicCreator.Tests.Repositories
{
    [TestClass]
    public class SongRepositoryTests
    {
        private SqlConnection _connection = null!;
        private ISongRepository _songRepository = null!;

        [TestInitialize]
        public void Initialize()
        {
            _connection = new SqlConnection("Data Source=localhost,2002;Initial Catalog=MusicDB;User Id=user;Password=root;Encrypt=False;Integrated Security=false;TrustServerCertificate=true");
            _connection.Open();

            var truncateCommand = new SqlCommand("TRUNCATE TABLE SONG", _connection);
            truncateCommand.ExecuteNonQuery();

            SqlConnectionFactory connectionFactory = new ();
            _songRepository = new SongRepository(connectionFactory);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _connection.Close();
        }

        [TestMethod]
        public void TestAdd_SuccessfulAddition_ReturnsNewId()
        {
            // Arrange
            var song = new Song(0, "Test Song", 0, [ 0x10, 0x20, 0x30 ], "Pop");

            // Act
            int newSongId = _songRepository.Add(song);

            // Assert
            Assert.IsTrue(newSongId > 0, "New song ID should be greater than 0.");
        }

        [TestMethod]
        public void TestGetAll_ReturnsAllSongs()
        {
            // Arrange
            var song1 = new Song(0, "Test Song 1", 0, [ 0x10, 0x20, 0x30 ], "Pop");
            var song2 = new Song(0, "Test Song 2", 0, [ 0x40, 0x50, 0x60 ], "Rock");
            _songRepository.Add(song1);
            _songRepository.Add(song2);

            // Act
            List<Song> songs = _songRepository.GetAll();

            // Assert
            Assert.AreEqual(2, songs.Count, "Expected two songs to be returned.");
        }

        [TestMethod]
        public void TestSearch_ExistentId_ReturnsCorrectSong()
        {
            // Arrange
            var song = new Song(0, "Test Song", 0, [ 0x10, 0x20, 0x30 ], "Pop");
            int newSongId = _songRepository.Add(song);

            // Act
            Song? foundSong = _songRepository.Search(newSongId);

            // Assert
            Assert.IsNotNull(foundSong);
            Assert.AreEqual("Test Song", foundSong.Title);
        }

        [TestMethod]
        public void TestSearch_InexistentId_ReturnsNull()
        {
            // Act
            Song? foundSong = _songRepository.Search(-1);

            // Assert
            Assert.IsNull(foundSong, "Should return null for a non-existent song ID.");
        }

        [TestMethod]
        public void TestDelete_RemovesSongCorrectly()
        {
            // Arrange
            var song = new Song(0, "Test Song", 0, [ 0x10, 0x20, 0x30 ], "Pop");
            int newSongId = _songRepository.Add(song);

            // Act
            _songRepository.Delete(newSongId);
            Song? foundSong = _songRepository.Search(newSongId);

            // Assert
            Assert.IsNull(foundSong, "Song should be null after deletion.");
        }
    }
}
