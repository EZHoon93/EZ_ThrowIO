using System.Collections;
using System.Collections.Generic;

using DG.Tweening;

using UnityEngine;

public struct ProjectileInfo
{
    public int viewID;
    public PlayerStats playerStats;
    public Vector3 startPoint;
    public Vector3 endPoint;
    public int damageLevel;
    public float projectileSpeedLevel;
    public float rangeLevel;
    public int p_fire;
    public int p_ice;
    public int p_lighting;
    public int p_posion;

}

public class ProjectileObject : PoolableObject
{
    
    public ProjectileData projectileData { get; private set; }
    public MeshRenderer meshRenderer;
    [SerializeField] Transform modelObject;
    [SerializeField] Transform rotationObject;
    [SerializeField] float arriveMinTime;    //최소 도착 시간
    [SerializeField] int maxcollisonCount;
    [SerializeField] LayerMask targetLayer;

    [Header("FX")]
    [SerializeField] ParticleSystem expolsionEffect;
    [SerializeField] ParticleSystem trailFX;

    ProjectileInfo projectileInfo;
    bool isPlay = false;


    public int Damage 
    {
        get => (int)(projectileData.sDamage * (1 + projectileInfo.damageLevel * DataEtc.Instance.S_AddDamageRatio));
    }

    public float Range
    {
        get => projectileData.sRange_explosion* (1 + projectileInfo.rangeLevel * DataEtc.Instance.S_AddRangeRatio);
    }
    public float Speed
    {
        get => 0.13f;
    }

    public int E_Fire
    {
        get => projectileData.sP_Fire + projectileInfo.p_fire;
    }

    public int E_Ice
    {
        get => projectileData.sP_Ice + projectileInfo.p_ice;
    }

    public int E_Posion
    {
        get => projectileData.sP_Posion + projectileInfo.p_posion;
    }

    public int E_Lighting
    {
        get => projectileData.sP_Light + projectileInfo.p_lighting;
    }


    public float ArriveMaxTime 
    {
        get => arriveMinTime + (Speed * 10);
    }

    public void Play(ProjectileInfo newProjectileInfo)
    {
        projectileInfo = newProjectileInfo;
        StartCoroutine(ThrowSimulator());
    }

    private void Reset()
    {
        meshRenderer = rotationObject.GetComponentInChildren<MeshRenderer>();
    }
    void OnEnable()
    {
        
        isPlay = false;
        if (trailFX)
        {
            trailFX.gameObject.SetActive(false);
        }
        rotationObject.rotation = Quaternion.Euler(Vector2.zero);
    }

