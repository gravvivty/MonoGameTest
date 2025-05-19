using System;
using System.Collections.Generic;
using LDtk;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SWEN_Game
{
    public class Renderer
    {
        private static readonly float DepthRadius = 500f;
        private Player _player;
        private SpriteManager _spriteManager;
        private SpriteCalculator _spriteCalculator;

        public Renderer(Player player, SpriteManager spriteManager, SpriteCalculator spriteCalculator)
        {
            _player = player;
            _spriteManager = spriteManager;
            _spriteCalculator = spriteCalculator;
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
            // Precompute anchor depths for sprite groups within a specific radius around the player.
            // This groups tiles by their EnumTag and uses the nearest anchor tile's Y value for consistent depth.
            var anchorDepths = _spriteCalculator.SpriteGroupAnchorCalculation(DepthRadius);

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
                Texture2D tilesetTexture = _spriteManager.GetTilesetTextureFromRenderer(level, layer._TilesetRelPath);
                float renderRadius = 1000f; // 2000px total area (1000px in all directions)
                Vector2 playerPos = _player.RealPos;

                // Process each tile in the current layer.
                foreach (var tile in layer.GridTiles)
                {
                    // Calculate the world position of the tile by applying layer offsets.
                    Vector2 position = new(tile.Px.X + layer._PxTotalOffsetX, tile.Px.Y + layer._PxTotalOffsetY);
                    Rectangle srcRect = new(tile.Src.X, tile.Src.Y, layer._GridSize, layer._GridSize);

                    if (Vector2.DistanceSquared(position, playerPos) > renderRadius * renderRadius)
                    {
                        continue;
                    }

                    // For background layers, draw with a forced depth of 0 (ensuring they render behind all other tiles).
                    if (isBackground)
                    {
                        DrawTile(Globals.SpriteBatch, tilesetTexture, srcRect, position, layer);
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
                        DrawTile(Globals.SpriteBatch, tilesetTexture, srcRect, position, anchorDepth, layer);
                    }
                    else
                    {
                        DrawTile(Globals.SpriteBatch, tilesetTexture, srcRect, position, layer);
                    }
                }
            }

            _player.Draw();
        }

        public Matrix CalcTranslation()
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 screenCenter = new Vector2(
                Globals.Graphics.PreferredBackBufferWidth / 2f,
                Globals.Graphics.PreferredBackBufferHeight / 2f);

            // Raw mouse offset from the screen center -> cuz character is center of screen
            Vector2 rawMouseOffset = new Vector2(mouseState.X, mouseState.Y) - screenCenter;

            float maxMouseRange = Globals.WindowSize.X; // Mouse can affect camera within this range
            float maxCameraOffset = 30f; // Camera shifts within this range

            // Scales the Offset down - 0->maxMouseRange gets scaled to 0->maxCameraOffset
            // Ensures Camera smoothness
            Vector2 mouseOffset = rawMouseOffset * (maxCameraOffset / maxMouseRange);

            // Ensure the final offset never exceeds maxCameraOffset
            if (mouseOffset.Length() > maxCameraOffset)
            {
                mouseOffset.Normalize(); // Keep direction
                mouseOffset = mouseOffset * maxCameraOffset; // Clamp to maxCameraOffset
            }

            Vector2 cameraTarget = _player.RealPos + mouseOffset;
            cameraTarget = new Vector2(
                (float)Math.Floor(cameraTarget.X),
                (float)Math.Floor(cameraTarget.Y));

            return Matrix.CreateTranslation(
                -cameraTarget.X,
                -cameraTarget.Y,
                0) *
                Matrix.CreateScale(Globals.Zoom, Globals.Zoom, 1f) *
                Matrix.CreateTranslation(
                    Globals.Graphics.PreferredBackBufferWidth / 2f,
                    Globals.Graphics.PreferredBackBufferHeight / 2f,
                    0);
        }

        // Draw a tile with its depth computed from its world position
        private void DrawTile(SpriteBatch spriteBatch, Texture2D texture, Rectangle sourceRect, Vector2 position, LayerInstance layer)
        {
            float depth = _spriteManager.GetDepth(position, sourceRect.Width, layer);
            spriteBatch.Draw(texture, position, sourceRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, depth);
        }

        private void DrawTile(SpriteBatch spriteBatch, Texture2D texture, Rectangle sourceRect, Vector2 position, float anchorDepth, LayerInstance layer)
        {
            float depth = _spriteManager.GetDepth(anchorDepth, layer);
            spriteBatch.Draw(texture, position, sourceRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, depth);
        }
    }
}
