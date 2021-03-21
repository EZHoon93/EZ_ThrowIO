using System.Collections;
using System.Collections.Generic;

using Photon.Pun;
using UnityEngine;

// 주어진 Gun 오브젝트를 쏘거나 재장전
// 알맞은 애니메이션을 재생하고 IK를 사용해 캐릭터 양손이 총에 위치하도록 조정
public class PlayerShooter : MonoBehaviourPun 
{


    PlayerInput playerInput;
    PlayerUI playerUI;
    PlayerStats playerStats;

    [Header("컴포넌트")]
    [SerializeField] LayerMask mapLayer;
    [SerializeField] LayerMask autoTargetLayer;
    [SerializeField] LineRenderer zoomLineRenderer;
    
    
    [Header("Transform")]
    [SerializeField] Transform upperSpine;
    [SerializeField] Transform fireTransform;
    [SerializeField] Transform addTransform;    //추가 회전값 좌표를 얻기위한 임시 트랜스폼

    //[SerializeField] Transform handTransform;

    [SerializeField] int maxcollisonCount;

    protected Vector3 upperBodyDir;
    protected bool rotate = false;

    public float maxRangeRadidus => projectileObject.projectileData.sRange_reach;
    public float maxRangeReach => projectileObject.projectileData.sRange_reach;
    public ProjectileObject projectileObject { get; private set; }
    public string projectileId { get; private set; }

    public Vector3 targetPoint;
    #region Awake,Reset,Updates

