using UnityEditor;

using UnityEngine;

[CreateAssetMenu(fileName = "EfC_ Data", menuName = "EZ_Poolable/Create Effect", order = 4)]

public class EffectContainer : PoolableContainer
{
    [SerializeField] EffectType effectType;

    [SerializeField] float effectTime;

    [SerializeField] int effectCode ;
    public EffectType sEffectType => effectType;
    public float sEffectTime => effectTime;

    public int sEffectCode => effectCode;


    public void Inialize()
    {
        effectCode = (int)effectType;
    }

}
