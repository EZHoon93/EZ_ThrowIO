using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;
using TMPro;

public class MultiManager : MonoBehaviourPunCallbacks, IPunObservable
{
    #region 싱글톤
    public static MultiManager instacne
    {
        get
        {
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<MultiManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }
    private static MultiManager m_instance; // 싱글톤이 할당될 static 변수

    #endregion

    public enum NetworkState { LocalIsMaster, MyDisconnect, MasterConnect, MasterDisconnect };
    public NetworkState netState;

    double masterTime;
    double pausedTime;

    float waitChangeBet = 0.98f;
    float waitChangeInterval = 0.0f;
    float waitChangeIntervalAddAmount = 0.4f;

    bool bPasued = false;
    int test = 0;


    

    #region 인터페이스 구현
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 네트워크를 통해 score 값을 보내기
            stream.SendNext(masterTime);
        }
        else
        {
            masterTime = (double)stream.ReceiveNext();
            netState = NetworkState.MasterConnect;
        }

    }

    #endregion



    #region Awake,OnEnable,Start ,Update 초기설정
    private void Awake()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (instacne != this)
        {
            // 자신을 파괴
            Destroy(this.gameObject);
        }
    }


    private void Start()
    {

        
    }

    

    #endregion


    private void FixedUpdate()
    {
        //UpdateClinetCheckMasterNetworkState();  //마스터가 접속중인지 체크
    }

    #region 방장 바꾸기 알고리즘 

    /// <summary>
    /// 게임 시작전. 방장을 바꿀 알고리즘 
    /// </summary>
    private void GameBeforeStateMasterChange()
    {
        waitChangeInterval = 0.0f; //자기자신의 차례 기다림 초기화
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            //방장이아니고 일시 정지한 캐릭이 아닌 유저, 제일먼저 유저
            if (player.Value.IsMasterClient == false
                && (bool)player.Value.CustomProperties["paused"] == false)
            {
                //만약 자기자신이라면... 자신의 우선순위권을 가지고 방장이된다
                if (PhotonNetwork.LocalPlayer.ActorNumber == player.Value.ActorNumber)
                {
                    break;
                }
                else
                {
                    waitChangeInterval += waitChangeIntervalAddAmount;
                }
            }
        }
    }

    /// <summary>
    /// 게임 진행중에 방장을 바꿀 알고리즘
    /// </summary>
    private void GameingStateMasterChange()
    {
        waitChangeInterval = 0.0f; //자기자신의 차례 기다림 초기화

        var players = FindObjectsOfType<LivingEntity>();
        foreach (var player in players)
        {
            //방장이아닌 유저만 ..
            if (player.photonView.Controller.IsMasterClient == false
                && (bool)player.photonView.Controller.CustomProperties["paused"] == false)
            {
                if (PhotonNetwork.LocalPlayer.ActorNumber == player.photonView.ControllerActorNr)
                {
                    break;
                }
                else
                {
                    waitChangeInterval += waitChangeIntervalAddAmount;
                }
            }
        }

    }
    #endregion

    #region 네트워크 상태 체크 
    private void UpdateClinetCheckMasterNetworkState()
    {

        switch (netState)
        {
            case NetworkState.LocalIsMaster:
                masterTime = PhotonNetwork.Time;
                break;
            case NetworkState.MyDisconnect:
                pausedTime = PhotonNetwork.Time;
                //타임 잰다.
                break;
            case NetworkState.MasterConnect:
                //마스터의 연결이 끊겻다고 간주
                if (PhotonNetwork.Time - masterTime >= waitChangeBet)
                {
                    netState = NetworkState.MasterDisconnect;

                }
                break;
            case NetworkState.MasterDisconnect:
                //자기자신의 차례가되면
                if (PhotonNetwork.Time - masterTime >= waitChangeBet + waitChangeInterval)
                {
                    netState = NetworkState.LocalIsMaster;

                    PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
                    photonView.RPC("InitalizeMasterTime", RpcTarget.Others, PhotonNetwork.Time);
                    PhotonNetwork.SendAllOutgoingCommands();
                }
                break;
        }
    }
    [PunRPC]
    public void InitalizeMasterTime(double _newMasterTime)
    {
        masterTime = _newMasterTime;
        if (PhotonNetwork.IsMasterClient)
        {
            netState = NetworkState.MasterConnect;
        }
    }
    #endregion


    #region 포톤 콜백 함수

    //방장이 바뀌었을 떄
    public override void OnMasterClientSwitched(Player newMasterClient)
    {

    }

    //플레이어 나갈시.
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
    }

    //플레이어 참가시
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
    }

    /// <summary>
    /// 랜덤 룸참가 실패시. 방이없으므로 방을만든다.
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        int Ran = UnityEngine.Random.Range(0, 999);
        // 최대 4명을 수용 가능한 빈방을 생성
        PhotonNetwork.CreateRoom(Ran.ToString(), new RoomOptions { MaxPlayers = 10 });
    }

    public override void OnLeftRoom()
    {

        SceneManager.LoadScene("lobby1");
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
       
    }

    //PlayerStats GetPlayerStats(int viewID , Dictionary<string , int > newStats)
    //{
    //    PlayerStats playerStats = new PlayerStats(viewID);
    //    playerStats.level = newStats["level"];
    //    playerStats.health = newStats["heath"];
    //    playerStats.power = newStats["power"];
    //    playerStats.speed = newStats["speed"];
    //    playerStats.sight = newStats["sight"];

    //    return playerStats;
    //}

    //public void UpdatePlayerStatsOnLocal(PlayerStats playerStats)
    //{
    //    print("Update PlayerStats" + playerStats.viewID);
    //    Dictionary<string, int> statsDictionry = new Dictionary<string, int>()
    //    {
    //        ["level"] = playerStats.level,
    //        ["heath"] = playerStats.health,
    //        ["power"] = playerStats.power,
    //        ["speed"] = playerStats.speed,
    //        ["sight"] = playerStats.sight
    //    };
    //    PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable {
    //        {"stats", playerStats.viewID },
    //        { playerStats.viewID.ToString(), statsDictionry },
    //    });

    //}



    #endregion

    #region

    //빠른 방 찾기 클릭시
    public void GoToLooby()
    {
        PhotonNetwork.LeaveRoom();

    }
    //그냥 채널 변경을 누를시. 1. 방을만든다 => 방이있으면 참가, 방이없으면 만듬.
    public void Click_JoinRoom(string _newRoomName, bool _isSecret)
    {

    }
    [PunRPC]
    public void SendChattingMessage(string _content, PhotonMessageInfo _photonMessageInfo)
    {
        var _chattingContent = _photonMessageInfo.Sender.NickName + ": " + _content;
        //UIManager.instance.UpdateChattingText(_chattingContent, Color.white);
    }

    public Dictionary<int, Player> GetUserList()
    {
        return PhotonNetwork.CurrentRoom.Players;
    }
    #endregion
    #region 일시정지
    public override void OnDisconnected(DisconnectCause cause)
    {

    }
    /// <summary>
    /// 1. 방장이 나간 동시 방장이되려는 유저가 나갈 경우
    /// info => 
    /// 2. 방장이되려는 유저가 나간 동시 방장이 된경우
    /// info
    /// </summary>
    /// <param name="info"></param>
    [PunRPC]
    public void PlayerApplicationPause(PhotonMessageInfo info)
    {
        //UIManager.instance.UpdateChattingText("PlayerApp" + info.Sender.IsMasterClient + "\n");

    }
    private void OnApplicationPause(bool pause)
    {
        //중지 상태
        if (pause)
        {
            bPasued = true;
            //방장만
            if (PhotonNetwork.IsMasterClient)
            {
                foreach (var s in PhotonNetwork.PlayerList)
                {
                    bool paused = (bool)s.CustomProperties["pasued"]; //플레이어들의 현재 일시정지상태여부
                    //마스터가 아닌 클라이언트중 1명이라도 접속중인 사람이 있다면
                    if (paused == false && s.IsMasterClient == false)
                    {
                        //해당플레이어는 데이터 전송보낼시 접속중으로 간주 => 마스터 변경
                        PhotonNetwork.SetMasterClient(s);
                        break;
                    }

                }
            }
            //로컬 플레이어 상태 변경, 현재 마스터의 상태.
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable
            {
                { "pasued", true  }
            });
            //다른유저들에게 나갔다고 알림.
            photonView.RPC("PlayerApplicationPause", RpcTarget.AllViaServer);
            //즉시전송
            PhotonNetwork.SendAllOutgoingCommands();



        }
        else
        {
            //재접속시
            if (bPasued)
            {
                bPasued = false;
            }
        }
    }


    #endregion

}
