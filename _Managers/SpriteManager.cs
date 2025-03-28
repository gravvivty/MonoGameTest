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
    // Maps a TileIDs to the EnumTag - what TileID belongs to which Sprite
    private Dictionary<string, List<int>> tileMappings = new Dictionary<string, List<int>>();
    private Dictionary<string, Dictionary<int, Vector2>> tileGroups = new Dictionary<string, Dictionary<int, Vector2>>();

    public SpriteManager()
	{
        mapTileToTexture();
	}

    private void mapTileToTexture()
    {
        foreach (var item in Globals.File.Defs.Enums[0].Values)
        {
            string enumID = item.Id; // ID House, Tree_Big, etc.
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
                tilePositions = position;
            }
            tileGroups[enumName] = tilePositions;
        }
    }

    private Vector2 CalculateTileWorldPosition(int tileID)
    {
        var level = Globals.World.Levels[0];
        int tileSize = 16;
        int tilesPerRow = Globals.World.Levels[0].LayerInstances[Globals.World.Levels[0].LayerInstances.Length-1];
        int x = (tileID % tilesPerRow) * tileSize + level.WorldX;
        int y = (tileID / tilesPerRow) * tileSize + level.WorldY; 
        return new Vector2(x, y);
    }
}
