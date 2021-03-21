using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ASkill_Dark : ActiveSkill
{
    int buffCode;
    public ASkill_Dark(SkillContainer skillContainer)
    {
        buffCode = DataContainer.Instance.GetBuffCodeByType(BuffType.Dark);
        abilityType = skillContainer.sAbilityType;
        coolTime = skillContainer.sCoolTime;
        durationTime = skillContainer.sDurationTime;

    }
    public override void ProcessSkill(PlayerController playerController)
    {
        playerController.Local_EffectPlay(EffectType.Heal);
        playerController.playerStats.CheckLoopEffect(EffectType.Loop_Dark);

    }
}
