﻿using System;
using System.Collections.Generic;
using LDtk;
using LDtkTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SWEN_Game
{
    // Globals because this HAS to be accessible for ALL the classes
    public static class Globals
    {
        public static float Time { get; set; }
        // Access everything you want e.g. 
        public static ContentManager Content { get; set; }
        public static GraphicsDeviceManager Graphics { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }
        public static Point WindowSize { get; set; }
        public static LDtkFile File { get; set; }
        public static LDtkWorld World { get; set; }
        public static List<Rectangle> Collisions { get; set; }

        public static void UpdateTime(GameTime gameTime)
        {
            Time = (float)gameTime.ElapsedGameTime.TotalSeconds;        
        }

        public static void calculateAllCollisions()
        {
            var level0 = World.Levels[0];
            var collisionLayer = level0.LayerInstances[0];

            if (collisionLayer != null)
            {
                int gridSize = collisionLayer._GridSize;
                // Number of Cells in a row
                int gridCellWidth = collisionLayer._CWid;
                int cells = 0;
                foreach (var element in collisionLayer.IntGridCsv)
                {
                    if (collisionLayer.IntGridCsv[cells] == 1)
                    {
                        // e.g. x = 52 % 75 -> 52 * 16
                        // e.g. y = 6 / 75 -> 6 * 16 
                        // --> loops around cuz math
                        int x = (cells % gridCellWidth) * gridSize + level0.WorldX; //<-- WorldX/Y needed for offset
                        int y = (cells / gridCellWidth) * gridSize + level0.WorldY;
                        // Add to Global Collisions List
                        Collisions.Add(new Rectangle(x, y, gridSize, gridSize));
                    }
                    cells++;
                }
            }
        }
        public static bool isColliding(Vector2 pos, Texture2D texture)
        {
            // Assumes entity collision as small rectangle at the very bottom of the Sprite
            Rectangle entityRect = new Rectangle((int)pos.X + texture.Width / 2 - 2,
                (int)pos.Y + texture.Height - 3, texture.Width / 4, texture.Height / 15);
            foreach (var rect in Collisions)
            {
                if (entityRect.Intersects(rect))
                {
                    return true;
                }
            }
            return false;
        }
    }
}


