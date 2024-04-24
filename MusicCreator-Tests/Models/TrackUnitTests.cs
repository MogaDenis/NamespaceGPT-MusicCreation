using Moq;
using Music.MusicDomain;
using Plugin.Maui.Audio;

namespace MusicCreator_Tests.Models
{
    [TestClass]
    public class TrackUnitTests
    {
        [TestMethod]
        public void TestPlay_EmptySongData_ReturnsEarly()
        {
            // Arrange
            byte[] songData = [];
            Track track = new(0, "test", 0, songData);

            var mockAudioPlayer = new Mock<IAudioPlayer>();
            track.AudioPlayer = mockAudioPlayer.Object;

            // Act
            track.Play();

            // Assert
            mockAudioPlayer.Verify(audioPlayer => audioPlayer.Stop(), Times.Never);
            mockAudioPlayer.Verify(audioPlayer => audioPlayer.Play(), Times.Never);
        }

        [TestMethod]
        public void TestPlay_CorrectSongData_TriggersStopAndPlayOnAudioPlayer()
        {
            // Arrange
            byte[] songData = [0x10, 0x20, 0x30];
            Track track = new(0, "test", 0, songData);

            var mockAudioPlayer = new Mock<IAudioPlayer>();
            track.AudioPlayer = mockAudioPlayer.Object;

            // Act
            track.Play();

            // Assert
            mockAudioPlayer.Verify(audioPlayer => audioPlayer.Stop(), Times.Once);
            mockAudioPlayer.Verify(audioPlayer => audioPlayer.Play(), Times.Once);
        }

        [TestMethod]
        public void TestStop_TriggersStopOnAudioPlayer()
        {
            // Arrange
            Track track = new(0, "test", 0, []);

            var mockAudioPlayer = new Mock<IAudioPlayer>();
            track.AudioPlayer = mockAudioPlayer.Object;

            // Act
            track.Stop();

            // Assert
            mockAudioPlayer.Verify(audioPlayer => audioPlayer.Stop(), Times.Once);
        }
    }
}
