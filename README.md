Endless Runner Mobile

A casual 3-lane endless runner built for Android using Unity 6. Navigate through obstacles, collect rewards, and see how long you can survive as the speed builds up!

🎮 Features

Intuitive Mobile Controls: Simple swipe or tap mechanics to switch between three lanes.

Dynamic Difficulty: Game speed and obstacle density increase over time to challenge player reflexes.

Score & Rewards: Collectible items to boost your high score.

Performance Optimized: Designed for mobile hardware with efficient memory management.

🕹️ How to Play

Start: Tap the "Play" button on the main menu.

Move: Swipe Left/Right (or use A/D keys in the editor) to switch lanes.

Objective: Avoid incoming obstacles while collecting gold coins.

Game Over: The run ends if you collide with an obstacle. Try to beat your high score!

🛠️ Technical Details

Engine: Unity 6 (6000.0.55f1)

Language: C#

Platform: Android

Graphics API: Universal Render Pipeline (URP)

Version Control: Git LFS (Large File Storage) for textures and audio assets.

🚀 Getting Started

Prerequisites

Unity Hub installed.

Unity 6 (6000.0.55f1) Editor version.

Android Build Support module.

Git LFS installed on your machine.

Installation

Clone the Repo:

git clone [https://github.com/YourUsername/EndlessRunner.git](https://github.com/YourUsername/EndlessRunner.git)


Pull LFS Assets:

git lfs install
git lfs pull


Open in Unity: Add the project folder to Unity Hub and ensure the correct editor version is selected.

🏗️ Architecture & Optimization

To maintain a high frame rate on mobile devices, this project utilizes several optimization patterns:

Object Pooling: Instead of frequently calling Instantiate() and Destroy() for road segments and obstacles, objects are recycled from a pre-allocated pool to prevent frame stutters.

Shader Optimization: Uses URP with mobile-optimized Lit/Unlit shaders to minimize GPU overhead.

Inversion of Control: Implementation of a Service Locator pattern, with strict limitations on Singleton usage.

📜 Credits & Assets

Code: Developed by Craig Lacey

Assets: 

Donut Set https://assetstore.unity.com/packages/3d/props/food/donut-set-282633

LowPolyMegapolis https://assetstore.unity.com/packages/3d/environments/urban/low-poly-megapolis-195499

Farland Skies https://assetstore.unity.com/packages/2d/textures-materials/sky/farland-skies-low-poly-64604

📄 License

This project is licensed under the MIT License - see the LICENSE file for details.
