using System.Collections.Generic;

namespace Events.MapEvent
{
    public record struct TileHighlightRequestedEvent(List<Tile> TilesToHighlight) : IEvent;

    public struct TileHighlightClearEvent : IEvent { }
}