using UnityEngine;

public class Tile : MonoBehaviour, IHighlight
{
    public Vector2Int Pos { get => _pos; }
    [SerializeField] Vector2Int _pos;

    public TileData Data { get => _data; }
    [SerializeField] private TileData _data;

    [SerializeField] GameObject _view;

    public void Init(Vector2Int pos, TileData data)
    {
        this._pos = pos;
        this._data = data;
    }

    public void HighlightTile()
    {
        _view.SetActive(true);
    }

    public void DeHighlightTile()
    {
        _view.SetActive(false);
    }
}
