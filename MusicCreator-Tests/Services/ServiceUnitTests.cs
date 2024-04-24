using Moq;
using Music.MusicDomain;
using MusicCreator.Repository.Interfaces;
using MusicCreator.Services;

namespace MusicCreator_Tests.Services
{
    [TestClass]
    public class ServiceUnitTestsWithInit
    {
        private Mock<ITrackRepository> _mockTrackRepository = null!;
        private Mock<ICreationRepository> _mockCreationRepository = null!;
        private Mock<ISongRepository> _mockSongRepository = null!;

        [TestInitialize]
        public void Initialize()
        {
            _mockTrackRepository = new Mock<ITrackRepository>();
            _mockCreationRepository = new Mock<ICreationRepository>();
            _mockSongRepository = new Mock<ISongRepository>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Service.ResetInstance();
        }

        [TestMethod]
        public void TestGetService_CorrectlyInstantiated_ReturnsInstance()
        {
            // Arrange
            _ = new Service(_mockTrackRepository.Object, _mockCreationRepository.Object, _mockSongRepository.Object);

            // Act
            var instance = Service.GetService();

            // Assert
            Assert.IsNotNull(instance);
        }

        [TestMethod]
        public void TestGetTracks_ReturnsListOfTracks()
        {
            // Arrange
            List<Track> tracks = [new(1, "test1", 0, [0x10, 0x20]), new(2, "test2", 0, [0x20, 0x30])];

            _mockTrackRepository.Setup(trackRepository => trackRepository.GetAll())
                .Returns(tracks);

            _ = new Service(_mockTrackRepository.Object, _mockCreationRepository.Object, _mockSongRepository.Object);

            // Act
            var resultTracks = Service.GetService().GetTracks();

            // Assert
            Assert.IsNotNull(resultTracks);
            Assert.AreEqual(tracks.Count, resultTracks.Count);

            int index = 0;
            foreach (Track track in tracks)
            {
                Assert.AreEqual(track.Title, resultTracks[index].Title);
                index++;
            }
        }

        [TestMethod]
        public void TestGetTracksByType_ExistentType_ReturnsListOfTracks()
        {
            // Arrange
            List<Track> tracks = [
                new(1, "test1", 0, [0x10, 0x20]),
                new(2, "test2", 0, [0x20, 0x30]),
                new(3, "test3", 1, [0x30, 0x40])
            ];

            _mockTrackRepository.Setup(trackRepository => trackRepository.GetAll())
                .Returns(tracks);

            _ = new Service(_mockTrackRepository.Object, _mockCreationRepository.Object, _mockSongRepository.Object);

            int chosenType = 0;

            // Act
            var resultTracks = Service.GetService().GetTracksByType(chosenType);

            // Assert
            Assert.IsNotNull(resultTracks);
            Assert.AreEqual(2, resultTracks.Count);

            foreach (Track track in resultTracks)
            {
                Assert.AreEqual(chosenType, track.Type);
            }
        }

        [TestMethod]
        public void TestGetTrackByID_ExistentId_ReturnsTrack()
        {
            // Arrange
            List<Track> tracks = [new(1, "test1", 0, [0x10, 0x20]), new(2, "test2", 0, [0x20, 0x30])];

            _mockTrackRepository.Setup(trackRepository => trackRepository.GetAll())
                .Returns(tracks);

            _ = new Service(_mockTrackRepository.Object, _mockCreationRepository.Object, _mockSongRepository.Object);

            int chosenId = 1;

            // Act
            var resultTrack = Service.GetService().GetTrackById(chosenId);

            // Assert
            Assert.IsNotNull(resultTrack);
            Assert.AreEqual(chosenId, resultTrack.Id);
            Assert.AreEqual(tracks[0].Title, resultTrack.Title);
        }

        [TestMethod]
        public void TestGetTrackByID_InexistentId_ReturnsNull()
        {
            // Arrange
            List<Track> tracks = [new(1, "test1", 0, [0x10, 0x20]), new(2, "test2", 0, [0x20, 0x30])];

            _mockTrackRepository.Setup(trackRepository => trackRepository.GetAll())
                .Returns(tracks);

            _ = new Service(_mockTrackRepository.Object, _mockCreationRepository.Object, _mockSongRepository.Object);

            int chosenId = 3;

            // Act
            var resultTrack = Service.GetService().GetTrackById(chosenId);

            // Assert
            Assert.IsNull(resultTrack);
        }

