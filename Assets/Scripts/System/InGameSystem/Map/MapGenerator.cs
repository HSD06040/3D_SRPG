using UnityEngine;

public class MapGenerator
{            
    public static void MapGenerate(MapData mapData, MapSystem mapSystem, GameObject tilePrefab)
    {
        Transform tileParent = new GameObject("MapTiles").transform;
        
        for (int y = 1; y <= mapData.MapSize.x; y++)
        {
            for (int x = 1; x <= mapData.MapSize.y; x++)
            {
                Vector2Int tilePos = new Vector2Int(y, x);
                Tile tile = Object.Instantiate(tilePrefab,
                    new Vector3(tilePos.x, 0, tilePos.y),
                    Quaternion.identity, tileParent).GetComponent<Tile>();

                tile.Init(tilePos, null);
                mapSystem.RegisterTile(tilePos, tile);
            }
        }        
    }
}

