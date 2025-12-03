using System;
using System.Collections.Generic;

/// <summary>
/// 유닛의 이동 가능한 타일을 캐싱하는 서비스 / 턴 종료 마다 갱신
/// </summary>
public class UnitMovementCache : IDisposable
{
    readonly PathfindingService _pathfindingService;
    readonly Dictionary<BaseUnit, List<Tile>> moveableTileCache = new();

    readonly EventBinding<UnitTurnEndedEvent> commitBinding;

    public UnitMovementCache(PathfindingService pathfindingService)
    {
        _pathfindingService = pathfindingService;

        commitBinding = new EventBinding<UnitTurnEndedEvent>(OnUnitMoveCommitted);
        EventBus<UnitTurnEndedEvent>.Register(commitBinding);
    }

    public List<Tile> GetMoveableTiles(BaseUnit unit)
    {
        if (moveableTileCache.TryGetValue(unit, out List<Tile> cachedTiles))
        {
            return cachedTiles;
        }

        List<Tile> calculatedTiles = _pathfindingService.CalculateMoveableTiles(unit);
        moveableTileCache[unit] = calculatedTiles;
        return calculatedTiles;
    }

    private void OnUnitMoveCommitted(UnitTurnEndedEvent evt)
    {
        BaseUnit unit = evt.Unit;

        moveableTileCache[unit] = _pathfindingService.CalculateMoveableTiles(unit);
    }

    public void Dispose()
    {
        EventBus<UnitTurnEndedEvent>.Deregister(commitBinding);
    }
}