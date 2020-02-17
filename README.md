# project-beats-game
Implementation of Project: Beats game
  
## This project is a huge WIP.
Currently, I'm restructuring my original source to make things more organized and scalable.  
Slowly making progress 🧩

## Dependencies
- project-beats-framework (https://github.com/jerryrox/project-beats-framework)
- pbeffect-coffee (https://github.com/jerryrox/pbeffect-coffee)
- Newtonsoft.Json (Tested with net45 version)

## To-Dos
- Implement visualizer module.
- Implement quick message displayer overlay.
- Initially sort the mapsets on load.
- Store play record to record manager and apply to user statistics.
- Map background image transition on change. (Apply on other areas where necessary)
- Refactor code to support logging in using other API providers than just Osu.
- Fix issue where the song won't change to the next song in home screen occasionally.
- Fix issue where metronome doesn't update when changing song mid-way.
- Find out how to retrieve a map's description.

- Support for interactive unit testing for UI.

### In-progress
- Implement osu standard hit objects
- Implement osu standard judgements
- Implement osu standard map
- Implement osu standard difficulty
- Implement osu standard scoring
- Implement mode service for osu standard.

### PBGame
- BaseGame
- ProjectBeatsGame
#### IO/Decoding/Beats
- Establish standard data format for Beats data.
- Implement decoder for Beats file formats.
#### IO/Decoding/Osu/Catch
- HitObjectParser
#### IO/Decoding/Osu/Mania
- HitObjectParser
#### IO/Decoding/Osu/Taiko
- HitObjectParser
#### Rulesets
- Ruleset provider for different game modes.
#### Rulesets/Beats/Simple
#### Rulesets/Beats/Standard
- GameSession
#### Rulesets/Beats/Standard/Difficulty
- Performance calculator
#### Rulesets/Beats/Standard/Inputs
#### Rulesets/Beats/Standard/UI
- Implement from Rulesets/UI
#### Rulesets/Beats/Standard/UI/HUD
- Implement from Rulesets/UI/HUD
#### Rulesets/Osu/Catch
#### Rulesets/Osu/Mania
#### Rulesets/Osu/Standard
#### Rulesets/Osu/Taiko
#### Rulesets/Osu/Catch
#### Rulesets/UI
- Implement gameplay layer
- Implement hud container
- Implement play area container
- Implement storyboard layer when storyboarding should be supported.
#### Rulesets/UI/HUD
- Implement health display using progress bar element.
#### Stores
- MapAssetStore
#### UI/Navigations/Overlays/Networking
#### UI/Navigations/Screens
- (SplashScreen) Think of the UI design.
- PlayScreen
- ResultScreen
#### UI/Navigations/Screens/Networking
- DownloaderScreen
#### Others
- Utilize MapPlaylist object in music player on menu overlay.