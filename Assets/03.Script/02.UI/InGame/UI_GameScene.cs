using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_GameScene : MonoBehaviour
{
    [SerializeField] PoolableContainer deathUIContainer;
    [SerializeField] GameObject panel_gamejoin;
    [Header("Common")]
    public Transform deathInfoPanel;
    public RankingInfo rankingInfo;
    [Header("Playing Game")]
    public UI_playerExp uI_PlayerExp;
    public UI_PlayerAbilityIconController uI_PlayerAbilityIconController;
    public UI_AbilityPanel uI_AbilityPanel;
    [Header("End  Game")]
    public UI_GameResult uI_GameResult;

   

    public void SetActive(UIState state)
    {
        switch (state)
        {
            case UIState.Lobby:
                this.gameObject.SetActive(false);
                break;
            case UIState.Wait:
                this.gameObject.SetActive(true);
                uI_GameResult.gameObject.SetActive(false);
                uI_AbilityPanel.gameObject.SetActive(false);
                uI_PlayerExp.gameObject.SetActive(false);
                uI_PlayerAbilityIconController.gameObject.SetActive(false);
                panel_gamejoin.SetActive(true);

                break;
            case UIState.Game:
                this.gameObject.SetActive(true);
                uI_GameResult.gameObject.SetActive(false);
                uI_AbilityPanel.gameObject.SetActive(true);
                uI_PlayerExp.gameObject.SetActive(true);
                uI_PlayerAbilityIconController.gameObject.SetActive(true);

                panel_gamejoin.SetActive(false);
                break;
        }

    }
    public void NoticeDeathInfo(string killPlayer, string deathPlayer)
    {
        var deathInfoObject = ObjectPoolManger.Instance.PopPoolableObject(deathUIContainer.sId) as DeathInfo;
        deathInfoObject.transform.SetParent(deathInfoPanel);
        deathInfoObject.SetupInfo(killPlayer, deathPlayer);
    }
    /// <summary>
    /// 게임모드로  
    /// </summary>
    public void ClickGameJoin(bool _active)
    {
        if (_active)
        {
            GameManager.instance.GameJoin();
            UIManager.instance.SetActive(UIState.Game);
        }
        else
        {

        }

    }

    public void ClickGoToLobby()
    {
        MultiManager.instacne.GoToLooby();  //로비로 이동
    }

    public void ClickGoWait()
    {
        UIManager.instance.SetActive(UIState.Wait);
        //ShopManager.Instance.SetActive(true);// 상점 on
    }

}
