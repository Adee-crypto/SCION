# Project Starseed Pilgrim

A 2D atmospheric platformer built with **C#** and the **MonoGame** framework. Inspired by the growth mechanics of *Starseed Pilgrim*, this project features a world where the terrain is alive, growing, and reacting to the player.

---

## 🌌 Overview
Project Starseed is a platformer where the environment is built through organic growth. Rather than navigating a static map, players interact with different plant species that spread across the screen using a cellular expansion algorithm. This project serves as a foundation for a game where "gardening" is the primary way to navigate and solve puzzles.

## 🛠️ Key Features
* **Cellular Growth Engine:** A custom `Plant` system where cells (buds and stems) expand based on randomized neighbor logic.
* **State-Driven Animation:** A robust `LinkSprite` class that manages 8 distinct animation states (Running, Falling, Attacking, etc.) with precise frame-timing.
* **Physics-Based Platforming:** Custom AABB collision detection handling gravity, ground detection, and momentum.
* **Dynamic UI System:** A scalable `PauseMenu` featuring interactive, hover-aware buttons and a screen-dimming overlay.
* **Command Pattern Controllers:** Separated Keyboard and Mouse logic that distinguishes between "held" and "tapped" inputs for a responsive feel.



## 🎮 Controls
| Action | Key / Input |
| :--- | :--- |
| **Move Left / Right** | `A` / `D` or `Left` / `Right` Arrow |
| **Jump** | `W` or `Up` Arrow |
| **Attack** | `Z` or `N` |
| **Pause Game** | `Esc` |
| **Menu Interaction** | `Mouse Hover` & `Left Click` |
| **Restart Level** | `R` |
| **Debug: Force Growth** | `1` (Hold) |
| **Debug: Cycle Plant Type** | `2` |

## 🏗️ Technical Architecture
The project is built with clean code principles and clear separation of concerns:

### 1. The Core Loop (`Game1.cs`)
Coordinates the update/draw calls between the player, the plants, and the UI. It manages the global game state (Paused vs. Playing).

### 2. Animation Engine (`LinkSprite.cs`)
Unlike simple static images, the animation system uses an `enum` to switch between frame sets stored in `LinkUtil`. It calculates elapsed time to ensure animations play at the correct speed regardless of frame rate.

### 3. Physics & Collisions (`Collisions.cs`)
The collision system is "predictive." It checks where the player *will* be in the next frame and adjusts the position to prevent overlapping with platforms or plants.

### 4. UI Framework (`Button.cs` & `PauseMenu.cs`)
A custom UI layer that doesn't rely on external libraries. It uses a 1x1 white texture scaled to the viewport to create the pause overlay and manages button bounds for mouse interaction.



## 🚀 Getting Started
1.  **Prerequisites:** Install the [MonoGame Framework](https://www.monogame.net/).
2.  **Clone the Repository:**
    ```bash
    git clone 
    ```
3.  **Open & Build:** Open the project in Visual Studio, restore NuGet packages, and press **F5** to run.

---

## 📝 Development Status
**Current Version:** Sprint 2 (Prototyping)
* ✅ Basic Player Movement & Physics
* ✅ Animation State Machine
* ✅ Multi-species Plant Growth Logic
* ✅ Functional Pause & Reset UI
* 🔄 *In Progress: Block breaking and seed-planting mechanics.*

---

*Developed using C# and MonoGame.*# CSE3902Sprint2