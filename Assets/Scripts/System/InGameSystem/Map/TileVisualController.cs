using System;

/// <summary>
/// 유닛을 선택하였을 때 이벤트를 호출하여 해당 유닛이 이동할 수 있는 타일들을 하이라이트하는 클래스
/// </summary>
public class TileVisualController : IDisposable
{
    private UnitMovementCache _moveCache;

    readonly EventBinding<UnitSelectEvent> selectBinding;

    public TileVisualController(UnitMovementCache cache)
    {
        _moveCache = cache;

        selectBinding = new EventBinding<UnitSelectEvent>(OnUnitSelected);
        EventBus<UnitSelectEvent>.Register(selectBinding);
    }

    private void OnUnitSelected(UnitSelectEvent evt)
    {
        VisibleMoveableTiles(evt.Unit);
    }

    public void VisibleMoveableTiles(BaseUnit unit)
    {
        EventBus<TileHighlightClearEvent>.Raise(new TileHighlightClearEvent());

        EventBus<TileHighlightRequestedEvent>.Raise(new TileHighlightRequestedEvent(_moveCache.GetMoveableTiles(unit)));
    }

    public void Dispose()
    {
        EventBus<UnitSelectEvent>.Deregister(selectBinding);
    }
}