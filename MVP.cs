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
            { "All for Dust", "MVPMusic/All-for-Dust" },
            { "Bachram", "MVPMusic/Bachram" },
            { "Desert Fire", "MVPMusic/Desert-Fire" },
            { "Mocha Petal", "MVPMusic/Mocha-Petal" },
            { "I Am By AWOLNATION", "MVPMusic/I-Am-By-AWOLNATION" },
            { "u mad!", "MVPMusic/u-mad" },
            { "Aggressive", "MVPMusic/Aggressive" },
            { "Disgusting", "MVPMusic/Disgusting" },
            { "The Good Youth", "MVPMusic/The-Good-Youth" },
            { "Yellow Magic", "MVPMusic/Yellow-Magic" },
            { "The Talos Principle", "MVPMusic/The-Talos-Principle" },
            { "Crimson Assault", "MVPMusic/Crimson-Assault" },
            { "Eye of the Dragon", "MVPMusic/Eye-of-the-Dragon" },
            { "Total Domination", "MVPMusic/Total-Domination" },
            { "The 8-Bit Kit", "MVPMusic/The-8-Bit-Kit" },
            { "Moments CSGO", "MVPMusic/Moments-CSGO" },
            { "ULTIMATE", "MVPMusic/ULTIMATE" },
            { "Death's Head Demolition", "MVPMusic/Deaths-Head-Demolition" },
            { "Gunman Taco", "MVPMusic/Gunman-Taco" },
            { "Feel The Power", "MVPMusic/Feel-ThePower" },
            { "High Noon", "MVPMusic/High-Noon" },
            { "Vici", "MVPMusic/Vici" },
            { "Void", "MVPMusic/Void" },
            { "FREE", "MVPMusic/FREE" },
            { "Lion's Mouth", "MVPMusic/Lions-Mouth" },
            { "inhuman", "MVPMusic/inhuman" },
            { "Astro Bellum", "MVPMusic/Astro-Bellum" },
            { "Shooters", "MVPMusic/Shooters" },
            { "Hazardous Environments", "MVPMusic/Hazardous-Environments" },
            { "All Night", "MVPMusic/All-Night" },
            { "Heading for the Source", "MVPMusic/Heading-for-the-Source" },
            { "MOLOTOV", "MVPMusic/MOLOTOV" },
            { "Make U SWEAT!", "MVPMusic/Make-U-SWEAT" },
            { "dashstar", "MVPMusic/dashstar" },
            { "Work Hard, Play Hard", "MVPMusic/Work-Hard" },
            { "Java Havana", "MVPMusic/Java-Havana" },
            { "For No Mankind", "MVPMusic/For-No-Mankind" },
            { "IsoRhythm", "MVPMusic/IsoRhythm" },
            { "Drifter", "MVPMusic/Drifter" },
            { "Gothic Luxury", "MVPMusic/Gothic-Luxury" },
            { "Invasion!", "MVPMusic/Invasion" },
            { "All I Want for Christmas", "MVPMusic/All-I-Want-for-Christmas" },
            { "Diamonds", "MVPMusic/Diamonds" },
            { "Life's Not Out To Get You", "MVPMusic/Lifes-Not-Out-To-Get-You" },
            { "The Lowlife Pack", "MVPMusic/The-Lowlife-Pack" },
            { "Sponge Fingerz", "MVPMusic/Sponge-Fingerz" },
            { "Sharpened", "MVPMusic/Sharpened" },
            { "Hua Lian", "MVPMusic/Hua-Lian" },
            { "Battlepack", "MVPMusic/Battlepack" },
            { "Reason", "MVPMusic/Reason" },
            { "Backbone", "MVPMusic/Backbone" },
            { "Insurgency", "MVPMusic/Insurgency" },
            { "Bodacious", "MVPMusic/Bodacious" },
            { "KOLIBRI", "MVPMusic/KOLIBRI" },
            { "LNOE", "MVPMusic/Hua-Lian" },
            { "CHAIN$AW.LXADXUT.", "MVPMusic/chain" },
            { "King, Scar", "MVPMusic/King-Scar" },
            { "A*D*8", "MVPMusic/AD8" },
            { "Metal", "MVPMusic/Metal" },
            { "II-Headshot", "MVPMusic/II-Headshot" },
            { "III-Arena", "MVPMusic/III-Arena" },
            { "Lock Me Up", "MVPMusic/Lock-Me-Up" },
            { "EZ4ENCE", "MVPMusic/EZ4ENCE" },
            { "Flashbang Dance", "MVPMusic/Flashbang-Dance" },
            { "Neo Noir", "MVPMusic/Neo-Noir" },
            { "M.U.D.D. FORCE", "MVPMusic/MUDD-FORCE" },
            { "Uber Blasto Phone", "MVPMusic/Uber-Blasto-Phone" },
            { "Under Bright Lights", "MVPMusic/Under-Bright-Lights" },
            { "GLA", "MVPMusic/GLA" },
            { "Hotline Miami", "MVPMusic/Hotline-Miami" },
            { "Hades", "MVPMusic/Hades" },
            { "Anti-Citizen", "MVPMusic/Anti-Citizen" },
            { "The Master Chief", "MVPMusic/The-Master-Chief" },
            { "Default CS:GO First", "MVPMusic/Default-CSGO" },
            { "Default CS:GO Second", "MVPMusic/Default2-CSGO" },
            { "Default CS:GO 2", "MVPMusic/Default-CSGO2" }
        };
        private static Player firstEscaper = null;
        private static Player firstScpKiller = null;
        public static DropdownSetting MusicDropdownSetting { get; private set; }

        public static void RegisterEvents()
        {
            if (Main.Instance.Config.MusicList != null)
            {
                foreach (var kv in Main.Instance.Config.MusicList)
                {
                    if ((kv.Key == "Example Music" && kv.Value == "MVPMusic/example") || (kv.Key == "Another Example Music" && kv.Value == "MVPMusic/example2"))
                    {
                        continue;
                    }
                    if (!MusicMapping.ContainsKey(kv.Key))
                    {
                        MusicMapping.Add(kv.Key, kv.Value);
                    }
                }
            }

            MusicDropdownSetting = new DropdownSetting(
            id: Main.Instance.Config.MusicDropdownId,
            label: Main.Instance.Translation.SettingLabel,
            options: MusicMapping.Keys.Cast<string>().ToArray(),
            defaultOptionIndex: 0,
            dropdownEntryType: SSDropdownSetting.DropdownEntryType.Regular,
            hintDescription: Main.Instance.Translation.HintDescription,
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
                string hint = new string('\n', 4) + Main.Instance.Config.Mvphint
                    .Replace("{Nickname}", mvpPlayer.Nickname)
                    .Replace("{Scenario}", Main.Instance.Config.Mvp);

                foreach (var player in Player.List)
                {
                    if (!player.IsHost)
                        player.ShowHint(hint, 10);
                }

                string musicUrl = GetMusicUrlFromPlayer(mvpPlayer);
                string command = $"/audio play {musicUrl}";
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
                if (setting.OriginalDefinition == null)
                    return string.Empty;

                string friendlyName = setting.SelectedOption;

                if (MusicMapping.TryGetValue(friendlyName, out string actualUrl))
                    return actualUrl;
            }
            return string.Empty;
        }
    }
}