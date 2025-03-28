using SWEN_Game;
using System;
using System.Collections.Generic;
using LDtk;
using LDtkTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Enum = LDtkTypes.Enum;

public class SpriteManager
{
    private Dictionary<string, List<int>> tileMappings = new Dictionary<string, List<int>>();

    private Dictionary<string, Dictionary<int, Vector2>>
        tileGroups = new Dictionary<string, Dictionary<int, Vector2>>();

    public SpriteManager()
    {
        mapTileToTexture();
        SetTilePositions();
    }

    private void mapTileToTexture()
    {
        foreach (var item in Globals.File.Defs.Enums[0].Values)
        {
            string enumID = item.Id; // e.g. House, Tree_Big, etc.
            foreach (var tileset in Globals.File.Defs.Tilesets)
            {
                if (tileset.EnumTags != null)
                {
                    foreach (var enumTag in tileset.EnumTags)
                    {
                        if (enumTag.EnumValueId == enumID)
                        {
                            if (!tileMappings.ContainsKey(enumTag.EnumValueId))
                            {
                                tileMappings[enumTag.EnumValueId] = new List<int>();
                            }

                            tileMappings[enumTag.EnumValueId].AddRange(enumTag.TileIds);
                        }
                    }
                }
            }
        }
    }

    private void SetTilePositions()
    {
        foreach (var entry in tileMappings)
        {
            string enumName = entry.Key;
            List<int> tileIDs = entry.Value;
            Dictionary<int, Vector2> tilePositions = new Dictionary<int, Vector2>();

            foreach (var tile in tileIDs)
            {
                Vector2 position = CalculateTileWorldPosition(tile);
                tilePositions[tile] = position;
            }

            tileGroups[enumName] = tilePositions;
        }
    }

    // Calculate world position based on grid index
    private Vector2 CalculateTileWorldPosition(int tileID)
    {
        var level = Globals.World.Levels[0];
        int tileSize = 16;

        int col = tileID % level.Size.X;
        int row = tileID / level.Size.X;
        return new Vector2(col * tileSize, row * tileSize);
    }

    // Computes a layer depth value based on the object's Y position.
    // Lower Y (closer to the top) yields a lower depth value.
    public float GetDepth(Vector2 position, float spriteHeight)
    {
        // Using the bottom of the sprite as the reference
        return
            (position.Y + spriteHeight) /
            1000f; // TODO: ADJUST SCALING FOR UR LEVEL - I HAD THIS ON 256f, but seems to work with 1000f :)
    }

    // Draw a tile with its depth computed from its world position
    public void DrawTile(SpriteBatch spriteBatch, Texture2D texture, Rectangle sourceRect, Vector2 position)
    {
        float depth = GetDepth(position, sourceRect.Height);
        spriteBatch.Draw(texture, position, sourceRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None,
            depth);
    }

    // Forced depth (with overload)
    public void DrawTile(SpriteBatch spriteBatch, Texture2D texture, Rectangle sourceRect, Vector2 position,
        float forcedDepth)
    {
        spriteBatch.Draw(texture, position, sourceRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None,
            forcedDepth);
    }

    // Old method - deprecated :) (or u can make it un-depreacted by moving stuff from renderer to here)
    public void DrawTiles(SpriteBatch spriteBatch, Texture2D tileset, int tileSize, int tilesetColumns, LDtkLevel level)
    {
        foreach (var layer in level.LayerInstances)
        {
            foreach (var tile in layer.GridTiles)
            {
                Rectangle srcRect = new Rectangle(
                    (tile.T % tilesetColumns) * tileSize,
                    (tile.T / tilesetColumns) * tileSize,
                    tileSize,
                    tileSize);
                DrawTile(spriteBatch, tileset, srcRect, new Vector2(tile.Px.X, tile.Px.Y));
            }
        }
    }


    // Draw the player sprite with a depth so it can be interleaved with tiles.
    public void DrawPlayer(SpriteBatch spriteBatch, Texture2D texture, Vector2 position)
    {
        // Use the bottom of the player sprite to compute depth.
        float depth = GetDepth(new Vector2(position.X, position.Y + texture.Height), 0);
        spriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, depth);
    }
}