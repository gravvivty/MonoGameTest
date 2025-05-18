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
            // Select the correct anchor Tile depending on Sprite (bottomost tile of sprite)
            int anchorID = 0;
            switch (enumName)
            {
                case "BigCleanHouse": // FOREST
                    anchorID = 413;
                    break;
                case "BigMossHouse":
                    anchorID = 377;
                    break;
                case "ShackMoss":
                    anchorID = 92;
                    break;
                case "FrogStatue":
                    anchorID = 249;
                    break;
                case "MonkStatue":
                    anchorID = 160;
                    break;
                case "SmallHouseOrange":
                    anchorID = 85;
                    break;
                case "Cart":
                    anchorID = 330;
                    break;
                case "SmallHouseCrashed":
                    anchorID = 89;
                    break;
                case "GiantLog":
                    anchorID = 447;
                    break;
                case "SingleTree":
                    anchorID = 284;
                    break;
                case "GroupTree":
                    anchorID = 321;
                    break;
                case "Ruins":
                    anchorID = 178;
                    break;
                case "Pillar":
                    anchorID = 202;
                    break;
                case "SmallPillar":
                    anchorID = 163;
                    break;
                case "HouseRuins":
                    anchorID = 81;
                    break;
                case "Walls":
                    anchorID = 410;
                    break;
                case "Bookshelf":
                    anchorID = 458;
                    break;
                case "EmptyShelf":
                    anchorID = 459;
                    break;
                case "Drawer":
                    anchorID = 457;
                    break;
                case "RoundShelf":
                    anchorID = 456;
                    break;
                case "Pool":
                    anchorID = 348; // DESERT
                    break;
                case "SmallGreenHouseDesert":
                    anchorID = 41;
                    break;
                case "BigGreenHouseDesert":
                    anchorID = 101;
                    break;
                case "MarketStandDesert":
                    anchorID = 462;
                    break;
                case "SmallRectangleHouseDesert":
                    anchorID = 107;
                    break;
                case "BigRectangleHouseDesert":
                    anchorID = 267;
                    break;
                case "CastleTower":
                    anchorID = 395;
                    break;
                case "SingleTreeDesert":
                    anchorID = 472;
                    break;
                case "GroupTreeDesert":
                    anchorID = 391;
                    break;
                case "Watchtower":
                    anchorID = 175; // FOREST TOWER
                    break;
                case "BushDesert":
                    anchorID = 468;
                    break;
                case "LionStatue":
                    anchorID = 460;
                    break;
                default:
                    break;

                    // Sprites that are one singular tile like most Small_Deco tiles - wont need an anchor
            }

            return anchorID;
        }
    }
}
