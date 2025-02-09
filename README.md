# MVPMusic Plugin

MVPMusic is a plugin designed for **SCP: Secret Laboratory** that allows you to manage and play music from external sources on the server. The plugin enables synchronization of music selection with player settings and automatic downloading of music files from a GitHub repository.

---

## ✨ Features

- **Music Download**: Automatically download music files from GitHub.
- **Dynamic Music Change**: Players can choose tracks from an available list.
- **Supports `DropdownSetting`**: Players can change their music selection via a dropdown menu.
- **Contain All MVP Music**: (https://www.youtube.com/watch?v=NqDtEO2Smaw)
---

## 🛠️ Installation

1. **Download the Plugin**:
   - Copy the `.dll` plugin file to the `plugins` folder of your SCP:SL installation.

2. **Configuration**:
   - Set MVP selection method

3. **Requirements**:
   - [AudioPlayer](https://github.com/Antoniofo/AudioPlayer/releases/download/v2.3.0/AudioPlayer.dll)
   - [SCPSLAudioApi](https://github.com/CedModV2/SCPSLAudioApi/releases/download/0.0.8/SCPSLAudioApi.dll)
---

## ⚙️ Configuration

In the configuration file, you can adjust the following settings:

```yaml
MVPMusic:
# Whether the plugin is enabled.
  is_enabled: true
  debug: false
  # MVP selection method. Options available: FirstEscaper, FirstScpKiller, TopKiller, TopDamageDealer.
  mvp: 'TopKiller'
  # Set Bot Nickname at the end of the round.
  bot_name: 'Round MVP: {Nickname}'
  # UserSettings ID
  music_dropdown_id: 101
  # Additional music list in the format: 'DisplaySongName': 'Path'. For example, 'TakeMeOut': 'MVPMusic/takemeout.ogg'.
  music_list: {}
```
