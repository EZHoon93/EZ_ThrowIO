using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class itemExpObject : MonoBehaviourPun, IDamageable, IPunInstantiateMagicCallback, IPunObservable
{
    [SerializeField] int expAmount;
    [SerializeField] int startHp;
    [SerializeField] int currentHp;
    [SerializeField] ItemType itemType;
    [SerializeField] HealthUI healthUI;
    [SerializeField] GameObject modelObject;
    [SerializeField] float createTimeBiet;
    Collider boxCollider;
    bool n_dead;
    float n_lastDeatTime;


    public bool Dead
    {
        get => n_dead;
        set
        {
            if (n_dead == value) return;
            n_dead = value;
            //사망
            if (n_dead)
            {
                modelObject.SetActive(false);
                boxCollider.enabled = false;

                if (photonView.IsMine)
                {
                    n_lastDeatTime = Time.time;
                }
            }
            else
            {
                currentHp = startHp;
                modelObject.SetActive(true);
                boxCollider.enabled = true;
                healthUI.ResetComponent();
                healthUI.UpdateHealthSlider(startHp);
            }
        }
    }

    public float LastDeadTime 
    {
        get => n_lastDeatTime;
        set
        {
            if (n_lastDeatTime == value) return;
            
        }
    }

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();

    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        currentHp = startHp;
        healthUI.SetUpHealthUI(currentHp, false);
        Dead = false;

    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(n_dead);
            stream.SendNext(n_lastDeatTime);
        }
        else
        {
            Dead = (bool)stream.ReceiveNext();
            LastDeadTime =(float)stream.ReceiveNext();
        }

    }

    private void Update()
    {
        if (photonView.IsMine && Dead)
        {
            if(Time.time >= LastDeadTime + createTimeBiet)
            {
                Dead = false;
            }
        }
    }

    public void Local_ApplyDamage(int damagerViewID, int damage, Vector3 hitPoint)
    {
        var player = GameManager.instance.GetPlayer(damagerViewID);
        if (player.photonView.IsMine && !Dead)
        {
            currentHp -= damage;
            photonView.RPC("Local_ApplyDamage", RpcTarget.Others, damagerViewID, damage, hitPoint);

            if (currentHp <= 0 )
            {
                Dead = true;
                player.playerStats.playerScore.CurrentExp += expAmount;
                modelObject.gameObject.SetActive(false);
                var effectObject = ObjectPoolManger.Instance.PopEffectObject(EffectType.CoinExplsion);
                effectObject.transform.position = this.transform.position;
            }
            else
            {
                this.photonView.TransferOwnership(player.photonView.ControllerActorNr);
            }
        }

        healthUI.UpdateHealthSlider(currentHp);
        healthUI.OnDamage(damage, currentHp+damage);
        


    }

    public bool IsMine()
    {
        return photonView.IsMine;
    }
}
