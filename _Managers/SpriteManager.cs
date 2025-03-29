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
    // Maps all the tiles with matching Enum - also saves the Position and ID of those tiles
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
                                tileGroups[enumTag.EnumValueId] = new Dictionary<int, Vector2>();
                            }
                            tileMappings[enumTag.EnumValueId].AddRange(enumTag.TileIds);

                            foreach (var tileID in enumTag.TileIds)
                            {
                                Vector2 tilePosition = GetTileWorldPosition(tileID);
                                tileGroups[enumTag.EnumValueId][tileID] = tilePosition;
                            }
                        }
                    }
                }
            }
        }
    }

    private Vector2 GetTileWorldPosition(int tileID)
    {
        // Go through all levels
        foreach (var level in Globals.World.Levels)
        {
            // Go through all layers
            foreach (var layer in level.LayerInstances)
            {
                // Check if it is a Tiles layer
                if (layer._Type == LayerType.Tiles)
                {
                    // Go through all gridTiles
                    foreach (var gridTile in layer.GridTiles)
                    {
                        // Check if tileIDs match
                        if (gridTile.T == tileID)
                        {
                            // there is a matching Tile with ID at X,Y
                            return new Vector2(gridTile.Px.X, gridTile.Px.Y);
                        }
                    }
                }
            }
        }
        // No matching tile with ID
        return new Vector2(0, 0);
    }

    public Vector3 GetAnchorTile(string enumName)
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
            default:
                return new Vector3(-1,-1,-1);
        }
        // Check if the tile group contains this sprite type
        if (tileGroups.TryGetValue(enumName, out Dictionary<int, Vector2> tiles))
        {
            // Find the tile with the matching anchor ID
            if (tiles.TryGetValue(anchorID, out Vector2 position))
            {
                return new Vector3(position.X, position.Y, anchorID);
            }
        }
        return new Vector3(-1,-1,-1);
    }

    public Dictionary<string, Dictionary<int, Vector2>> GetTileGroups()
    {
        return tileGroups;
    }

    public Dictionary<string, List<int>> GetTileMappings()
    {
        return tileMappings;
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
    // I imported ExampleRender Class into Renderer
    // Change RenderPrerenderedLevel at the bottom of Renderer to adjust Depth and rendering changes
}
