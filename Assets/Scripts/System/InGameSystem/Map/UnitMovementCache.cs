using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유닛의 이동 가능한 타일을 캐싱하는 서비스 / 턴 종료 마다 갱신
/// </summary>
public class UnitMovementCache : IDisposable
{
    readonly PathfindingService pathfindingService;
    readonly Dictionary<BaseUnit, List<Tile>> moveableTileCache = new();

    readonly EventBinding<UnitTurnEndedEvent> commitBinding;
    readonly EventBinding<UnitMoveEvent> moveBinding;

    public UnitMovementCache(PathfindingService pathfindingService)
    {
        this.pathfindingService = pathfindingService;

        commitBinding = new EventBinding<UnitTurnEndedEvent>(OnUnitMoveCommitted);
        EventBus<UnitTurnEndedEvent>.Register(commitBinding);

        moveBinding = new(OnUnitMove);
        EventBus<UnitMoveEvent>.Register(moveBinding);
    }

    public List<Tile> GetMoveableTiles(BaseUnit unit)
    {
        if (moveableTileCache.TryGetValue(unit, out List<Tile> cachedTiles))
        {
            Debug.Log("Moveable Tiles Retrieved From Cache");
            return cachedTiles;
        }

        Debug.Log("Moveable Tiles Calculated");
        List<Tile> calculatedTiles = pathfindingService.CalculateMoveableTiles(unit);
        moveableTileCache.Add(unit, calculatedTiles);

        return calculatedTiles;
    }

    private void OnUnitMoveCommitted(UnitTurnEndedEvent evt)
    {
        BaseUnit unit = evt.Unit;

        moveableTileCache[unit] = pathfindingService.CalculateMoveableTiles(unit);
    }
    private void OnUnitMove(UnitMoveEvent evt)
    {
        BaseUnit unit = evt.Unit;
        Tile toTile = evt.ToTile;

        pathfindingService.OnUnitMove(unit, toTile, GetMoveableTiles(unit));
    }

    public void Dispose()
    {
        EventBus<UnitTurnEndedEvent>.Deregister(commitBinding);
    }
}