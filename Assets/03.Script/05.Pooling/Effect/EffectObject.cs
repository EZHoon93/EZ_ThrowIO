using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObject : PoolableObject
{
    [SerializeField] protected EffectContainer effectContainer;
    [SerializeField] protected ParticleSystem[] effectParticle;


    public virtual void Initalize(EffectContainer _effectContainer)
    {
        effectContainer = _effectContainer;
    }


    public virtual void Play( float range = 1.0f )
    {
        Invoke("Push", effectContainer.sEffectTime);

        this.transform.localScale = new Vector3(range, range, range);

        foreach (var e in effectParticle)
        {
            e.Play();
        }

        //스킬이펙트 사용시 해당효과도 발생
        //if (playerController)
        //{
        //    if (!playerController.photonView.IsMine) return;
        //    switch (effectContainer.sEffectType)
        //    {
        //        case EffectType.Heal:
        //            //if(p)
        //            playerController.playerStats.CurrentHealth += (int)(playerController.playerStats.MaxHealth * 0.3f);
        //            break;

        //        case EffectType.Trap:
        //            break;
                    
        //    }
        //}
    }

    public virtual void LoopPlay()
    {
        

        foreach (var e in effectParticle)
        {
            e.Play();
        }
    }


    public override void Push()
    {
        base.Push();
    }


}
