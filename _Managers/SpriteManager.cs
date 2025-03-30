using SWEN_Game;
using System.Collections.Generic;
using LDtk;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class SpriteManager
{
    private Dictionary<string, List<int>> tileMappings = new Dictionary<string, List<int>>();

    private Dictionary<string, Dictionary<int, List<Vector2>>> tileGroups = new();

    /*
    "House": {
       231: [ (100, 200), (150, 250) ],
       432: [ (300, 400) ] }
    ,
    "Tree": {
        512: [ (50, 50), (75, 125) ]
    }*/
    // How TileGroups looks like --> so now we can go through this --> 
    // since we have a radius around the player we should check idk 64px 
    // so now we can go through this collection and see which anchorTileID is closest to the tile that just came into our radius
    public SpriteManager()
    {
        mapTileToTexture();
    }

    private void mapTileToTexture()
    {
        Vector2 invalid = new Vector2(-1, -1);
        // Go through all Enum Values
        foreach (var item in Globals.File.Defs.Enums[0].Values)
        {
            string enumID = item.Id; // e.g. House, Tree_Big, etc.
            // Go through all tilesets
            foreach (var tileset in Globals.File.Defs.Tilesets)
            {
                if (tileset.EnumTags != null)
                {
                    // Every EnumTag
                    foreach (var enumTag in tileset.EnumTags)
                    {
                        // if Tileset contains Enum that exists
                        if (enumTag.EnumValueId == enumID)
                        {
                            // New Key if it doesn't exist yet (e.g. "House")
                            if (!tileMappings.ContainsKey(enumTag.EnumValueId))
                            {
                                tileMappings[enumTag.EnumValueId] = new List<int>();
                                tileGroups[enumTag.EnumValueId] = new Dictionary<int, List<Vector2>>();
                            }

                            // Add all tileIDs to the correct enum
                            tileMappings[enumTag.EnumValueId].AddRange(enumTag.TileIds);

                            // Get correct WorldPos
                            foreach (var tileID in enumTag.TileIds)
                            {
                                List<Vector2> tilePositions = GetTileWorldPosition(tileID, enumTag.EnumValueId);
                                // New Key if (e.g. "House", 130 doesnt exist)
                                if (!tileGroups[enumTag.EnumValueId].ContainsKey(tileID))
                                {
                                    tileGroups[enumTag.EnumValueId][tileID] = new List<Vector2>();
                                }

                                foreach (var pos in tilePositions)
                                {
                                    tileGroups[enumTag.EnumValueId][tileID].Add(pos);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private List<Vector2> GetTileWorldPosition(int tileID, string enumTag)
    {
        List<Vector2> positions = new List<Vector2>();
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
                            // If Tile with EnumTag, ID and same coords exists --> dont return - iterate again
                            Vector2 tilePos = new Vector2(gridTile.Px.X, gridTile.Px.Y);
                            positions.Add(tilePos);
                        }
                    }
                }
            }
        }

        // No matching tile with ID
        return positions;
    }

    

    public Dictionary<string, Dictionary<int, List<Vector2>>> GetTileGroups()
    {
        return tileGroups;
    }

    public Dictionary<string, List<int>> GetTileMappings()
    {
        return tileMappings;
    }

    // Computes a layer depth value based on the object's Y position.
    // Lower Y (closer to the top) yields a lower depth value.
    public float GetDepth(Vector2 position, float spriteHeight, LayerInstance layer)
    {
        // Adjusted depth based on layer - there were cases where a flower was above lantern cuz it was closer to top
        float depth = (position.Y + spriteHeight/2) / 1000f;
        switch (layer._Identifier)
        {
            case "Deco_Big3":
                //depth -= 0.001f;
                break;
            case "Deco_Big2":
                //depth -= 0.001f;
                break;
            case "Deco_Big1":
                //depth -= 0.0015f;
                break;
            case "Deco_Small":
                depth -= 0.001f;
                break;
            case "Deco_Background":
                depth = 0.0001f;
                break;
        }


        // Using the bottom of the sprite as the reference
        return depth;
    }

    public float GetDepth(Vector2 position, float spriteHeight)
    {
        float depth = (position.Y + spriteHeight/2) / 1000f;
        // Using the bottom of the sprite as the reference
        return depth;
    }


    // Draw a tile with its depth computed from its world position
    public void DrawTile(SpriteBatch spriteBatch, Texture2D texture, Rectangle sourceRect, Vector2 position,
        LayerInstance layer)
    {
        float depth = GetDepth(position, sourceRect.Height, layer);
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

    // Old method - deprecated
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