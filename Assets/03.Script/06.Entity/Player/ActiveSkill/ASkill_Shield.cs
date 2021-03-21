

using UnityEngine;
using System.Collections;
using Photon.Pun;
public class ASkill_Shield : ActiveSkill
{
    int buffCode;

   

    public ASkill_Shield(SkillContainer skillContainer)
    {
        buffCode = DataContainer.Instance.GetBuffCodeByType(BuffType.Shield);
        abilityType = skillContainer.sAbilityType;
        durationTime = skillContainer.sDurationTime;
        coolTime = skillContainer.sCoolTime;
    }
    public override void ProcessSkill(PlayerController playerController)
    {

        playerController.playerStats.CheckLoopEffect(EffectType.Loop_Shield);

    }


}
