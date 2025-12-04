using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public record struct PathFindData(List<Vector3> Positions, List<Vector3> Direction);

public class MovementManager
{
    public Vector2Int TargetPos { get; private set; }
    readonly Transform owner;
    const float MOVE_SPEED = 30f;
    const float ROTATE_DURATION = .03f;
    CancellationTokenSource cts = new();

    public MovementManager(Transform owner)
    {
        this.owner = owner;        
    }

    public void MoveUnit(PathFindData pathFindData)
    {
        MoveUnitAsync(pathFindData).Forget();
    }

    public async UniTask MoveUnitAsync(PathFindData pathFindData)
    {
        for (int i = 0; i < pathFindData.Positions.Count; i++)
        {
            Vector3 targetPos = pathFindData.Positions[i];
            Vector3 direction = pathFindData.Direction[i];

            if (direction != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(direction);

                if (Quaternion.Angle(owner.rotation, targetRot) > 0.1f)
                {
                    await owner.DORotateQuaternion(targetRot, ROTATE_DURATION)
                               .SetEase(Ease.Linear)
                               .AsyncWaitForCompletion();
                }
            }
            await owner.DOMove(targetPos, MOVE_SPEED)
                       .SetSpeedBased()
                       .SetEase(Ease.Linear)
                       .AsyncWaitForCompletion();

            owner.position = targetPos;
        }
    }

    public void SetTurnPosition(Vector2Int pos) => TargetPos = pos;

    public void CancelMovement()
    {
        cts.Cancel();
        cts.Dispose();
        cts = new CancellationTokenSource();
    }    
}