    //Animator playerAnimator;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerUI = GetComponent<PlayerUI>();
        playerStats = GetComponent<PlayerStats>();
    }
    public void SetupProjectileObject(ProjectileObject _projectileObject , string _projectileId) 
    {
        projectileObject = _projectileObject;
        projectileId = _projectileId;
    }
   
    public void ResetComponent()
    {
        this.enabled = true;
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        
        //User
        if (playerInput.MyCharacter)
        {
            //자동공격
            if (playerInput.AttackAuto)
            {
                if (playerStats.State == PlayerState.Run || playerStats.noInputStats.Count > 0) return;
                AttackAuto();
                return;
            }
            else
            {
                Zoom(playerInput.AttackVector);
                if (playerInput.AttackVector.sqrMagnitude == 0)
                {
                    SkillZoom(playerInput.SkillVector);

                }
                if (playerStats.State == PlayerState.Run || playerStats.noInputStats.Count > 0) return;
                AttackManual(playerInput.LastAttackVector);
                SkillManual(playerInput.LastSkillVector);
            }

        }

        //AI
        else
        {

        }


        
    }

    private void LateUpdate()
    {

        if (rotate)
        {
            if (upperBodyDir == Vector3.zero) return;
            if(upperSpine == null)
            {
                upperSpine = GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Spine);
                
            }
            Vector3 spineRot = Quaternion.LookRotation(upperBodyDir).eulerAngles;
            spineRot -= transform.eulerAngles;

            upperSpine.transform.localRotation = Quaternion.Euler(
                upperSpine.transform.localEulerAngles.x + spineRot.y,
                upperSpine.transform.localEulerAngles.y,
                upperSpine.transform.localEulerAngles.z
                );

        }


    }

    #endregion

   

    #region Zoom & Attack
    public void Zoom(Vector2 inputVector2)
    {
        if (inputVector2.sqrMagnitude == 0)
        {
            //zoomLineRenderer.enabled = false;
            playerUI.GetDamageUI().gameObject.SetActive(false);
            return;
        }
        Plane playerPlane = new Plane(Vector3.up, this.transform.position);
        Vector3 theArc;
        Vector3 center;
        var temp = new Vector3(inputVector2.x, 0, inputVector2.y);  //direct 변환을 위해 사용
        fireTransform.localPosition = inputVector2.normalized * 0.4f;
        //upperBodyDir = temp;
        var target = this.transform.position + (temp * maxRangeRadidus); //타겟범위
        float hitdist; // out 값.
        
        //레이캐스트
        Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(target));
        targetPoint = Vector3.zero;
        if (playerPlane.Raycast(ray, out hitdist))
        {
            targetPoint = ray.GetPoint(hitdist);
            center = (fireTransform.position + targetPoint) * 0.5f;
            center.y -= 0.5f;
            //Quaternion targetRotation = Quaternion.LookRotation(center - fireTransform.position);
            //transform.rotation = Quaternion.Slerp(fireTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            RaycastHit hitInfo;

            if (Physics.Linecast(fireTransform.position, targetPoint, out hitInfo, mapLayer))
            {
                targetPoint = hitInfo.point;
            }
        }
        else
        {
            targetPoint = this.transform.position;
            center = Vector3.zero;
        }

        playerUI.UpdateDamageUI(targetPoint , projectileObject.projectileData.sRange_explosion);   //타겟 위치 UI 표시
        Vector3 RelCenter = fireTransform.position - center;
        Vector3 aimRelCenter = targetPoint - center;
        for (float index = 0.0f, interval = -0.0417f; interval < 1.0f;)
        {
            theArc = Vector3.Slerp(RelCenter, aimRelCenter, interval += 0.0417f);
            zoomLineRenderer.SetPosition((int)index++, theArc + center);
        }
        

        zoomLineRenderer.enabled = true;
    }

  
    void AttackAuto()
    {
        if (playerStats.CurrentEnergy < 1)
        {
            playerUI.ShakeEnergy(1);
            return;
        }//최소 공격에너지 값. 1개이상.
        if (playerStats.State == PlayerState.Throwing) return;


        Collider[] colliders = new Collider[maxcollisonCount];
        var hitCount = Physics.OverlapSphereNonAlloc(this.transform.position, maxRangeRadidus, colliders, autoTargetLayer);
        Transform target = null;
        float minDistance = 9999;
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

                    var newDistacne = Vector3.Distance(this.transform.position, colliders[i].transform.position);
                    if (newDistacne <= minDistance && newDistacne >= 1)
                    {
                        target = colliders[i].transform;
                        minDistance = newDistacne;
                    }
                }
            }
        }
        if (target != null)
        {
            var startPoint = fireTransform.position;
            var endPoint = target.position;
            playerStats.CurrentEnergy--;
            playerStats.State = PlayerState.Throwing;
            photonView.RPC("AttackOnServer", RpcTarget.AllViaServer, startPoint, endPoint);
        }
    }

    void AttackManual(Vector2 inputVector2)
    {
        if (inputVector2.magnitude == 0.0f) return;

        if (playerStats.CurrentEnergy < 1)
        {
            playerUI.ShakeEnergy(1);
            return;
        }//최소 공격에너지 값. 1개이상.
        if (playerStats.State == PlayerState.Throwing) return;
        playerStats.CurrentEnergy--;
        playerStats.State = PlayerState.Throwing;

        var startPoint = zoomLineRenderer.GetPosition(0);
        var endPoint = zoomLineRenderer.GetPosition(24);
        photonView.RPC("AttackOnServer", RpcTarget.AllViaServer, startPoint, endPoint);
    }

  

    [PunRPC]
    public void AttackOnServer(Vector3 startPoint, Vector3 endPoint  , PhotonMessageInfo photonMessageInfo)
    {
        var gap = PhotonNetwork.Time - photonMessageInfo.SentServerTime;
        ProjectileInfo projectileInfo;
        projectileInfo.viewID = photonMessageInfo.photonView.ViewID; ;
        projectileInfo.playerStats = this.playerStats;
        //projectileInfo.startPoint = startPoint;
        projectileInfo.startPoint = fireTransform.position;
        projectileInfo.endPoint = endPoint;
        projectileInfo.damageLevel = playerStats.playerAbilityStats.AbilityStatsDic["SP_d"];       //대미지 코드
        projectileInfo.rangeLevel = playerStats.playerAbilityStats.AbilityStatsDic["SP_r"];    //대미지범위코드
        projectileInfo.projectileSpeedLevel = playerStats.playerAbilityStats.AbilityStatsDic["SP_v"];   //무기 속도
        projectileInfo.p_fire = playerStats.playerAbilityStats.AbilityStatsDic["PP_f"];
        projectileInfo.p_ice = playerStats.playerAbilityStats.AbilityStatsDic["PP_i"];
        projectileInfo.p_lighting = playerStats.playerAbilityStats.AbilityStatsDic["PP_l"];
        projectileInfo.p_posion = playerStats.playerAbilityStats.AbilityStatsDic["PP_p"];
        upperBodyDir = endPoint - this.transform.position;
        upperBodyDir.y = 0;
        StartCoroutine(AttackProcessOnAllClinets(projectileInfo));
    }

    IEnumerator AttackProcessOnAllClinets(ProjectileInfo _projectileInfo)
    {
        yield return new WaitForSeconds(0.2f);  //던지기애니메이션 위한 딜레이
        rotate = true;      //회전 ok
        StartCoroutine(ProjectileAnimation(0.5f));
        CreateProjectile(_projectileInfo);
        
        yield return new WaitForSeconds(0.5f);  //다시 회전 및애니메이션 돌아옴

        //if (playerStats.abilityStats.p_multy > 0)
        //{
        //    CreateProjectile(_projectileInfo);
        //    yield return new WaitForSeconds(0.3f);  //다시 회전 및애니메이션 돌아옴
        //}
        
        

        playerStats.State = PlayerState.Idle;
        rotate = false;
    }

    IEnumerator ProjectileAnimation(float delayTime)
    {
        var orinalSize = projectileObject.transform.localScale.x;
        print("원래사이즈 " + orinalSize);
        projectileObject.transform.localScale = Vector3.zero;
        var startTime = Time.time;
        float currentSize = 0;
        var currentTime = Time.time - startTime;
        while(currentTime < delayTime)
        {
            currentTime = Time.time - startTime;
            currentSize =  orinalSize * (currentTime/delayTime);
            projectileObject.transform.localScale = new Vector3(currentSize, currentSize, currentSize);    
            yield return null;
        }
        projectileObject.transform.localScale = new Vector3(orinalSize, orinalSize, orinalSize);
    }

    void CreateProjectile(ProjectileInfo _projectileInfo)
    {
        
            //ProjectileInfo _projectileInfoAdd = _projectileInfo;
            //var throwObject1 = ObjectPoolManger.Instance.PopProjectileObject(projectileId) as ProjectileObject;
            //var throwObject2 = ObjectPoolManger.Instance.PopProjectileObject(projectileId) as ProjectileObject;
            //addTransform.position = _projectileInfo.endPoint;
            //addTransform.RotateAround(_projectileInfo.startPoint, Vector3.up, -7.5f);

            //_projectileInfo.endPoint = addTransform.position;
            //addTransform.RotateAround(_projectileInfo.startPoint, Vector3.up, 15f);
            //_projectileInfoAdd.endPoint = addTransform.position;


            //_projectileInfo.startPoint = fireTransform.position;
            //_projectileInfoAdd.startPoint = fireTransform.position;

            //throwObject1.Setup(_projectileInfo);
            //throwObject2.Setup(_projectileInfoAdd);

        _projectileInfo.startPoint = fireTransform.position;
        var throwObject = ObjectPoolManger.Instance.PopProjectileObject(projectileId) as ProjectileObject;
        throwObject.Play(_projectileInfo);
    }

    #endregion

    #region Skill




    public void SkillZoom(Vector2 inputVector2)
    {
        if (inputVector2.sqrMagnitude == 0)
        {
            zoomLineRenderer.enabled = false;
            playerUI.GetSkillDamageUI().gameObject.SetActive(false);
            return;
        }

        if (inputVector2.sqrMagnitude <= 0.1f) return;
        Plane playerPlane = new Plane(Vector3.up, this.transform.position);
        Vector3 theArc;
        Vector3 center;
        var temp = new Vector3(inputVector2.x, 0, inputVector2.y);  //direct 변환을 위해 사용
        fireTransform.localPosition = inputVector2.normalized * 0.4f;
        //upperBodyDir = temp;
        var target = this.transform.position + (temp * maxRangeRadidus); //타겟범위
        float hitdist; // out 값.

        //레이캐스트
        Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(target));
        targetPoint = Vector3.zero;
        if (playerPlane.Raycast(ray, out hitdist))
        {
            targetPoint = ray.GetPoint(hitdist);
            center = (fireTransform.position + targetPoint) * 0.5f;
            center.y -= 0.5f;
            //Quaternion targetRotation = Quaternion.LookRotation(center - fireTransform.position);
            //transform.rotation = Quaternion.Slerp(fireTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            RaycastHit hitInfo;

            if (Physics.Linecast(fireTransform.position, targetPoint, out hitInfo, mapLayer))
            {
                targetPoint = hitInfo.point;
            }
        }
        else
        {
            targetPoint = this.transform.position;
            center = Vector3.zero;
        }

        playerUI.UpdateSkillDamageUI(targetPoint ,projectileObject.projectileData.sRange_explosion );   //타겟 위치 UI 표시
        Vector3 RelCenter = fireTransform.position - center;
        Vector3 aimRelCenter = targetPoint - center;
        for (float index = 0.0f, interval = -0.0417f; interval < 1.0f;)
        {
            theArc = Vector3.Slerp(RelCenter, aimRelCenter, interval += 0.0417f);
            zoomLineRenderer.SetPosition((int)index++, theArc + center);
        }

        zoomLineRenderer.enabled = true;
    }



    void SkillManual(Vector2 inputVector2)
    {
        if (inputVector2.magnitude <= 0.1f) return;
        if (playerStats.CurrentEnergy < 1) return;   //최소 공격에너지 값. 1개이상.
        if (playerStats.State == PlayerState.Throwing) return;
        playerStats.CurrentEnergy--;
        playerStats.State = PlayerState.Skill;
        var startPoint = zoomLineRenderer.GetPosition(0);
        var endPoint = zoomLineRenderer.GetPosition(24);
        photonView.RPC("SkillOnServer", RpcTarget.AllViaServer, startPoint, endPoint);

    }



    [PunRPC]
    public void SkillOnServer(Vector3 startPoint, Vector3 endPoint, PhotonMessageInfo photonMessageInfo)
    {
        print("스킬!!!");
        var gap = PhotonNetwork.Time - photonMessageInfo.SentServerTime;
        ProjectileInfo projectileInfo;
        projectileInfo.viewID = photonMessageInfo.photonView.ViewID; ;
        projectileInfo.playerStats = this.playerStats;
        //projectileInfo.startPoint = startPoint;
        projectileInfo.startPoint = fireTransform.position;
        projectileInfo.endPoint = endPoint;
        projectileInfo.damageLevel = playerStats.playerAbilityStats.AbilityStatsDic["SP_d"];       //대미지 코드
        projectileInfo.rangeLevel = playerStats.playerAbilityStats.AbilityStatsDic["SP_r"];    //대미지범위코드
        projectileInfo.projectileSpeedLevel = playerStats.playerAbilityStats.AbilityStatsDic["SP_v"];   //무기 속도
        projectileInfo.p_fire = playerStats.playerAbilityStats.AbilityStatsDic["PP_f"];
        projectileInfo.p_ice = playerStats.playerAbilityStats.AbilityStatsDic["PP_i"];
        projectileInfo.p_lighting = playerStats.playerAbilityStats.AbilityStatsDic["PP_l"];
        projectileInfo.p_posion = playerStats.playerAbilityStats.AbilityStatsDic["PP_p"];
        upperBodyDir = endPoint - this.transform.position;
        upperBodyDir.y = 0;
        StartCoroutine(SkillProcessOnAllClinets(projectileInfo));
    }
    IEnumerator SkillProcessOnAllClinets(ProjectileInfo _projectileInfo)
    {
        yield return new WaitForSeconds(0.2f);  //던지기애니메이션 위한 딜레이
        rotate = true;      //회전 ok

        for(int i = 0; i < 5; i++)
        {
            SkillCreateProjectile(_projectileInfo, i);
            yield return new WaitForSeconds(0.2f);  //다시 회전 및애니메이션 돌아옴
        }
        yield return new WaitForSeconds(0.2f);  //다시 회전 및애니메이션 돌아옴

        playerStats.State = PlayerState.Idle;
        rotate = false;

    }
    void SkillCreateProjectile(ProjectileInfo _projectileInfo , int index)
    {
        _projectileInfo.startPoint = fireTransform.position;
        addTransform.rotation = this.transform.rotation;
        
        var angle = 0;
        switch (index)
        {
            case 0:
                addTransform.position = _projectileInfo.endPoint;
                break;
            case 1:
                angle = -45;
                addTransform.position = _projectileInfo.endPoint + (addTransform.forward * projectileObject.projectileData.sRange_explosion);
                break;
            case 2:
                angle = 45;
                addTransform.position = _projectileInfo.endPoint + (addTransform.forward * projectileObject.projectileData.sRange_explosion);
                break;
            case 3:
                angle = -135;
                addTransform.position = _projectileInfo.endPoint + (addTransform.forward * projectileObject.projectileData.sRange_explosion);
                break;
            case 4:
                angle = 135;
                addTransform.position = _projectileInfo.endPoint + (addTransform.forward * projectileObject.projectileData.sRange_explosion);
                break;
        }
        addTransform.RotateAround(_projectileInfo.endPoint, Vector3.up, angle);
        _projectileInfo.endPoint = addTransform.position;
        var throwObject = ObjectPoolManger.Instance.PopProjectileObject(projectileId) as ProjectileObject;
        throwObject.Play(_projectileInfo);

    }
    #endregion


}