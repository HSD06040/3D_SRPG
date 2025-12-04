using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastInputSystem : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private LayerMask filterMask = ~0;
    [SerializeField] private QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore;

    Vector2 currentScreenPosition;

    RayController controller;

    private void Awake()
    {
        RayControllerData rayControllerData = new RayControllerData(Camera.main, Hit, maxDistance, filterMask, triggerInteraction, Cancel);

#if UNITY_STANDALONE_WIN
        controller = new MouseClickInputController(rayControllerData);
#elif UNITY_ANDROID || UNITY_IOS
        controller = new MobileTouchController(rayControllerData);
#endif
    }

    public void OnPoint(InputValue value)
    {
        currentScreenPosition = value.Get<Vector2>();
    }

    public void OnClick(InputValue value)
    {
        if (value.isPressed)
        {
            controller.Cast(currentScreenPosition);
        }
    }    

    void Hit(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent(out BaseUnit unit))
        {
            EventBus<UnitSelectEvent>.Raise(new UnitSelectEvent(unit));
            Debug.Log("Unit Selected: " + unit.name);
        }
        else if (hit.collider.TryGetComponent(out Tile tile))
        {
            EventBus<TileSelectEvent>.Raise(new TileSelectEvent(tile));
            Debug.Log("Tile Selected: " + tile.Pos);
        }
    }

    void Cancel()
    {
        EventBus<TileHighlightClearEvent>.Raise(new TileHighlightClearEvent());
        EventBus<UnitSelectEvent>.Raise(new UnitSelectEvent(null));
    }
}
