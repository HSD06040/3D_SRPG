namespace Events.InputEvent
{
    public record struct UnitSelectEvent(BaseUnit Unit) : IEvent;
    public record struct TileSelectEvent(Tile Tile) : IEvent;
}