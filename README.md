# Sandbox Tour

*Sandbox Tour* is a RPG game centered around a little girl's journey through her own inner world. Players step into the role of a young girl who enters a sandbox world she built herself, discovering hidden facets of her psyche through exploration, dialogue, and symbolic battles against negative emotions.

![Platform](https://img.shields.io/badge/Platform-PC-blue) ![Engine](https://img.shields.io/badge/Engine-Unity_C%23-green)

---

## Table of Contents

- [Game Overview](#game-overview)
- [Gameplay](#gameplay)
- [Boss Design Philosophy](#boss-design-philosophy)
- [Core Systems Architecture](#core-systems-architecture)
- [Technical Highlights](#technical-highlights)
- [How to Run](#how-to-run)

---

## Game Overview

*Sandbox Tour* tells the story of a little girl exploring a sandbox world of her own making. This world should feel incredibly familiar, but hidden within are details she never imagined. Through interactions with characters and confrontations with negative emotions, she gradually explores her inner world.

- **Perspective**: Top-down 2.5D exploration + Arkanoid-style boss battles
- **Art style**: Cute mascot-like characters, soft flat shading
- **Core themes**: Growth, self-awareness, emotional management

---

## Gameplay

The game features two primary modes:

### 1. Free Exploration

- **WASD / Arrow keys** – move the character
- **I** – interact with objects (dialogue, save points, teleporters, etc.)
- **Q / E** – rotate the camera; obstructing objects become semi-transparent
- **Esc** – toggle the tutorial panel

### 2. Boss Battle Mode (Arkanoid + Bullet Hell)

- **Arrow keys** – move the paddle left/right
- **Space** – launch the ball to break bricks
- The swirling circular pattern at the center of the paddle represents the player's "psychological weak point" – avoid enemy bullets hitting it
- Normal bricks symbolize actions that consume varying degrees of energy; gold bricks represent negative behaviors that should not be chosen
- Each boss has unique brick and bullet symbolism (see below)

### Objective

- Explore, fulfill conditions (condition system)
- Defeat four emotional bosses (Gray Emptiness, Blue Regret, Brown Stubbornness, Purple Fear)
- Use the save system to record progress and seamlessly return to previous scenes

---

## Boss Design Philosophy

Each boss embodies a specific negative emotion, with bricks and bullets carrying symbolic meaning.

| Boss | Visual Form | Arena Shape | Brick Symbolism | Bullet Symbolism | Theme |
|------|-------------|-------------|-----------------|------------------|-------|
| **Gray Emptiness** | Gray ghost | Oval (vast empty space for daydreaming) | Sleep (hit once) vs. Tasks (hit twice) | Intrusive thoughts of inner turmoil | Abandon idle fantasies; rest or complete tasks in an orderly manner |
| **Blue Regret** | Blue poison dart frog | Slanted heart (wavering, sensitive heart) | Share joy (hit once) vs. Share sorrow (hit twice) | Others' sharp words | Actively share inner thoughts; break away from a silent atmosphere |
| **Brown Stubbornness** | Brown cow | Hourglass (ideas that solidify over time) | Effort (hit once) vs. Progress (hit twice) | The gaze of others | Work hard with both feet on the ground; let go of concern for others' attention |
| **Purple Fear** | Purple jellyfish | Square (structured daily schedule) | Complete tasks (hit once) vs. Delay tasks (hit twice) | Approaching pressure of time | Adjust your schedule proactively; don't easily abandon plans; embrace an uncertain future |

> The fluffy, harmless appearance of each mascot-like creature conveys the message that these emotions *can* be overcome.

---

## Core Systems Architecture

### Condition & Progress Management

- Centralized system using string IDs to gate progression
- `ConditionManager` is a singleton that stores completed conditions
- Updated via ScriptableObject event channels; other scripts query it to check accessibility
- Unmet conditions trigger a hint panel listing missing requirements

### Scene Management & Save/Load

- Asynchronous scene transitions using Unity's Addressable system through a dedicated event channel
- `SceneLoader` manages loading/unloading and supports returning to the previous scene (e.g., after boss battles)
- Save points write player position, scene name, and conditions to `PlayerPrefs`, allowing exact resume from the main menu

### Interaction & UI Systems

- All interactive objects implement the `IInteractable` interface with a single `TriggerAction()` method
- `Sign` script detects nearby interactables and prompts the player to act
- Dialog system types text character-by-character and can trigger condition events when finished
- Prologue sequences play before boss fights
- Other UI: tutorial panel, save confirmation, health/ball counters

### Character Control & Camera

- Rigidbody-based top-down movement with Animator-driven animations
- Camera smoothly follows the player, supports Q/E rotation, and handles occlusion by making blocking objects semi-transparent

### Event Framework

- ScriptableObject event channels for loose coupling:
  - `VoidEventSO` – no-parameter events
  - `SceneLoadEventSO` – scene load requests
  - `ReturnToPrevSceneEventSO` – return to previous scene
  - `ConditionEventSO` – condition change notifications
- A simple enum `SceneType` distinguishes scene types (`Location`, `Menu`), and each `GameSceneSO` asset holds its Addressable reference and type.

---

## Technical Highlights

- **Addressable asynchronous scene loading** – smooth transitions without blocking the main thread
- **ScriptableObject event system** – fully decoupled modules, easy to extend
- **Rigidbody physics** – stable player movement and bullet mechanics
- **Complete save system** – persists position, conditions, and scene name
- **Symbolic boss design** – every boss carries deep psychological metaphors; game mechanics are tightly integrated with narrative

---

## How to Run

1. Open the project in **Unity 2022.3.34** or newer
2. Wait for Addressables to finish loading
3. Open the scene `Assets/Scenes/Persistent` and press Play
4. From the main menu, choose **New Game** or **Continue** (if a save exists)

### Tested Environment

- Platform: PC (Windows / Mac)
- Input: Keyboard (recommended)
- Resolution: Any, 1840x1380 recommended
