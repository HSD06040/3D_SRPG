using SRPG.Command;
using System;
using System.Collections.Generic;

public class GameUndoSystem : IDisposable
{
    readonly Stack<ICommand> commandStack = new Stack<ICommand>();

    readonly EventBinding<UnitMoveCommittedEvent> commitBinding;

    MoveUnitCommand moveUnitCommand;

    public GameUndoSystem()
    {
        commitBinding = new EventBinding<UnitMoveCommittedEvent>();
        commitBinding.Add(OnMoveCommitted);
        EventBus<UnitMoveCommittedEvent>.Register(commitBinding);
    }

    private void OnMoveCommitted(UnitMoveCommittedEvent evt)
    {
        moveUnitCommand = evt.MoveCommand;
    }

    private void PushAll()
    {
        Push(moveUnitCommand);
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
        EventBus<UnitMoveCommittedEvent>.Deregister(commitBinding);
    }
}