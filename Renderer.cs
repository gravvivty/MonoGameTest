using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using LDtk;
using LDtk.Renderer;
using LDtkTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Enum = LDtkTypes.Enum;

namespace SWEN_Game
{
    public class Renderer
    {
        // Default Renderer from NuGet
        //private ExampleRenderer _renderer;
        private Player _player;
        private SpriteManager _spriteManager;
        private GraphicsDevice _graphicsDevice;
        /// <summary> Gets or sets the levels identifier to layers Dictionary. </summary>
        Dictionary<string, RenderedLevel> PrerenderedLevels { get; } = [];

        /// <summary> Gets or sets the levels identifier to layers Dictionary. </summary>
        Dictionary<string, Texture2D> TilemapCache { get; } = [];

        readonly Texture2D pixel;
        readonly Texture2D error;
        public float zoom { get; private set; } = 4f;

        public Renderer(Player player, SpriteManager spriteManager)
        {
            _player = player;
            _spriteManager = spriteManager;
            //_renderer = new ExampleRenderer(Globals.SpriteBatch, Globals.Content);
            _graphicsDevice = Globals.SpriteBatch.GraphicsDevice;
            RenderInit();
        }

        public void drawWorld()
        {
            // Apply Matrix for correct Zoom, Recentering and moving world around player
            Globals.SpriteBatch.Begin(transformMatrix: calcTranslation(), samplerState: SamplerState.PointClamp);

            var level = Globals.World.Levels[0];
            // Draw prerendered Level
            RenderPrerenderedLevel(level);
            // Draw Player
            Globals.SpriteBatch.Draw(_player.texture, _player.position, Color.White);

            // Visual Rectangle Player Collision -> copied this from Globals.isColliding for consistency
            Rectangle entityRect = new Rectangle((int)_player.position.X + _player.texture.Width / 2 - 2, 
                (int)_player.position.Y + _player.texture.Height-3, _player.texture.Width / 4, _player.texture.Height / 15);

            Globals.SpriteBatch.Draw(_player.texture, entityRect, Color.Green);

            // Draws all the collisions to the screen
            foreach (var rect in Globals.Collisions)
            {
                Globals.SpriteBatch.Draw(_player.texture, rect, Color.Red);
            }

            Globals.SpriteBatch.End();
        }

        public void SetZoom(float newZoom)
        {
            zoom = newZoom;
        }
        private Matrix calcTranslation()
        {
            // Moves world * creates zoom * centers player
            Matrix translation;
            translation = Matrix.CreateTranslation(-_player.position.X, -_player.position.Y, 0) *
                Matrix.CreateScale(this.zoom, this.zoom, 1f) *
                Matrix.CreateTranslation(Globals.Graphics.PreferredBackBufferWidth / 2f - _player.texture.Width*3,
                Globals.Graphics.PreferredBackBufferHeight / 2f - _player.texture.Height, 0);
            return translation;
        }

        private void RenderInit()
        {
            foreach (LDtkLevel level in Globals.World.Levels)
            {
                // Prerender the Level whatever that means xdd
                PrerenderLevel(level);
            }
        }

        /// <summary> Prerender out the level to textures to optimize the rendering process. </summary>
        /// <param name="level">The level to prerender.</param>
        /// <returns>The prerendered level.</returns>
        /// <exception cref="Exception">The level already has been prerendered.</exception>
        public RenderedLevel PrerenderLevel(LDtkLevel level)
        {
            if (PrerenderedLevels.TryGetValue(level.Identifier, out RenderedLevel cachedLevel))
            {
                return cachedLevel;
            }

            RenderedLevel renderLevel = new();

            Globals.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            {
                renderLevel.Layers = RenderLayers(level);
            }
            Globals.SpriteBatch.End();

            PrerenderedLevels.Add(level.Identifier, renderLevel);
            _graphicsDevice.SetRenderTarget(null);

            return renderLevel;
        }

