using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class FirePlane : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;
    Dictionary<GameObject, float> damageDic = new Dictionary<GameObject, float>();
    Collider collider;

    string enemyTag;
    int damage;
    int shooterViewID;
    int elementLevel;
    float range;
    float tickTime;
    float durationTime;

    private void Awake()
    {
        damage = DataEtc.Instance.S_FireDamage;
        tickTime = DataEtc.Instance.S_FireTickTime;
        collider = GetComponent<Collider>();
    }

    private void OnDisable()
    {
        damageDic.Clear();
    }
    public void Play(int _shooterViewID, int _level, float _durationTime, float _range)
    {
        shooterViewID = _shooterViewID;
        elementLevel = _level;
        durationTime = _durationTime;
        range = _range;
        collider.enabled = true;
        StartCoroutine(DestorySelf(_durationTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        var idamaeable = other.GetComponent<IDamageable>();
        if (idamaeable != null)
        {
            damageDic.Add(other.gameObject, Time.time);
            idamaeable.Local_ApplyDamage(shooterViewID, damage, Vector3.zero);
        }
    }

    private void OnTriggerStay(Collider other)
    {

        var idamaeable = other.GetComponent<IDamageable>();
        if (idamaeable != null)
        {
            float lastDamageTime;
            //목록에있는 캐릭이라면..
            if( damageDic.TryGetValue(other.gameObject , out lastDamageTime))
            {
                if (Time.time >= lastDamageTime + tickTime)
                {
                    idamaeable.Local_ApplyDamage(shooterViewID, damage, Vector3.zero);
                    damageDic[other.gameObject] = Time.time;
                }
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        var idamaeable = other.GetComponent<IDamageable>();
        if (idamaeable != null)
        {
            damageDic.Remove(other.gameObject);
        }
    }

    IEnumerator DestorySelf(float time)
    {
        yield return new WaitForSeconds(time * 0.9f);
        collider.enabled = false;


        float startTime = 0;
        float destroyTime = time * 0.1f;
        while (startTime <= destroyTime)
        {
            startTime += Time.deltaTime;
            var scale = range - range *( startTime / destroyTime);
            transform.localScale =
                new Vector3(scale, scale, scale);

            yield return null;
        }

        //Destroy(gameObject);
    }

    //IEnumerator ProcessFire(int viewID, int level, float range)
    //{

    //    for (int j = 0; j < 10; j++)
    //    {
    //        Collider[] colliders = new Collider[10];
    //        var hitCount = Physics.OverlapSphereNonAlloc(this.transform.position, range, colliders, targetLayer);
    //        if (hitCount > 0)
    //        {
    //            for (int i = 0; i < hitCount; i++)
    //            {
    //                IDamageable damageable = colliders[i].GetComponent<IDamageable>();
    //                // LivingEntity 컴포넌트가 존재하며, 해당 LivingEntity가 살아있다면,
    //                if (damageable != null)
    //                {
    //                    if (damageable.IsMine())
    //                    {
    //                        if (damageDic.ContainsKey(colliders[i].gameObject) == false)
    //                        {
    //                            //damageDic.Add(colliders[i].gameObject);
    //                            //StartCoroutine(RemoveDamageList(colliders[i].gameObject));
    //                            damageable.Local_ApplyDamage(viewID, DataBaseManager.fireDamage * level, Vector3.zero);
    //                        }

    //                    }
    //                }
    //            }
    //        }
    //        yield return new WaitForSeconds(0.2f);
    //    }

    //}
}
