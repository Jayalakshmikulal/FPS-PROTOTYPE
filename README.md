# FPS Prototype

A Unity first-person shooter prototype built around wave-based enemy encounters, a hitscan weapon, and a compact arena scene. The repository contains the Unity project source so it can be inspected and opened in the Unity Editor.

## Project overview

The current project is centered on `Assets/Scenes/SampleScene.unity`. It combines first-person movement and aiming, enemy waves, target practice, a basic score and health HUD, and win or game-over screens. The gameplay systems described below are based on the scripts and scene setup currently in this repository.

## Features

### Gameplay

- First-person walking, sprinting, jumping, and mouse look.
- A small arena with cover, spawn points, a crosshair, and a target dummy.
- Three enemy waves in the scene configuration, with the spawn count increasing by one each wave.
- Enemies pursue the player, stop nearby, and attack on a timed interval. An Animator can drive their movement and attack states.
- A score HUD that awards points for damaging and destroying targets.

### Weapon and damage

- Camera-based raycast shooting with configurable damage, range, and fire interval.
- A 12-round magazine and 36 reserve rounds in the weapon script defaults, with timed reload and ammo display.
- Muzzle flash, weapon kick, hit impact, and optional shot/reload audio feedback.
- Player health with a health bar, damage flash, and optional hurt audio.
- Target health with a health bar, hit flash/audio, and score awards on hits and destruction.

### Game flow and UI

- Wave, score, ammo, and player health HUD elements.
- Game-over and victory panels that show the final score.
- Restart buttons wired to reload the active scene.

## Controls

The controls below are defined by the current player scripts and Unity input settings.

| Action | Control |
| --- | --- |
| Move | `W`, `A`, `S`, `D` (arrow keys are also mapped) |
| Look | Mouse |
| Sprint | Hold `Left Shift` |
| Jump | `Space` |
| Shoot | Left mouse button (left `Ctrl` is also mapped to `Fire1`) |
| Reload | `R` |
| Damage test | `H` applies 10 damage through the player health script |

## Technology

- **Engine:** Unity `6000.4.4f1` (version recorded in `ProjectSettings/ProjectVersion.txt`)
- **Language:** C#
- **UI text:** TextMesh Pro
- **Unity packages:** The project manifest includes Input System, AI Navigation, uGUI, Timeline, and Visual Scripting packages. The player scripts currently read input through Unity's `Input` API.

## Open and run

1. Install Unity `6000.4.4f1` through Unity Hub.
2. In Unity Hub, choose **Add** or **Open**, then select this repository's root folder.
3. Open `Assets/Scenes/SampleScene.unity` if it is not already open.
4. Press **Play** in the Unity Editor.

Unity will create its local import/cache files when the project is opened. Those generated files are excluded by the repository's `.gitignore`.

## Repository layout

```text
Assets/
├── New Folder/              # Prototype scripts, models, materials, audio, and prefabs
├── Scenes/                  # SampleScene and its lighting/navigation data
└── TextMesh Pro/            # TextMesh Pro resources and bundled Examples & Extras
Packages/                    # Unity package manifest and lock file
ProjectSettings/             # Unity editor and project configuration
```

Key gameplay scripts are in `Assets/New Folder/Scripts/`, including `PlayerMovement`, `PlayerMouseLook`, `PlayerWeapon`, `PlayerHealth`, `EnemyAI`, `EnemySpawner`, `TargetHealth`, and `GameManager`.

## Screenshots and gameplay

Screenshots and a gameplay recording have not been added yet. Add genuine captures from the running project here when available; no preview media is included until then.

## Planned improvements

These are possible future directions, not features currently claimed as complete:

- Add representative in-game screenshots and a short gameplay recording.
- Add a packaged playable build with platform and setup notes.
- Expand the gameplay and enemy variety.

## Developer

**Jayalakshmi** · [GitHub profile](https://github.com/Jayalakshmikulal)

Portfolio and LinkedIn links were not present in the repository, so they are not listed here.

## Licensing

There is no project-level license in this repository. The bundled Unity and TextMesh Pro content may have separate terms; review its included notices before redistribution. Contact the developer for permission to reuse project-specific work.

## Thanks for visiting

Thanks for taking a look at this FPS prototype. The repository is intended to make the current Unity project and its implementation easy to explore.
