# project-beats-game
Implementation of Project: Beats game
  
## This project is a huge WIP.
Currently, I'm restructuring my original source to make things more organized and scalable.  
Slowly making progress 🧩

## Dependencies
- project-beats-framework (1.0.2) (https://github.com/jerryrox/project-beats-framework)
- pbeffect-coffee (https://github.com/jerryrox/pbeffect-coffee)
- Newtonsoft.Json (Tested with net45 version)

## Development progress
[https://trello.com/b/5gpuJrRa/project-beats-renewed]

## Versions
### 0.9.2 (WIP)
#### New features
- Added button hover sound toggle configuration.
- Added scrolling capability for dropdown menu popup.
#### Improvements
- Clamped max height of the dropdown menu holder.
- Abstraction of "Blocker object" using a common component.
#### Changes
- Changed most "value change events" to using Bindable values instead for consistency.
- Moved FontManager away to the framework's context.
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