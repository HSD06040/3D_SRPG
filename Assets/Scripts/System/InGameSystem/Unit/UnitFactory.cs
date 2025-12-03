using UnityEngine;

public interface IUnitFactory
{
    void Create(GameObject unitPrefab, Vector3 pos);
}

public class UnitFactory : MonoBehaviour, IUnitFactory
{
    #region Test
    [SerializeField] GameObject prefab;
    [ContextMenu("Create")]
    private void TestCreate()
    {
        Create(prefab, Vector3.zero);
    }
    #endregion

    public void Create(GameObject unitPrefab, Vector3 pos)
    {
        BaseUnit unit = Instantiate(unitPrefab, pos, Quaternion.identity).GetComponent<BaseUnit>();
    }
}
