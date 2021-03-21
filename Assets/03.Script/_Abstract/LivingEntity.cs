using System;
using Photon.Pun;
using UnityEngine;

// 생명체로서 동작할 게임 오브젝트들을 위한 뼈대를 제공
// 체력, 데미지 받아들이기, 사망 기능, 사망 이벤트를 제공
public class LivingEntity : MonoBehaviourPun ,IDamageable, IPunObservable  {

    [SerializeField] HealthUI healthUI;
    protected int m_MaxHealth;
    protected int m_CurrentHealth;
    protected bool m_Dead;
    protected int lastAttackViewID; //마지막으로 공격한 플레이어
    protected int addExpAmount;


    public virtual int MaxHealth {
        get => m_MaxHealth;
        set
        {
            if (m_MaxHealth == value) return;
            m_MaxHealth = value;
        }
    }

    public virtual int CurrentHealth
    {
        get => m_CurrentHealth;
        set
        {
            if (m_CurrentHealth == value) return;
            CurrentHealth = (int)Mathf.Clamp(value, 0, MaxHealth);

        }
    }


    public virtual bool Dead {  
        get => m_Dead; 
        protected set 
        {
            if (m_Dead == value) return;
            m_Dead = value;
        }
    } 

    public event Action onDeath; // 사망시 발동할 이벤트
    public event Action<int> OnDamageEvent;  //대미지 받을때 이벤트

    public virtual void ResetComponent()
    {
        Dead = false;
    }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(m_CurrentHealth);
            stream.SendNext(m_MaxHealth);
            stream.SendNext(m_Dead);
        }
        else
        {
            CurrentHealth = (int)stream.ReceiveNext();
            MaxHealth= (int)stream.ReceiveNext();
            Dead = (bool)stream.ReceiveNext();
        }
    }



    //public void 
    [PunRPC]
    public virtual void Local_RestoreHealth(int addHealth) {
        if (Dead)
        {
            // 이미 사망한 경우 체력을 회복할 수 없음
            return;
        }
        // 로컬만 체력을 직접 갱신 가능

        if (photonView.IsMine)
        {
            CurrentHealth = (int)Mathf.Clamp(CurrentHealth + addHealth, 0, MaxHealth);
        }
    }

    [PunRPC]
    public virtual void Local_ApplyDamage( int damagerViewID, int damage , Vector3 hitPoint)
    {
        if (photonView.IsMine)
        {
            CurrentHealth-= damage;
            lastAttackViewID = damagerViewID;
            photonView.RPC("Local_ApplyDamage", RpcTarget.Others, damagerViewID, damage, hitPoint);
        }

        OnDamageEvent?.Invoke(lastAttackViewID);
        healthUI.OnDamage(damage, CurrentHealth + damage);
        if (CurrentHealth <= 0 && !Dead)
        {
            Die();
        }
        

    }

    public virtual void Die()
    {
        // onDeath 이벤트에 등록된 메서드가 있다면 실행
        print("Livie Di");
        if (onDeath != null)
        {
            onDeath();
        }

        if (photonView.IsMine)
        {
            //GameManager.instance.Kill(lastAttackViewID, this.photonView.ViewID, addExpAmount);
        }
        
    
        Dead= true;
    }

    public bool IsMine()
    {
        return photonView.IsMine;
    }
}