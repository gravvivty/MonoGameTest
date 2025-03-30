using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SWEN_Game
{
    public class SpriteCalculator
    {
        private readonly SpriteManager _spriteManager;
        private readonly Player _player;
        public SpriteCalculator(SpriteManager spriteManager, Player player)
        {
            _spriteManager = spriteManager;
            _player = player;
        }

        /// <summary>
        /// Calculates the rendering depth for sprite groups based on the position of their anchor tiles.
        /// </summary>
        /// <param name="radius">The radius within which to consider anchor tiles for depth calculation.</param>
        /// <returns>
        /// A dictionary mapping each sprite group identifier (EnumTag) to its computed depth value.
        /// </returns>
        /// <remarks>
        /// For each sprite group, this function identifies the designated anchor tile (using GetAnchorTileID) and selects the instance
        /// closest to the player's position. If this instance is within the specified radius, its depth (based on its Y coordinate) is used
        /// for the entire group, ensuring consistent layer ordering.
        /// </remarks>
        public Dictionary<string, float> SpriteGroupAnchorCalculation(float radius)
        {
            // Dictionary to store computed depth for each sprite group (keyed by EnumTag).
            var result = new Dictionary<string, float>();

            // Retrieve all tile groups categorized by their EnumTag.
            var tileGroups = _spriteManager.GetTileGroups();

            // Iterate over each sprite group.
            foreach (var (enumTag, group) in tileGroups)
            {
                // Get the designated anchor tile ID for this group.
                int anchorID = GetAnchorTileID(enumTag);

                // If there is no valid anchor or the group doesn't contain the anchor tile, skip this group.
                if (anchorID == 0 || !group.ContainsKey(anchorID))
                {
                    continue;
                }

                float minDist = float.MaxValue;
                Vector2 bestAnchorPos = Vector2.Zero;

                // Find the anchor tile occurrence that is closest to the player.
                foreach (var anchorPos in group[anchorID])
                {
                    float dist = Vector2.Distance(anchorPos, _player.RealPos);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        bestAnchorPos = anchorPos;
                    }

                    // Draw Anchor Tiles
                    /*Globals.SpriteBatch.Draw(_player.Texture, new Rectangle((int)anchorPos.X, (int)anchorPos.Y, 16, 16), null, Color.Blue, 0f, new Vector2(0, 0),
                SpriteEffects.None, 1f);*/
                }

                // If the closest anchor is within the specified radius, compute its depth and assign it to the sprite group.
                if (minDist <= radius)
                {
                    float anchorDepth = _spriteManager.GetDepth(bestAnchorPos, 16f);
                    result[enumTag] = anchorDepth;
                }
            }

            return result;
        }

        public int GetAnchorTileID(string enumName)
        {
            // Select the correct anchor Tile depending on Sprite
            int anchorID = 0;
            switch (enumName)
            {
                case "House":
                    anchorID = 324;
                    break;
                case "Tree_Big":
                    anchorID = 264;
                    break;
                case "Tree_Small":
                    anchorID = 237;
                    break;
                case "Lantern":
                    anchorID = 213;
                    break;
                case "Stump":
                    anchorID = 81;
                    break;
                case "Fence_Big":
                    anchorID = 10;
                    break;
                case "Log":
                    anchorID = 241;
                    break;
                case "Bridge":
                    anchorID = 3;
                    break;
                default:
                    break;

                    // Sprites that are one singular tile like most Small_Deco tiles - wont need an anchor
            }

            return anchorID;
        }
    }
}
