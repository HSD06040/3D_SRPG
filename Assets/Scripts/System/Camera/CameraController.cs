using Cysharp.Threading.Tasks;
using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public record struct CameraSequence(Vector3 targetPos, float duration);

public class CameraController : MonoBehaviour
{
    Camera cam;
    readonly Queue<CameraSequence> sequenceQueue = new();
    Tween currentTween;
    bool isProcessing = false;
    CameraHandler cameraHandler;

    private void Awake()
    {
        cam = Camera.main;
        cameraHandler = new CameraHandler(this);
    }

    private void OnDestroy()
    {
        currentTween?.Kill();
        cameraHandler.Dispose();
    }

    public async UniTask EnqueueSequenceAndProcessAsync(CameraSequence cameraSequence, bool isSkip = false)
    {
        EnqueueSequence(cameraSequence);

        if (isSkip)
        {
            await ProcessSequencesAsync();
        }        
        else if (!isProcessing)
        {
            await ProcessSequencesAsync();
        }
    }

    public async UniTask EnqueueSequencesAndProcessAsync(List<CameraSequence> cameraSequences, bool isSkip = false)
    {
        EnqueueSequences(cameraSequences);

        if (isSkip)
        {
            await ProcessSequencesAsync();
        }
        else if (!isProcessing)
        {
            await ProcessSequencesAsync();
        }
    }

    public void EnqueueSequence(CameraSequence cameraSequence) => sequenceQueue.Enqueue(cameraSequence);
    public void EnqueueSequences(List<CameraSequence> cameraSequences)
    {
        foreach (var seq in cameraSequences)
        {
            sequenceQueue.Enqueue(seq);
        }
    }

    public async UniTask ProcessSequencesAsync()
    {
        isProcessing = true;
        while (sequenceQueue.Count > 0)
        {
            var nextSequence = sequenceQueue.Dequeue();

            currentTween?.Kill();

            currentTween = cam.transform.DOMove(nextSequence.targetPos, nextSequence.duration);

            try
            {
                await currentTween.AsyncWaitForCompletion();
            }
            catch (System.Exception)
            {
                Debug.Log("Camera tween was killed or interrupted.");
            }
            finally
            {
                currentTween = null;
            }
        }
        isProcessing = false;
    }

    public void Skip()
    {
        if (currentTween == null) return;

        currentTween.Kill(true);
        currentTween = null;
    }
}

public class CameraHandler : IDisposable
{
    readonly EventBinding<CameraSequenceEvent> cameraSequenceBinding;
    readonly EventBinding<CameraSequencesEvent> cameraSequencesBinding;
    readonly CameraController controller;

    public CameraHandler(CameraController controller)
    {
        this.controller = controller;

        cameraSequenceBinding = new(OnCameraSequenceEvent);
        EventBus<CameraSequenceEvent>.Register(cameraSequenceBinding);

        cameraSequencesBinding = new(OnCameraSequencesEvent);
        EventBus<CameraSequencesEvent>.Register(cameraSequencesBinding);
    }

    void OnCameraSequenceEvent(CameraSequenceEvent evt)
    {
        controller.EnqueueSequenceAndProcessAsync(evt.CameraSequence).Forget();
    }
    void OnCameraSequencesEvent(CameraSequencesEvent evt)
    {
        controller.EnqueueSequencesAndProcessAsync(evt.CameraSequences).Forget();
    }

    public void Dispose()
    {
        EventBus<CameraSequenceEvent>.Deregister(cameraSequenceBinding);
        EventBus<CameraSequencesEvent>.Deregister(cameraSequencesBinding);
    }
}