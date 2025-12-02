using SRPG.Command;

namespace Events.UnitEvent
{
    public record struct UnitEvent(BaseUnit Unit) : IEvent;

    public record struct UnitMoveRequestedEvent(BaseUnit UnitToMove, Tile TargetTile) : IEvent;

    public record struct UnitMoveCommittedEvent(MoveUnitCommand MoveCommand) : IEvent;

    public record struct UnitActionEndedEvent() : IEvent;
}