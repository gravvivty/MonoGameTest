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
        // Iterates through all EnumTags and creates a Map with Key e.g. "House" and List<int> as Values
        // Basically the TileIDs under "House" all belong to a bigger Sprite - in this case some house
        // Useful since we now have a way to make tiles with THAT matching ID to render in the Back-/Foreground
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

    // TODO:
    // We now know which TileIDs belong to what Sprite 
    // set an anchor tile for each Sprite (enumTags) in tileMappings - (a Sprite is just the Key in tileMappings)
    // when that anchor's Y-Value (e.g. Tree - 264 Tile in LDTK - bottom right as the Y-Value) gets crossed
    // then render all of these tiles within that enumTag group correctly and only look
    // if they have to be rendered - in a 64px Radius around the player - that's where we apply Y-Sorting
    // Depth Values can be from 0f - 1.0f
    // --> probably will need a new Map where the actual on map coordinates of the Tiles are stored with EnumTag
    // private Dictionary<string, Dictionary<int, Vector2>> tileGroups = new Dictionary<string, Dictionary<int, Vector2>>();
    // TileIDs with pos are stored in Globals.World.Levels[0].GridTiles
    // Change ExampleRenderer Class if needed
}
