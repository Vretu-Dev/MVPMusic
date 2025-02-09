using System;
using System.Linq;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using UserSettings.ServerSpecific;

namespace MVPMusic
{
    public static class MVPMusic
    {
        private static Dictionary<Player, int> scpKills = new Dictionary<Player, int>();
        private static Dictionary<Player, int> humanKills = new Dictionary<Player, int>();
        private static Dictionary<Player, float> humanDamage = new Dictionary<Player, float>();
        public static readonly Dictionary<string, string> MusicMapping = new Dictionary<string, string>
        {
            { "All for Dust", "MVPMusic/All-for-Dust.ogg" },
            { "Bachram", "MVPMusic/Bachram.ogg" },
            { "Desert Fire", "MVPMusic/Desert-Fire.ogg" },
            { "Mocha Petal", "MVPMusic/Mocha-Petal.ogg" },
            { "I Am By AWOLNATION", "MVPMusic/I-Am-By-AWOLNATION.ogg" },
            { "u mad!", "MVPMusic/u-mad.ogg" },
            { "Aggressive", "MVPMusic/Aggressive.ogg" },
            { "Disgusting", "MVPMusic/Disgusting.ogg" },
            { "The Good Youth", "MVPMusic/The-Good-Youth.ogg" },
            { "Yellow Magic", "MVPMusic/Yellow-Magic.ogg" },
            { "The Talos Principle", "MVPMusic/The-Talos-Principle.ogg" },
            { "Crimson Assault", "MVPMusic/Crimson-Assault.ogg" },
            { "Eye of the Dragon", "MVPMusic/Eye-of-the-Dragon.ogg" },
            { "Total Domination", "MVPMusic/Total-Domination.ogg" },
            { "The 8-Bit Kit", "MVPMusic/The-8-Bit-Kit.ogg" },
            { "Moments CSGO", "MVPMusic/Moments-CSGO.ogg" },
            { "ULTIMATE", "MVPMusic/ULTIMATE.ogg" },
            { "Death's Head Demolition", "MVPMusic/Deaths-Head-Demolition.ogg" },
            { "Gunman Taco", "MVPMusic/Gunman-Taco.ogg" },
            { "Feel The Power", "MVPMusic/Feel-ThePower.ogg" },
            { "High Noon", "MVPMusic/High-Noon.ogg" },
            { "Vici", "MVPMusic/Vici.ogg" },
            { "Void", "MVPMusic/Void.ogg" },
            { "FREE", "MVPMusic/FREE.ogg" },
            { "Lion's Mouth", "MVPMusic/Lions-Mouth.ogg" },
            { "inhuman", "MVPMusic/inhuman.ogg" },
            { "Astro Bellum", "MVPMusic/Astro-Bellum.ogg" },
            { "Shooters", "MVPMusic/Shooters.ogg" },
            { "Hazardous Environments", "MVPMusic/Hazardous-Environments.ogg" },
            { "All Night", "MVPMusic/All-Night.ogg" },
            { "Heading for the Source", "MVPMusic/Heading-for-the-Source.ogg" },
            { "MOLOTOV", "MVPMusic/MOLOTOV.ogg" },
            { "Make U SWEAT!", "MVPMusic/Make-U-SWEAT.ogg" },
            { "dashstar", "MVPMusic/dashstar.ogg" },
            { "Work Hard, Play Hard", "MVPMusic/Work-Hard.ogg" },
            { "Java Havana", "MVPMusic/Java-Havana.ogg" },
            { "For No Mankind", "MVPMusic/For-No-Mankind.ogg" },
            { "IsoRhythm", "MVPMusic/IsoRhythm.ogg" },
            { "Drifter", "MVPMusic/Drifter.ogg" },
            { "Gothic Luxury", "MVPMusic/Gothic-Luxury.ogg" },
            { "Invasion!", "MVPMusic/Invasion.ogg" },
            { "All I Want for Christmas", "MVPMusic/All-I-Want-for-Christmas.ogg" },
            { "Diamonds", "MVPMusic/Diamonds.ogg" },
            { "Life's Not Out To Get You", "MVPMusic/Lifes-Not-Out-To-Get-You.ogg" },
            { "The Lowlife Pack", "MVPMusic/The-Lowlife-Pack.ogg" },
            { "Sponge Fingerz", "MVPMusic/Sponge-Fingerz.ogg" },
            { "Sharpened", "MVPMusic/Sharpened.ogg" },
            { "Hua Lian", "MVPMusic/Hua-Lian.ogg" },
            { "Battlepack", "MVPMusic/Battlepack.ogg" },
            { "Reason", "MVPMusic/Reason.ogg" },
            { "Backbone", "MVPMusic/Backbone.ogg" },
            { "Insurgency", "MVPMusic/Insurgency.ogg" },
            { "Bodacious", "MVPMusic/Bodacious.ogg" },
            { "KOLIBRI", "MVPMusic/KOLIBRI.ogg" },
            { "LNOE", "MVPMusic/Hua-Lian.ogg" },
            { "CHAIN$AW.LXADXUT.", "MVPMusic/chain.ogg" },
            { "King, Scar", "MVPMusic/King-Scar.ogg" },
            { "A*D*8", "MVPMusic/AD8.ogg" },
            { "Metal", "MVPMusic/Metal.ogg" },
            { "II-Headshot", "MVPMusic/II-Headshot.ogg" },
            { "III-Arena", "MVPMusic/III-Arena.ogg" },
            { "Lock Me Up", "MVPMusic/Lock-Me-Up.ogg" },
            { "EZ4ENCE", "MVPMusic/EZ4ENCE.ogg" },
            { "Flashbang Dance", "MVPMusic/Flashbang-Dance.ogg" },
            { "Neo Noir", "MVPMusic/Neo-Noir.ogg" },
            { "M.U.D.D. FORCE", "MVPMusic/MUDD-FORCE.ogg" },
            { "Uber Blasto Phone", "MVPMusic/Uber-Blasto-Phone.ogg" },
            { "Under Bright Lights", "MVPMusic/Under-Bright-Lights.ogg" },
            { "GLA", "MVPMusic/GLA.ogg" },
            { "Hotline Miami", "MVPMusic/Hotline-Miami.ogg" },
            { "Hades", "MVPMusic/Hades.ogg" },
            { "Anti-Citizen", "MVPMusic/Anti-Citizen.ogg" },
            { "The Master Chief", "MVPMusic/The-Master-Chief.ogg" },
            { "Default CS:GO First", "MVPMusic/Default-CSGO.ogg" },
            { "Default CS:GO Second", "MVPMusic/Default2-CSGO.ogg" },
            { "Default CS:GO 2", "MVPMusic/Default-CSGO2.ogg" }
        };
        private static Player firstEscaper = null;
        private static Player firstScpKiller = null;
        public static DropdownSetting MusicDropdownSetting { get; private set; }

