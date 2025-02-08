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
        public static string MVPDirectoryPath { get; } = Path.Combine(Paths.Configs, "Audio");
        public HeaderSetting SettingsHeader { get; set; } = new HeaderSetting("MVP MUSIC");
        public override void OnEnabled()
        {
            Instance = this;

            if (!Directory.Exists(MVPDirectoryPath))
            {
                Log.Warn("MVPMusic directory does not exist. Creating...");
                Directory.CreateDirectory(MVPDirectoryPath);
            }

            string MVPMusicDirectory = Path.Combine(MVPDirectoryPath, "MVPMusic");
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
            string MVPZip = MVPMusicDirectory + ".zip";
            string MVPTemp = MVPMusicDirectory + "_Temp";

            using WebClient client = new();

            Log.Warn("Downloading MVPMusic.zip...");
            client.DownloadFile(
                $"https://github.com/Vretu-Dev/MVPMusic/releases/download/v{Version}/MVPMusic.zip",
                MVPZip);

            Log.Info("ExampleTimer.zip has been downloaded!");

            Log.Warn("Extracting...");
            ZipFile.ExtractToDirectory(MVPZip, MVPTemp);
            Directory.Move(Path.Combine(MVPTemp, "MVPMusic"), MVPTemp);

            Directory.Delete(MVPTemp);
            File.Delete(MVPZip);

            Log.Info("Done!");
        }
    }
}