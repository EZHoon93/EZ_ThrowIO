using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ThrowObjectInfo
{
    public int viewID;
    public Vector3 startPoint;
    public Vector3 endPoint;
    public int damage;
    public float range;
    public float speed;
}

public class ThrowObject : MonoBehaviour , IPrefabPoolable<ThrowPool>
{
    [SerializeField] ThrowPool Pool;
    ThrowObjectInfo throwObjectInfo;

    [SerializeField] Transform modelObject;
    [SerializeField] float arriveMinTime;    //최소 도착 시간
    [SerializeField] float arriveMaxtime;    //최대 도착시간
    [SerializeField] int maxcollisonCount;

    [SerializeField] LayerMask targetLayer;
    [SerializeField] ParticleSystem[] expolsionEffect;

    public void Create(ThrowPool pool)
    {
        Pool = pool;
    }

    public void Push()
    {
        Pool.PushObject(this);
    }

    public void Setup(ThrowObjectInfo newThrowObjectInfo)
    {
        throwObjectInfo = newThrowObjectInfo;

        StartCoroutine(ThrowSimulator());
    }

    IEnumerator ThrowSimulator()
    {
        var distance = Vector3.Distance(throwObjectInfo.startPoint, throwObjectInfo.endPoint);
        this.transform.position = throwObjectInfo.startPoint;
        var center = (throwObjectInfo.endPoint + throwObjectInfo.startPoint) * 0.5f;
        center.y -= 2.0f;
        float arriveTime = arriveMinTime + distance * 0.1f;
        arriveTime = arriveTime < arriveMaxtime ? arriveTime : arriveMaxtime;   //arriveTime은 최대값을 넘길수없음, max보다작으면 그대로
        modelObject.gameObject.SetActive(true);

        for (float i = 0; i < 1.0f; i += Time.deltaTime / arriveTime)
        {
            Vector3 RelCenter = throwObjectInfo.startPoint - center;
            Vector3 aimRelCenter = throwObjectInfo.endPoint - center;
            var pos = Vector3.Slerp(RelCenter, aimRelCenter, i);    //시간
            modelObject.Rotate(120 * Time.deltaTime, 60 * Time.deltaTime, 120 * Time.deltaTime);
            this.transform.position = center + pos;
            yield return null;
        }

        Explosion();
        yield return new WaitForSeconds(2.0f);

        this.Push();

    }

    public void Explosion()
    {
        print("Exp");

        modelObject.gameObject.SetActive(false);
        foreach (var e in expolsionEffect)
        {
            e.Play();
        }
        //expolsionEffect.Play();
        // 단, targetLayers에 해당하는 레이어를 가진 콜라이더만 가져오도록 필터링
        Collider[] colliders = new Collider[maxcollisonCount];
        var hitCount = Physics.OverlapSphereNonAlloc(this.transform.position, throwObjectInfo.range, colliders, targetLayer, QueryTriggerInteraction.Ignore);

        if (hitCount > 0)
        {
            // 모든 콜라이더들을 순회하면서, 살아있는 플레이어를 찾기
            for (int i = 0; i < hitCount; i++)
            {
                print("수류탄대미지");
                // 콜라이더로부터 LivingEntity 컴포넌트 가져오기
                LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();
                // LivingEntity 컴포넌트가 존재하며, 해당 LivingEntity가 살아있다면,
                if (livingEntity != null)
                {
                    if (livingEntity.photonView.IsMine)
                    {
                        livingEntity.Local_ApplyDamage(throwObjectInfo.viewID, throwObjectInfo.damage, Vector3.zero);
                    }
                }
            }

        }

    }


}
