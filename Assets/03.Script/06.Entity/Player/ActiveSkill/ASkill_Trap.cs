using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ASkill_Trap : ActiveSkill
{

    public ASkill_Trap(SkillContainer skillContainer)
    {
        abilityType = skillContainer.sAbilityType;
        durationTime = skillContainer.sDurationTime;
        coolTime = skillContainer.sCoolTime;
    }
    public override void ProcessSkill(PlayerController playerController)
    {
        playerController.photonView.RPC("Skill_TrapOnServer", RpcTarget.All, durationTime);

        

        //Collider[] colliders = new Collider[10];
        //var hitCount = Physics.OverlapSphereNonAlloc(this.transform.position, 3, colliders);

        //if (hitCount > 0)
        //{
        //    // 모든 콜라이더들을 순회하면서, 살아있는 플레이어를 찾기
        //    for (int i = 0; i < hitCount; i++)
        //    {
        //        // 콜라이더로부터 LivingEntity 컴포넌트 가져오기
        //        MoveEntity moveEntity = colliders[i].GetComponent<MoveEntity>();
        //        // LivingEntity 컴포넌트가 존재하며, 해당 LivingEntity가 살아있다면,
        //        if (moveEntity != null)
        //        {

        //            moveEntity.Stop(3.0f);
        //        }
        //    }
        //}
    }
}
