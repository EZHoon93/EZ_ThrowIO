using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UI_LobbyScene : MonoBehaviour
{
    public Button gameJoinButton;
    public TMP_InputField InputField_roomName;


    private void Awake()
    {
        gameJoinButton.interactable = false;
    }
    
    public void SetActive(UIState state)
    {
        switch (state)
        {
            case UIState.Lobby:
                this.gameObject.SetActive(true);
                break;
            case UIState.Wait:
                this.gameObject.SetActive(false);
                break;
            case UIState.Game:
                this.gameObject.SetActive(false);
                break;
        }
    }

    
}