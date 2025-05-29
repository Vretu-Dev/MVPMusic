![MVPMusic](https://github.com/user-attachments/assets/ec7fffb0-1fb5-422a-8e42-7e9b90df4f39)<br><br><br>
[![downloads](https://img.shields.io/github/downloads/Vretu-Dev/MVPMusic/total?style=for-the-badge&logo=icloud&color=%233A6D8C)](https://github.com/Vretu-Dev/MVPMusic/releases/latest)
![Latest](https://img.shields.io/github/v/release/Vretu-Dev/MVPMusic?style=for-the-badge&label=Latest%20Release&color=%23D91656)

## Downloads:
| Framework | Version    |  Release                                                              |
|:---------:|:----------:|:----------------------------------------------------------------------:|
| Exiled    | ≥ 9.6.0    | [⬇️](https://github.com/Vretu-Dev/MVPMusic/releases/latest)        |

## Features
- **Music Download**: Automatically download music files from GitHub.
- **Dynamic Music Change**: Players can choose tracks from an available list.
- **Supports `DropdownSetting`**: Players can change their music selection via a dropdown menu.
- **Contain All MVP Music**: (https://www.youtube.com/watch?v=NqDtEO2Smaw)

## Requirements:
   - [AudioPlayer](https://github.com/Antoniofo/AudioPlayer/releases/latest)
   - [SCPSLAudioApi](https://github.com/CedModV2/SCPSLAudioApi/releases/latest)

## Config:
```yaml
MVPMusic:
# Whether the plugin is enabled.
  is_enabled: true
  debug: false
  # MVP selection method. Options available: FirstEscaper, FirstScpKiller, TopKiller, TopDamageDealer.
  mvp: 'FirstEscaper'
  # Set Bot Nickname at the end of the round.
  bot_name: 'Round MVP: {Nickname}'
  # UserSettings ID
  music_dropdown_id: 101
  # Additional music list in the format: 'DisplaySongName': 'Path'. For example, 'TakeMeOut': 'MVPMusic/takemeout.ogg'.
  music_list:
    Example Music: MVPMusic/example.ogg
    Another Example Music: MVPMusic/example2.ogg
```
