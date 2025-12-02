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
        if (hit.collider.TryGetComponent<BaseUnit>(out BaseUnit unit))
        {
            EventBus<UnitSelectEvent>.Raise(new UnitSelectEvent(unit));
        }
        else if (hit.collider.TryGetComponent<Tile>(out Tile tile))
        {
            EventBus<TileSelectEvent>.Raise(new TileSelectEvent(tile));
        }
    }

    void Cancel()
    {
        EventBus<TileHighlightClearEvent>.Raise(new TileHighlightClearEvent());
    }
}
