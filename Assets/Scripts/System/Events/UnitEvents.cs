using SRPG.Command;

namespace Events
{
    public record struct UnitEvent(BaseUnit Unit) : IEvent;

    public record struct UnitMoveRequestedEvent(BaseUnit UnitToMove, Tile TargetTile) : IEvent;

    // Command
    public record struct UnitCommandCommittedEvent(ICommand Command) : IEvent;

    // Turn
    public record struct UnitTurnEndedEvent(CharacterUnit Unit) : IEvent;
    public record struct UnitTurnStartedEvent(CharacterUnit Unit) : IEvent;
    public record struct UnitTurnResetEvent() : IEvent;

    // Action
    public record struct UnitMoveEvent(BaseUnit Unit, Tile ToTile) : IEvent;
    public record struct UnitAttackEvent(BaseUnit Attacker, BaseUnit Defender) : IEvent;
    public record struct UnitItemUseEvent(BaseUnit User) : IEvent;
}