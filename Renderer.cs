using System;
using System.Collections.Generic;
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
        private ExampleRenderer _renderer;
        private Player _player;
        private SpriteManager _spriteManager;
        public float zoom { get; private set; } = 4f;

        public Renderer(Player player, SpriteManager spriteManager)
        {
            _player = player;
            _spriteManager = spriteManager;
            RenderInit();
        }

        public void drawWorld()
        {
            Dictionary<string, Dictionary<int, List<Vector2>>> tileGroups = _spriteManager.GetTileGroups();
            Dictionary<string, List<int>> tileMappings = _spriteManager.GetTileMappings();

            Globals.SpriteBatch.Begin(
                sortMode: SpriteSortMode.FrontToBack,
                transformMatrix: calcTranslation(),
                samplerState: SamplerState.PointClamp);

            var level = Globals.World.Levels[0];

            foreach (var layer in level.LayerInstances)
            {
                // If this layer is specifically the "Background" layer, we’ll force it behind everything
                bool isBackground = layer._Identifier == "Background";
                if (layer._TilesetRelPath == null) continue;

                Texture2D tilesetTexture = GetTilesetTextureFromRenderer(level, layer._TilesetRelPath);

                foreach (var tile in layer.GridTiles)
                {
                    Vector2 position = new Vector2(tile.Px.X + layer._PxTotalOffsetX,
                        tile.Px.Y + layer._PxTotalOffsetY);
                    Rectangle srcRect = new Rectangle(tile.Src.X, tile.Src.Y, layer._GridSize, layer._GridSize);

                    if (isBackground)
                    {
                        // Draw the background tiles with a forced depth of 0 (which is behind anything > 0).
                        _spriteManager.DrawTile(Globals.SpriteBatch, tilesetTexture, srcRect, position, 0f);
                    }
                    else
                    {
                        // Normal tile depth calculation
                        _spriteManager.DrawTile(Globals.SpriteBatch, tilesetTexture, srcRect, position, layer);
                    }

                    // Check within 64px around Player
                    // if there is a tile with an ID that occurs in tileMappings (means it is part of a bigger Sprite)
                    // get that tile that just entered your radius - look for that EnumTag - figure out the anchortileID 
                    // which anchortile do we use? cuz there will be multiple
                    // use anchortile closest to the tile that entered radius WITHIN THE SAME LAYER!!
                    // now we can use this anchortiles Y for the whole EnumTags that are within radius
                    // this way we can dynamically draw based on entity proximity
                    //
                    // As of now each tile's Y gets treated seperately
                }
            }

            // Draw the player
            _spriteManager.DrawPlayer(Globals.SpriteBatch, _player.texture, _player.position);

            // Player Collision Box
            Rectangle entityRect = new Rectangle(
                (int)_player.position.X + _player.texture.Width / 2 - 2,
                (int)_player.position.Y + _player.texture.Height - 3,
                _player.texture.Width / 4,
                _player.texture.Height / 15);

            //float collisionDepth = _spriteManager.GetDepth(new Vector2(entityRect.X, entityRect.Y), entityRect.Height);
            Globals.SpriteBatch.Draw(_player.texture, entityRect, null, Color.Pink, 0f, Vector2.Zero,
                SpriteEffects.None, 1f);

            foreach (var collision in Globals.Collisions)
            {
                Globals.SpriteBatch.Draw(_player.texture, collision, null, Color.Red, 0f, new Vector2(0, 0), SpriteEffects.None, 1f);
            }

            Globals.SpriteBatch.End();
        }

        // Not complete but wanted to use this to calculate which AnchorTile to use from the tileGroups Map
        private float SpriteGroupAnchorCalculation(Dictionary<string, Dictionary<int, List<Vector2>>> tileGroups,
            Dictionary<string, List<int>> tileMappings)
        {
            float spriteGroupDepth = 0f;
            foreach (var (enumTag, tileGroup) in tileGroups)
            {
                // All tiles within the EnumTag
                foreach (var (tileID, tileList) in tileGroup)
                {
                    int anchorID = _spriteManager.GetAnchorTileID(enumTag);

                }
            }
            return spriteGroupDepth;
        }


        // Helper method to retrieve the tileset texture using ExampleRenderer logic.
        private Texture2D GetTilesetTextureFromRenderer(LDtkLevel level, string tilesetPath)
        {
            if (Globals.Content == null)
            {
                string directory = System.IO.Path.GetDirectoryName(level.WorldFilePath)!;
                string assetName = System.IO.Path.Join(directory, tilesetPath);
                return Texture2D.FromFile(Globals.Graphics.GraphicsDevice, assetName);
            }
            else
            {
                string file = System.IO.Path.ChangeExtension(tilesetPath, null);
                string directory = System.IO.Path.GetDirectoryName(level.WorldFilePath)!;
                string assetName = System.IO.Path.Join(directory, file);
                return Globals.Content.Load<Texture2D>(assetName);
            }
        }

        public void SetZoom(float newZoom)
        {
            zoom = newZoom;
        }

        private Matrix calcTranslation()
        {
            return Matrix.CreateTranslation(-_player.position.X, -_player.position.Y, 0) *
                   Matrix.CreateScale(zoom, zoom, 1f) *
                   Matrix.CreateTranslation(
                       Globals.Graphics.PreferredBackBufferWidth / 2f - _player.texture.Width * 3,
                       Globals.Graphics.PreferredBackBufferHeight / 2f - _player.texture.Height,
                       0);
        }

        private void RenderInit()
        {
        }
    }
}