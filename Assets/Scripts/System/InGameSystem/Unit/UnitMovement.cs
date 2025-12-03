using UnityEngine;
using Zenject;

[RequireComponent(typeof(BaseUnit))]
public class UnitMovement : MonoBehaviour
{
    public Vector2Int CurPos { get; private set; }
    MapSystem mapSystem;

    [Inject]
    void Inject(MapSystem mapSystem)
    {
        this.mapSystem = mapSystem;
    }

    public void SetPosition(Vector2Int pos)
    {
        CurPos = pos;               
    }

    public void RequestMoveTo()
    {

    }
}