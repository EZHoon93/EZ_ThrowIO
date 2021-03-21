using System.Collections;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

/// <summary>
/// AI X 유저만.
/// </summary>
public class PlayerSetup : MonoBehaviourPun, IPunInstantiateMagicCallback , IOnPhotonViewPreNetDestroy
{
    public void Reset()
    {
    }
    private void OnDisable()
    {
        if (GameManager.instance == null) return;
        //GameManager.instance.UnRegisterPlayerController(this.photonView.ViewID);
    }
    
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {

        if (photonView.InstantiationData == null) return;   //데이터가없으면 null
        
        string nickName = (string)info.photonView.InstantiationData[0]; //닉네임
        string characterId = (string)info.photonView.InstantiationData[1]; //캐릭터
        string projectileId = (string)info.photonView.InstantiationData[2]; //무기

        this.gameObject.SetActive(false);

        var playerController = GetComponent<PlayerController>();
        var characterContainer = DataContainer.Instance.GetCharacterContainerByContainerId(characterId);
        var projectileContainer = DataContainer.Instance.GetProjectileContainerByContainerId(projectileId);
        //----------------------------------------------------------------------------------------//데이터 가져옴
        GameManager.instance.RegisterPlayerController(this.photonView.ViewID, playerController);    //게임매니저등록 
        CreateCharacter(playerController, characterContainer);      // 캐릭터 설정
        CreateProjectileObject(playerController, projectileContainer);  //무기 설정

        //각자 로컬에서,
        if (photonView.IsMine)
        {
            CameraManager.instance.FollowTarget(this.transform);    //카메라 세팅
            UIManager.instance.SetActiveJoysticks(true);
            object[] datas = {
                this.photonView.ViewID,
                nickName ,
                true
            };
            var playerInGameInfo = PhotonNetwork.Instantiate("PlayerScore", new Vector3(0, 100, 0), Quaternion.identity, 0, datas);   //점수 & 경험치 오브젝트
            var playerAbilityStats = PhotonNetwork.Instantiate("PlayerAbilityStats", new Vector3(0, 100, 0), Quaternion.identity, 0, new object[] { this.photonView.ViewID }).GetComponent<PlayerAbilityStats>();   //점수 & 경험치 오브젝트
            //playerAbilityStats.projectileData = projectileContainer.sProjectileData;
            GetComponent<PlayerStats>().SetupMaxEnergy(projectileContainer.sProjectileData.sMaxInstallCount);
        }
        else
        {
            this.tag = "Enemy";
        }

        

        playerController.ResetComponets();  //초기화 하고 다시시작


        this.gameObject.SetActive(true);


    }


    void CreateCharacter(PlayerController playerController,  CharacterContainer characterContainer)
    {
        var characterObject = playerController.playerStats.characterObject;
        if(characterObject)
        {
            playerController.playerStats.UnRegisterTransparentObject(characterObject.gameObject);
            characterObject.Push();
        }
        characterObject = ObjectPoolManger.Instance.PopCharacterObject(characterContainer.sId).GetComponent<CharacterObject>();
        this.GetComponent<Animator>().avatar = characterObject.GetComponent<Animator>().avatar;
        characterObject.transform.SetParent(playerController.playerStats.playerObject.transform);
        characterObject.transform.localPosition = Vector3.zero;
        characterObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        characterObject.CharacterStatsData = characterContainer.sCharacterStatsData;

        playerController.playerStats.SetupPlayerCharacter(characterObject);
        playerController.playerStats.RegitserTransparentObject(characterObject.skinnedMesh.gameObject); //투명가능한지 추가
    }

    void CreateProjectileObject(PlayerController playerController, ProjectileContainer projectileContainer)
    {
        var projectileObject = playerController.playerShooter.projectileObject;
        if (playerController.playerShooter.projectileObject)
        {
            playerController.playerStats.UnRegisterTransparentObject(projectileObject.gameObject);
            projectileObject.Push();
        }
        projectileObject = ObjectPoolManger.Instance.PopProjectileObject(projectileContainer.sId) as ProjectileObject;
        projectileObject.transform.SetParent(playerController.playerStats.handTransform);
        projectileObject.transform.localPosition = projectileContainer.sHandPosition;
        projectileObject.transform.localRotation = Quaternion.Euler(projectileContainer.sHandRotation);

        playerController.playerShooter.SetupProjectileObject(projectileObject , projectileContainer.sId);
        playerController.playerStats.RegitserTransparentObject(projectileObject.gameObject);        //투명 가능한지 추가

    }

    //void SetupPlayerUI(PlayerController playerController , int maxHealth,  int maxInstallCount)
    //{
    //    var playerUI = playerController.playerStats.playerUI;
    //    playerUI.SetUpMaxEnergyhSlider(maxInstallCount);
    //}





    public void OnPreNetDestroy(PhotonView rootView)
    {
        if (GameManager.instance == null) return;
        //GameManager.instance.UnRegisterPlayerController(this.photonView.ViewID);
        if (ObjectPoolManger.Instance == null) return;

        var playerController = GetComponent<PlayerController>();
        if (playerController)
        {
            //playerController.playerStats.characterObject.Push();

            var buffers = GetComponentsInChildren<BuffState>();
            foreach(var b in buffers)
            {
                b.Push();
            }

            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(playerController.playerStats.playerAbilityStats.gameObject);
                PhotonNetwork.Destroy(playerController.playerStats.playerScore.gameObject);
            }
            
        }
      
    }
}