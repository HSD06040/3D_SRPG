using HSD.DI;
using UnityEngine;

public interface IUnitFactory
{
    void Create(GameObject unitPrefab, Vector3 pos);
}

public class UnitFactory : MonoBehaviour, IUnitFactory
{
    private GameSystemManager gameSystemManager;

    #region Test
    [SerializeField] GameObject prefab;
    [ContextMenu("Create")]
    private void TestCreate()
    {
        Create(prefab, Vector3.zero);
    }
    #endregion

    [Inject]
    void Init(GameSystemManager gameSystemManager)
    {
        this.gameSystemManager = gameSystemManager;
    }

    public void Create(GameObject unitPrefab, Vector3 pos)
    {
        BaseUnit unit = Instantiate(unitPrefab, pos, Quaternion.identity).GetComponent<BaseUnit>();
    }
}
