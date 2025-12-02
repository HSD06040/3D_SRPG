using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapSystem : IDisposable
{
    readonly Dictionary<BaseUnit, List<Tile>> moveableTileCache = new();

    #region Events
    readonly EventBinding<UnitMoveCommittedEvent> commitBinding;
    readonly EventBinding<UnitSelectEvent> selectBinding;
    #endregion

    readonly Dictionary<Vector2Int, Tile> tileMap = new();

    public MapSystem()
    {
        commitBinding = new EventBinding<UnitMoveCommittedEvent>();
        selectBinding = new EventBinding<UnitSelectEvent>();

        EventBinding();
    }
    
    public void RegisterTile(Vector2Int pos, Tile tile)
    {
        tileMap[pos] = tile;
    }

    private void EventBinding()
    {
        commitBinding.Add(OnUnitMoveCommitted);
        EventBus<UnitMoveCommittedEvent>.Register(commitBinding);

        selectBinding.Add(VisibleTile);
        EventBus<UnitSelectEvent>.Register(selectBinding);
    }

    public List<Tile> GetMoveableTiles(BaseUnit unit)
    {
        if (moveableTileCache.TryGetValue(unit, out List<Tile> cachedTiles))
        {
            return cachedTiles;
        }

        List<Tile> calculatedTiles = CalculateMoveableTiles(unit);

        moveableTileCache[unit] = calculatedTiles;

        return calculatedTiles;
    }

    private void OnUnitMoveCommitted(UnitMoveCommittedEvent evt)
    {
        BaseUnit unit = evt.MoveCommand.Unit;
        moveableTileCache[unit] = CalculateMoveableTiles(unit);
    }

    private void VisibleTile(UnitSelectEvent unitSelectEvent)
    {
        VisibleTile(unitSelectEvent.Unit);
    }

    public List<Tile> CalculateMoveableTiles(BaseUnit unit)
    {
        if (!tileMap.TryGetValue(unit.CurPos, out Tile startTile))
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
                if (tileMap.TryGetValue(neighborPos, out Tile neighborTile))
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

    public void VisibleTile(BaseUnit unit)
    {
        EventBus<TileHighlightClearEvent>.Raise(new TileHighlightClearEvent());

        List<Tile> tiles = GetMoveableTiles(unit);

        EventBus<TileHighlightRequestedEvent>.Raise(new TileHighlightRequestedEvent(tiles));
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

    public void Dispose()
    {
        EventBus<UnitMoveCommittedEvent>.Deregister(commitBinding);
    }
}
