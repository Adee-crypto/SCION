# SCION

Will growth outpace decay?

## Overview
SCION is a platformer where the environment is constantly built through organic growth and eroded by the Void. Rather than navigating a static map, players interact with different plant species that spread across the screen in ways unique to their species and biomes. The goal is to halt the world's consumption from an unknown enemy.

## Key Features
* **Cellular Growth Engine:** A custom `Plant` system where cells (buds and stems) expand based on randomized neighbor logic.
* **State-Driven Animation:** A robust `PlayerSprite` class that manages 8 distinct animation states (Running, Falling, Attacking, etc.) with precise frame-timing.
* **Physics-Based Platforming:** Custom AABB collision detection handling gravity, ground detection, and momentum.
* **Dynamic UI System:** A scalable `PauseMenu` featuring interactive, hover-aware buttons and a screen-dimming overlay.
* **Command Pattern Controllers:** Separated Keyboard and Mouse logic that distinguishes "held" and "tapped" inputs.



## Controls
| Action | Key / Input |
| :--- | :--- |
| **Move Left / Right** | `A` / `D` or `Left` / `Right` Arrow |
| **Jump** | `W` or `Up` Arrow |
| **Dig** | `S` or `Down` Arrow |
| **Shoot seed** | `Left Click` |
| **Pause Game** | `Esc` |
| **Restart Level** | `R` |
| **Quit Game** | `Q` |
| **Menu Navigation** | `Mouse Hover` & `Left Click` |
| **(Un)Mute Music** | `M` |
| **Debug: Story Level Movements** | `IJKL` (Tap) |
| **Debug: Damage Player** | `E` (Tap) |
| **(deprecated) Debug: Display Attack State** | `Z` or `N` |


## How to Install
1.  **Prerequisites:** Install the [MonoGame Framework](https://www.monogame.net/).
2.  **Clone the Repository:**
    ```bash
    git clone 
    ```
3.  **Open & Build:** Open the project in Visual Studio, restore NuGet packages, and press **F5** to run.

---
## Development Status
**Current Version:** Sprint 3
* ✅ Player controls
* ✅ Enemy AI
* ✅ Newtonian momentum-based collisions
* ✅ Enemy projectiles damage player
* ✅ Player projectiles plant plants
* ✅ Animation state machines
* ✅ Various randomized plant species
* ✅ UI Buttons: Pause, Reset, Game mode, settings (WIP), save & load (WIP)
* ✅ Randomly generated level biomes
* ✅ Dynamic Consonant and Dissonant SFX
* ✅ Various static physics (sticky, slippery, and bouncy surfaces)
* ✅ Massive csv reading
* ✅ Break ein block
* ✅ Player projectiles damage enemies
* 🔄 *In Progress: More species.*
* 🔄 *In Progress: Customize projectile physics (different weights, air resistances).*
* 🔄 *In Progress: Arcade mode (+ camera movement).*
Misc development notes:
* Instead of using specific task-tracking software, due to our small team, we use a Google Doc and strikethrough tasks as we complete them. We communicate heavily over Discord so we don't get in each other's way and can take on any role when someone's stuck
*Developed using C# and MonoGame.*
Inspired by *Starseed Pilgrim*
