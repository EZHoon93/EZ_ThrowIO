using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_CommonScene : MonoBehaviour
{

    [Header("GameObject")]
    [SerializeField] GameObject panel_gameFind;
    [Header("Controller")]
    [SerializeField] UI_UserInfo uI_UserInfo;
    [SerializeField] UI_Stats uI_Stats;
    [Header("Text")]
    [SerializeField] TextMeshProUGUI text_RoomState;

    [SerializeField] Button button_gameFindConfirm; //게임찾기 OK버튼
    [SerializeField] TextMeshProUGUI inputField_roomName;
    [SerializeField] Toggle toggle_isSceret;


    public void SetActive(UIState state)
    {
        switch (state)
        {
            case UIState.Lobby:
                this.gameObject.SetActive(true);
                uI_UserInfo.gameObject.SetActive(true);
                panel_gameFind.SetActive(true);
                break;
            case UIState.Wait:
                this.gameObject.SetActive(true);
                uI_UserInfo.gameObject.SetActive(true);
                panel_gameFind.SetActive(true);
                break;
            case UIState.Game:
                this.gameObject.SetActive(true);
                uI_UserInfo.gameObject.SetActive(false);
                panel_gameFind.SetActive(false);
                
                break;
        }
        uI_Stats.SetActive(state);
    }

    public void SetActiveGameConfirmButton(bool active)
    {
        button_gameFindConfirm.interactable = active;
    }

    public void Click_GameFindConfirm()
    {
        string roomName = inputField_roomName.text;
        var isScret = toggle_isSceret.isOn;


        switch (UIManager.instance.currentState)
        {
            case UIState.Lobby:
                LobbyManager.Click_GameJoin(roomName, isScret);
                break;
        }

        UIManager.instance.SetActiveLoading(true);
    }

    public void Click_ExitConfirm()
    {
        switch (UIManager.instance.currentState)
        {
            case UIState.Game:
                GameManager.instance.GameLeft();    //게임부분 처리
                UIManager.instance.SetActive(UIState.Wait); //UI상태변경

                break;
            case UIState.Wait:
                UIManager.instance.SetActiveLoading(true);
                MultiManager.instacne.GoToLooby();  //로비 씬 으로 이동
                break;
            case UIState.Lobby:
                //UIManager.instance.SetActive(UIState.);
                break;
        }
    }
}
