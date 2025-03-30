using System.Collections.Generic;
using LDtk;
using LDtk.Renderer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SWEN_Game
{
    public class Renderer
    {
        private static readonly float DepthRadius = 100f;

        private Player _player;
        private SpriteManager _spriteManager;

        public Renderer(Player player, SpriteManager spriteManager)
        {
            _player = player;
            _spriteManager = spriteManager;
        }

        /// <summary>
        /// Draws the entire game world including background tiles, sprite groups, the player, and collision boxes.
        /// </summary>
        /// <remarks>
        /// This method precomputes anchor depths for sprite groups near the player, applies the camera transformation,
        /// and renders background layers, grouped tiles with shared depth based on their anchor tile, as well as other game entities.
        /// </remarks>
        public void DrawWorld()
        {
            // Begin the sprite batch with depth sorting (FrontToBack) and apply the camera transformation.
            Globals.SpriteBatch.Begin(
                SpriteSortMode.FrontToBack,
                transformMatrix: CalcTranslation(),
                samplerState: SamplerState.PointClamp);

            // Precompute anchor depths for sprite groups within a specific radius around the player.
            // This groups tiles by their EnumTag and uses the nearest anchor tile's Y value for consistent depth.
            var anchorDepths = SpriteGroupAnchorCalculation(DepthRadius);

            // Get the current level and mapping between EnumTags and tile IDs.
            var level = Globals.World.Levels[0];
            var tileMappings = _spriteManager.GetTileMappings();

            // Process each layer in the level.
            foreach (var layer in level.LayerInstances)
            {
                bool isBackground = layer._Identifier == "Background";

                // Skip layers without an associated tileset texture.
                if (layer._TilesetRelPath == null)
                {
                    continue;
                }

                // Retrieve the texture for this tileset.
                Texture2D tilesetTexture = GetTilesetTextureFromRenderer(level, layer._TilesetRelPath);

                // Process each tile in the current layer.
                foreach (var tile in layer.GridTiles)
                {
                    // Calculate the world position of the tile by applying layer offsets.
                    Vector2 position = new(tile.Px.X + layer._PxTotalOffsetX, tile.Px.Y + layer._PxTotalOffsetY);
                    Rectangle srcRect = new(tile.Src.X, tile.Src.Y, layer._GridSize, layer._GridSize);

                    // For background layers, draw with a forced depth of 0 (ensuring they render behind all other tiles).
                    if (isBackground)
                    {
                        _spriteManager.DrawTile(Globals.SpriteBatch, tilesetTexture, srcRect, position, 0f);
                        continue;
                    }

                    // Determine if the tile belongs to a sprite group by checking its tile ID in the mappings.
                    string foundEnumTag = null;
                    foreach (var (enumTag, ids) in tileMappings)
                    {
                        if (ids.Contains(tile.T))
                        {
                            foundEnumTag = enumTag;
                            break;
                        }
                    }

                    // If the tile is part of a sprite group and an anchor depth has been computed for that group,
                    // use the group's anchor depth for all its tiles. Otherwise, calculate depth per tile normally.
                    if (!string.IsNullOrEmpty(foundEnumTag) &&
                        anchorDepths.TryGetValue(foundEnumTag, out float anchorDepth))
                    {
                        _spriteManager.DrawTile(Globals.SpriteBatch, tilesetTexture, srcRect, position, anchorDepth);
                    }
                    else
                    {
                        _spriteManager.DrawTile(Globals.SpriteBatch, tilesetTexture, srcRect, position, layer);
                    }
                }
            }

            _player.Draw();

            // DrawPlayerDebug();

            // End the sprite batch.
            Globals.SpriteBatch.End();
        }

        public void DrawPlayerDebug()
        {
            // Draw the player sprite using its calculated depth.
            // _spriteManager.DrawPlayer(Globals.SpriteBatch, _player.texture, _player.position);

            // Draw the player's collision box for debugging, using a pink overlay.
            Rectangle entityRect = new Rectangle(
              (int)_player.Position.X + 5,
              (int)_player.Position.Y + 10,
              _player.Texture.Width / 16,
              _player.Texture.Height / 36);
            Globals.SpriteBatch.Draw(
                Globals.Content.Load<Texture2D>("hm_1"),
                entityRect,
                null,
                Color.Pink,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                1f);

            // Draw any collision areas in red.
            foreach (var collision in Globals.Collisions)
            {
                Globals.SpriteBatch.Draw(
                    Globals.Content.Load<Texture2D>("hm_1"),
                    collision,
                    null,
                    Color.Red,
                    0f,
                    new Vector2(0, 0),
                    SpriteEffects.None,
                    1f);
            }

            // Draw Player Position/Rectangle
            Rectangle posRect = new Rectangle(
                (int)_player.RealPos.X,
                (int)_player.RealPos.Y,
                _player.Texture.Width / 32,
                _player.Texture.Height / 72);
            Globals.SpriteBatch.Draw(
                Globals.Content.Load<Texture2D>("hm_1"),
                posRect,
                null,
                Color.Blue,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                1f);
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
        private Dictionary<string, float> SpriteGroupAnchorCalculation(float radius)
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

        /// <summary>
        /// Helper method to retrieve the tileset texture using ExampleRenderer logic.
        /// </summary>
        /// <param name="level">Level to load Textures from.</param>
        /// <param name="tilesetPath">Path of desired texture (Usually a layer).</param>
        /// <returns>Texture Sheet.</returns>
        private Texture2D GetTilesetTextureFromRenderer(LDtkLevel level, string tilesetPath)
        {
            if (Globals.Content == null)
            {
                string directory = System.IO.Path.GetDirectoryName(level.WorldFilePath) !;
                string assetName = System.IO.Path.Join(directory, tilesetPath);
                return Texture2D.FromFile(Globals.Graphics.GraphicsDevice, assetName);
            }
            else
            {
                string file = System.IO.Path.ChangeExtension(tilesetPath, null);
                string directory = System.IO.Path.GetDirectoryName(level.WorldFilePath) !;
                string assetName = System.IO.Path.Join(directory, file);
                return Globals.Content.Load<Texture2D>(assetName);
            }
        }

        private Matrix CalcTranslation()
        {
            return Matrix.CreateTranslation(-_player.Position.X, -_player.Position.Y, 0) *
                   Matrix.CreateScale(Globals.Zoom, Globals.Zoom, 1f) *
                   Matrix.CreateTranslation(
                       Globals.Graphics.PreferredBackBufferWidth / 2f,
                       Globals.Graphics.PreferredBackBufferHeight / 2f,
                       0);
        }
    }
}