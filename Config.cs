using Exiled.API.Interfaces;
using System.Collections.Generic;
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
        [Description("Set Bot Nickname at the end of the round.")]
        public string Mvphint { get; set; } = "<size=35><b><color=yellow>Round MVP:<color=#70EE9C>{Nickname}</color> as <color=red>{Scenario}</color></color></b></size>";
        [Description("UserSettings ID")]
        public int MusicDropdownId { get; set; } = 101;
        [Description("Additional music list in the format: 'DisplaySongName': 'Path'. For example, 'TakeMeOut': 'MVPMusic/takemeout'.")]
        public Dictionary<string, string> MusicList { get; set; } = new Dictionary<string, string>
        {
            { "Example Music", "MVPMusic/example" },
            { "Another Example Music", "MVPMusic/example2" }
        };
    }
}