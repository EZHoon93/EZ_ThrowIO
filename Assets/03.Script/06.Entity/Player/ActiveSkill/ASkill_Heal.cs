using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ASkill_Heal : ActiveSkill
{
    //[SerializeField] EffectContainer effectContainer;
    public ASkill_Heal(SkillContainer skillContainer)
    {
        abilityType = skillContainer.sAbilityType;
        coolTime = skillContainer.sCoolTime;
        durationTime = skillContainer.sDurationTime;
    }
    public override void ProcessSkill(PlayerController playerController)
    {
        playerController.Local_EffectPlay(EffectType.Heal);
        playerController.playerStats.CurrentHealth += (int)(playerController.playerStats.MaxHealth * 0.3f);
    }
}
