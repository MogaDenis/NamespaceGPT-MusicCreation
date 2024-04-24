using Microsoft.Data.SqlClient;
using MusicCreator.Repository.Interfaces;
using MusicCreator.Repository;
using Music.MusicDomain;

namespace MusicCreator_Tests.Repositories
{
    [TestClass]
    public class TrackRepositoryTests
    {
        private SqlConnection _connection = null!;
        private ITrackRepository _trackrepository = null!;

        [TestInitialize]
        public void Initialize()
        {
            _connection = new SqlConnection("Data Source=localhost,2002;Initial Catalog=MusicDB;" +
                "User Id=user;Password=root;Encrypt=False;Integrated Security=false;TrustServerCertificate=true");
            _connection.Open();

            var truncateCommand = new SqlCommand("TRUNCATE TABLE TRACK", _connection);
            truncateCommand.ExecuteNonQuery();

            SqlConnectionFactory connectionFactory = new ();
            _trackrepository = new TrackRepository(connectionFactory);
        }

        [TestCleanup]
        public void Cleanup()
        {
            var truncateCommand = new SqlCommand("TRUNCATE TABLE TRACK", _connection);
            truncateCommand.ExecuteNonQuery();

            _connection.Close();
        }

        [TestMethod]
        public void TestAdd_ValidTrack_SuccessfullAdd()
        {
            // Arrange
            var track = new Track(0,"Test Track", 1, [ 0x01, 0x02, 0x03 ]);

            // Act
            int newTrackId = _trackrepository.Add(track);

            // Assert
            var addedTrack = _trackrepository.GetById(newTrackId);
            Assert.IsNotNull(addedTrack);
            Assert.AreEqual(newTrackId, addedTrack.Id);
        }

        [TestMethod]
        public void TestDelete_ValidId_SuccessfullDelete()
        {
            // Arrange
            var track = new Track(0, "Test Track", 1, [0x01, 0x02, 0x03]);
            int trackId = _trackrepository.Add(track);

            // Act
            _trackrepository.Delete(trackId);

            // Assert
            var deletedTrack = _trackrepository.GetById(trackId);
            Assert.IsNull(deletedTrack);
        }

        [TestMethod]
        public void TestGetById_ValidId_returnsTrack()
        {
            // Arrange
            var track = new Track(0, "Test Track", 1, [0x01, 0x02, 0x03]);
            int trackId = _trackrepository.Add(track);

            // Act
            var getByIdTrack = _trackrepository.GetById(trackId);

            // Assert
            Assert.IsNotNull(getByIdTrack);
            Assert.AreEqual(track.Title, getByIdTrack.Title);
        }

        [TestMethod]
        public void TestGetById_InexistentId_returnsNull()
        {
            // Arrange
            var track = new Track(0, "Test Track", 1, [0x01, 0x02, 0x03]);
            _ = _trackrepository.Add(track);

            // Act
            var getByIdTrack = _trackrepository.GetById(-1);

            // Assert
            Assert.IsNull(getByIdTrack);
        }

        [TestMethod]
        public void TestGetAll_RetrievesCorrect_ReturnsList()
        {
            // Arrange
            var track1 = new Track(0, "Test Track1", 1, [0x01, 0x02, 0x03]);
            var track2 = new Track(0, "Test Track2", 2, [0x04, 0x05, 0x06]);
            _trackrepository.Add(track1);
            _trackrepository.Add(track2);

            // Act
            var tracks = _trackrepository.GetAll();

            // Assert
            Assert.AreEqual(2, tracks.Count);
            Assert.AreEqual(tracks[0].Title, "Test Track1");
            Assert.AreEqual(tracks[1].Title, "Test Track2");
        }
    }
}
