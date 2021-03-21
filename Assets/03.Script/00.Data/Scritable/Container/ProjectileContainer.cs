using UnityEditor;

using UnityEngine;

[CreateAssetMenu(fileName = "PjC_", menuName = "EZ_Poolable/ProjectileContainer", order = 2)]


public class ProjectileContainer : PoolableContainer
{
    [SerializeField] ProjectileData projectileData;
    [SerializeField] Sprite sprite;
    [SerializeField] int needLevel;
    [SerializeField] string excContent;
    [SerializeField] string UIName;
    [SerializeField] Vector3 handPosion;
    [SerializeField] Vector3 handRotation;



    public ProjectileData sProjectileData => projectileData;
    public Sprite sSprite => sprite;

    public int sNeedLevel => needLevel;
    public string sContent => excContent;

    public string sUIName => UIName;

    public Vector3 sHandPosition=> handPosion;

    public Vector3 sHandRotation => handRotation;

}