    IEnumerator ThrowSimulator()
    {
        yield return null;  //onEnable기다림.
        //패시브 스킬 구분.
        isPlay = true;
        if (trailFX)
        {
            trailFX.gameObject.SetActive(true);
        }

        #region Dotween

        var distance = Vector3.Distance(projectileInfo.startPoint, projectileInfo.endPoint);
        this.transform.position = projectileInfo.startPoint;

        float arriveTime = arriveMinTime + (distance * Speed);     // 최대거리 10 *0.1f => 1초,  10프로씩 증
        arriveTime = arriveTime < ArriveMaxTime ? arriveTime : ArriveMaxTime;   //arriveTime은 최대값을 넘길수없음, max보다작으면 그대로
        modelObject.gameObject.SetActive(true);

        //transform.DOJump(projectileInfo.endPoint, arriveTime * 1.4f, 1, arriveTime);
        //yield return new WaitForSeconds(arriveTime - 0.3f);
        transform.DOMoveX(projectileInfo.endPoint.x, arriveTime).SetEase(Ease.Linear);
        transform.DOMoveZ(projectileInfo.endPoint.z, arriveTime).SetEase(Ease.Linear);
        transform.DOMoveY(0.3f + distance * 0.6f, arriveTime * 0.5f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(arriveTime * 0.5f);
        transform.DOMoveY(projectileInfo.endPoint.y, arriveTime * 0.5f).SetEase(Ease.InSine);
        yield return new WaitForSeconds(arriveTime * 0.5f);
        #endregion

        isPlay = false;
        if (trailFX)
        {
            trailFX.gameObject.SetActive(false);
        }
        Explosion(); //속성 구분
        yield return new WaitForSeconds(3.0f);

        this.Push();
    }

   

    public void Explosion()
    {
        
        modelObject.gameObject.SetActive(false);
        expolsionEffect.Play();

        var _groundEffect = ObjectPoolManger.Instance.PopEffectObject(EffectType.Ground) as EffectObject;
        _groundEffect.transform.position = this.transform.position;
        _groundEffect.Play(Range);


        bool element = false;
        if (E_Fire> 0)
        {
            var _effectObject = ObjectPoolManger.Instance.PopEffectObject(EffectType.Fire) as Effect_Element;
            _effectObject.transform.position = this.transform.position;
            _effectObject.PlaySkill(projectileInfo.viewID , projectileInfo.p_fire , Range);
            element = true;
        }
        if (E_Ice> 0)
        {
            var _effectObject = ObjectPoolManger.Instance.PopEffectObject(EffectType.Ice) as Effect_Element;
            _effectObject.transform.position = this.transform.position;
            _effectObject.PlaySkill(projectileInfo.viewID, projectileInfo.p_ice, Range);

            element = true;
        }
        if (E_Lighting> 0)
        {
            var _effectObject = ObjectPoolManger.Instance.PopEffectObject(EffectType.Lighting) as Effect_Element;
            _effectObject.transform.position = this.transform.position;
            _effectObject.PlaySkill(projectileInfo.viewID, projectileInfo.p_lighting, Range);

            element = true;
        }
        if (E_Posion> 0)
        {
            var _effectObject = ObjectPoolManger.Instance.PopEffectObject(EffectType.Posion) as Effect_Element;
            
            _effectObject.transform.position = this.transform.position;
            _effectObject.PlaySkill(projectileInfo.viewID, projectileInfo.p_posion, Range);

            element = true;
        }

        if (!element)
        {
            var _effectObject = ObjectPoolManger.Instance.PopEffectObject(EffectType.Smoke) as EffectObject;
            _effectObject.transform.position = this.transform.position;
            _effectObject.Play(Range);
        }

   
        //expolsionEffect.Play();
        // 단, targetLayers에 해당하는 레이어를 가진 콜라이더만 가져오도록 필터링
        Collider[] colliders = new Collider[maxcollisonCount];
        var hitCount = Physics.OverlapSphereNonAlloc(this.transform.position, Range, colliders, targetLayer);

        if (hitCount > 0)
        {
            // 모든 콜라이더들을 순회하면서, 살아있는 플레이어를 찾기
            for (int i = 0; i < hitCount; i++)
            {
                // 콜라이더로부터 LivingEntity 컴포넌트 가져오기
                IDamageable damageable = colliders[i].GetComponent<IDamageable>();
                // LivingEntity 컴포넌트가 존재하며, 해당 LivingEntity가 살아있다면,
                if (damageable != null)
                {
                    
                    damageable.Local_ApplyDamage(projectileInfo.viewID, Damage, Vector3.zero);
                }
            }
        }

        if (projectileInfo.playerStats)
        {
            print("게이지이징지");
            projectileInfo.playerStats.SkillGage += 10 * hitCount;  //맞춘 수만큼 스킬 게이지증가.
        }


        if (IsView(this.transform.position))
        {
            CameraManager.instance.ShakeCamera(0.3f, 5, 5);
        }




    }
    public bool IsView(Vector3 pos)
    {

        var viewPos = Camera.main.WorldToViewportPoint(pos);
        if (viewPos.x > 0.0F && viewPos.x < 1.0F)
        {
            //움직이고 있는 상태만
            return true;
        }
        else
        {
            return false;
        }
    }


    public void SetupData(ProjectileData _projectileData) => projectileData = _projectileData;

    private void Update()
    {
        if (isPlay)
        {
            //modelObject.localRotation = Quaternion.Lerp(modelObject.rotation, Quaternion.Euler(Vector3.zero), 80 * Time.deltaTime);
            this.rotationObject.transform.Rotate(0, 0, 120 * Time.deltaTime, Space.Self);
        }
    }

}
