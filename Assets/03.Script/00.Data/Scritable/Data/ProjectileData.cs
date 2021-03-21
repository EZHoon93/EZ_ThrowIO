using UnityEditor;

using UnityEngine;


[CreateAssetMenu(fileName = "PjD", menuName = "EZ_Data/ProjectileData", order = 2)]

public class ProjectileData : ProductData
{
    [SerializeField] int damage;
    [SerializeField] float range_explosion;
    [SerializeField] float range_reach;
    [SerializeField] int maxInstallCount;
    
    [SerializeField] int p_fire;
    [SerializeField] int p_ice;
    [SerializeField] int p_lighting;
    [SerializeField] int p_posion;


    public int sDamage => damage;
    public float sRange_explosion => range_explosion;
    public float sRange_reach => range_reach;
    public int sMaxInstallCount => maxInstallCount;

    
    public int sP_Fire => p_fire;
    public int sP_Ice => p_ice;
    public int sP_Light => p_lighting;
    public int sP_Posion => p_posion;


    

}
