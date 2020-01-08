# project-beats-game
Implementation of Project: Beats game
  
## This project is a huge WIP.
Currently, I'm restructuring my original source to make things more organized and scalable.  
Slowly making progress 🧩

## Dependencies
- Newtonsoft.Json (Tested with net45 version)

## To-Dos
### PBGame
- BaseGame
- ProjectBeatsGame
#### Audio
- SFX controller poool
- Music manager
- Music playlist
- Metronome
#### Graphics
- Color definitions
#### IO/Decoding/Beats
- Establish standard data format for Beats data.
- Implement decoder for Beats file formats.
- Decoder for skins
#### IO/Decoding/Osu
- Decoder for skins
#### IO/Decoding/Osu/Catch
- HitObjectParser
#### IO/Decoding/Osu/Mania
- HitObjectParser
#### IO/Decoding/Osu/Taiko
- HitObjectParser
#### Maps
- Implement date sorting for MapsetList
#### Networking
- API to Osu server
#### Networking/Maps
- Mapset and map info for maps listed on server side
#### Rulesets
- Ruleset provider for different game modes.
#### Rulesets/Beats/Simple
#### Rulesets/Beats/Standard
- GameSession
- ModeService
#### Rulesets/Beats/Standard/Inputs
#### Rulesets/Beats/Standard/Maps
- MapConverter
- MapProcessor
#### Rulesets/Beats/Standard/UI
#### Rulesets/Osu/Catch
#### Rulesets/Osu/Mania
#### Rulesets/Osu/Standard
#### Rulesets/Osu/Taiko
#### Rulesets/Osu/Catch
#### Rulesets/UI
#### Skins
- SkinManager
#### Stores
- MapAssetStore
#### UI
#### UI/Navigations
#### UI/Navigations/Overlays
#### UI/Navigations/Overlays/Networking
#### UI/Navigations/Screens
- SplashScreen
- MainMenuScreen
- SongsScreen
- PlayScreen
- ResultScreen
#### UI/Navigations/Screens/Networking
- DownloaderScreen