        Texture2D[] RenderLayers(LDtkLevel level)
        {
            List<Texture2D> layers = [];

            // Usually this is null
            if (level.BgRelPath != null) 
            {
                layers.Add(RenderBackgroundToLayer(level));
            }

            if (level.LayerInstances == null)
            {
                if (level.ExternalRelPath != null)
                {
                    throw new LDtkException("Level has not been loaded.");
                }
                else
                {
                    throw new LDtkException("Level has no layers.");
                }
            }

            // Render Tile, Auto and Int grid layers
            for (int i = level.LayerInstances.Length - 1; i >= 0; i--)
            {
                LayerInstance layer = level.LayerInstances[i];

                if (layer._TilesetRelPath == null)
                {
                    continue;
                }

                if (layer._Type == LayerType.Entities)
                {
                    continue;
                }

                Texture2D texture = GetTexture(level, layer._TilesetRelPath);

                // Pretty sure there is nothing you need to change here to adjust the rendering logic
                int width = layer._CWid * layer._GridSize;
                int height = layer._CHei * layer._GridSize;
                RenderTarget2D renderTarget = new(_graphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);

                _graphicsDevice.SetRenderTarget(renderTarget);
                layers.Add(renderTarget);

                switch (layer._Type)
                {
                    case LayerType.Tiles:
                        foreach (TileInstance tile in layer.GridTiles.Where(_ => layer._TilesetDefUid.HasValue))
                        {
                            Vector2 position = new(tile.Px.X + layer._PxTotalOffsetX, tile.Px.Y + layer._PxTotalOffsetY);
                            Rectangle rect = new(tile.Src.X, tile.Src.Y, layer._GridSize, layer._GridSize);
                            SpriteEffects mirror = (SpriteEffects)tile.F;
                            Globals.SpriteBatch.Draw(texture, position, rect, new Color(1f, 1f, 1f, layer._Opacity), 0, Vector2.Zero, 1f, mirror, 0);
                        }
                        break;

                    case LayerType.AutoLayer:
                    case LayerType.IntGrid:
                        if (layer.AutoLayerTiles.Length > 0)
                        {
                            foreach (TileInstance tile in layer.AutoLayerTiles.Where(_ => layer._TilesetDefUid.HasValue))
                            {
                                Vector2 position = new(tile.Px.X + layer._PxTotalOffsetX, tile.Px.Y + layer._PxTotalOffsetY);
                                Rectangle rect = new(tile.Src.X, tile.Src.Y, layer._GridSize, layer._GridSize);
                                SpriteEffects mirror = (SpriteEffects)tile.F;
                                Globals.SpriteBatch.Draw(texture, position, rect, new Color(1f, 1f, 1f, layer._Opacity), 0, Vector2.Zero, 1f, mirror, 0);
                            }
                        }
                        break;
                }
            }

            return layers.ToArray();
        }

        RenderTarget2D RenderBackgroundToLayer(LDtkLevel level)
        {
            Texture2D texture = GetTexture(level, level.BgRelPath);

            RenderTarget2D layer = new(_graphicsDevice, level.PxWid, level.PxHei, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);

            _graphicsDevice.SetRenderTarget(layer);
            {
                LevelBackgroundPosition bg = level._BgPos;
                if (bg != null)
                {
                    Vector2 pos = bg.TopLeftPx.ToVector2();
                    Globals.SpriteBatch.Draw(texture, pos, new Rectangle((int)bg.CropRect[0], (int)bg.CropRect[1], (int)bg.CropRect[2], (int)bg.CropRect[3]), Color.White, 0, Vector2.Zero, bg.Scale, SpriteEffects.None, 0);
                }
            }
            _graphicsDevice.SetRenderTarget(null);

            return layer;
        }

