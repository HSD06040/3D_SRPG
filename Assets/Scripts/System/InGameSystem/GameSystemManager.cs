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

    private void Awake()
    {
        undoSystem = new GameUndoSystem();
        mapSystem = new MapSystem();
        mapVisualSystem = new MapVisualSystem();
    }

    private void OnDestroy()
    {
        undoSystem.Dispose();
        mapVisualSystem.Dispose();
        gameSystem.Dispose();
    }
}