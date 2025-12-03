using System;
using System.Collections.Generic;

/// <summary>
/// 이벤트를 받으면 이벤트에 맞는 타일 하이라이팅, 하이라이트 클리어, 
/// </summary>
public class MapVisualSystem : IDisposable
{
    private readonly List<Tile> hightlightTiles;

    private readonly EventBinding<TileHighlightRequestedEvent> highlightBinding;
    private readonly EventBinding<TileHighlightClearEvent> clearBinding;
    private readonly EventBinding<UnitTurnEndedEvent> unitTurnEndedBinding;

    public MapVisualSystem()
    {
        highlightBinding = new EventBinding<TileHighlightRequestedEvent>();
        clearBinding = new EventBinding<TileHighlightClearEvent>();
        unitTurnEndedBinding = new EventBinding<UnitTurnEndedEvent>();
        hightlightTiles = new List<Tile>(50);

        EventBinding();
    }

    private void EventBinding()
    {
        highlightBinding.Add(OnHighlightRequest);
        EventBus<TileHighlightRequestedEvent>.Register(highlightBinding);

        clearBinding.Add(ClearVisuals);
        EventBus<TileHighlightClearEvent>.Register(clearBinding);

        unitTurnEndedBinding.Add(ClearVisuals);
        EventBus<UnitTurnEndedEvent>.Register(unitTurnEndedBinding);
    }

    public void OnHighlightRequest(TileHighlightRequestedEvent evt)
    {
        ClearVisuals();
        foreach (Tile tile in evt.TilesToHighlight)
        {
            tile.HighlightTile();
            hightlightTiles.Add(tile);
        }
    }

    private void ClearVisuals()
    {
        foreach (var tile in hightlightTiles)
        {
            tile.DeHighlightTile();
        }

        hightlightTiles.Clear();
    }

    public void Dispose()
    {
        EventBus<TileHighlightRequestedEvent>.Deregister(highlightBinding);
        EventBus<TileHighlightClearEvent>.Deregister(clearBinding);
        EventBus<UnitTurnEndedEvent>.Deregister(unitTurnEndedBinding);
    }
}