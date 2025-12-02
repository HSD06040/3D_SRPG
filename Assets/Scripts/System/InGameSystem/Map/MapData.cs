using System;
using UnityEngine;

[Serializable]
public struct MapData
{
    public string Name;
    public float MapScale;
    public Vector2Int MapSize;
    public Vector2Int MapStartPoint;
    public Vector2 MapOffset;
    public Vector2 TileScale;

    public int MapTotalSize => MapSize.x * MapSize.y;
}