using UnityEngine;

public abstract class BaseUnit : MonoBehaviour, IHighlight
{
    public Vector2Int CurPos { get; private set; }
    public UnitData UnitData { get; protected set; }

    public static EventBinding<UnitEvent> UnitEvent = new();

    internal void SetPosition(Vector2Int pos) => CurPos = pos;

    public void MoveVisual(Vector2Int tilePos)
    {
        // 움직임 알고리즘
    }

    public void HighlightTile()
    {
        
    }

    public void DeHighlightTile()
    {
        
    }
}