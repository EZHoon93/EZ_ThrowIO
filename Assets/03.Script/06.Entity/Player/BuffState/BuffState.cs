using System.Collections;

using UnityEngine;
using Photon.Pun;

public class BuffState : MonoBehaviourPun , IPunObservable
{

    [SerializeField]  PlayerStats playerStats;
    [SerializeField]  EffectObject effectObject;

    [SerializeField] EffectType n_effectType;
    [SerializeField] bool n_isSetup = false;
    [SerializeField] bool isPlay = false;
    [SerializeField] bool isEnd = false;
    [SerializeField] float n_effectTime;
    [SerializeField] float n_endTime;

    [SerializeField] int n_enemyViewID;
    [SerializeField] int n_posionStackCount; //독같은 중첩 이펙트들 카운트



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(n_isSetup);

            stream.SendNext(n_effectType);

            stream.SendNext(n_effectTime);
            stream.SendNext(n_endTime);


            stream.SendNext(n_enemyViewID);
            stream.SendNext(n_posionStackCount);

        }
        else
        {
            n_isSetup = (bool)stream.ReceiveNext();
            n_effectType = (EffectType)stream.ReceiveNext();
            n_effectTime = (float)stream.ReceiveNext();
            n_endTime = (float)stream.ReceiveNext();
            n_enemyViewID = (int)stream.ReceiveNext();
            n_posionStackCount = (int)stream.ReceiveNext();
            print(n_effectType + "이펙트타입");
        }
    }


    private void Update()
    {
        if (!n_isSetup) return; //셋업 전이라면 X


        if (!isPlay)  //실행전이라면
        {
            StartProcess();
        }

        if (isPlay)
        {
            if(PhotonNetwork.Time >= n_endTime)
            {
                isPlay = false;
                playerStats.RemoveBuffState(n_effectType);
                this.photonView.ObservedComponents.Remove(this);
                effectObject.Push();
                Destroy(this);
            }
        }
        
    }

    public void SetupPlayer(PlayerStats _playerStats) => playerStats = _playerStats;
    //로컬만 최초 실행
    public virtual void Setup(EffectType _effectType,PlayerStats _playerStats  ,int _enemyViewId = 0 )
    {
        SetupPlayer(_playerStats);
        n_isSetup = true;
        n_effectType = _effectType;
        n_effectTime = DataContainer.Instance.GetEffectContainerByEffectType(n_effectType).sEffectTime;
        n_endTime = (float)PhotonNetwork.Time + n_effectTime;
        n_enemyViewID = _enemyViewId;
        n_posionStackCount = 1;
        isPlay = false;
        this.photonView.ObservedComponents.Add(this);


    }

    public void StartProcess()
    {
        isPlay = true;
        playerStats.RegisterLoopEffect(n_effectType, this);
        switch (n_effectType)
        {
            case EffectType.Loop_Posion:
                effectObject = CreateEffect(EffectType.Loop_Posion);
                StartCoroutine(Process_Posion(playerStats));
                break;
            case EffectType.Loop_Slow:
                effectObject = CreateEffect(EffectType.Loop_Slow);
                StartCoroutine(Process_Slow(playerStats));
                break;

            case EffectType.Loop_Stun:
                effectObject = CreateEffect(EffectType.Loop_Stun);
                StartCoroutine(Process_Stun(playerStats));
                break;
            case EffectType.Loop_Run:
                effectObject = CreateEffect(EffectType.Loop_Run);
                StartCoroutine(Process_Run(playerStats));
                break;
            case EffectType.Loop_Shield:
                effectObject = CreateEffect(EffectType.Loop_Shield);
                StartCoroutine(Process_Shield(playerStats));
                break;
            case EffectType.Loop_Sleep:
                effectObject = CreateEffect(EffectType.Loop_Sleep);
                StartCoroutine(Process_Sleep(playerStats));
                break;
            case EffectType.Loop_Dark:
                effectObject = CreateEffect(EffectType.Loop_Dark);
                StartCoroutine(Process_Dark(playerStats));
                break;
        }


        if (effectObject)
        {
            effectObject.Play();
        }

    }


    EffectObject CreateEffect(EffectType effectType)
    {
        effectObject = ObjectPoolManger.Instance.PopEffectObject(effectType) as EffectObject;
        effectObject.transform.position = playerStats.transform.position;
        effectObject.transform.localScale = new Vector3(1, 1, 1);
        effectObject.transform.rotation = playerStats.transform.rotation;
        effectObject.transform.SetParent(playerStats.playerObject.transform);
        effectObject.gameObject.layer = this.gameObject.layer;
        //playerStats.RegitserTransparentObject(effectObject.gameObject); //투명리스트 추가
        return effectObject;
    }


    //시간만 업데이트.
    public virtual void RenewPlay(int enemyViewID = 0)
    {
        if(n_posionStackCount < 3)
        {
            n_posionStackCount++;
        }
        n_endTime = (float)PhotonNetwork.Time + n_effectTime;
    }

    //효과
    public virtual void Push()
    {
        if (effectObject)
        {
            //playerStats.UnRegisterTransparentObject(effectObject.gameObject);       //투명 리스트 제거
            effectObject.Push();
            isPlay = false;
        }
    }


    IEnumerator Process_Posion(PlayerStats playerStats)
    {
        //5초 1초마다 딜
        float tickTimeInterval = DataEtc.Instance.S_PosionTickTime;
        int dmage = DataEtc.Instance.S_PosionTickDamage;
        while (this.gameObject.activeSelf)
        {
            if (playerStats)
            {
                playerStats.Local_ApplyDamage(n_enemyViewID, dmage * n_posionStackCount, Vector3.zero);        //대미지
                yield return new WaitForSeconds(tickTimeInterval);
            }
        }
    }

    IEnumerator Process_Slow(PlayerStats playerStats)
    {

        //플레이어 상태에서 이속버프감소가 없다면. 등록 //로컬이아닌플레이어도 실행하지만 결과엔 영향X => 만약 AI가 소유자가 바뀔경우를 대비해 값은갖고있는다.
        if (!playerStats.speedBuffDic.ContainsKey(n_effectType))
        {
            playerStats.speedBuffDic.Add(n_effectType, DataEtc.Instance.S_IceSlowRatio);
            playerStats.UpdateSpeed();
        }
        while (isPlay)
        {
            yield return null;
        }

        playerStats.speedBuffDic.Remove(n_effectType);
        playerStats.UpdateSpeed();

    }

    IEnumerator Process_Stun(PlayerStats playerStats)
    {
        //확률
       

        playerStats.State = PlayerState.Stun;
        if (!playerStats.noInputStats.Contains(n_effectType))
        {
            playerStats.noInputStats.Add(n_effectType);

        }
        while (isPlay)
        {
            yield return null;
        }
        if (playerStats.noInputStats.Contains(n_effectType))
        {
            playerStats.noInputStats.Remove(n_effectType);

        }
        playerStats.UpdateNoInputState();
    }

    IEnumerator Process_Run(PlayerStats playerStats)
    {
        playerStats.State = PlayerState.Run;
        playerStats.GetComponent<PlayerMovement>().PlayerRotateImmeditaly();
        yield return null;  //Move에서 Rotate를위해 한프레임 기다려줌
        while (isPlay)
        {
            yield return null;
        }
        playerStats.State = PlayerState.Idle;
    }


    IEnumerator Process_Shield(PlayerStats playerStats)
    {
        playerStats.Shield = true;
        print("실드중... ");
        //끝날대까진 기다림
        while (isPlay)
        {

            yield return null;
        }


        playerStats.Shield = false;
        print("실드끝.. " + playerStats.Shield);

    }


    IEnumerator Process_Sleep(PlayerStats playerStats)
    {
        playerStats.State = PlayerState.Sleep;
        if (!playerStats.noInputStats.Contains(n_effectType))
        {
            playerStats.noInputStats.Add(n_effectType);
        }
        while (isPlay)
        {
            yield return null;
        }
        if (playerStats.noInputStats.Contains(n_effectType))
        {
            playerStats.noInputStats.Remove(n_effectType);

        }
        playerStats.UpdateNoInputState();
    }


    IEnumerator Process_Dark(PlayerStats playerStats)
    {
        
        while (isPlay)
        {
            //if (playerStats.State == PlayerState.Throwing)
            //{
            //    if (playerStats.playerInput.MyCharacter)
            //    {
            //        playerStats.characterObject.skinnedMesh.material = playerStats.characterObject.transMaterial;
            //    }
            //    else
            //    {
            //        playerStats.SetActiveTrans(false);
            //    }
            //}
            //else
            //{
            //    //자기자신 캐릭은
            //    if (playerStats.playerInput.MyCharacter)
            //    {
            //        playerStats.characterObject.skinnedMesh.material = playerStats.characterObject.transMaterial;
            //    }
            //    else
            //    {
            //        playerStats.SetActiveTrans(true);
            //    }
            //}


            //자기자신 캐릭은
            if (playerStats.playerInput.MyCharacter)
            {
                playerStats.characterObject.skinnedMesh.material = playerStats.characterObject.transMaterial;
            }
            else
            {
                playerStats.SetActiveTrans(true);
            }

            yield return null;
        }

        if (playerStats.playerInput.MyCharacter)
        {
            playerStats.characterObject.skinnedMesh.material = playerStats.characterObject.orinealMaterial;
        }
        else
        {
            playerStats.SetActiveTrans(false);
        }
    }
}
