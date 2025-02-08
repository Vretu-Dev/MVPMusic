using Exiled.API.Interfaces;
using System.ComponentModel;

namespace MVPMusic
{
    public class Config : IConfig
    {
        [Description("Whether the plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        [Description("MVP selection method. Options available: FirstEscaper, FirstScpKiller, TopKiller, TopDamageDealer.")]
        public string Mvp { get; set; } = "FirstEscaper";
        [Description("UserSettings ID")]
        public int MusicDropdownId { get; set; } = 101;
    }
}
