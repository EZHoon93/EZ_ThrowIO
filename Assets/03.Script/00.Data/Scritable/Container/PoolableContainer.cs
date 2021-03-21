using UnityEngine;

[CreateAssetMenu(fileName = "PoC_", menuName = "EZ_Poolable/PoolableData", order = 0)]

public class PoolableContainer : ScriptableObject
{
    [SerializeField] GameObject prefab;
    [SerializeField] int allowCount;
    [SerializeField] int addCount;

    public GameObject sPrefab => prefab;

    public string sId => this.name;

    public int sAllowCount => allowCount;
    public int sAddCount => addCount;


}
