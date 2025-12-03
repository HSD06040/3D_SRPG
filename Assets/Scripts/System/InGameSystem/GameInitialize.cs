using UnityEngine;
using Zenject;

public class GameInitialize : MonoBehaviour
{
    public MapData mapData;

    [Header("Unit Test")]
    GameObject unitPrefab;
    Vector2Int unitPos;


    [Inject]
    private void Init(MapSystem mapSystem)
    {
        MapGenerator.MapGenerate(mapData, mapSystem, Resources.Load<GameObject>("Prefabs/Tile"));

        GameObject unit = Instantiate(unitPrefab);

        if(mapSystem.TryGetTile(unitPos, out Tile tile))
            unit.transform.position = tile.transform.position;
    }
}
