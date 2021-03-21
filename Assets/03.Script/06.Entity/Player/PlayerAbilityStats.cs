using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System.Linq;

public class PlayerAbilityStats : MonoBehaviourPun , IPunInstantiateMagicCallback , IPunObservable , IOnPhotonViewPreNetDestroy
{

    Dictionary<string, int> net_abilityStatsDic = new Dictionary<string, int>();    //전송용.




    public Dictionary<string,int> AbilityStatsDic
    {
        get => net_abilityStatsDic;
        set
        {
            net_abilityStatsDic = value;
        }
    }


    private void Awake()
    {
        foreach (var d in DataContainer.Instance.sAbilityContainers)
        {
            net_abilityStatsDic.Add(d.sCode, 0);
        }
    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {

        if (photonView.InstantiationData == null) return;   //데이터가없으면 null

        var viewID = (int)photonView.InstantiationData[0]; //무기 아이디. 능력치 금지를위해
        var _playerController = GameManager.instance.GetPlayer(viewID);
        if (_playerController)
        {
            _playerController.playerStats.SetupPlayerAbilityStats(this);
        }
        ResetComponent();
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(net_abilityStatsDic);
        }
        else
        {
            var net_abilityStatsDics = (Dictionary<string,int>)stream.ReceiveNext();
            if (net_abilityStatsDic == net_abilityStatsDics) return;
            net_abilityStatsDic = net_abilityStatsDics;
          
        }
    }

  
    public void ResetComponent()
    {
        if (photonView.IsMine)
        {

        }
        Dictionary<string, int>.KeyCollection keys = net_abilityStatsDic.Keys;
        
        foreach(string s in net_abilityStatsDic.Keys.ToList())
        {
            net_abilityStatsDic[s] = 0;
        }


        this.enabled = true;

    }



    public void Local_UpdateAbilityStats(AbilityContainer abilityContainer , PlayerController playerController)
    {
        ActiveSkill activeSkill = null;
        switch (abilityContainer.sAbilityType)
        {

            case AbilityType.SC_h:  //체력
                net_abilityStatsDic["SC_h"]++;
                playerController.playerStats.MaxHealth += (int)(playerController.playerStats.MaxHealth * DataEtc.Instance.S_AddMaxHealthRatio);
                break;
            case AbilityType.SC_e:           //기력
                net_abilityStatsDic["SC_e"]++;
                break;
            case AbilityType.SC_s:  //이속
                net_abilityStatsDic["SC_s"]++;
                break;
            //------------------------------------------------------------//
            case AbilityType.SP_d: //대미지
                net_abilityStatsDic["SP_d"]++;

                break;
            case AbilityType.SP_v: //수류탄속도
                net_abilityStatsDic["SP_v"]++;


                break;
            case AbilityType.SP_r:  //범위
                net_abilityStatsDic["SP_r"]++;

                break;
            //------------------------------------------------------------//
            case AbilityType.PC_d: //한발더
                net_abilityStatsDic["PC_d"]++;
                net_abilityStatsDic["PC_m"] = -10;

                break;
            case AbilityType.PC_m:  //동시두발
                net_abilityStatsDic["PC_d"] = -10;
                net_abilityStatsDic["PC_m"]++;

                break;
            //------------------------------------------------------------//
            case AbilityType.PP_f:   //속성불
                net_abilityStatsDic["PP_f"]++;
                net_abilityStatsDic["PP_i"] = -10;
                net_abilityStatsDic["PP_l"] = -10;
                net_abilityStatsDic["PP_p"] = -10;

                break;
            case AbilityType.PP_i:    //속성얼음
                net_abilityStatsDic["PP_f"] = -10;
                net_abilityStatsDic["PP_i"]++;
                net_abilityStatsDic["PP_l"] = -10;
                net_abilityStatsDic["PP_p"] = -10;


                break;
            case AbilityType.PP_l:   //속성번개
                net_abilityStatsDic["PP_f"] = -10;
                net_abilityStatsDic["PP_i"] = -10;
                net_abilityStatsDic["PP_l"]++;
                net_abilityStatsDic["PP_p"] = -10;

                break;
            case AbilityType.PP_p: //속성독
                net_abilityStatsDic["PP_f"] = -10;
                net_abilityStatsDic["PP_i"] = -10;
                net_abilityStatsDic["PP_l"] = -10;
                net_abilityStatsDic["PP_p"]++;
                break;

           case AbilityType.AP_d:
                activeSkill = new ASkill_Dark(abilityContainer as SkillContainer);
                net_abilityStatsDic["AP_d"] = 3;
               break;
           case AbilityType.AP_h:
                activeSkill = new ASkill_Heal(abilityContainer as SkillContainer);
                net_abilityStatsDic["AP_h"] = 3;
           
               break;
           case AbilityType.AP_r:
                activeSkill = new ASkill_Run(abilityContainer as SkillContainer);
                net_abilityStatsDic["AP_r"] = 3;
           
               break;
           case AbilityType.AP_s:
                activeSkill = new ASkill_Shield(abilityContainer as SkillContainer);
                net_abilityStatsDic["AP_s"] = 3;
           
               break;
           case AbilityType.AP_t:
                activeSkill = new ASkill_Trap(abilityContainer as SkillContainer);
                net_abilityStatsDic["AP_t"] = 3;

                break;




        }


        if (activeSkill != null)
        {
            var skillCount = playerController.SetupSkill(activeSkill, abilityContainer as SkillContainer);
            //스킬 가득참.. => 나머지 안올린 스킬들 봉인
            if (skillCount == 1)
            {
               net_abilityStatsDic["AP_d"] = net_abilityStatsDic["AP_d"] == 3 ? 3 : -1;
               net_abilityStatsDic["AP_h"] = net_abilityStatsDic["AP_h"] == 3 ? 3 : -1;
               net_abilityStatsDic["AP_r"] = net_abilityStatsDic["AP_r"] == 3 ? 3 : -1;
               net_abilityStatsDic["AP_s"] = net_abilityStatsDic["AP_s"] == 3 ? 3 : -1;
               net_abilityStatsDic["AP_t"] = net_abilityStatsDic["AP_t"] == 3 ? 3 : -1;
            }
        }

        playerController.Local_EffectPlay(EffectType.AbilityUp);

        if (playerController.playerInput.MyCharacter) //로컬&&자기자신
        {
            UIManager.instance.uI_GameScene.uI_PlayerAbilityIconController.UpdateIcon(abilityContainer, net_abilityStatsDic);
        }


      

    }
    public void GetAbiltiyDatas(Dictionary<AbilityType, int> keyValuePairs)
    {
    

    }

    public void GetNotAbiltiys()
    {
        
    }

    public void OnPreNetDestroy(PhotonView rootView)
    {
        print("초기화");
    }
}