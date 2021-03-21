using System.Collections;
using UnityEngine;
using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPun 
{
    public PlayerMovement playerMovement { get; set; }
    public PlayerStats playerStats { get; set; }
    public PlayerShooter playerShooter { get; set; }
    public PlayerInput playerInput { get; set; }
    public PlayerGrassDetect playerGrassDetect { get; set; }

    public ActiveSkill[] activeSkills = new ActiveSkill[] { null, null };

    public bool isGrass;    //부쉬 여부 // 로컬에서 
    public string playerName;
    public LayerMask layerMask;


    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerStats = GetComponent<PlayerStats>();
        playerShooter = GetComponent<PlayerShooter>();
        playerGrassDetect = GetComponentInChildren<PlayerGrassDetect>();
        playerInput = GetComponent<PlayerInput>();
        playerStats.onDeath += HandeDeath;
        playerStats.OnDamageEvent += HandleDamage;
    }
    
    public void ResetComponets()
    {
        playerMovement.ResetComponent();
        playerStats.ResetComponent();
        playerShooter.ResetComponent();
        playerInput.ResetComponent();



        if (photonView.IsMine)
        {
            playerGrassDetect.enabled = true;
        }
        else
        {
            playerGrassDetect.enabled = false;
        }

    }

    public void HandleDamage(int damage)
    {
    }

    public void HandeDeath()
    {
        playerMovement.enabled = false;
        playerStats.enabled = false;
        playerStats.playerAbilityStats.enabled = false;
        playerStats.playerScore.enabled = false;
        if (playerInput.MyCharacter)
        {
            var reusltData = playerStats.playerScore.GetPlayerResultData();
            UIManager.instance.uI_GameScene.uI_GameResult.SetupGameResultInfo(reusltData);
            UIManager.instance.uI_GameScene.uI_AbilityPanel.SetActiveSelectUI(false, null);
            UIManager.instance.SetActiveJoysticks(false);
        }
        Invoke("DeathDelay", 3.0f);
    }

    void DeathDelay()
    {
        //자기자신 캐릭이 죽었을 경우.
        if (playerInput.MyCharacter)
        {
            PhotonNetwork.Destroy(this.gameObject);
            GameManager.instance.MyChacterDie();
            
        }
    }



    #region  ActiveSkill

    public int SetupSkill(ActiveSkill _activeSkill, SkillContainer skillContainer)
    {
        for (int i = 0; i < activeSkills.Length; i++)
        {
            if (activeSkills[i] == null)
            {
                activeSkills[i] = _activeSkill;
                if (playerInput.MyCharacter)
                {
                    if(i == 0)
                    {
                        UltimateJoystick.GetUltimateJoystick("Skill1").GetComponent<SkillButton>().SetupAcitveSkill(skillContainer.sAbilityImage);
                    }
                    if(i == 1)
                    {
                        UltimateJoystick.GetUltimateJoystick("Skill2").GetComponent<SkillButton>().SetupAcitveSkill(skillContainer.sAbilityImage);
                    }
                    
                }
                return i;
            }
        }

        return -1;
    }

    void CheckActiveSkill()
    {
        if(playerInput.ActiveSkill1)
        {
            if (activeSkills[0] == null || UltimateJoystick.GetUltimateJoystick("Skill1").isSkill ) return;
            activeSkills[0].Use(this);

            UltimateJoystick.GetUltimateJoystick("Skill1").GetComponent<SkillButton>().PlayCoolTime(activeSkills[0].GetCoolTime());
        }
        if (playerInput.ActiveSkill2 )
        {
            if (activeSkills[1] == null || UltimateJoystick.GetUltimateJoystick("Skill2").isSkill) return;
            activeSkills[1].Use(this);
            UltimateJoystick.GetUltimateJoystick("Skill2").GetComponent<SkillButton>().PlayCoolTime(activeSkills[1].GetCoolTime());
        }
    }
    public bool UserUseSkill(int index, out float coolTime)
    {
        //등록된 스킬이없다면 false
        if (activeSkills[index] == null)
        {
            coolTime = 0;
            return false;
        }
        //스킬이 성공한다면 true
        if (activeSkills[index].Use(this))
        {
            coolTime = activeSkills[index].GetCoolTime();
            return true;
        }
        //실패시 
        coolTime = 0;
        return false;
    }

    [PunRPC]
    public void Skill_TrapOnServer(float durationTime)
    {
        Local_EffectPlay(EffectType.Trap);
        Collider[] colliders = new Collider[10];
        var hitCount = Physics.OverlapSphereNonAlloc(this.transform.position, 3, colliders);

        if (hitCount > 0)
        {
            // 모든 콜라이더들을 순회하면서, 살아있는 플레이어를 찾기
            for (int i = 0; i < hitCount; i++)
            {
                // 콜라이더로부터 LivingEntity 컴포넌트 가져오기
                PlayerController playerController = colliders[i].GetComponent<PlayerController>();
                // LivingEntity 컴포넌트가 존재하며, 해당 LivingEntity가 살아있다면,
                if (playerController != null)
                {
                    if (playerController.photonView.IsMine)
                    {
                        print(playerController + "슬립");
                        var buffCode = DataContainer.Instance.GetBuffCodeByType(BuffType.Sleep);
                        //playerStats.Local_DurationEffectPlay(buffCode, (float)PhotonNetwork.Time);
                    }
                }
            }
        }
    }

    #endregion


    #region  상태

    public void Local_EffectPlay(EffectType effectType)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("Server_ImmediatelyEffectPlay", RpcTarget.All, effectType);
        }
    }
    [PunRPC]
    public void Server_ImmediatelyEffectPlay(EffectType effectType)
    {
        var effectObject = ObjectPoolManger.Instance.PopEffectObject(effectType) as EffectObject;
        effectObject.transform.position = this.transform.position;
        effectObject.transform.SetParent(this.transform);
        effectObject.transform.rotation = this.transform.rotation;
        effectObject.Play();
    }

    #endregion
    private void FixedUpdate()
    {
        isGrass = false;
        CheckActiveSkill();
    }

    private void LateUpdate()
    {
        
        playerStats.SetActiveTrans(isGrass);
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerStats.State == PlayerState.Throwing) return;
        if (isGrass) return;

        if (other.CompareTag("Grass"))
        {
            isGrass = true;
        }
    }
}