        [TestMethod]
        public void TestGetTrackByTitle_ExistentTitle_ReturnsTrack()
        {
            // Arrange
            List<Track> tracks = [new(1, "test1", 0, [0x10, 0x20]), new(2, "test2", 0, [0x20, 0x30])];

            _mockTrackRepository.Setup(trackRepository => trackRepository.GetAll())
                .Returns(tracks);

            _ = new Service(_mockTrackRepository.Object, _mockCreationRepository.Object, _mockSongRepository.Object);

            string chosenTitle = "test2";

            // Act
            var resultTrack = Service.GetService().GetTrackByTitle(chosenTitle);

            // Assert
            Assert.IsNotNull(resultTrack);
            Assert.AreEqual(chosenTitle, resultTrack.Title);
            Assert.AreEqual(tracks[1].Id, resultTrack.Id);
        }

        [TestMethod]
        public void TestGetTrackByID_InexistentTitle_ReturnsNull()
        {
            // Arrange
            List<Track> tracks = [new(1, "test1", 0, [0x10, 0x20]), new(2, "test2", 0, [0x20, 0x30])];

            _mockTrackRepository.Setup(trackRepository => trackRepository.GetAll())
                .Returns(tracks);

            _ = new Service(_mockTrackRepository.Object, _mockCreationRepository.Object, _mockSongRepository.Object);

            string chosenTitle = "wrongTitle";

            // Act
            var resultTrack = Service.GetService().GetTrackByTitle(chosenTitle);

            // Assert
            Assert.IsNull(resultTrack);
        }

        [TestMethod]
        public void TestGetTracksByTypeAndFilterByTitle_ExistentMatchingEntities_ReturnsListOfTracks()
        {
            // Arrange
            List<Track> tracks = [
                new(1, "test1", 0, [0x10, 0x20]),
                new(2, "test2", 0, [0x20, 0x30]),
                new(3, "test3", 1, [0x30, 0x40])
            ];

            _mockTrackRepository.Setup(trackRepository => trackRepository.GetAll())
                .Returns(tracks);

            _ = new Service(_mockTrackRepository.Object, _mockCreationRepository.Object, _mockSongRepository.Object);

            int chosenType = 0;
            string chosenTitle = "test1";

            // Act
            var resultTracks = Service.GetService().GetTracksByTypeAndFilterByTitle(chosenType, chosenTitle);

            // Assert
            Assert.IsNotNull(resultTracks);
            Assert.AreEqual(1, resultTracks.Count);
            Assert.AreEqual(chosenType, resultTracks[0].Type);
            Assert.AreEqual(chosenTitle, resultTracks[0].Title);
        }

        [TestMethod]
        public void TestGetTracksByTypeAndFilterByTitle_NoMatchingEntities_ReturnsEmptyList()
        {
            // Arrange
            List<Track> tracks = [
                new(1, "test1", 0, [0x10, 0x20]),
                new(2, "test2", 0, [0x20, 0x30]),
                new(3, "test3", 1, [0x30, 0x40])
            ];

            _mockTrackRepository.Setup(trackRepository => trackRepository.GetAll())
                .Returns(tracks);

            _ = new Service(_mockTrackRepository.Object, _mockCreationRepository.Object, _mockSongRepository.Object);

            int chosenType = 0;
            string chosenTitle = "test3";

            // Act
            var resultTracks = Service.GetService().GetTracksByTypeAndFilterByTitle(chosenType, chosenTitle);

            // Assert
            Assert.IsNotNull(resultTracks);
            Assert.AreEqual(0, resultTracks.Count);
        }

        [TestMethod]
        public void TestGetCreationTracks_ReturnsListOfTracks()
        {
            // Arrange
            List<Track> tracks = [new(1, "test1", 0, [0x10, 0x20]), new(2, "test2", 0, [0x20, 0x30])];

            _mockCreationRepository.Setup(creationRepository => creationRepository.GetTracks())
                .Returns(tracks);

            _ = new Service(_mockTrackRepository.Object, _mockCreationRepository.Object, _mockSongRepository.Object);

            // Act
            var resultTracks = Service.GetService().GetCreationTracks();

            // Assert
            Assert.IsNotNull(resultTracks);
            Assert.AreEqual(tracks.Count, resultTracks.Count);

            int index = 0;
            foreach (Track track in tracks)
            {
                Assert.AreEqual(track.Title, resultTracks[index].Title);
                index++;
            }
        }

        [TestMethod]
        public void TestAddTrack_TrackParameter_CallsAddOnCreationRepository_Void()
        {
            // Arrange
            Track track = new(1, "test1", 0, [0x10, 0x20]);

            _ = new Service(_mockTrackRepository.Object, _mockCreationRepository.Object, _mockSongRepository.Object);

            // Act
            Service.GetService().AddTrack(track);

            // Assert
            _mockCreationRepository.Verify(creationRepository => creationRepository.AddTrack(track), Times.Once);
        }