        public static void RegisterEvents()
        {
            MusicDropdownSetting = new DropdownSetting(
            id: Main.Instance.Config.MusicDropdownId,
            label: "Choose music",
            options: MusicMapping.Keys.Cast<string>().ToArray(),
            defaultOptionIndex: 0,
            dropdownEntryType: SSDropdownSetting.DropdownEntryType.Regular,
            hintDescription: "Select the song that will be played at the end of the round.",
            onChanged: (player, setting) =>
            {
                string friendlyName = (setting as DropdownSetting)?.SelectedOption;
                if (MusicMapping.TryGetValue(friendlyName, out string url))
                {
                    player.SessionVariables["SelectedMusicUrl"] = url;
                }
            });

            SettingBase.Register(new[] { MusicDropdownSetting });
            Exiled.Events.Handlers.Player.Died += OnPlayerDied;
            Exiled.Events.Handlers.Player.Dying += OnPlayerDying;
            Exiled.Events.Handlers.Player.Escaping += OnPlayerEscaping;
            Exiled.Events.Handlers.Player.Hurting += OnPlayerHurting;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
            Exiled.Events.Handlers.Server.RestartingRound += OnRestartingRound;
        }

        public static void UnregisterEvents()
        {
            SettingBase.Unregister(settings: new[] { MusicDropdownSetting });
            Exiled.Events.Handlers.Player.Died -= OnPlayerDied;
            Exiled.Events.Handlers.Player.Dying -= OnPlayerDying;
            Exiled.Events.Handlers.Player.Escaping -= OnPlayerEscaping;
            Exiled.Events.Handlers.Player.Hurting -= OnPlayerHurting;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            Exiled.Events.Handlers.Server.RestartingRound -= OnRestartingRound;
        }

