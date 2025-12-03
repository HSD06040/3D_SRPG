using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapSystem
{
    readonly Dictionary<Vector2Int, Tile> tileMap = new();    

    public bool TryGetTile(Vector2Int pos, out Tile tile)
    {
        return tileMap.TryGetValue(pos, out tile);
    }

    public void RegisterTile(Vector2Int pos, Tile tile)
    {
        tileMap[pos] = tile;
    }
}
