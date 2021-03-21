using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using System.Linq;


public class PlayerStats : LivingEntity, IPunObservable
{

    public PlayerInput playerInput { get; private set; }
    public PlayerUI playerUI { get; private set; }
    public Animator playerAnimator { get; private set; }
    public CharacterObject characterObject { get; private set; }
    public PlayerScore playerScore { get; private set; }
    public PlayerAbilityStats playerAbilityStats { get; private set; }

    public GameObject playerObject;
    public Transform handTransform;

    [SerializeField] float recoveryTimeBet = 1.5f;
    [SerializeField] float recvoeryLastTime;
    [SerializeField] float recoveryWaitTime = 3.0f;

    public bool isIk;   //IK애니메이션 여부
    public bool isTransparent; //투명 여부
    public bool isDectet { get; set; } //부쉬속에서 다른유저들이 보일경우
    float speedBufferRatio;
    public Dictionary<EffectType, BuffState> n_loopEffectDic = new Dictionary<EffectType, BuffState>();
    public Dictionary<EffectType, float> speedBuffDic = new Dictionary<EffectType, float>();
    public List<EffectType> noInputStats = new List<EffectType>();  //스턴 및 수면같은 상태들모음


    [SerializeField] List<GameObject> canTransparentObjectList = new List<GameObject>();

    #region 프로퍼티
    PlayerState state;
    public PlayerState State
    {
        get => state;
        set
        {

            if (state == value) return;
            state = value;
            switch (state)
            {
                case PlayerState.Idle:
                    isIk = true;
                    playerAnimator.SetFloat("Speed", 0.0f);
                    break;
                case PlayerState.Move:
                    isIk = true;
                    playerAnimator.SetFloat("Speed", 1.0f);
                    break;
                case PlayerState.Throwing:
                    isIk = false;
                    playerAnimator.SetTrigger("Attack");
                    recvoeryLastTime = Time.time + recoveryWaitTime;
                    break;
                case PlayerState.Die:
                    isIk = false;
                    playerAnimator.SetTrigger("Die");
                    recvoeryLastTime = Time.time + recoveryWaitTime;
                    break;
                case PlayerState.Skill:
                    isIk = false;
                    playerAnimator.SetTrigger("Skill");
                    recvoeryLastTime = Time.time + recoveryWaitTime;
                    break;
                case PlayerState.Stun:
                    isIk = true;
                    playerAnimator.SetFloat("Speed", 0.0f);
                    recvoeryLastTime = Time.time + recoveryWaitTime;
                    break;
                case PlayerState.Run:
                    isIk = true;
                    playerAnimator.SetFloat("Speed", 1.0f);
                    recvoeryLastTime = Time.time + recoveryWaitTime;
                    break;
                case PlayerState.Sleep:
                    isIk = true;
                    playerAnimator.SetFloat("Speed", 0.0f);
                    recvoeryLastTime = Time.time + recoveryWaitTime;
                    break;
            }
        }
    }




    public override int MaxHealth
    {
        get => base.MaxHealth;
        set
        {

            if (m_MaxHealth == value) return;
            var healAmount = value - m_MaxHealth;  //최대체력 증가함으로써 체력회복 양
            m_MaxHealth = value;
            m_CurrentHealth += healAmount;
            //playerUI.SetUpMaxHealthSlider(m_MaxHealth);
            playerUI.GetHealthSlider().value = m_CurrentHealth;
        }
    }

    public override int CurrentHealth {
        get => base.CurrentHealth;
        set
        {
            //if (m_CurrentHealth == value) return;
            m_CurrentHealth = Mathf.Clamp(value ,0 , MaxHealth);
            UpdageHealthUI();
        }

    }
    [SerializeField] float currentEnrgy;
    public float CurrentEnergy
    {
        get => currentEnrgy;
        set
        {
            //if (currentEnrgy == value) return;
            if (value >= maxEnergy)
            {
                currentEnrgy = maxEnergy;
            }
            else
            {
                currentEnrgy = value;
            }
            if (photonView.IsMine)
            {
                UpdateEnergyUI();
            }
        }
    }

    [SerializeField] float maxEnergy;
    public float MaxEnergy
    {
        get => maxEnergy;
        set
        {
            if (maxEnergy == value) return;
            maxEnergy = value;
        }
    }
    [SerializeField] int skillGage;

