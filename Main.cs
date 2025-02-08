using System;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Exiled.Loader;

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
        public HeaderSetting SettingsHeader { get; set; } = new HeaderSetting("MVP MUSIC");
        public override void OnEnabled()
        {
            Instance = this;
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
    }
}