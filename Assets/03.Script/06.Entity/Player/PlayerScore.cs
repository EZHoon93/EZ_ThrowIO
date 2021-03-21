using System.Collections;
using System.Collections.Generic;

using Photon.Pun;
using TMPro;
using UnityEngine;
using System;

public struct PlayerGameResultScore
{
    public string userNickName;
    public float startTime;    //처음 캐릭터 생성시 시작시간
    public float endTime;  //사망 시 시간
    public int maxKillCount;   //최대 킬 
    public int score;  //점수
    public int level;  //사망시 레벨
    public int exp;
    public int coin;
 

}

public class PlayerScore : MonoBehaviourPun , IPunInstantiateMagicCallback , IPunObservable , IOnPhotonViewPreNetDestroy
{
    [SerializeField] int net_score;
    [SerializeField] int net_exp;
    [SerializeField] int viewID;
    [SerializeField] string nickName;
    [SerializeField] TextMeshProUGUI textScore;


    PlayerGameResultScore playerGameResultScore;

    #region Ability
    public event Action<int> AddAbilityPointEvent;    //레벨업시 
    bool isUser;
    IEnumerator enumatorProcesss;

    

    #region 프로퍼티
    int maxExp;
    int currentExp;
    public int CurrentExp
    {
        get => currentExp;
        set
        {
            currentExp = value;
            if (!photonView.IsMine || isUser) return;   //AI가아니가 로컬이아니라면 x

            if (currentExp >= maxExp) Level++;  //AI는 즉시레벨업 
        }
    }
    int level;
    public int Level
    {
        get => level;
        set
        {
            level = value;
            maxExp += Utility.GetMaxExp(level);
            if(photonView.IsMine && isUser)
            {
                UIManager.instance.uI_GameScene.uI_PlayerExp.UpdateLevelText(Level);
            }
            if (level == 1) return;
            AbilityPoint++;
        }
    }

    int abilityPoint;
    public int AbilityPoint
    {
        get => abilityPoint;
        set
        {
            abilityPoint = value;
            AddAbilityPointEvent?.Invoke(abilityPoint);  //능력치 선택 보여줌,
        }
    }

    #endregion

    public int Score {
        get => net_score;
        set
        {
            if (net_score == value) return;

            net_score = value;
            UpdateScore(Score);

        }
    }


    public int KillCount { get; set; }
    #endregion




    private void Awake()
    {
        textScore.text = null;
    }


    /// <summary>
    /// 셋업 
    /// </summary>
    /// <param name="info"></param>
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (photonView.InstantiationData == null) return;   //데이터가없으면 null
        
        AddAbilityPointEvent = null;
        viewID = (int)photonView.InstantiationData[0];
        nickName = (string)photonView.InstantiationData[1];
        isUser = (bool)photonView.InstantiationData[2];
        var _playerController = GameManager.instance.GetPlayer(viewID);
        if (_playerController)
        {
            _playerController.playerStats.SetupPlayerScore( this);
            AddAbilityPointEvent += _playerController.playerStats.Local_AddAbilityPoint; //레벨업시 이벤트, 스텟올리기위해
        }

        switch (GameManager.instance.gameType)
        {
            case GameType.Io:
                UIManager.instance.uI_GameScene.rankingInfo.SetupRakingUI(this);
                break;

        }


        //리셋..
        ResetComponet();

    }



    void ResetComponet()
    {
        Level = 1;
        AbilityPoint = 0;
        Score = 0;
        CurrentExp = 0;
        UpdateScore(Score);
        AddAbilityPointEvent?.Invoke(abilityPoint);
        playerGameResultScore.userNickName = photonView.Controller.NickName;
        playerGameResultScore.startTime = Time.time;
        playerGameResultScore.endTime = 0;
        playerGameResultScore.level = 1;
        playerGameResultScore.score = 0;
        playerGameResultScore.maxKillCount = 0;
        playerGameResultScore.coin = 0;
        playerGameResultScore.exp = 0;
        this.enabled = true;


        if (photonView.IsMine && isUser)
        {
            //PlayerUIController.instance.expUI.UpdateLevelText(Level);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(net_score);
        }
        else
        {
            net_score = (int)stream.ReceiveNext();
            UpdateScore(net_score);
        }
    }

    private void Update()
    {
        //로컬캐릭이면서 자기자신 캐릭만 .. 
        if (photonView.IsMine && isUser)
        {
            Local_UserUpdateExpUI();
        }
    }

    public void UpdateScore(int _score)
    {
        net_score = _score;
        textScore.text = nickName +" : " + net_score.ToString();
        UIManager.instance.uI_GameScene.rankingInfo.Sort();
    }


    void Local_UserUpdateExpUI()
    {
        var uiPlayerExp = UIManager.instance.uI_GameScene.uI_PlayerExp;
        uiPlayerExp.UpdateSliderUI(CurrentExp);
        var sliderValue = uiPlayerExp.GetSliderValue();

        if (sliderValue >= maxExp)
        {
            currentExp -= maxExp;
            Level++;
            uiPlayerExp.SetUpMaxValue(maxExp);
            uiPlayerExp.UpdateLevelText(Level);
        }
    }

   

    public PlayerGameResultScore GetPlayerResultData()
    {
        playerGameResultScore.level = level;
        playerGameResultScore.score = Score;
        playerGameResultScore.maxKillCount = KillCount;
        playerGameResultScore.endTime = Time.time;
        playerGameResultScore.coin = 333;
        playerGameResultScore.exp = 222;

        return playerGameResultScore;
    }
    

    public void OnPreNetDestroy(PhotonView rootView)
    {

    }
}
