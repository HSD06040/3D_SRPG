using System.Collections.Generic;
using UnityEngine;

public class PathfindingService
{
    readonly MapSystem mapSystem;

    public PathfindingService(MapSystem mapSystem)
    {
        this.mapSystem = mapSystem;
    }

    public List<Tile> CalculateMoveableTiles(BaseUnit unit)
    {
        Vector2Int unitPos = unit.CurPos;

        if (!mapSystem.TryGetTile(unitPos, out Tile startTile))
        {
            Debug.LogError("유닛의 현재 위치에 타일이 없습니다.");
            return new List<Tile>();
        }

        int movement = unit.UnitData.StatData.Movement;
        List<Tile> moveableTiles = new List<Tile>();

        Queue<(Tile tile, int distance)> queue = new();
        Dictionary<Tile, int> visited = new();

        queue.Enqueue((startTile, 0));
        visited.Add(startTile, 0);

        while (queue.Count > 0)
        {
            var (currentTile, currentDist) = queue.Dequeue();

            if (currentDist >= movement) continue;

            Vector2Int curPos = currentTile.Pos;
            Vector2Int[] neighbors = new Vector2Int[]
            {
                curPos + Vector2Int.up,
                curPos + Vector2Int.down,
                curPos + Vector2Int.left,
                curPos + Vector2Int.right,
            };

            foreach (Vector2Int neighborPos in neighbors)
            {
                if (mapSystem.TryGetTile(unitPos, out Tile neighborTile))
                {
                    if (visited.ContainsKey(neighborTile) || !CanMoveTo(neighborTile, unit))
                    {
                        continue;
                    }

                    int nextDist = currentDist + 1;

                    if (nextDist <= movement)
                    {
                        visited.Add(neighborTile, nextDist);
                        moveableTiles.Add(neighborTile);
                        queue.Enqueue((neighborTile, nextDist));
                    }
                }
            }
        }

        return moveableTiles;
    }

    private bool CanMoveTo(Tile tile, BaseUnit unit)
    {
        if (tile.Data.isWall)
            return false;

        if (!tile.Data.isWakable)
        {
            return unit.UnitData.isFly;
        }

        return true;
    }
}