using System;
using UnityEngine;

[Serializable]
public struct MapData
{
    public string Name;
    public float MapScale;
    public Vector2Int MapSize;
    public Vector2Int MapStartPoint;
    public float Offset;
    public Vector2 TileScale;

    public int MapTotalSize => MapSize.x * MapSize.y;
}