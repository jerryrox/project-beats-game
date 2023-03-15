# project-beats-game
Implementation of Project: Beats game
  
## This project is a huge WIP.
Slowly making progress 🧩

## Dependencies
- project-beats-framework (Tested on 1.3.0) (https://github.com/jerryrox/project-beats-framework)
- pbeffect-coffee (https://github.com/jerryrox/pbeffect-coffee)
- Newtonsoft.Json (Tested with net45 version)
### Extra
- project-beats-api (Only if using Network feature. Tested on 0.6.1) (https://github.com/jerryrox/project-beats-api)

## Script Execution Order set up
- `PBFramework.Inputs.InputManager` should be assigned before `Default Time`.

## Development progress
[https://trello.com/b/5gpuJrRa/project-beats-renewed]

## Versions
### 0.14.1.1
#### New features
- Made app files be available directly from the device's file browser app.

### 0.14.1
#### New features
- Implemented ability to load downloaded maps manually via settings.
- Implemented sharing result to external targets via native interface.
- Implemented notification clear button.
- Implemented `LabelButton` based on `HoverableTrigger` for general purpose.
- Implemented representing a failed play by making rank type to "F".
- Implemented settings to discard records and replays of failed plays.
- Implemented option to toggle saving replays.
#### Changes
- Upgraded Unity version to 2019.4.23f1.
- Changed api request URLs due to discontinued Heroku free tier.
#### Fixes
- Fixed inconsistent dragger final hit effect.
- Fixed issue where the UI root resolution didn't resize at the desired timing, which caused certain effects be out of position.

### 0.14.0
#### New features
- Implemented displayal of map count for their playable mode on `SongListItem`.
- Implemented ability to view result of a record by tapping on it.
- Implemented replay feature.
- Implemented `PBGame.Threading.ThreadedLoader` to process a list of inputs into outputs using multiple tasks. May be moved to PBFramework in the future.
- Added support for iOS File Sharing in case download becomes unusable in the future.
- Added a game configuration entry to set what type of notification is always stored in the notification box.
- Added a game configuration entry to pipe log messages above certain level to the notification box.
#### Improvements
- Improved initial loading time by integrating `ThreadedLoader` to `MapsetStore` reload routine.
#### Changes
- Make the mobile platform use the acceleration supported by `Cursor` input.
- Removed judgement detail data from record files.
#### Fixes
- Fixed button sound not playing on escape button trigger.

### 0.13.0
#### New features
- Implemented local rankings.
- Implemented damaged effect.
- Implemented touch pulse effects.
- Implemented game mode selection menu.
- Implemented BPM info display on `PrepareScreen`.
- Implemented support for selecting game modes even when they are unplayable.
- Implemented garbage collection and Resource unloading tasks for game loading process.
- (Beats Standard) Implemented combo displayer on the lane sprite.
#### Improvements
- Added visual improvements on the health bar.
- Implemented recyclers on dialog option buttons.
#### Changes
- Made pause indicator container be disabled on hiding end animation.
- Made the selection of map for offset tweaking done through `OffsetsModel`, not indirectly via `MapSelection`.
- Removed `RecordManager` and made use of `RecordStore` instead.
- The default value for `IsClickToTrigger` on `BasicTrigger` has been changed to `true`.
- (Beats Standard) Removed old touch effect on `HitBarDisplay`.
#### Fixes
- Fixed layout issue in `PrepareScreen` where the detail panel does not fully hide the inner content when in brief mode.
- Fixed issue where `WebTexture` component's alpha could not be customized.
- Fixed bug where `CommonBpm` was returned as its `BeatLength` value, not the actual BPM.
- (Beats Standard) Fixed issue where touching anywhere on the screen would occasionally register as a valid hit on objects.

### 0.12.0
#### New features
- Implemented AvatarDisplay component to simplify loading of avatar images.
- Implemented NotificationMenuOverlay.
- Implemented map actions dialog.
- Implemented map deletion.
- Implemented mapset deletion.
- Implemented proper escape buttons during in-game.
- Implemented on displaying log message through notifications.
- Implemented scroll-to-top button in DownloadScreen.
- Reworked on notification displayal. (Added support for task cancellation, progress check, action handling, cover image).
- Added judgemnt counter display for ResultScreen.
#### Improvements
- Sign-in process will continue even when login menu overlay has become hidden.
#### Changes
- Moved ScoreProcessor creation from GameSession to ModeService.
- Moved MapImageDisplay component to its proper namespace.
- Changed the way offline user is handled by UserManager.
- Changed the way some images are loaded.
- Changed some HoverableTriggers to IconButton.
#### Fixes
- Fixed internal logic error while saving play records.
- Fixed exp displaying wrongly.
- Fixed best score evaluation being done using ALL records, not filtered by current user's id.
- Fixed preview bar showing even when previewing a different song.
- Fixed dragger tick view being mispositioned on judgement.
- Fixed inaccurate sample points for sliders.
- Fixed issue where DownloadScreen's result cell does not load image immediately when there is already a cache.
- Fixed prepare screen's details scrollview not having full width.

### 0.11.0
#### New features
- Added offset menu button in `PrepareScreen`.
- Added version information in settings menu.
- Navigate to prepare screen when the selected song list item has been selected again.
#### Changes
- Refactored Cacher.
- Removed dependency to `Promise` and `Progress` and replaced with `ITask` and `TaskListener`.
- Increased timeout time for mapset download.
#### Fixes
- Fixed online mapset result cell's label not being constrained.
- Fixed online mapset list not resetting to the beginning after search caused by changing one of the search options.
- Fixed music continuously playing when entering DownloadScreen.
- Fixed bug where pressing preview stop button on DownloadScreen makes it replay instead.
- Fixed offset not applying immediately when changing via OffsetOverlay.
- Fixed online mapset list not showing.
- Fixed issue where game freezes after the play when logged in.

### 0.10.0
#### New features
- Added ability to test using localhost api server.
- Added ability to check whether game environment is test mode.
#### Improvements
- Decoupled logics away from most UI elements.
#### Changes
- Abstracted away shared logics of BaseScreen/BaseOverlay views into BaseNavView.
- Removed UI screen/overlay interfaces as they are now useless.
- Access IRaycastable directly instead of casting.
#### Fixes
- Fixed input box's hover sprite being able to receive raycast.
- Fixed bug where focusing on song search bar and releasing without change triggers a search.

### 0.9.3
#### New features
- Allow selection of different API providers for logging in.
- Added view component for OAuth login.
- Integrated DeepLinker with API.
- Display a message when mapset download has initiated.
#### Changes
- Reworked on networking API to adapt to project-beats-api.
- Moved local user data loading routine away from UI to ProjectBeatsGame event.
- Upgraded Unity to 2019.4 LTS
#### Fixes
- Fixed mapsets result list keep on refreshing when there are no more results.
- Fixed loader indicator not displaying correctly.
- API provider will now be passed without lowered case due to parsing issues using JSON.

### 0.9.2
#### New features
- Added button hover sound toggle configuration.
- Added scrolling capability for dropdown menu popup.
- Added metronome for OffsetsOverlay.
#### Improvements
- Clamped max height of the dropdown menu holder.
- Abstraction of "Blocker object" using a common component.
#### Changes
- Changed most "value change events" to using Bindable values instead for consistency.
- Moved FontManager away to the framework's context.
- Made Metronome class no longer strongly dependent on IAudioController and IMapSelection.
#### Fixes
- Fixed GameConfiguration failing when not initialized within test runtime environment.
- Fixed issue where IRoot dependency is not cached.

### 0.9.1.1
#### Fixes
- Fixed Game pause state not resetting on disposal.
- Fixed skip button not resetting size after using it once.

### 0.9.1
#### New features
- Implementation of Beats Standard game mode. (Not fully complete, but playable.)
- Offset settings overlay
#### Improvements
- When selecting a mapset, select the first map that appears when sorted for the current game mode, and prioritize the maps that support the mode.
#### Changes
- Ensured a minimum "protected" visibility for UguiObject hierarchy objects' Update method.
#### Fixes
- Fixed ratio issue in PrepareScreen where information panel doesn't scroll up more when using a tall aspect ratio.
- Fixed settings not visually updating when the value has changed without interaction of UI.
- Fixed dragger start circle showing visual feedback on press/release even when no judgement was made yet.
- Prevent pause again if already paused.
- Prevent new input when paused.
#### Others
- Cleaned up README.