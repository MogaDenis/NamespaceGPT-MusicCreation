using Microsoft.Data.SqlClient;
using MusicCreator.Repository.Interfaces;
using MusicCreator.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            SqlConnectionFactory connectionFactory = new();
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
        public void TestAdd_ValidTrack_Successfulladd()
        {
            var track = new Track(0,"Test Track", 1, new byte[] { 0x01, 0x02, 0x03 });
            int newTrackId = _trackrepository.Add(track);
            var addedTrack = _trackrepository.GetById(newTrackId);
            Assert.IsNotNull(addedTrack);
            Assert.AreEqual(newTrackId, addedTrack.Id);
        }

        [TestMethod]
        public void TestDelete_ValidId_Successfulldelete()
        {
            var track = new Track(0, "Test Track", 1, new byte[] { 0x01, 0x02, 0x03 });
            int trackId = _trackrepository.Add(track);
            _trackrepository.Delete(trackId);
            var deletedTrack = _trackrepository.GetById(trackId);
            Assert.IsNull(deletedTrack);
        }

        [TestMethod]
        public void TestGetById_ValidId_returnsTrack()
        {
            var track = new Track(0, "Test Track", 1, new byte[] { 0x01, 0x02, 0x03 });
            int trackId = _trackrepository.Add(track);
            var getByIdTrack = _trackrepository.GetById(trackId);
            Assert.AreEqual(getByIdTrack.Title, "Track 1");
        }

        [TestMethod]
        public void TestGetById_ValidId_returnsNull()
        {
            var track = new Track(0, "Test Track", 1, new byte[] { 0x01, 0x02, 0x03 });
            int trackId = _trackrepository.Add(track);
            var getByIdTrack = _trackrepository.GetById(25);
            Assert.IsNull(getByIdTrack);
        }

        [TestMethod]
        public void TestGetAll_RetrievesCorrect_ReturnsList()
        {
            var track1 = new Track(0, "Track 1", 1, new byte[] { 0x01, 0x02, 0x03 });
            var track2 = new Track(0, "Track 2", 2, new byte[] { 0x04, 0x05, 0x06 });
            _trackrepository.Add(track1);
            _trackrepository.Add(track2);
            var tracks = _trackrepository.GetAll();
            Assert.AreEqual(2, tracks.Count);
            Assert.AreEqual(tracks[0].Title, "Track 1");
        }
    }
}
