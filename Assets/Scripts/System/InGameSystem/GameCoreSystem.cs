public class GameCoreSystem
{
    public BaseUnit SelectedUnit { get; private set; }
    public Tile SelectedTile { get; private set; }

    #region Events
    readonly EventBinding<TileSelectEvent> tileSelectBinding;
    readonly EventBinding<UnitSelectEvent> unitSelectBinding;
    #endregion

    public GameCoreSystem()
    {
        tileSelectBinding = new EventBinding<TileSelectEvent>();
        unitSelectBinding = new EventBinding<UnitSelectEvent>();

        EventBinding();
    }

    private void EventBinding()
    {
        tileSelectBinding.Add(SelectTile);
        EventBus<TileSelectEvent>.Register(tileSelectBinding);

        unitSelectBinding.Add(SelectUnit);
        EventBus<UnitSelectEvent>.Register(unitSelectBinding);
    }

    void SelectTile(TileSelectEvent selectEvent)
    {
        SelectedTile = selectEvent.Tile;

        if (SelectedUnit == null) return;

        SelectedUnit.MoveTo(selectEvent.Tile.Pos);
    }

    void SelectUnit(UnitSelectEvent selectEvent) => SelectedUnit = selectEvent.Unit;
}