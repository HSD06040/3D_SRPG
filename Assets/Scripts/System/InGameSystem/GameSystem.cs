using SRPG.Command;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : IDisposable
{
    private readonly MapSystem mapSystem;
    public BaseUnit SelectedUnit { get; private set; }
    public Tile SelectedTile { get; private set; }

    readonly EventBinding<TileSelectEvent> tileSelectBinding;
    readonly EventBinding<UnitSelectEvent> unitSelectBinding;

    public GameSystem(MapSystem mapSystem)
    {
        this.mapSystem = mapSystem;

        tileSelectBinding = new EventBinding<TileSelectEvent>();
        unitSelectBinding = new EventBinding<UnitSelectEvent>();

        EventBinding();
    }

    private void EventBinding()
    {
        tileSelectBinding.Add(OnTileSelected);
        EventBus<TileSelectEvent>.Register(tileSelectBinding);

        unitSelectBinding.Add(OnUnitSelected);
        EventBus<UnitSelectEvent>.Register(unitSelectBinding);
    }

    private void OnTileSelected(TileSelectEvent evt)
    {
        SelectedTile = evt.Tile;

        if (SelectedUnit == null) return;

        List<Tile> moveableTiles = mapSystem.GetMoveableTiles(SelectedUnit);
        if (!moveableTiles.Contains(evt.Tile)) return;

        MoveUnitCommand moveCommand = new MoveUnitCommand(SelectedUnit, evt.Tile);
        moveCommand.Execute();

        EventBus<UnitMoveCommittedEvent>.Raise(new UnitMoveCommittedEvent(moveCommand));
    }

    private void OnUnitSelected(UnitSelectEvent evt)
    {
        SelectedUnit = evt.Unit;
    }

    private bool IsValidMove(BaseUnit unit, Tile tile)
    {
        List<Tile> moveableTiles = mapSystem.CalculateMoveableTiles(unit);

        return moveableTiles.Contains(tile);
    }

    public void Dispose()
    {
        EventBus<TileSelectEvent>.Deregister(tileSelectBinding);
        EventBus<UnitSelectEvent>.Deregister(unitSelectBinding);
    }
}
