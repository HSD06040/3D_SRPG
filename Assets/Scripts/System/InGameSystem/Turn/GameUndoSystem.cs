using SRPG.Command;
using System;
using System.Collections.Generic;

/// <summary>
/// 게임 내에서 실행된 커맨드를 저장하고, 실행 취소(Undo) 기능을 제공하는 시스템
/// </summary>
public class GameUndoSystem : IDisposable
{
    readonly Stack<ICommand> commandStack = new Stack<ICommand>();

    readonly EventBinding<UnitCommandCommittedEvent> commitBinding;

    public GameUndoSystem()
    {
        commitBinding = new EventBinding<UnitCommandCommittedEvent>();
        commitBinding.Add(OnMoveCommitted);
        EventBus<UnitCommandCommittedEvent>.Register(commitBinding);
    }

    private void OnMoveCommitted(UnitCommandCommittedEvent evt)
    {
        Push(evt.Command);
    }

    private void Push(ICommand command)
    {
        commandStack.Push(command);
    }

    public void Undo(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (commandStack.TryPop(out ICommand action))
                action.Undo();
        }
    }

    public void UndoAll()
    {
        for (int i = 0; i < commandStack.Count; i++)
        {
            if (commandStack.TryPop(out ICommand action))
            {
                action.Undo();
            }
        }
    }

    public void Dispose()
    {
        EventBus<UnitCommandCommittedEvent>.Deregister(commitBinding);
    }
}