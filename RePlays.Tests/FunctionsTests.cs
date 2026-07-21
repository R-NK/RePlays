using RePlays.Utils;
using Xunit;

namespace RePlays.Tests {
    public class FunctionsTests {
        [Fact]
        public void GetReadableFileSize_ReturnsBytes_WhenLessThan1024() {
            Assert.Equal("500 B", Functions.GetReadableFileSize(500));
        }

        [Fact]
        public void GetReadableFileSize_ReturnsKilobytes_WhenExactly1024() {
            Assert.Equal("1 KB", Functions.GetReadableFileSize(1024));
        }

        [Fact]
        public void GetReadableFileSize_ReturnsFractionalKilobytes() {
            Assert.Equal("1.5 KB", Functions.GetReadableFileSize(1536));
        }

        [Fact]
        public void GetReadableFileSize_ReturnsMegabytes() {
            Assert.Equal("1 MB", Functions.GetReadableFileSize(1024 * 1024));
        }

        [Fact]
        public void GetReadableFileSize_CapsAtTerabytes() {
            double petabyte = 1024d * 1024 * 1024 * 1024 * 1024;
            Assert.Equal("1024 TB", Functions.GetReadableFileSize(petabyte));
        }

        [Theory]
        [InlineData(1920, 1080, "16:9")]
        [InlineData(1280, 800, "8:5")]
        [InlineData(640, 480, "4:3")]
        [InlineData(3840, 1080, "32:9")]
        public void GetAspectRatio_ReturnsReducedRatio(int width, int height, string expected) {
            Assert.Equal(expected, Functions.GetAspectRatio(width, height));
        }

        [Fact]
        public void GetAspectRatio_ReturnsZeroRatio_WhenBothDimensionsAreZero() {
            Assert.Equal("0:0", Functions.GetAspectRatio(0, 0));
        }

        [Theory]
        [InlineData(1920, 1080)]
        [InlineData(5120, 1440)]
        public void IsValidAspectRatio_ReturnsTrue_ForSupportedRatios(int width, int height) {
            Assert.True(Functions.IsValidAspectRatio(width, height));
        }

        [Theory]
        [InlineData(1000, 1000)]
        [InlineData(1080, 1920)]
        // 16:10 is reduced to "8:5", so it never matches "16:10" in the valid list
        [InlineData(1280, 800)]
        public void IsValidAspectRatio_ReturnsFalse_ForUnsupportedRatios(int width, int height) {
            Assert.False(Functions.IsValidAspectRatio(width, height));
        }

        [Fact]
        public void MakeValidFolderNameSimple_RemovesInvalidFileNameChars() {
            Assert.Equal("GameName", Functions.MakeValidFolderNameSimple("Game<>:\"/\\|?*Name"));
        }

        [Fact]
        public void MakeValidFolderNameSimple_ReturnsSameString_WhenAllCharsAreValid() {
            Assert.Equal("Valid Folder-Name_1", Functions.MakeValidFolderNameSimple("Valid Folder-Name_1"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void MakeValidFolderNameSimple_ReturnsInput_WhenNullOrEmpty(string input) {
            Assert.Equal(input, Functions.MakeValidFolderNameSimple(input));
        }

        [Fact]
        public void GetDamerauLevenshteinDistance_ReturnsZero_ForIdenticalStrings() {
            Assert.Equal(0, Functions.GetDamerauLevenshteinDistance("replay", "replay"));
        }

        [Fact]
        public void GetDamerauLevenshteinDistance_ReturnsOtherLength_WhenOneStringIsEmpty() {
            Assert.Equal(6, Functions.GetDamerauLevenshteinDistance("", "replay"));
            Assert.Equal(6, Functions.GetDamerauLevenshteinDistance("replay", ""));
        }

        [Fact]
        public void GetDamerauLevenshteinDistance_ReturnsKnownDistance() {
            Assert.Equal(3, Functions.GetDamerauLevenshteinDistance("kitten", "sitting"));
        }

        [Fact]
        public void CalculateStringSimilarity_ReturnsOne_ForIdenticalStrings() {
            Assert.Equal(1f, Functions.CalculateStringSimilarity("replays", "replays"));
        }

        [Fact]
        public void CalculateStringSimilarity_ReturnsZero_ForCompletelyDifferentStrings() {
            Assert.Equal(0f, Functions.CalculateStringSimilarity("abc", "xyz"));
        }
    }
}
