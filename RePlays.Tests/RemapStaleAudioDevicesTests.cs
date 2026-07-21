using RePlays.Utils;
using System.Collections.Generic;
using Xunit;

namespace RePlays.Tests {
    public class RemapStaleAudioDevicesTests {
        [Fact]
        public void KeepsDeviceId_WhenSavedIdStillExists() {
            var devices = new List<AudioDevice> {
                new("{0.0.1.00000000}.{aaaa}", "Microphone (Shure MV7)", true),
            };
            var cache = new List<AudioDevice> {
                new("default", "Default Device", true),
                new("{0.0.1.00000000}.{aaaa}", "Microphone (Shure MV7)", true),
            };

            bool changed = Functions.RemapStaleAudioDevices(devices, cache);

            Assert.False(changed);
            Assert.Equal("{0.0.1.00000000}.{aaaa}", devices[0].deviceId);
        }

        [Fact]
        public void RemapsToNewId_WhenSavedIdIsStaleAndLabelMatches() {
            var devices = new List<AudioDevice> {
                new("{0.0.1.00000000}.{old0}", "Microphone (Shure MV7)", true),
            };
            var cache = new List<AudioDevice> {
                new("default", "Default Device", true),
                new("{0.0.1.00000000}.{new0}", "Microphone (Shure MV7)", true),
            };

            bool changed = Functions.RemapStaleAudioDevices(devices, cache);

            Assert.True(changed);
            Assert.Equal("{0.0.1.00000000}.{new0}", devices[0].deviceId);
        }

        [Fact]
        public void KeepsDeviceId_WhenDeviceIsNotPresent() {
            var devices = new List<AudioDevice> {
                new("{0.0.1.00000000}.{old0}", "Microphone (Shure MV7)", true),
            };
            var cache = new List<AudioDevice> {
                new("default", "Default Device", true),
            };

            bool changed = Functions.RemapStaleAudioDevices(devices, cache);

            Assert.False(changed);
            Assert.Equal("{0.0.1.00000000}.{old0}", devices[0].deviceId);
        }

        [Fact]
        public void RemapsOnlyStaleDevice_AndPreservesOtherSettings() {
            var devices = new List<AudioDevice> {
                new("default", "Default Device", true) { deviceVolume = 80 },
                new("{0.0.1.00000000}.{old0}", "Microphone (Shure MV7)", true) { deviceVolume = 150, denoiser = true },
            };
            var cache = new List<AudioDevice> {
                new("default", "Default Device", true),
                new("{0.0.1.00000000}.{new0}", "Microphone (Shure MV7)", true),
            };

            bool changed = Functions.RemapStaleAudioDevices(devices, cache);

            Assert.True(changed);
            Assert.Equal("default", devices[0].deviceId);
            Assert.Equal(80, devices[0].deviceVolume);
            Assert.Equal("{0.0.1.00000000}.{new0}", devices[1].deviceId);
            Assert.Equal(150, devices[1].deviceVolume);
            Assert.True(devices[1].denoiser);
        }
    }
}
