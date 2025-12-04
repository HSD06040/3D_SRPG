using UnityEngine;

public class Tile : MonoBehaviour, IHighlight
{
    public Vector2Int Pos { get => _pos; }
    [SerializeField] Vector2Int _pos;

    public TileData Data { get => _data; }
    [SerializeField] TileData _data;

    [SerializeField] GameObject visual;

    public void Init(Vector2Int pos, TileData data)
    {
        this._pos = pos;
        this._data = data;
        
        name = $"Tile_{pos.x}_{pos.y}";

        visual ??= transform.Find("Visual").gameObject;
        visual.SetActive(false);
    }

    public void HighlightTile()
    {
        visual.SetActive(true);
    }

    public void DeHighlightTile()
    {
        visual.SetActive(false);
    }
}