        [TestMethod]
        public void TestAddTrack_ExistentIdParameter_CallsAddOnCreationRepository_Void()
        {
            // Arrange
            Track track = new(1, "test1", 0, [0x10, 0x20]);
            int trackId = track.Id;

            _mockTrackRepository.Setup(trackRepository => trackRepository.GetAll()).Returns([track]);

            _ = new Service(_mockTrackRepository.Object, _mockCreationRepository.Object, _mockSongRepository.Object);

            // Act
            Service.GetService().AddTrack(trackId);

            // Assert
            _mockCreationRepository.Verify(creationRepository => creationRepository.AddTrack(track), Times.Once);
        }

        [TestMethod]
        public void TestAddTrack_InexistentIdParameter_DoesNotCallAddOnCreationRepository_Void()
        {
            // Arrange
            Track track = new(1, "test1", 0, [0x10, 0x20]);
            int trackId = track.Id;
            int inexistentId = 2;

            _mockTrackRepository.Setup(trackRepository => trackRepository.GetAll()).Returns([track]);

            _ = new Service(_mockTrackRepository.Object, _mockCreationRepository.Object, _mockSongRepository.Object);

            // Act
            Service.GetService().AddTrack(inexistentId);

            // Assert
            _mockCreationRepository.Verify(creationRepository => creationRepository.AddTrack(track), Times.Never);
        }

        [TestMethod]
        public void TestRemoveTrack_TrackParameter_CallsRemoveOnCreationRepository_Void()
        {
            // Arrange
            Track track = new(1, "test1", 0, [0x10, 0x20]);

            _ = new Service(_mockTrackRepository.Object, _mockCreationRepository.Object, _mockSongRepository.Object);

            // Act
            Service.GetService().RemoveTrack(track);

            // Assert
            _mockCreationRepository.Verify(creationRepository => creationRepository.RemoveTrack(track), Times.Once);
        }

        [TestMethod]
        public void TestRemoveTrack_ExistentIdParameter_CallsRemoveOnCreationRepository_Void()
        {
            // Arrange
            Track track = new(1, "test1", 0, [0x10, 0x20]);
            int trackId = track.Id;

            _mockTrackRepository.Setup(trackRepository => trackRepository.GetAll()).Returns([track]);

            _ = new Service(_mockTrackRepository.Object, _mockCreationRepository.Object, _mockSongRepository.Object);

            // Act
            Service.GetService().RemoveTrack(trackId);

            // Assert
            _mockCreationRepository.Verify(creationRepository => creationRepository.RemoveTrack(trackId), Times.Once);
        }

        [TestMethod]
        public void TestPlayCreation_CallsPlayCreationOnCreationRepository_Void()
        {
            // Arrange
            _ = new Service(_mockTrackRepository.Object, _mockCreationRepository.Object, _mockSongRepository.Object);

            // Act
            Service.GetService().PlayCreation();

            // Assert
            _mockCreationRepository.Verify(creationRepository => creationRepository.PlayCreation(), Times.Once);
        }

        [TestMethod]
        public void TestStopCreation_CallsStopCreationOnCreationRepository_Void()
        {
            // Arrange
            _ = new Service(_mockTrackRepository.Object, _mockCreationRepository.Object, _mockSongRepository.Object);

            // Act
            Service.GetService().StopCreation();

            // Assert
            _mockCreationRepository.Verify(creationRepository => creationRepository.StopCreation(), Times.Once);
        }

        [TestMethod]
        public void TestSaveCreation_CallsAddOnSongRepository_Void()
        {
            // Arrange
            string title = "test";
            Song song = new(1, title, 0, [0x10, 0x20], "artist");

            _mockCreationRepository.Setup(creationRepository => creationRepository.SaveCreation(title))
                .Returns(song);

            _ = new Service(_mockTrackRepository.Object, _mockCreationRepository.Object, _mockSongRepository.Object);

            // Act
            Service.GetService().SaveCreation(title);

            // Assert
            _mockSongRepository.Verify(songRepository => songRepository.Add(song), Times.Once);
        }

        [TestMethod]
        public void TestStopAll_CallsGetAllOnTrackRepository_Void()
        {
            // Arrange
            List<Track> tracks = [
                new(1, "test1", 0, [0x10, 0x20]),
                new(2, "test2", 0, [0x20, 0x30]),
                new(3, "test3", 1, [0x30, 0x40])
            ];

            _mockTrackRepository.Setup(trackRepository => trackRepository.GetAll()).Returns(tracks);

            _ = new Service(_mockTrackRepository.Object, _mockCreationRepository.Object, _mockSongRepository.Object);

            // Act
            Service.GetService().StopAll();

            // Assert
            _mockTrackRepository.Verify(trackRepository => trackRepository.GetAll(), Times.Once);
        }
    }

    [TestClass]
    public class ServiceUnitTestsWithoutInit
    {
        [TestMethod]
        public void TestGetService_NotInstantiated_ThrowsNullReferenceException()
        {
            // Assert
            Assert.ThrowsException<NullReferenceException>(() => { Service.GetService(); });
        }
    }
}
