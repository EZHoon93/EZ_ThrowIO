
using UnityEngine;
using UnityEngine.UI;

public class UI_Stats : MonoBehaviour
{
    [SerializeField] GameObject panel_price;
    [SerializeField] Button panel_settingButton;
    [SerializeField] Button panel_exitButton;

    

    public void SetActive(UIState state)
    {
        this.gameObject.SetActive(true);
        switch (state)
        {
            case UIState.Lobby:
                panel_price.SetActive(true);
                break;
            case UIState.Wait:
                panel_price.SetActive(true);
                break;
            case UIState.Game:
                panel_price.SetActive(false);

                break;
        }
    }

    public void Click_ExitButton()
    {

    }

}