        Texture2D GetTexture(LDtkLevel level, string path)
        {
            if (path == null)
            {
                throw new LDtkException("Tileset path is null.");
            }

            if (TilemapCache.TryGetValue(path, out Texture2D texture))
            {
                return texture;
            }

            Texture2D tilemap;
            if (Globals.Content == null)
            {
                string directory = Path.GetDirectoryName(level.WorldFilePath)!;
                string assetName = Path.Join(directory, path);
                tilemap = Texture2D.FromFile(_graphicsDevice, assetName);
            }
            else
            {
                string file = Path.ChangeExtension(path, null);
                string directory = Path.GetDirectoryName(level.WorldFilePath)!;
                string assetName = Path.Join(directory, file);
                tilemap = Globals.Content.Load<Texture2D>(assetName);
            }

            TilemapCache.Add(path, tilemap);

            return tilemap;
        }

        
        // TODO: Adjust rendering logic 
        public void RenderPrerenderedLevel(LDtkLevel level)
        {
            Dictionary<string, Dictionary<int, Vector2>> tileGroups = _spriteManager.GetTileGroups();
            Dictionary<string, List<int>> tileMappings = _spriteManager.GetTileMappings();

            if (PrerenderedLevels.TryGetValue(level.Identifier, out RenderedLevel prerenderedLevel))
            {
                // Draw the while world as is
                for (int i = 0; i < prerenderedLevel.Layers.Length; i++)
                {
                    Globals.SpriteBatch.Draw(prerenderedLevel.Layers[i], level.Position.ToVector2(), Color.White);
                }
                // Now check for all EnumTags
                foreach (var (enumTag, tileGroup) in tileGroups)
                {
                    // All tiles within the EnumTag
                    foreach (var (tileID, tilePos) in tileGroup)
                    {
                        // Check Position from player to that tile with EnumTag (e.g. House during first iteration)
                        if (Vector2.Distance(tilePos, _player.position) < 64)
                        {
                            float depth = 0;
                            // Player Y will compare to this Y
                            // X,Y,anchorTileID
                            Vector3 anchor = _spriteManager.GetAnchorTile(enumTag);

                            var (tileTexture, sourceRect)  = GetTextureForTile((int)anchor.Z, level);
                            if (_player.position.Y > anchor.Y)
                            {
                                depth = 0.3f;
                                //--> define player depth somewhere, shouldnt be here probably
                                //float player_depth = 0.5f;
                                //render tile in background
                            }
                            else
                            {
                                depth = 0.8f;
                                // render tile in foreground
                            }
                            Globals.SpriteBatch.Draw(tileTexture, tilePos, null, Color.White, 0f,
                                   Vector2.Zero, 1f, SpriteEffects.None, depth);
                        }
                    }
                }
            }
            else
            {
                throw new LDtkException($"No prerendered level with Identifier {level.Identifier} found.");
            }
        }

        // TODO: there is a problem with this GetTexture setup - no idea why - need to fix
        public (Texture2D, Rectangle) GetTextureForTile(int tileID, LDtkLevel level)
        {
            Dictionary<string, List<int>> tileMappings = _spriteManager.GetTileMappings();
            Dictionary<string, Dictionary<int, Vector2>> tileGroups = _spriteManager.GetTileGroups();

            foreach (var (tilesetPath, tileList) in tileMappings)
            {
                if (tileList.Contains(tileID))
                {
                    Texture2D tilesetTexture = GetTexture(level, tilesetPath);

                    // Get the tile position within the tileset
                    if (tileGroups.TryGetValue(tilesetPath, out var tilePositions) && tilePositions.TryGetValue(tileID, out Vector2 tilePos))
                    {
                        int tileSize = 16; // Adjust based grid size
                        Rectangle sourceRect = new Rectangle((int)tilePos.X * tileSize, (int)tilePos.Y * tileSize, tileSize, tileSize);
                        return (tilesetTexture, sourceRect);
                    }
                }
            }

            throw new Exception($"TileID {tileID} not found in any tileset.");
        }
    }
}

