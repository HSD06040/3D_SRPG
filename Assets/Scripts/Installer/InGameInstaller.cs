using Zenject;

public class InGameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<MapSystem>().AsSingle();
        Container.Bind<PathfindingService>().AsSingle();
        Container.Bind<UnitMovementCache>().AsSingle().NonLazy();
        Container.Bind<GameUndoSystem>().AsSingle().NonLazy();
        Container.Bind<GameSystem>().AsSingle().NonLazy();
        Container.Bind<TileVisualController>().AsSingle().NonLazy();
        Container.Bind<MapVisualSystem>().AsSingle().NonLazy();
    }
}