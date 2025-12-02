using UnityEngine;

public interface IHighlight
{
    void HighlightTile();
    void DeHighlightTile();
}

public interface ICommand
{
    void Execute();
    void Undo();
}
