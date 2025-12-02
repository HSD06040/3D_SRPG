using UnityEngine;
using System.Collections.Generic;

public class GameSystemManager : MonoBehaviour
{
    #region Systems
    private MapSystem mapSystem;
    private MapVisualSystem mapVisualSystem;
    private GameSystem gameSystem;
    private GameUndoSystem undoSystem;
    #endregion

    private Dictionary<Vector2Int, BaseUnit> unitMap;

    private void Awake()
    {
        undoSystem = new GameUndoSystem();
        mapSystem = new MapSystem();
        mapVisualSystem = new MapVisualSystem();
        gameSystem = new GameSystem(mapSystem);

        Init();
    }

    #region Test
    [SerializeField] MapData data;
    [ContextMenu("Map G")]
    private void MapG()
    {
        //mapSystem.MapGenerate(data);
    }
    #endregion

    public void Init()
    {
        unitMap = new();
    }

    private void OnDestroy()
    {
        undoSystem.Dispose();
        mapSystem.Dispose();
        mapVisualSystem.Dispose();
        gameSystem.Dispose();
    }
}