using System;
using System.Collections.Generic;

public class MapVisualSystem : IDisposable
{
    private readonly List<Tile> hightlightTiles = new();

    #region Events
    private readonly EventBinding<TileHighlightRequestedEvent> highlightBinding;
    private readonly EventBinding<TileHighlightClearEvent> clearBinding;
    private readonly EventBinding<UnitMoveCommittedEvent> commitBinding;

    private void EventBinding()
    {
        highlightBinding.Add(OnHighlightRequest);
        EventBus<TileHighlightRequestedEvent>.Register(highlightBinding);

        clearBinding.Add(ClearVisuals);
        EventBus<TileHighlightClearEvent>.Register(clearBinding);

        commitBinding.Add(ClearVisuals);
        EventBus<UnitMoveCommittedEvent>.Register(commitBinding);
    }
    #endregion

    public MapVisualSystem()
    {
        highlightBinding = new EventBinding<TileHighlightRequestedEvent>();
        clearBinding = new EventBinding<TileHighlightClearEvent>();
        commitBinding = new EventBinding<UnitMoveCommittedEvent>();

        EventBinding();
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
        EventBus<UnitMoveCommittedEvent>.Deregister(commitBinding);
    }
}