    public int SkillGage
    {
        get => skillGage;
        set
        {
            if (skillGage == value) return;
            skillGage = value;
            if (value > 100)
            {
                skillGage = 100;
            }
            if (playerInput.MyCharacter)
            {
                print("게이지");
                UltimateJoystick.GetUltimateJoystick("Skill").GetComponent<SkillButton>().SetupFillAmount(skillGage *0.01f);
                if (SkillGage >= 100)
                {
                    //스킬게이지 찼음 이펙트. 및 컨트롤러 on
                    skillGage = 100;
                    UltimateJoystick.GetUltimateJoystick("Skill").enabled = true;
                }
                else
                {
                    UltimateJoystick.GetUltimateJoystick("Skill").enabled = false;
                }
            }


        }
    }


    public float MoveSpeed
    {
        get => characterObject.CharacterStatsData.sSpeed * (1 + playerAbilityStats.AbilityStatsDic["SC_e"] * DataEtc.Instance.S_AddMoveSpeedRatio);
    }

    public float ResultSpeed => MoveSpeed * speedBufferRatio;

    public bool Shield { get; set; }



    #endregion

    #region OnPhotonSerializeView
    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream, info);
        if (stream.IsWriting)
        {
            stream.SendNext(state);
            stream.SendNext(this.photonView.ObservedComponents.Count);
        }
        else
        {
            State = (PlayerState)stream.ReceiveNext();
            var newCount = (int)stream.ReceiveNext();
            if (newCount > this.photonView.ObservedComponents.Count)
            {
                var buffstate = this.gameObject.AddComponent<BuffState>();
                buffstate.SetupPlayer(this);
                this.photonView.ObservedComponents.Add(buffstate);
            }
        }
    }

    #endregion
    #region Awake
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
        playerUI = GetComponent<PlayerUI>();
    }
    #endregion

    #region Setup

    public void SetupPlayerCharacter(CharacterObject _characterObject) => characterObject = _characterObject;

    public void SetupPlayerScore(PlayerScore _playerScore) => playerScore = _playerScore;

    public void SetupPlayerAbilityStats(PlayerAbilityStats _playerAbilityStats) => playerAbilityStats = _playerAbilityStats;



    public void SetupMaxEnergy(int value)
    {
        MaxEnergy = value;
        playerUI.SetUpMaxEnergyhSlider(MaxEnergy);
    }

    public void SetupMaxHealth(int value)
    {
        MaxHealth = value;
        if (playerInput.IsUser && photonView.IsMine)
        {
            playerUI.SetUpHealthUI(MaxHealth, true);
        }
        else
        {
            playerUI.SetUpHealthUI(MaxHealth, false);
        }
    }
    

    #endregion

    #region Rest

    public override void ResetComponent()
    {
        base.ResetComponent();
        this.enabled = true;
        State = PlayerState.Idle;
        MaxHealth = characterObject.CharacterStatsData.sHp;


        playerUI.SetUpMaxEnergyhSlider(maxEnergy);
        playerUI.SetUpHealthUI(MaxHealth, playerInput.MyCharacter);


        CurrentHealth = MaxHealth;
        CurrentEnergy = maxEnergy;
        skillGage = -1;
        SkillGage = 0;
        speedBufferRatio = 1;
        Shield = false;
        isTransparent = false;
        
        noInputStats.Clear();
        n_loopEffectDic.Clear();
        speedBuffDic.Clear();

        
        playerUI.ResetComponent();

        if (photonView.IsMine)
        {
            CheckLoopEffect(EffectType.Loop_Shield);
        }
    }

    #endregion

    private void Update()
    {
        //UpdageHealthUI();
        //UpdateEnergyUI();
        RecoveryEnergyUI();
        CheckRecovery();

    }


    #region Ability

    /// <summary>
    /// 이미 레벨이 차서 선택불가능한 목록들
    /// </summary>
    /// <returns></returns>
    public List<AbilityType> Local_GetNotAbiltiyList()
    {
        //선택불가능한 데이터들 전부 갖고온다. 
        var selectAbilitys = from s in playerAbilityStats.AbilityStatsDic
                             where s.Value >= 3 || s.Value < 0
                             select s;



        List<AbilityType> abilityTypes = new List<AbilityType>();
        //열거형으로 전환
        foreach (var a in selectAbilitys)
        {
            abilityTypes.Add(DataContainer.Instance.GetAbilityByCode(a.Key).sAbilityType);
        }

        return abilityTypes;
    }

    public void Local_AddAbilityPoint(int point)
    {
        if (Dead) return;
        //유저는 직접선택
        if (playerInput.MyCharacter)
        {
            if (point > 0)
            {
                UIManager.instance.uI_GameScene.uI_AbilityPanel.SetActiveSelectUI(true, Local_GetNotAbiltiyList());    //선택불가능한 능력치를 보냄
            }
            else
            {
                UIManager.instance.uI_GameScene.uI_AbilityPanel.SetActiveSelectUI(false, null);    //선택불가능한 능력치를 보냄

            }
        }
        //AI라면 랜덤 //로컬에서만실행
        if (playerInput.IsUser == false && photonView.IsMine)
        {
        }
    }

    #endregion


    #region Health 

    [PunRPC]
    public override void Local_ApplyDamage(int _damagerViewID, int _damage, Vector3 _hitPoint)
    {
        print(Shield + "실드....");
        //스킬 사용시 무적.//부활시 무적
        if (Shield)
        {
            return;
        }
        base.Local_ApplyDamage(_damagerViewID, _damage, _hitPoint); //체력 업데이트
        playerUI.OnDamage(_damage, CurrentHealth + _damage);    //UI 업데이트
        characterObject.OnDamage(); //캐릭터 깜빡임.
        recvoeryLastTime = Time.time + recoveryWaitTime;
        var damagerPlayer = GameManager.instance.GetPlayer(_damagerViewID);
        if (damagerPlayer == null) return;
        if (damagerPlayer.photonView.IsMine)
        {
            damagerPlayer.playerStats.skillGage += 10;
        }
    }
    [PunRPC]
    public override void Local_RestoreHealth(int addHealth)
    {
        base.Local_RestoreHealth(addHealth);
        var effect = ObjectPoolManger.Instance.PopEffectObject(EffectType.Heal) as EffectObject;
        effect.transform.position = this.transform.position;
        effect.transform.rotation = this.transform.rotation;
        effect.transform.SetParent(this.transform);
        Utility.SetLayerRecursively(effect.gameObject, this.gameObject.layer);
        effect.Play();
    }

    public void CheckRecovery()
    {
        if (CurrentHealth >= MaxHealth) return;
        if (Time.time >= recvoeryLastTime + recoveryTimeBet)
        {

            recvoeryLastTime = Time.time;
            photonView.RPC("Local_RestoreHealth", RpcTarget.All, (int)(MaxHealth / 7));
        }

    }


    public override void Die()
    {
        base.Die();
        //GameManger에서 해결.. phoviewGroup에서 못받을 수 있으므로 
        if (photonView.IsMine)
        {
            State = PlayerState.Die;
            playerUI.UpdateHealthSlider(CurrentHealth);
            //addExpAmount = playerStats.Level * 20;
            GameManager.instance.Local_HandleDeathOnEntity(lastAttackViewID, this.photonView.ViewID, addExpAmount); //경험치증가해야할 양을 보냄

            
        }
    }
    #endregion

    #region Shooter
    public void RecoveryEnergyUI() => CurrentEnergy += Time.deltaTime * 0.3f;
    #endregion
    #region UI
    public void UpdageHealthUI() => playerUI.UpdateHealthSlider(CurrentHealth);
    public void UpdateEnergyUI() => playerUI.UpdateEnergySlider(CurrentEnergy);
    #endregion




    public void UpdateSpeed()
    {
        speedBufferRatio = 1;
        if (speedBuffDic.Count == 0) return;

        foreach (var d in speedBuffDic)
        {
            speedBufferRatio *= (1 + d.Value);  //  20퍼감소면  0.8을 곱해야하므로., 1 - d.value/
        }
    }
    /// <summary>
    /// 기절 및 수면상태가 아무것도없어야 원래상태로 전환.
    /// </summary>
    public void UpdateNoInputState()
    {
        if (noInputStats.Count == 0)
        {
            State = PlayerState.Idle;
        }
    }

    #region Buff State


    /// <summary>
    /// 로컬클라이언트만 실행
    /// </summary>
    /// <param name="effectType"></param>
    /// <param name="enemyViewID"></param>
    public void CheckLoopEffect(EffectType effectType ,int enemyViewID = 0)
    {
        if (Shield)
        {
            //무적상태라면 해당 상태이상들적용X
            switch (effectType)
            {
                case EffectType.Loop_Posion:
                case EffectType.Loop_Sleep:
                case EffectType.Loop_Slow:
                case EffectType.Loop_Dark:
                    break;
            }
        }
        //이미있는 이펙트 라면
        if (n_loopEffectDic.ContainsKey(effectType))
        {
            if (enemyViewID == 0)
            {
                n_loopEffectDic[effectType].RenewPlay();
            }
            else
            {
                n_loopEffectDic[effectType].RenewPlay(enemyViewID);
            }
            return;
        }

        //새로운 이펙트라면.
        var buffstate = this.gameObject.AddComponent<BuffState>();
        buffstate.Setup(effectType, this, enemyViewID);
        
    }


    #region Transparent 투명 비투명

    public void RegitserTransparentObject(GameObject gameObject)
    {
        if (canTransparentObjectList.Contains(gameObject)) return;
        canTransparentObjectList.Add(gameObject);
    }

    public void UnRegisterTransparentObject(GameObject gameObject)
    {
        canTransparentObjectList.Remove(gameObject);
    }

    /// <summary>
    /// 로컬이아닌 다른유저들의 투명전환
    /// </summary>
    /// <param name="_isTransparent"></param>
    void Others_ChangeTransparent(bool _isTransparent)
    {

        if (isTransparent == _isTransparent ) return;  //이미 같은상태라면
        isTransparent = _isTransparent;

        if (isTransparent)
        {
            Utility.SetLayerRecursively(this.gameObject, 1);
            //foreach (var g in canTransparentObjectList)
            //{
            //    g.gameObject.layer = 1;
            //}
        }

        else
        {
            Utility.SetLayerRecursively(this.gameObject, LayerMask.NameToLayer("Player"));

            //foreach (var g in canTransparentObjectList)
            //{
            //    g.gameObject.layer = LayerMask.NameToLayer("Player");

            //}
        }

    }
    void Local_ChangeTransparent(bool _isTransparent)
    {
        if (isTransparent == _isTransparent) return;  //이미 같은상태라면
        isTransparent = _isTransparent;

        characterObject.Local_UpdateTransparent(isTransparent);
    }

    /// <summary>
    /// 부쉬,or 스킬로 투명/비투명 처리 => 스킬우선시. 스킬 
    /// </summary>
    /// <param name="isTrans"></param>
    public void SetActiveTrans(bool isTrans)
    {
        //내캐릭
        if (playerInput.MyCharacter)
        {
            if (isTrans)
            {
                //보여줌 상태
                if (state == PlayerState.Throwing || characterObject.isHit)
                {
                    Local_ChangeTransparent(false);
                }
                //투명 상태로만들어줌
                else
                {
                    Local_ChangeTransparent(true);
                }
            }
            else
            {
                //스킬 상태일시에는 풀지않음 부쉬밖에도 풀지않는다
                if (n_loopEffectDic.ContainsKey(EffectType.Loop_Dark))
                {
                    return;
                }
                Local_ChangeTransparent(false);
            }

        }
        //적 캐릭 
        else
        {
            if (isTrans)
            {
                //보여줌 상태
                if (state == PlayerState.Throwing || characterObject.isHit || isDectet)
                {
                    Others_ChangeTransparent(false);
                }
                //투명 상태로만들어줌
                else
                {
                    Others_ChangeTransparent(true);
                }
            }
            else
            {
                //스킬 상태일시에는 풀지않음 부쉬밖에도 풀지않는다
                if (n_loopEffectDic.ContainsKey(EffectType.Loop_Dark))
                {
                    return;
                }
                //해체
                Others_ChangeTransparent(false);
            }
        }


      
    }


    #endregion;

    public void RegisterLoopEffect(EffectType effectType , BuffState buffState)
    {
        n_loopEffectDic.Add(effectType , buffState);
    }


    //public void Other_RegisterLoop

    public void RemoveBuffState(EffectType effectType)
    {
        n_loopEffectDic.Remove(effectType);
    }



    #endregion

    // 애니메이터의 IK 갱신
    private void OnAnimatorIK(int layerIndex)
    {
        // 총의 기준점 gunPivot을 3D 모델의 오른쪽 팔꿈치 위치로 이동
        if (!isIk) return;
        // IK를 사용하여 오른손의 위치와 회전을 총의 오른쪽 손잡이에 맞춘다
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, handTransform.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, handTransform.rotation);
    }

}
