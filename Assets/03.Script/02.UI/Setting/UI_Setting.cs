
using UnityEngine;

public class UI_Setting : MonoBehaviour
{
    [SerializeField] GameObject button_soundOn;
    [SerializeField] GameObject button_soundOff;
    [SerializeField] GameObject button_bgmOn;
    [SerializeField] GameObject button_bmgOff;


    private void OnEnable()
    {
        if(PlayerInfo.optionData.soundValue)
        {
            button_soundOn.SetActive(true);
            button_soundOff.SetActive(false);

        }
        else
        {
            button_soundOff.SetActive(true);
            button_soundOn.SetActive(false);
        }

        if (PlayerInfo.optionData.bgmValue)
        {
            button_bgmOn.SetActive(true);
            button_bmgOff.SetActive(false);
        }
        else
        {
            button_bmgOff.SetActive(true);
            button_bgmOn.SetActive(false);

        }

    }

    public void Click_Save()
    {

    }
    

}
