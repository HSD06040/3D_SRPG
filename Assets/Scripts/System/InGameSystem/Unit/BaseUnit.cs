using UnityEngine;

public abstract class BaseUnit : MonoBehaviour
{
    public Vector2Int CurPos { get; private set; }
    public UnitData UnitData { get; protected set; } = new();
    public MovementManager movementManager { get; private set; }

    public static EventBinding<UnitEvent> UnitEvent = new();

    public void Init(UnitData unitData, Vector2Int pos = default)
    {
        UnitData = unitData;

        movementManager = new MovementManager(transform);

        if(pos != default)
            SetPosition(pos);
    }

    public void SetPosition(Vector2Int pos)
    {
        CurPos = pos;
        movementManager.SetTurnPosition(pos);
    }
}
