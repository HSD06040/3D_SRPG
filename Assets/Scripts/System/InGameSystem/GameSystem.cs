using SRPG.Command;
using System;
using System.Collections.Generic;

/// <summary>
/// 게임 내 주요 시스템을 관리하는 클래스
/// </summary>
public class GameSystem : IDisposable
{
    readonly UnitMovementCache unitCache;
    public BaseUnit SelectedUnit { get; private set; }
    public Tile SelectedTile { get; private set; }

    readonly Stack<ICommand> commands;

    readonly EventBinding<TileSelectEvent> tileSelectBinding;
    readonly EventBinding<UnitSelectEvent> unitSelectBinding;
    readonly EventBinding<UnitTurnEndedEvent> unitTurnEndedBinding;
    readonly EventBinding<UnitTurnResetEvent> unitTurnResetEvent;

    public GameSystem(UnitMovementCache unitCache)
    {
        this.unitCache = unitCache;

        commands = new Stack<ICommand>();

        tileSelectBinding = new EventBinding<TileSelectEvent>(OnTileSelected);
        EventBus<TileSelectEvent>.Register(tileSelectBinding);

        unitSelectBinding = new EventBinding<UnitSelectEvent>(OnUnitSelected);
        EventBus<UnitSelectEvent>.Register(unitSelectBinding);

        unitTurnResetEvent = new EventBinding<UnitTurnResetEvent>(OnUnitTurnReset);
        EventBus<UnitTurnResetEvent>.Register(unitTurnResetEvent);

        unitTurnEndedBinding = new EventBinding<UnitTurnEndedEvent>(TurnEnd);
        EventBus<UnitTurnEndedEvent>.Register(unitTurnEndedBinding);
    }

    private void OnTileSelected(TileSelectEvent evt)
    {
        SelectedTile = evt.Tile;

        if (SelectedUnit == null) return;

        List<Tile> moveableTiles = unitCache.GetMoveableTiles(SelectedUnit);
        if (!moveableTiles.Contains(evt.Tile)) return;

        MoveUnitCommand moveCommand = new MoveUnitCommand(SelectedUnit, evt.Tile);
        moveCommand.Execute();
        commands.Push(moveCommand);

        EventBus<UnitMoveEvent>.Raise(new UnitMoveEvent(SelectedUnit, evt.Tile));
    }

    private void OnUnitSelected(UnitSelectEvent evt)
    {
        SelectedUnit = evt.Unit;
    }

    private void TurnEnd(UnitTurnEndedEvent turnEndedEvents)
    {
        for (int i = 0; i < commands.Count; i++)
        {
            if (commands.TryPop(out ICommand command))
            {
                EventBus<UnitCommandCommittedEvent>.Raise(new UnitCommandCommittedEvent { Command = command });
            }
        }

        commands.Clear();
    }

    private void OnUnitTurnReset(UnitTurnResetEvent evt)
    {
        for (int i = 0; i < commands.Count; i++)
        {
            if (commands.TryPop(out ICommand command))
            {
                command.Undo();
            }
        }
    }

    public void Dispose()
    {
        EventBus<TileSelectEvent>.Deregister(tileSelectBinding);
        EventBus<UnitSelectEvent>.Deregister(unitSelectBinding);
        EventBus<UnitTurnEndedEvent>.Deregister(unitTurnEndedBinding);
        EventBus<UnitTurnResetEvent>.Deregister(unitTurnResetEvent);
    }
}