        private static void OnPlayerHurting(HurtingEventArgs ev)
        {
            Player attacker = ev.Attacker;
            Player victim = ev.Player;

            if (attacker != null && attacker != victim && attacker.Role.Team != Team.SCPs && victim.Role.Team == Team.SCPs)
            {
                if (!humanDamage.ContainsKey(attacker))
                    humanDamage[attacker] = 0;

                humanDamage[attacker] += (int)Math.Round(ev.Amount);
            }
        }

        private static void OnPlayerDying(DyingEventArgs ev)
        {
            Player attacker = ev.Attacker;
            Player victim = ev.Player;

            if (victim.Role.Team == Team.SCPs && victim.Role.Type != RoleTypeId.Scp0492)
            {
                if (firstScpKiller == null)
                {
                    firstScpKiller = attacker;
                }
            }
        }

        private static void OnPlayerDied(DiedEventArgs ev)
        {
            Player attacker = ev.Attacker;

            if (attacker != null && attacker != ev.Player)
            {
                if (attacker.Role.Team == Team.SCPs)
                {
                    if (!scpKills.ContainsKey(attacker))
                        scpKills[attacker] = 0;
                    scpKills[attacker]++;
                }
                else
                {
                    if (!humanKills.ContainsKey(attacker))
                        humanKills[attacker] = 0;
                    humanKills[attacker]++;
                }
            }
        }

        private static void OnPlayerEscaping(EscapingEventArgs ev)
        {
            if (firstEscaper == null && ev.IsAllowed)
            {
                firstEscaper = ev.Player;
            }
        }

        private static void OnRoundEnded(RoundEndedEventArgs ev)
        {
            Player mvpPlayer = null;
            switch (Main.Instance.Config.Mvp)
            {
                case "FirstEscaper":
                    mvpPlayer = firstEscaper;
                    break;
                case "FirstScpKiller":
                    mvpPlayer = firstScpKiller;
                    break;
                case "TopKiller":
                    mvpPlayer = GetOverallTopKiller();
                    break;
                case "TopDamageDealer":
                    mvpPlayer = GetTopDamageDealer(humanDamage);
                    break;
                default:
                    mvpPlayer = firstScpKiller;
                    break;
            }

            if (mvpPlayer != null)
            {
                string musicUrl = GetMusicUrlFromPlayer(mvpPlayer);
                string botName = Main.Instance.Config.BotName.Replace("{Nickname}", mvpPlayer.Nickname).Replace(" ", "\u00A0");
                string command = $"/audio play {musicUrl} {botName}";
                Server.ExecuteCommand(command);
            }
        }

        private static void OnRestartingRound()
        {
            scpKills.Clear();
            humanKills.Clear();
            firstEscaper = null;
            humanDamage.Clear();
            firstScpKiller = null;
        }

        private static Player GetTopKiller(Dictionary<Player, int> kills)
        {
            Player topKiller = null;
            int maxKills = 0;
            foreach (var entry in kills)
            {
                if (entry.Value > maxKills)
                {
                    topKiller = entry.Key;
                    maxKills = entry.Value;
                }
            }
            return topKiller;
        }

        private static Player GetOverallTopKiller()
        {
            Dictionary<Player, int> combinedKills = new Dictionary<Player, int>();

            foreach (var kv in humanKills)
            {
                if (!combinedKills.ContainsKey(kv.Key))
                    combinedKills[kv.Key] = 0;
                combinedKills[kv.Key] += kv.Value;
            }
            foreach (var kv in scpKills)
            {
                if (!combinedKills.ContainsKey(kv.Key))
                    combinedKills[kv.Key] = 0;
                combinedKills[kv.Key] += kv.Value;
            }
            return GetTopKiller(combinedKills);
        }

        private static Player GetTopDamageDealer(Dictionary<Player, float> damage)
        {
            Player topDealer = null;
            float maxDamage = 0;
            foreach (var entry in damage)
            {
                if (entry.Value > maxDamage)
                {
                    topDealer = entry.Key;
                    maxDamage = entry.Value;
                }
            }
            return topDealer;
        }

        private static string GetMusicUrlFromPlayer(Player player)
        {
            int dropdownId = Main.Instance.Config.MusicDropdownId;
            if (SettingBase.TryGetSetting<DropdownSetting>(player, dropdownId, out DropdownSetting setting))
            {
                string friendlyName = setting.SelectedOption;
                if (MusicMapping.TryGetValue(friendlyName, out string actualUrl))
                    return actualUrl;
            }
            return string.Empty;
        }
    }
}