using System;
using UnityEngine;

[Serializable]
public record struct RayControllerData(Camera cam, Action<RaycastHit> hitAction, float maxDistance, LayerMask filterMask, QueryTriggerInteraction triggerInteraction, Action cancelAction);

public abstract class RayController
{
    protected readonly RayControllerData data;

    public RayController(RayControllerData data)
    {
        this.data = data;
    }

    public abstract void Cast(Vector2 screenPosition);
}
