using System.Collections;
using System.Collections.Generic;

using Photon.Pun;
using UnityEngine;

public class ASkill_Run : ActiveSkill
{
    int buffCode;

    public ASkill_Run(SkillContainer skillContainer)
    {
        buffCode = DataContainer.Instance.GetBuffCodeByType(BuffType.Run);
        abilityType = skillContainer.sAbilityType;
        durationTime = skillContainer.sDurationTime;
        coolTime = skillContainer.sCoolTime;
    }
    public override void ProcessSkill(PlayerController playerController)
    {
        playerController.playerStats.CheckLoopEffect(EffectType.Loop_Run);
    }
}
