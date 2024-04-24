using Microsoft.Data.SqlClient;
using Music.MusicDomain;
using MusicCreator.Repository;
using MusicCreator.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCreator_Tests.Repositories
{
    [TestClass]
    public class CreationRepositoryTests
    {
        private SqlConnection _connection = null!;
        private ICreationRepository _creationRepository = null!;

        [TestInitialize]
        public void Initialize()
        {
            _connection = new SqlConnection("Data Source=localhost,2002;Initial Catalog=MusicDB;" +
               "User Id=user;Password=root;Encrypt=False;Integrated Security=false;TrustServerCertificate=true");
            _connection.Open();

            //asta nu stiu daca trebuie
            //var truncateCommand = new SqlCommand("TRUNCATE TABLE TRACK", _connection);
            //truncateCommand.ExecuteNonQuery();

            SqlConnectionFactory connectionFactory = new();
            _creationRepository = new CreationRepository(connectionFactory);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _connection.Close();
        }

        [TestMethod]
        public void TestAdd_SuccessfulAddition_ReturnsVoid()
        {
            var track = new Track(1, "Test Track", 1, new byte[3]);

            _creationRepository.AddTrack(track);

            Assert.AreEqual(1, _creationRepository.GetTracks().Count);  
        }

        [TestMethod]
        public void TestRemove_SuccessfulRemovalByID_ReturnsVoid()
        {
            var track = new Track(1, "Test Track", 1, new byte[3]);

            _creationRepository.AddTrack(track);
            _creationRepository.RemoveTrack(2);

            Assert.AreEqual(0, _creationRepository.GetTracks().Count());
        }

        [TestMethod]
        public void TestRemove_SuccesfulRemovalByTrack_ReturnsVoid()
        {
            var track = new Track(1, "Test Track", 1, new byte[3]);
            _creationRepository.AddTrack(track);

            _creationRepository.RemoveTrack(track);
            Assert.AreEqual(0, _creationRepository.GetTracks().Count());
        }

        [TestMethod]
        public void TestGetTracks()
        {
            var track1 = new Track(1, "Test Track", 1, new byte[3]);
            var track2 = new Track(2, "Test Track", 1, new byte[3]);

            _creationRepository.AddTrack(track1);
            _creationRepository.AddTrack(track2);

            List<Track> tracks = _creationRepository.GetTracks();
            Assert.AreEqual(2, tracks.Count);

            Assert.AreEqual(track1, tracks[0]);
            Assert.AreEqual(track2, tracks[1]);
        }

        [TestMethod]
        public void TestGenerateCreation()
        {
            var track1 = new Track(1, "Test Track", 1, new byte[3]);
            var track2 = new Track(2, "Test Track", 1, new byte[3]);

            _creationRepository.AddTrack(track1);
            _creationRepository.AddTrack(track2);

            Track creation = _creationRepository.GetCreation();
            Assert.IsNotNull(creation);
            Assert.IsNotNull(creation.SongData);
            Assert.AreNotEqual(creation.SongData.Length, 0);
        }

        [TestMethod]
        public void TestPlayCreation()
        {
            var track1 = new Track(1, "Test Track", 1, new byte[3]);
            var track2 = new Track(2, "Test Track", 1, new byte[3]);

            _creationRepository.AddTrack(track1);
            _creationRepository.AddTrack(track2);

            _creationRepository.PlayCreation();
        }

        [TestMethod]
        public void TestStopCreation()
        {
            var track1 = new Track(1, "Test Track", 1, new byte[3]);
            var track2 = new Track(2, "Test Track", 1, new byte[3]);

            _creationRepository.AddTrack(track1);
            _creationRepository.AddTrack(track2);

            _creationRepository.PlayCreation();
            _creationRepository.StopCreation();
        }

        [TestMethod]
        public void TestSaveCreation()
        {
            var track1 = new Track(1, "Test Track", 1, new byte[3]);
            var track2 = new Track(2, "Test Track", 1, new byte[3]);

            _creationRepository.AddTrack(track1);
            _creationRepository.AddTrack(track2);

            Song saveCreationOutput = new Song(123, "testTitle", 0, _creationRepository.GetCreation().SongData, "dummy");
            Assert.AreEqual(_creationRepository.SaveCreation("testTitle"), saveCreationOutput);
        }
    }
}
