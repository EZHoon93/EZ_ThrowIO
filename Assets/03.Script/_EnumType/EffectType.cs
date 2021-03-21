using UnityEngine;

public enum EffectType : int
{
    Null = 0,

    Smoke = 20,
    Fire,
    Ice,
    Lighting,
    Posion,
    Ground,

    AbilityUp = 40,   //즉시시전 이펙트
    CoinExplsion,
    Heal,
    Trap,
    Dark,





    Loop_Stun = 70,  //지속 이펙트
    Loop_Shield,
    Loop_Posion,
    Loop_Run,
    Loop_Slow,
    Loop_Sleep,
    Loop_Dark
}
