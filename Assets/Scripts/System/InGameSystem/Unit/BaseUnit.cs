using UnityEngine;

public abstract class BaseUnit : MonoBehaviour
{
    public Vector2Int CurPos { get; private set; }
    public UnitData UnitData { get; protected set; } = new();

    public static EventBinding<UnitEvent> UnitEvent = new();

    public void Init(UnitData unitData)
    {
        UnitData = unitData;
    }

    internal void SetPosition(Vector2Int pos) => CurPos = pos;

    public void MoveTo(Vector2Int tilePos)
    {
        // 움직임 알고리즘
    }
}
