using UnityEngine;

public abstract class BaseUnit : MonoBehaviour
{
    public Vector2Int CurPos { get; private set; }
    public UnitData UnitData { get; protected set; }

    public static EventBinding<UnitEvent> UnitEvent = new();

    internal void SetPosition(Vector2Int pos) => CurPos = pos;

    public void MoveTo(Vector2Int tilePos)
    {
        // 움직임 알고리즘
    }
}
