using UnityEngine;

public class MouseClickInputController : RayController
{
    public MouseClickInputController(RayControllerData data) : base(data)
    {
    }

    public override void Cast(Vector2 screenPosition)
    {
        Ray ray = data.cam.ScreenPointToRay(screenPosition);

        RaycastHit raycastHit;

        bool isHit = Physics.Raycast(
            ray,
            out raycastHit,
            data.maxDistance,
            data.filterMask,
            data.triggerInteraction
        );

        if (isHit)
        {
            data.hitAction?.Invoke(raycastHit);
        }
        else
        {
            data.cancelAction?.Invoke();
            Debug.Log("타겟이 없습니다.");
        }
    }
}
