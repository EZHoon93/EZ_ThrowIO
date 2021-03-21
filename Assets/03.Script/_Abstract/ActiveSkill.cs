using System.Collections;

using UnityEngine;

public abstract class ActiveSkill  
{
    protected float durationTime;   //즉시시전 = 0
    protected float coolTime;
    protected float lastUseTime;
    protected AbilityType abilityType;
   

    public virtual bool Use(PlayerController playerController)
    {
        if (IsUse() == false) return false;
        lastUseTime = Time.time + coolTime;
        ProcessSkill(playerController);
        return true;
    }

    public abstract void ProcessSkill(PlayerController playerController);

    public virtual bool IsUse()
    {
        return Time.time >= lastUseTime ? true : false;
    }

    public float GetCoolTime() => coolTime;
    
}
