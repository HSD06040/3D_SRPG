using System;
using System.Reflection;
using UnityEditor;

public class LockInspector
{
    [MenuItem("Edit/Lock Inspector %`")]
    public static void Lock()
    {
        ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;

        ActiveEditorTracker.sharedTracker.ForceRebuild();
    }

    [MenuItem("Edit/Lock Inspector %`", true)]
    public static bool Valid()
    {
        return ActiveEditorTracker.sharedTracker.activeEditors.Length != 0;
    }
}