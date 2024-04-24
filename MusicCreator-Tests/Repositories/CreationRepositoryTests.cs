using Microsoft.Data.SqlClient;
using Music.MusicDomain;
using MusicCreator.Repository;
using MusicCreator.Repository.Interfaces;

namespace MusicCreator_Tests.Repositories
{
    [TestClass]
    public class CreationRepositoryTests
    {
        private ICreationRepository _creationRepository = null!;

        [TestInitialize]
        public void Initialize()
        {
            _creationRepository = new CreationRepository();
        }

        [TestMethod]
        public void TestAdd_SuccessfulAddition_ReturnsVoid()
        {
            // Arrange
            var track = new Track(1, "Test Track", 1, [0x10, 0x20, 0x30]);

            // Act
            _creationRepository.AddTrack(track);

            // Assert
            Assert.AreEqual(1, _creationRepository.GetTracks().Count);  
        }

        [TestMethod]
        public void TestAdd_AddTwoEntities_SuccessfulAddition_ReturnsVoid()
        {
            // Arrange
            var track1 = new Track(1, "Test Track1", 1, [0x10, 0x20, 0x30]);
            var track2 = new Track(2, "Test Track2", 1, [0x10, 0x20, 0x30]);

            // Act
            _creationRepository.AddTrack(track1);
            _creationRepository.AddTrack(track2);

            // Assert
            Assert.AreEqual(2, _creationRepository.GetTracks().Count);
        }

        [TestMethod]
        public void TestRemove_SuccessfulRemovalByID_ReturnsVoid()
        {
            // Arrange
            var track = new Track(1, "Test Track", 1, [0x10, 0x20, 0x30]);
            var track2 = new Track(2, "Test Track", 1, [0x10, 0x20, 0x30]);

            _creationRepository.AddTrack(track);
            _creationRepository.AddTrack(track2);

            // Act
            _creationRepository.RemoveTrack(1);

            // Assert
            Assert.AreEqual(1, _creationRepository.GetTracks().Count);
        }

        [TestMethod]
        public void TestRemove_OneTrackLeft_SuccesfulRemovalByTrack_ReturnsVoid_()
        {
            // Arrange
            var track = new Track(1, "Test Track", 1, [0x10, 0x20, 0x30]);
            var track2 = new Track(2, "Test Track", 1, [0x10, 0x20, 0x30]);

            _creationRepository.AddTrack(track);
            _creationRepository.AddTrack(track2);

            // Act
            _creationRepository.RemoveTrack(1);

            // Assert
            Assert.AreEqual(1, _creationRepository.GetTracks().Count);
        }

        [TestMethod]
        public void TestRemove_SuccesfulRemovalByTrack_ReturnsVoid()
        {
            // Arrange
            var track = new Track(1, "Test Track", 1, [0x10, 0x20, 0x30]);

            _creationRepository.AddTrack(track);

            // Act
            _creationRepository.RemoveTrack(track);
            
            // Assert
            Assert.AreEqual(0, _creationRepository.GetTracks().Count);
        }

        [TestMethod]
        public void TestRemove_OneTrackLeft_SuccesfulRemovalById_ReturnsVoid_()
        {
            // Arrange
            var track = new Track(1, "Test Track", 1, [0x10, 0x20, 0x30]);
            var track2 = new Track(2, "Test Track", 1, [0x10, 0x20, 0x30]);

            _creationRepository.AddTrack(track);
            _creationRepository.AddTrack(track2);

            // Act
            _creationRepository.RemoveTrack(track);

            // Assert
            Assert.AreEqual(1, _creationRepository.GetTracks().Count);
        }

        [TestMethod]
        public void TestRemove_InexistentId_DoesNotRemove_ReturnsVoid()
        {
            // Arrange
            var track = new Track(1, "Test Track", 1, [0x10, 0x20, 0x30]);

            _creationRepository.AddTrack(track);

            // Act
            _creationRepository.RemoveTrack(-1);

            // Assert
            Assert.AreEqual(1, _creationRepository.GetTracks().Count);
        }

        [TestMethod]
        public void TestGetTracks()
        {
            // Arrange
            var track1 = new Track(1, "Test Track", 1, [0x10, 0x20, 0x30]);
            var track2 = new Track(2, "Test Track", 1, [0x10, 0x20, 0x30]);

            _creationRepository.AddTrack(track1);
            _creationRepository.AddTrack(track2);

            // Act
            List<Track> tracks = _creationRepository.GetTracks();

            // Assert
            Assert.AreEqual(2, tracks.Count);
            Assert.AreEqual(track1, tracks[0]);
            Assert.AreEqual(track2, tracks[1]);
        }

        [TestMethod]
        public void TestGetCreation()
        {
            // Arrange
            var track1 = new Track(1, "Test Track", 1, [0x10, 0x20, 0x30]);
            var track2 = new Track(2, "Test Track", 1, [0x10, 0x20, 0x30]);

            _creationRepository.AddTrack(track1);
            _creationRepository.AddTrack(track2);

            // Act
            Track creation = _creationRepository.GetCreation();

            // Assert
            Assert.IsNotNull(creation);
            Assert.IsNotNull(creation.SongData);
        }

        [TestMethod]
        public void TestPlayCreation()
        {
            // Arrange
            var track1 = new Track(1, "Test Track", 1, [0x10, 0x20, 0x30]);
            var track2 = new Track(2, "Test Track", 1, [0x10, 0x20, 0x30]);

            _creationRepository.AddTrack(track1);
            _creationRepository.AddTrack(track2);

            // Act
            _creationRepository.PlayCreation();
        }

        [TestMethod]
        public void TestStopCreation()
        {
            // Arrange
            var track1 = new Track(1, "Test Track", 1, [0x10, 0x20, 0x30]);
            var track2 = new Track(2, "Test Track", 1, [0x10, 0x20, 0x30]);

            _creationRepository.AddTrack(track1);
            _creationRepository.AddTrack(track2);

            _creationRepository.PlayCreation();

            // Act
            _creationRepository.StopCreation();
        }

        [TestMethod]
        public void TestSaveCreation()
        {
            // Arrange
            var track1 = new Track(1, "Test Track", 1, [0x10, 0x20, 0x30]);
            var track2 = new Track(2, "Test Track", 1, [0x10, 0x20, 0x30]);

            _creationRepository.AddTrack(track1);
            _creationRepository.AddTrack(track2);

            Song saveCreationOutput = new (123, "testTitle", 0, _creationRepository.GetCreation().SongData, "dummy");

            // Act
            _creationRepository.SaveCreation("testTitle");

            // Assert
            Assert.AreEqual("testTitle", saveCreationOutput.Title);
            Assert.AreEqual(0, saveCreationOutput.Type);
        }
    }
}
