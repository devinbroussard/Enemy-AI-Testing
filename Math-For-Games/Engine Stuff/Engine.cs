﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using Math_Library;
using Raylib_cs;

namespace Math_For_Games
{
    class Engine
    {
        private static bool _applicationShouldClose = false;
        private static int _currentSceneIndex;
        private Scene[] _scenes = new Scene[0];
        private Stopwatch _stopwatch = new Stopwatch();
        public static Scene CurrentScene;

        /// <summary>
        /// Called to begin the application
        /// </summary>
        public void Run()
        {
            //Call start for the entire application
            Start();

            float currentTime = 0;
            float lastTime = 0;
            float deltaTime = 0;

            //Loops until the application is told to close
            while (!_applicationShouldClose && !Raylib.WindowShouldClose())
            {
                //Get how much time has passed since the application started
                currentTime = _stopwatch.ElapsedMilliseconds / 1000.0f;

                //Set delta time to tbe the difference in time from the last time recorded to the current time
                deltaTime = currentTime - lastTime;

                //Update the application
                Update(deltaTime);
                //Draw all items
                Draw();

                //Set the last time recorded to be the current time
                lastTime = currentTime;
            }

            //Called when the application closes
            End();
        }

        /// <summary>
        /// Called when the application starts
        /// </summary>
        private void Start()
        {
            _stopwatch.Start();

            //Create a window using rayLib
            Raylib.InitWindow(800, 450, "Math For Games");
            Raylib.SetTargetFPS(60);

            Scene scene = new Scene();

            Player player = new Player(800, 50, 200, 3, 0.5f);
            player.SetScale(30, 30);

            Enemy enemy1 = new Enemy(150, 150, 75, 3, player, 340, new Vector2(-1, 0), 1);
            enemy1.SetScale(30, 30);
            Enemy enemy2 = new Enemy(350, 200, 75, 3, player, 340, new Vector2(1, 0), 1);
            enemy2.SetScale(30, 30);
            Enemy enemy3 = new Enemy(50, 250, 75, 3, player, 340, new Vector2(0, -1), 1);
            enemy3.SetScale(30, 30);
            Enemy enemy4 = new Enemy(450, 250, 75, 3, player, 340, new Vector2(-1, 0), 1);
            enemy4.SetScale(30, 30);
            Enemy enemy5 = new Enemy(450, 10, 75, 3, player, 340, new Vector2(-1, 0), 1);
            enemy5.SetScale(30, 30);



            CircleCollider playerCollider = new CircleCollider(30, player);
            //AABBCollider playerCollider = new AABBCollider(50, 50, player);
            player.Collider = playerCollider;

            AABBCollider enemy1Collider = new AABBCollider(30, 30, enemy1);
            AABBCollider enemy2Collider = new AABBCollider(30, 30, enemy2);
            AABBCollider enemy3Collider = new AABBCollider(30, 30, enemy3);
            AABBCollider enemy4Collider = new AABBCollider(30, 30, enemy4);
            AABBCollider enemy5Collider = new AABBCollider(30, 30, enemy5);

            enemy1.Collider = enemy1Collider;
            enemy2.Collider = enemy2Collider;
            enemy3.Collider = enemy3Collider;
            enemy4.Collider = enemy4Collider;
            enemy5.Collider = enemy5Collider;



            HealthCounter playerHealthCounter = new HealthCounter(player.Position.X, player.Position.Y, "Player Health Tracker", Color.SKYBLUE, player);
            HealthCounter enemy1HealthCounter = new HealthCounter(enemy1.Position.X, enemy1.Position.Y, "Enemy1 Health Tracker", Color.MAROON, enemy1);
            HealthCounter enemy2HealthCounter = new HealthCounter(enemy2.Position.X, enemy2.Position.Y, "Enemy2 Health Tracker", Color.MAROON, enemy2);
            HealthCounter enemy3HealthCounter = new HealthCounter(enemy3.Position.X, enemy3.Position.Y, "Enemy3 Health Tracker", Color.MAROON, enemy3);
            HealthCounter enemy4HealthCounter = new HealthCounter(enemy4.Position.X, enemy4.Position.Y, "Enemy4 Health Tracker", Color.MAROON, enemy4);
            HealthCounter enemy5HealthCounter = new HealthCounter(enemy5.Position.X, enemy5.Position.Y, "Enemy5 Health Tracker", Color.MAROON, enemy5);

            scene.AddActor(player);
            scene.AddActor(enemy1);
            scene.AddActor(enemy2);
            scene.AddActor(enemy3);
            scene.AddActor(enemy4);
            scene.AddActor(enemy5);

            scene.AddUIElement(playerHealthCounter);
            scene.AddUIElement(enemy1HealthCounter);
            scene.AddUIElement(enemy2HealthCounter);
            scene.AddUIElement(enemy3HealthCounter);
            scene.AddUIElement(enemy4HealthCounter);
            scene.AddUIElement(enemy5HealthCounter);

            _currentSceneIndex = AddScene(scene);
            CurrentScene = _scenes[_currentSceneIndex];
            _scenes[_currentSceneIndex].Start();

        }

        /// <summary>
        /// Called everytime the game loops
        /// </summary>
        private void Update(float deltaTime)
        {
            _scenes[_currentSceneIndex].Update(deltaTime);
            _scenes[_currentSceneIndex].UpdateUI(deltaTime);

            while (Console.KeyAvailable)
                Console.ReadKey(true);
        }

        /// <summary>
        /// Called every time the game loops to update visuals
        /// </summary>
        private void Draw()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.BLACK);

            //Adds all actor icons to buffer
            _scenes[_currentSceneIndex].Draw();
            _scenes[_currentSceneIndex].DrawUI();

            Raylib.EndDrawing();
        }

        /// <summary>
        /// Called when the application exits
        /// </summary>
        private void End()
        {
            _scenes[_currentSceneIndex].End();
            Raylib.CloseWindow();
        }

        /// <summary>
        /// Adds a scene to the engine's scene array
        /// </summary>
        /// <param name="scene">The scene that will be added to the scene array</param>
        /// <returns>The index where the new scene is located</returns>
        public int AddScene(Scene scene)
        {
            //Create a new temporary array
            Scene[] tempArray = new Scene[_scenes.Length + 1];

            //Copy all the values from the old array into the new array
            for (int i = 0; i < _scenes.Length; i++)
                tempArray[i] = _scenes[i];

            //Set the last index to be the new scene
            tempArray[_scenes.Length] = scene;

            //Set the old array to be the new array
            _scenes = tempArray;

            //Return the last index
            return _scenes.Length - 1;
        }

        /// <summary>
        /// Gets the next key in the input stream
        /// </summary>
        /// <returns>The key that was pressed</returns>
        public static ConsoleKey GetNextKey()
        {
            //If there is no key being pressed...
            if (!Console.KeyAvailable)
                //...return
                return 0;

            //Return the current key being pressed
            return Console.ReadKey(true).Key;
        }

        /// <summary>
        /// A function that can be used globally to end the application
        /// </summary>
        public static void CloseApplication()
        {
            _applicationShouldClose = true;
        }
    }
}
