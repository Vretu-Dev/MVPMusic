using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;

namespace MVPMusic
{
    public class Main : Plugin<Config>
    {
        public override string Name => "MVPMusic";
        public override string Author => "Vretu";
        public override string Prefix { get; } = "MVPMusic";
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(9, 4, 0);
        public override PluginPriority Priority { get; } = PluginPriority.Low;
        public static Main Instance { get; private set; }
        public static string AudioDirectoryPath { get; } = Path.Combine(Paths.Configs, "audio");
        public HeaderSetting SettingsHeader { get; set; } = new HeaderSetting("MVP MUSIC");
        public override void OnEnabled()
        {
            Instance = this;

            if (!Directory.Exists(AudioDirectoryPath))
            {
                Log.Warn("Audio directory does not exist. Creating...");
                Directory.CreateDirectory(AudioDirectoryPath);
            }

            string MVPMusicDirectory = Path.Combine(AudioDirectoryPath, "MVPMusic");
            if (!Directory.Exists(MVPMusicDirectory))
                DownloadMVPMusic(MVPMusicDirectory);

            SettingBase.Register(new[] { SettingsHeader });
            MVPMusic.RegisterEvents();
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            Instance = null;
            SettingBase.Unregister(settings: new[] { SettingsHeader });
            MVPMusic.UnregisterEvents();
            base.OnDisabled();
        }
        private void DownloadMVPMusic(string MVPMusicDirectory)
        {
            string MVPMusicZip = MVPMusicDirectory + ".zip";
            string MVPMusicTemp = MVPMusicDirectory + "_Temp";

            using WebClient client = new();

            Log.Warn("Downloading MVPMusic.zip...");
            client.DownloadFile(
                $"https://github.com/Vretu-Dev/MVPMusic/releases/download/{Version}/MVPMusic.zip",
                MVPMusicZip);

            Log.Info("MVPMusic.zip has been downloaded!");

            Log.Warn("Extracting...");
            ZipFile.ExtractToDirectory(MVPMusicZip, MVPMusicTemp);
            Directory.Move(Path.Combine(MVPMusicTemp, "MVPMusic"), MVPMusicDirectory);

            Directory.Delete(MVPMusicTemp);
            File.Delete(MVPMusicZip);

            Log.Info("Done!");
        }
    }
}