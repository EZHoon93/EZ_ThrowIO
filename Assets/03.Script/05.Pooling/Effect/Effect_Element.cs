using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Effect_Element : EffectObject
{
    [SerializeField] LayerMask targetLayer;
    List<GameObject> reciveDamageObjects = new List<GameObject>();


    
    public void PlaySkill(int viewID, int level,  float range = 1.0f )
    {
        Play(range); //폭팔이펙트
        switch (effectContainer.sEffectType)
        {
            case EffectType.Fire:
                GetComponent<FirePlane>().Play(viewID, level,effectContainer.sEffectTime, range);
                break;
            case EffectType.Ice:
                Process_Ice(viewID, level , range );
                break;
            case EffectType.Lighting:
                Process_Lighting(viewID, level, range);
                break;
            case EffectType.Posion:
                Process_Posion(viewID, level, range);
                break;
        }
    }
    

    void Process_Ice(int shooterViewID, int level , float range)
    {
        print("슬로우");
        Collider[] colliders = new Collider[10];
        var hitCount = Physics.OverlapSphereNonAlloc(this.transform.position, range, colliders, targetLayer);
        if (hitCount > 0)
        {
            for (int i = 0; i < hitCount; i++)
            {

                PlayerController playerController = colliders[i].GetComponent<PlayerController>();
                // LivingEntity 컴포넌트가 존재하며, 해당 LivingEntity가 살아있다면,
                if (playerController != null)
                {
                    if (playerController.photonView.IsMine)
                    {
                        playerController.playerStats.CheckLoopEffect(EffectType.Loop_Slow, shooterViewID);
                    }
                }
            }
        }
    }

    void Process_Lighting(int shooterViewID, int level, float range   )
    {
        Collider[] colliders = new Collider[10];
        var hitCount = Physics.OverlapSphereNonAlloc(this.transform.position, range, colliders, targetLayer);
        if (hitCount > 0)
        {
            for (int i = 0; i < hitCount; i++)
            {
                PlayerController playerController = colliders[i].GetComponent<PlayerController>();
                // LivingEntity 컴포넌트가 존재하며, 해당 LivingEntity가 살아있다면,
                if (playerController != null)
                {
                    if (playerController.photonView.IsMine)
                    {
                        //확률에 걸리면
                        var proablity = Random.Range(0, 1);
                        if (proablity < DataEtc.Instance.S_ProbiblityStun * level)
                        {
                            playerController.playerStats.CheckLoopEffect(EffectType.Loop_Stun, shooterViewID);
                        }
                    }
                }
            }
        }
    }

    void Process_Posion(int shooterViewID, int elementLevel, float range   )
    {
        print("포이즌"+effectContainer.sEffectType);
        Collider[] colliders = new Collider[10];
        var hitCount = Physics.OverlapSphereNonAlloc(this.transform.position, range, colliders, targetLayer);
        if (hitCount > 0)
        {
            for (int i = 0; i < hitCount; i++)
            {
                PlayerController playerController = colliders[i].GetComponent<PlayerController>();
                // LivingEntity 컴포넌트가 존재하며, 해당 LivingEntity가 살아있다면,
                if (playerController != null)
                {
                    if (playerController.photonView.IsMine)
                    {
                        playerController.playerStats.CheckLoopEffect(EffectType.Loop_Posion , shooterViewID);
                    }
                }
            }
        }
    }

    IEnumerator ProcessFire(int viewID, int level, float range)
    {

        for (int j = 0; j < 10; j++)
        {
            Collider[] colliders = new Collider[10];
            var hitCount = Physics.OverlapSphereNonAlloc(this.transform.position, range, colliders, targetLayer);
            if (hitCount > 0)
            {
                for (int i = 0; i < hitCount; i++)
                {
                    IDamageable damageable = colliders[i].GetComponent<IDamageable>();
                    // LivingEntity 컴포넌트가 존재하며, 해당 LivingEntity가 살아있다면,
                    if (damageable != null)
                    {
                        if (damageable.IsMine())
                        {
                            if (reciveDamageObjects.Contains(colliders[i].gameObject) ==  false)
                            {
                                reciveDamageObjects.Add(colliders[i].gameObject);
                                StartCoroutine(RemoveDamageList(colliders[i].gameObject));
                                damageable.Local_ApplyDamage(viewID, DataBaseManager.fireDamage * level, Vector3.zero);
                            }
                            
                        }
                    }
                }
            }
            yield return new WaitForSeconds(0.2f);
        }

    }

    IEnumerator RemoveDamageList(GameObject gameObject)
    {
        yield return new WaitForSeconds(0.5f);
        reciveDamageObjects.Remove(gameObject);
    }


   
    private void OnTriggerStay(Collider other)
    {
        //print("대미지줌.."+other.gameObject );
        //if (effectType != EffectType.Fire) return;
        //if (reciveDamageObjects.Contains(other.gameObject)) return;
        //var damageable = other.gameObject.GetComponent<IDamageable>();
        //if (damageable != null)
        //{
        //    if (damageable.IsMine())
        //    {
        //        reciveDamageObjects.Add(other.gameObject);
        //        StartCoroutine(RemoveDamageList(other.gameObject));
        //        damageable.Local_ApplyDamage(0, damage, Vector3.zero);
        //    }
        //}

    }
}
