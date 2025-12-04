using UnityEngine;
using Zenject;

public class GameInitialize : MonoBehaviour
{
    public GameObject[] instantiateSystems;
    public MapData mapData;

    [Header("Unit Test")]
    public GameObject unitPrefab;
    public UnitData unitData;
    public Vector2Int unitPos;


    [Inject]
    private void Init(MapSystem mapSystem)
    {
        SystemInstantiates();

        MapGenerator.MapGenerate(mapData, mapSystem, Resources.Load<GameObject>("Prefabs/Tile"));

        BaseUnit unit = Instantiate(unitPrefab).GetComponent<BaseUnit>();
        unit.SetPosition(unitPos);
        unit.Init(unitData);

        if (mapSystem.TryGetTile(unitPos, out Tile tile))
            unit.transform.position = tile.transform.position;
    }

    private void SystemInstantiates()
    {
        foreach (var obj in instantiateSystems)
        {
            Instantiate(obj);
        }
    }
}
