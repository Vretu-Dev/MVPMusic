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
            { "Music A", "https://status.dreamserwer.pl/test.ogg" },
            { "Music B", "https://myawesomewebserver.net/files/audio2.ogg" },
            { "Music C", "https://myawesomewebserver.net/files/audio3.ogg" }
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
                string command = $"/audio play {musicUrl} {mvpPlayer.Nickname}";
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