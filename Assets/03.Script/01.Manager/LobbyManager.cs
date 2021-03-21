using System.Collections;
using UnityEngine;
using Photon.Pun; // 유니티용 포톤 컴포넌트들
using Photon.Realtime; // 포톤 서비스 관련 라이브러리
public class LobbyManager : MonoBehaviourPunCallbacks
{
    public string gameVersion = "1.0.1";
    void Start()
    {
        UIManager.instance.SetActive(UIState.Lobby);
        PhotonJoin();

    }

    void PhotonJoin()
    {
        print("포톤조인");
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    // 마스터 서버 접속 성공시 자동 실행
    public override void OnConnectedToMaster()
    {
        Connect();
    }

    // 마스터 서버 접속 실패시 자동 실행
    public override void OnDisconnected(DisconnectCause cause)
    {
        //gameJoinButton.interactable = false;

        // 룸 접속 버튼을 비활성화
        // 접속 정보 표시
        // 마스터 서버로의 재접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    // 룸 접속 시도
    public void Connect()
    {
        // 중복 접속 시도를 막기 위해, 접속 버튼 잠시 비활성화
        // 마스터 서버에 접속중이라면
        if (PhotonNetwork.IsConnected)
        {
            // 룸 접속 실행
            UIManager.instance.uI_CommonScene.SetActiveGameConfirmButton(true);

            //PhotonNetwork.JoinRandomRoom();

        }
        else
        {
            // 마스터 서버에 접속중이 아니라면, 마스터 서버에 접속 시도
            // 마스터 서버로의 재접속 시도
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // (빈 방이 없어)랜덤 룸 참가에 실패한 경우 자동 실행
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // 접속 상태 표시
        // 최대 4명을 수용 가능한 빈방을 생성
        string roomName = "Room:" + Random.Range(0, 999);
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 10 });
    }

    // 룸에 참가 완료된 경우 자동 실행
    public override void OnJoinedRoom()
    {
        //ShopManager.Instance.SetActive(false);

        PhotonNetwork.IsMessageQueueRunning = false;
        // 접속 상태 표시
        // 모든 룸 참가자들이 Main 씬을 로드하게 함
        PhotonNetwork.LoadLevel("Main_Io");
        PhotonNetwork.IsMessageQueueRunning = false;
    }

    public static void Click_GameJoin(string roomName, bool isSceret )
    {
        //IO모드로 진입
        
        //ObjectPoolManger.Instance.AllPush();
        PhotonNetwork.JoinRandomRoom();

        //Invoke("Test1", 3.0f);

        //if (UIManager.instance.uI_LobbyScene.InputField_roomName == null)
        //{
        //    PhotonNetwork.JoinRandomRoom();
        //}

        //else
        //{
        //    if (UIManager.instance.uI_LobbyScene.IsScrentMode)
        //    {

        //    }
        //    //커스텀 방 만들기.
        //}


    }

}
