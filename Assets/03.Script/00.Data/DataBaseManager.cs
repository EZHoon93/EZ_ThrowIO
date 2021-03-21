using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataBaseManager 
{
    #region Health
    public static readonly int AddMaxHealth = 50;
    public static readonly float AddHealAmountRatio = 0.2f;
    public static readonly int AddHealRecoeryAmount = 50; //힐 사용시 회복량
    #endregion

    #region Move
    public static readonly float AddMoveSpeedRatio = 0.05f; //이동속도 증가량
    //public static readonly float AddMoveItemRatio;  //아이템으로이동속도 증가량
    public static readonly float AddDodgeTimeRatio = 0.2f;    //구르기 쿨타임 감소 비율
    #endregion

    #region Shooter
    public static readonly float AddAmmoDamageRatio = 0.05f;  //대미지 증가
    public static readonly float AddAmmoRegenRatio = -0.2f;   //장전 속도
    public static readonly float AddAmmoDistanceRatio = 0.1f;    //사거리증가
    public static readonly float AddGrenadeRangeRatio = 0.2f;    //수류탄 범위증가
    public static readonly float AddGrenadeTimeRatio  = -0.2f;    //수류탄 쿨타임
    public static readonly float AddGrenadeDamageRatio = 0.2f;  //수류틴 대미지 증가.
    public static readonly int AddBackPackCount = 1;
    #endregion

    #region Fog
    public static readonly float AddSightRatio = 0.1f;    //시야증가
    #endregion

    public static readonly int fireDamage = 50;
    
    //public static readonly int HumanHealth = 200;
    //public static readonly float GrenadeTime;    //수류탄 쿨타임
    //public static readonly float GrenadeRange;    //수류탄 쿨타임
    //public static readonly float DodgeTime;    //수류탄 쿨타임
    //public static readonly float DodgeSpeed;    //수류탄 쿨타임







    public static void InitializedData()
    {

     
    }

    

}
