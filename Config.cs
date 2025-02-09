﻿using Exiled.API.Interfaces;
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
        public string BotName { get; set; } = "Round MVP: {Nickname}";
        [Description("UserSettings ID")]
        public int MusicDropdownId { get; set; } = 101;
    }
}
