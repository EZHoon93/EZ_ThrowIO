using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPun
{

    #region 싱글톤
    // 외부에서 싱글톤 오브젝트를 가져올때 사용할 프로퍼티
    public static GameManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }
    private static GameManager m_instance; // 싱글톤이 할당될 static 변수
    #endregion

    public PlayerController myPlayer;

    Dictionary<int, PlayerController> playerDic = new Dictionary<int, PlayerController>(); //참가 플레이어 리스트
    public GameType gameType;
    public ItemSpawner itemSpawner;
    private void Awake()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }

        UIManager.instance.SetActive(UIState.Wait);
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {

     
    }

    public void GameJoin()
    {
        string testNickName = "Player" + Random.Range(0, 999);

        
        object[] data = { 
            testNickName ,  //닉네임
            PlayerInfo.GetUsingCharacterContainer().sId,    //캐릭터 
            PlayerInfo.GetUsingProjectileContainer().sId ,  //무기
        };
        myPlayer =  PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0, data).GetComponent<PlayerController>();
    }

    //대기화면으로.
    public void GameLeft()
    {
        if (myPlayer)
        {
            PhotonNetwork.Destroy(myPlayer.gameObject);
        }
        
    }

    public void MyChacterDie()
    {
        UIManager.instance.uI_GameScene.ClickGameJoin(false);
    }


    #region Register

    public void RegisterPlayerController(int viewID, PlayerController playerController)
    {
        if (playerDic.ContainsKey(viewID)) return;
        if (playerDic == null)
        {

        }

        print(viewID + "등록!!");
        playerDic.Add(viewID, playerController);
    }

    public void UnRegisterPlayerController(int viewID)
    {
        if (!playerDic.ContainsKey(viewID)) return;
        playerDic.Remove(viewID);
    }
    #endregion

    #region Get 
    public PlayerController GetPlayer(int _viewID)
    {
        //viewID가 플레이어 일경우 

        if (playerDic.ContainsKey(_viewID))
        {
            return playerDic[_viewID];
        }
        //없을경우
        else
        {
            return null;
        }
    }
    public PlayerController[] GetAllPlayer()
    {
        List<PlayerController> result = new List<PlayerController>();
        foreach (var p in playerDic.Values)
        {
            result.Add(p);
        }
        return result.ToArray();
    }
    #endregion

    /// <summary>
    /// 죽은 사람이 메시지 보냄.
    /// </summary>
    /// <param name="_killViewID"></param>
    /// <param name="_deathViewID"></param>
    /// <param name="_addExpAmount"></param>
    public void Local_HandleDeathOnEntity(int killViewID, int deathViewID, int addExpAmount)
    {
        photonView.RPC("KillToServer", RpcTarget.All, killViewID, deathViewID, addExpAmount);
    }
    [PunRPC]
    public void KillToServer(int killViewID, int deathViewID, int addExpAmount)
    {
        print(killViewID + "대스뷰 : " + deathViewID);
        var _killPlayer = GetPlayer(killViewID);
        var _deathPlayer = GetPlayer(deathViewID);
        print(_killPlayer + "킬플레이어" + _deathPlayer +"데스");
        string _killName = null;
        string _deathName = null;
        //죽인플레이어가 있다면
        if (_killPlayer)
        {
            //경험치 증가
            if (_killPlayer.photonView.IsMine)
            {
                print(addExpAmount + " 경험ㅊ증가");
                _killPlayer.playerStats.playerScore.CurrentExp += addExpAmount;
                //게임모드가 IO인 경우에만
                switch(gameType)
                {
                    case GameType.Io:
                        _killPlayer.playerStats.playerScore.Score += addExpAmount;
                        break;
                    default:
                        break;
                }
            }
            _killName = _killPlayer.playerName;
        }

        if (_deathPlayer)
        {
            _deathName = _deathPlayer.playerName;
        }

        UIManager.instance.uI_GameScene.NoticeDeathInfo(_killName, _deathName);



    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            myPlayer.playerStats.playerScore.CurrentExp += 80;
        }
    }



}
